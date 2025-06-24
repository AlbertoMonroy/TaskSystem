using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using TaskSystem.Server.Models;

[RoutePrefix("api/users")]
public class UsersController : ApiController
{
    private readonly TaskSystemEntities _context = new TaskSystemEntities();

    [HttpPost]
    [Route("login")]
    public IHttpActionResult Login([FromBody] LoginRequest request)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);
        if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return Unauthorized();

        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = ConfigurationManager.AppSettings["JwtSecretKey"];
        var key = Encoding.UTF8.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("UserId", user.Id.ToString())
        }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { token = tokenString, user = new UserDto { Id = user.Id, Username = user.Username, FullName = user.FullName } });
    }

    [HttpPost]
    [Route("logout/{id:int}")]
    public IHttpActionResult Logout(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        user.IsOnline = false;
        _context.SaveChanges();

        return Ok();
    }

    [HttpGet]
    public IHttpActionResult GetAll()
    {
        var users = _context.Users
       .ToList()
       .Select(u => new UserDto
       {
           Id = u.Id,
           Username = u.Username,
           FullName = u.FullName,
           Email = u.Email,
           PasswordHash = u.PasswordHash
       });

        return Ok(users);
    }

    [HttpGet]
    [Route("{id:int}")]
    public IHttpActionResult GetById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);

        if (user == null)
            return NotFound();

        var dto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName
        };

        return Ok(dto);
    }

    [HttpPost]
    public IHttpActionResult Create([FromBody] User user)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (_context.Users.Any(u => u.Username == user.Username))
            return Conflict();

        user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);
        user.CreatedAt = DateTime.Now;

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok(user);
    }

    [HttpPut]
    [Route("{id:int}")]
    public IHttpActionResult Update(int id, [FromBody] UserDto dto)
    {
        var user = _context.Users.Find(id);
        if (user == null)
            return NotFound();

        user.FullName = dto.FullName;
        user.Username = dto.Username;
        _context.SaveChanges();

        var result = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName
        };

        return Ok(result);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public IHttpActionResult Delete(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        _context.SaveChanges();
        return Ok();
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
