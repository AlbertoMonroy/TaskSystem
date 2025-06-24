using Microsoft.AspNet.SignalR;
using System;

public class TaskNotifier
{
    private static readonly Lazy<TaskNotifier> _instance = new Lazy<TaskNotifier>(() => new TaskNotifier());
    private readonly IHubContext _hubContext;

    private TaskNotifier()
    {
        _hubContext = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();
    }

    public static TaskNotifier Instance => _instance.Value;

    public void NotifyAllClients()
    {
        _hubContext.Clients.All.tasksUpdated();
    }
}
