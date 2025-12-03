using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talos.Shared.Data;
using Talos.Shared.Models;
using Talos.Server.Models.Dtos;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Talos.Server.Application.RealTime;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;

    public UserController(AppDbContext context, IMapper mapper, IDistributedCache cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }

    // GET all users
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        string cacheKey = "users_all";
        string cachedData = await _cache.GetStringAsync(cacheKey);

        if (cachedData != null)
        {
            var users = JsonSerializer.Deserialize<List<UserDto>>(cachedData);
            return Ok(new { source = "redis-cache", data = users });
        }

        var dbUsers = await _context.users.AsNoTracking().ToListAsync();
        var dtos = _mapper.Map<List<UserDto>>(dbUsers);

        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dtos),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });

        return Ok(new { source = "database", data = dtos });
    }

    // GET user by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        string cacheKey = $"user_{id}";
        string cachedData = await _cache.GetStringAsync(cacheKey);

        if (cachedData != null)
        {
            var user = JsonSerializer.Deserialize<UserDto>(cachedData);
            return Ok(new { source = "redis-cache", data = user });
        }

        var dbUser = await _context.users.AsNoTracking().FirstOrDefaultAsync(u => u.id == id);
        if (dbUser == null) return NotFound(new { message = "User not found" });

        var dto = _mapper.Map<UserDto>(dbUser);
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });

        return Ok(new { source = "database", data = dto });
    }

    // POST create user
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = _mapper.Map<User>(dto);
        entity.create_at = DateTime.UtcNow;

        await _context.users.AddAsync(entity);
        await _context.SaveChangesAsync();

        await _cache.RemoveAsync("users_all");

        var response = _mapper.Map<UserDto>(entity);
        return CreatedAtAction(nameof(GetUserById), new { id = entity.id }, response);
    }

    // PUT update user
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto dto)
    {
        var user = await _context.users.FirstOrDefaultAsync(u => u.id == id);
        if (user == null) return NotFound(new { message = "User not found" });

        _mapper.Map(dto, user);
        user.create_at = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _cache.RemoveAsync($"user_{id}");
        await _cache.RemoveAsync("users_all");

        var response = _mapper.Map<UserDto>(user);
        return Ok(response);
    }

    // DELETE user
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.users.FirstOrDefaultAsync(u => u.id == id);
        if (user == null) return NotFound(new { message = "User not found" });

        _context.users.Remove(user);
        await _context.SaveChangesAsync();

        await _cache.RemoveAsync($"user_{id}");
        await _cache.RemoveAsync("users_all");

        return NoContent();
    }
    
    [ApiController]
    [Route("test/realtime")]
    public class RealTimeTestController : ControllerBase
    {
        private readonly NotificationService _notifier;

        public RealTimeTestController(NotificationService notifier)
        {
            _notifier = notifier;
        }

        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            await _notifier.NotifyTemplateCreated("testUser", "testTemplate");
            return Ok("Notificación enviada");
        }
    }

}
