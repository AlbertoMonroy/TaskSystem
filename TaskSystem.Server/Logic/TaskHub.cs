using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;

public class TaskHub : Hub
{
    public async Task LockTask(int taskId, int userId)
    {
        await Clients.Others.TaskLocked(taskId, userId);
    }

    public async Task UnlockTask(int taskId)
    {
        await Clients.Others.TaskUnlocked(taskId);
    }

    public void NotifyTasksUpdated()
    {
        Clients.Others.tasksUpdated();
    }
}