using Microsoft.AspNet.SignalR.Client;
using System;
using System.Configuration;
using System.Threading.Tasks;

public class SignalRService
{
    private static readonly Lazy<SignalRService> _instance = new Lazy<SignalRService>(() => new SignalRService());
    public static SignalRService Instance => _instance.Value;

    private HubConnection _connection;
    private IHubProxy _hubProxy;

    public event Action TasksUpdated;
    public event Action<int, int> TaskLocked;
    public event Action<int> TaskUnlocked;
    public event Action ConnectionChanged;

    public string Status => _connection?.State.ToString() ?? "Disconnected";

    private SignalRService() { }

    public async Task ConnectAsync()
    {
        if (_connection != null && _connection.State == ConnectionState.Connected)
            return;

        _connection = new HubConnection(ConfigurationManager.AppSettings["HubConecction"]); 
        _hubProxy = _connection.CreateHubProxy("TaskHub");

        _hubProxy.On("tasksUpdated", () =>
        {
            TasksUpdated?.Invoke();
        });

        _hubProxy.On<int, int>("TaskLocked", (taskId, userId) =>
        {
            TaskLocked?.Invoke(taskId, userId);
        });

        _hubProxy.On<int>("TaskUnlocked", taskId =>
        {
            TaskUnlocked?.Invoke(taskId);
        });

        _connection.StateChanged += state =>
        {
            ConnectionChanged?.Invoke();
        };

        try
        {
            await _connection.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al conectar con SignalR: " + ex.Message);
        }
    }

    public Task DisconnectAsync()
    {
        if (_connection != null && _connection.State != ConnectionState.Disconnected)
            _connection.Stop();

        return Task.CompletedTask;
    }

    public async Task SendTaskLock(int taskId, int userId)
    {
        if (IsConnected())
            await _hubProxy.Invoke("LockTask", taskId, userId);
        else
            Console.WriteLine("No conectado. No se pudo enviar LockTask.");
    }

    public async Task SendTaskUnlock(int taskId)
    {
        if (IsConnected())
            await _hubProxy.Invoke("UnlockTask", taskId);
        else
            Console.WriteLine("No conectado. No se pudo enviar UnlockTask.");
    }

    public async Task NotifyTasksUpdated()
    {
        if (IsConnected())
        { 
            await _hubProxy.Invoke("NotifyTasksUpdated");
        }
        else
            Console.WriteLine("No conectado. No se pudo enviar NotifyTasksUpdated.");
    }

    private bool IsConnected()
    {
        return _connection?.State == ConnectionState.Connected;
    }
}
