using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskSystem.Server.Models;

public class TaskService
{
    private readonly TaskSystemEntities _context;

    public TaskService()
    {
        _context = new TaskSystemEntities();
    }

    public IEnumerable<TaskDto> GetAll()
    {
        return _context.Tasks
         .Include(t => t.Priority)
         .ToList()
         .Select(t => new TaskDto
         {
             Id = t.Id,
             Title = t.Title,
             Description = t.Description,
             IsCompleted = t.IsCompleted,
             DueDate = t.DueDate,
             PriorityId = t.PriorityId,
             PriorityName = t.Priority != null ? t.Priority.Name : null,
             UserId = t.UserId
         });
    }

    public Task GetById(int id)
    {
        return _context.Tasks.Find(id);
    }

    public Task Create(Task task)
    {
        task.CreatedAt = DateTime.Now;
        task.UpdatedAt = DateTime.Now;
        _context.Tasks.Add(task);
        _context.SaveChanges();

        TaskNotifier.Instance.NotifyAllClients();
        return task;
    }

    public void Update(Task task)
    {
        var existing = _context.Tasks.Find(task.Id);
        if (existing == null) return;

        existing.Title = task.Title;
        existing.Description = task.Description;
        existing.IsCompleted = task.IsCompleted;
        existing.DueDate = task.DueDate;
        existing.PriorityId = task.PriorityId;
        existing.UpdatedAt = DateTime.Now;

        _context.Entry(existing).State = EntityState.Modified;
        _context.SaveChanges();

        TaskNotifier.Instance.NotifyAllClients();
    }

    public void Delete(int id)
    {
        var task = _context.Tasks.Find(id);
        if (task == null) return;

        _context.Tasks.Remove(task);
        _context.SaveChanges();

        TaskNotifier.Instance.NotifyAllClients();
    }
}
