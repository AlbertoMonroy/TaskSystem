using System.Web.Http;
using TaskSystem.Server.Models;

[Authorize]
[RoutePrefix("api/tasks")]
public class TasksController : ApiController
{
    private readonly TaskService _taskService = new TaskService();

    [HttpGet]
    [Route("")]
    public IHttpActionResult GetAll()
    {
        var tasks = _taskService.GetAll();
        return Ok(tasks);
    }

    [HttpGet]
    [Route("{id:int}")]
    public IHttpActionResult Get(int id)
    {
        var task = _taskService.GetById(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    [Route("")]
    public IHttpActionResult Create([FromBody] Task task)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = _taskService.Create(task);
        return Ok(created);
    }

    [HttpPut]
    [Route("{id:int}")]
    public IHttpActionResult Update(int id, [FromBody] Task task)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        task.Id = id;
        _taskService.Update(task);
        return Ok();
    }

    [HttpDelete]
    [Route("{id:int}")]
    public IHttpActionResult Delete(int id)
    {
        _taskService.Delete(id);
        return Ok();
    }
}
