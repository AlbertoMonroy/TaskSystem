using System;
using System.Linq;
using System.Web.Http;
using TaskSystem.Server.Models;

[Authorize]
[RoutePrefix("api/task-locks")]
public class TaskLocksController : ApiController
{
    private readonly TaskSystemEntities _context = new TaskSystemEntities();

    [HttpPost]
    [Route("lock")]
    public IHttpActionResult LockTask([FromBody] TaskLock lockRequest)
    {
        // Desbloquea primero cualquier otro bloqueo activo
        var existing = _context.TaskLocks
            .FirstOrDefault(l => l.TaskId == lockRequest.TaskId && l.IsActive == true);

        if (existing != null)
        {
            return Conflict();
        }

        lockRequest.LockedAt = DateTime.Now;
        lockRequest.IsActive = true;

        _context.TaskLocks.Add(lockRequest);
        _context.SaveChanges();

        return Ok();
    }

    [HttpPost]
    [Route("unlock")]
    public IHttpActionResult UnlockTask([FromBody] TaskLock lockRequest)
    {
        var existing = _context.TaskLocks
            .FirstOrDefault(l => l.TaskId == lockRequest.TaskId && l.UserId == lockRequest.UserId && l.IsActive == true);

        if (existing == null)
            return NotFound();

        existing.IsActive = false;
        _context.SaveChanges();

        return Ok();
    }

    [HttpGet]
    [Route("active/{taskId:int}")]
    public IHttpActionResult GetActiveLock(int taskId)
    {
        var lockInfo = _context.TaskLocks
            .FirstOrDefault(l => l.TaskId == taskId && l.IsActive == true);

        return Ok(lockInfo);
    }
}
