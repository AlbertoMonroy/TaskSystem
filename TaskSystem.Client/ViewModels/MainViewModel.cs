using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskSystem.Client.Helpers;
using TaskSystem.Client.Models;

namespace TaskSystem.Client
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TaskModel> Tasks { get; set; } = new ObservableCollection<TaskModel>();
        public ObservableCollection<PriorityModel> Priorities { get; set; } = new ObservableCollection<PriorityModel>();

        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get => _selectedTask;
            set { _selectedTask = value; OnPropertyChanged(); }
        }

        public ICommand LoadTasksCommand { get; }
        public ICommand CreateTaskCommand { get; }
        public ICommand UpdateTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }

        private UserModel CurrentUser => App.Current.Properties["LoggedUser"] as UserModel;

        private string _newTitle;
        public string NewTitle
        {
            get => _newTitle;
            set { _newTitle = value; OnPropertyChanged(); }
        }

        private string _newDescription;
        public string NewDescription
        {
            get => _newDescription;
            set { _newDescription = value; OnPropertyChanged(); }
        }

        private int? _selectedPriorityId;
        public int? SelectedPriorityId
        {
            get => _selectedPriorityId;
            set { _selectedPriorityId = value; OnPropertyChanged(); }
        }

        private DateTime? _dueDate;
        public DateTime? DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(); }
        }

        public string SignalRStatus => SignalRService.Instance.Status;

        public MainViewModel()
        {
            LoadTasksCommand = new RelayCommand(async _ => await LoadTasks());
            CreateTaskCommand = new RelayCommand(async _ => await CreateTask());
            UpdateTaskCommand = new RelayCommand(async _ => await UpdateTask(), _ => SelectedTask != null);
            DeleteTaskCommand = new RelayCommand(async _ => await DeleteTask(), _ => SelectedTask != null);

            Task.Run(async () => await SignalRService.Instance.ConnectAsync());
            SignalRService.Instance.TasksUpdated += async () => { await LoadTasks(); };
            SignalRService.Instance.TaskLocked += OnTaskLocked;
            SignalRService.Instance.TaskUnlocked += OnTaskUnlocked;
            SignalRService.Instance.ConnectionChanged += OnSignalRStatusChanged;

            _ = ApiService.Instance.LoadUsersAsync();
            _ = LoadTasks(); // Carga inicial
            _ = LoadPriorities();
        }

        public async Task LoadTasks()
        {
            var tasks = await ApiService.Instance.GetTasksAsync();

            App.Current.Dispatcher.Invoke(() =>
            {
                Tasks.Clear();
                foreach (var t in tasks)
                    Tasks.Add(t);
            });
        }

        private async Task LoadPriorities()
        {
            var priorities = await ApiService.Instance.GetPrioritiesAsync();
            Priorities.Clear();
            foreach (var p in priorities)
                Priorities.Add(p);
        }

        public async Task CreateTask()
        {
            var newTask = new TaskModel
            {
                Title = this.NewTitle,
                Description = this.NewDescription,
                DueDate = this.DueDate,
                PriorityId = this.SelectedPriorityId,
                IsCompleted = false,
                UserId = CurrentUser.Id
            };

            await ApiService.Instance.CreateTaskAsync(newTask);
            await SignalRService.Instance.NotifyTasksUpdated();
            await LoadTasks();

            // Limpiar campos
            NewTitle = string.Empty;
            NewDescription = string.Empty;
            DueDate = null;
            SelectedPriorityId = null;
        }

        public async Task UpdateTask()
        {
            if (SelectedTask != null)
            {
                await ApiService.Instance.UpdateTaskAsync(SelectedTask);
                await SignalRService.Instance.SendTaskUnlock(SelectedTask.Id);
                await SignalRService.Instance.NotifyTasksUpdated();
                await LoadTasks();
            }
        }

        public async Task DeleteTask()
        {
            if (SelectedTask != null)
            {
                await ApiService.Instance.DeleteTaskAsync(SelectedTask.Id);
                await SignalRService.Instance.NotifyTasksUpdated();
                await LoadTasks();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private async void OnTaskLocked(int taskId, int userId)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.IsLocked = true;
                var user = await ApiService.Instance.GetUserByIdAsync(userId);
                task.LockedBy = user?.FullName ?? $"Usuario {userId}";
            }
        }

        private void OnTaskUnlocked(int taskId)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
                task.IsLocked = false;
        }

        private void OnSignalRStatusChanged()
        {
            OnPropertyChanged(nameof(SignalRStatus));
        }
    }
}
