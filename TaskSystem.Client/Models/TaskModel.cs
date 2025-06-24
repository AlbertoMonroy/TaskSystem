using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskSystem.Client.Models
{
    public class TaskModel : INotifyPropertyChanged
    {
        // Propiedades existentes
        public int Id { get; set; }

        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        private bool? _isCompleted;
        public bool? IsCompleted
        {
            get => _isCompleted;
            set { _isCompleted = value; OnPropertyChanged(); }
        }

        private DateTime? _dueDate;
        public DateTime? DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(); }
        }

        private int? _priorityId;
        public int? PriorityId
        {
            get => _priorityId;
            set { _priorityId = value; OnPropertyChanged(); }
        }

        private string _priorityName;
        public string PriorityName
        {
            get => _priorityName;
            set { _priorityName = value; OnPropertyChanged(); }
        }

        public int UserId { get; set; }

        private bool _isLocked;
        public bool IsLocked
        {
            get => _isLocked;
            set { _isLocked = value; OnPropertyChanged(); }
        }

        private string _lockedBy;
        public string LockedBy
        {
            get => _lockedBy;
            set { _lockedBy = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
