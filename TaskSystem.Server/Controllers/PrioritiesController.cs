using System.Linq;
using System.Web.Http;
using TaskSystem.Server.Models;
using TaskSystem.Server.Models.DTOs;

[RoutePrefix("api/priorities")]
public class PrioritiesController : ApiController
{
    private readonly TaskSystemEntities _context = new TaskSystemEntities();

    [HttpGet]
    [Route("")]
    public IHttpActionResult GetAll()
    {
        var priorities = _context.Priorities
        .OrderBy(p => p.Order)
        .ToList()
        .Select(p => new PriorityDto
        {
            Id = p.Id,
            Name = p.Name,
            Order = p.Order
        });

        return Ok(priorities);
    }

    [HttpPost]
    [Route("")]
    public IHttpActionResult Create([FromBody] Priority priority)
    {
        if (string.IsNullOrWhiteSpace(priority.Name))
            return BadRequest("El nombre de la prioridad es obligatorio.");

        var exists = _context.Priorities.Any(p => p.Name == priority.Name);
        if (exists)
            return Conflict();

        _context.Priorities.Add(priority);
        _context.SaveChanges();

        return Ok(priority);
    }

    [HttpPut]
    [Route("{id:int}")]
    public IHttpActionResult Update(int id, [FromBody] Priority updated)
    {
        var priority = _context.Priorities.Find(id);
        if (priority == null) return NotFound();

        priority.Name = updated.Name;
        priority.Order = updated.Order;
        _context.SaveChanges();

        return Ok(priority);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public IHttpActionResult Delete(int id)
    {
        var priority = _context.Priorities.Find(id);
        if (priority == null) return NotFound();

        _context.Priorities.Remove(priority);
        _context.SaveChanges();

        return Ok();
    }

}
