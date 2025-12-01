using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Talos.Shared.Data;
using Talos.Server.Models.Dtos;
using Talos.Shared.Models;

namespace Talos.Server.Controllers;

[ApiController]
[Route("api/templates")]
public class TemplateController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public TemplateController(AppDbContext context, IDistributedCache cache, IMapper mapper)
    {
        _context = context;
        _cache = cache;
        _mapper = mapper;
    }

    // GET all
    [HttpGet]
    public async Task<IActionResult> GetAllTemplates()
    {
        try
        {
            string cacheKey = "templates_all";
            string cachedData = await _cache.GetStringAsync(cacheKey);

            if (cachedData != null)
            {
                var cachedResult = JsonSerializer.Deserialize<List<TemplateDto>>(cachedData);
                return Ok(new { source = "redis-cache", data = cachedResult });
            }

            var templates = await _context.templates.AsNoTracking().ToListAsync();
            var dtos = _mapper.Map<List<TemplateDto>>(templates);

            var serialized = JsonSerializer.Serialize(dtos);
            await _cache.SetStringAsync(cacheKey, serialized, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return Ok(new { source = "database", data = dtos });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
        }
    }

    // GET by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplateById(int id)
    {
        try
        {
            string cacheKey = $"template_{id}";
            string cachedData = await _cache.GetStringAsync(cacheKey);

            if (cachedData != null)
            {
                var cachedResult = JsonSerializer.Deserialize<TemplateDto>(cachedData);
                return Ok(new { source = "redis-cache", data = cachedResult });
            }

            var template = await _context.templates.AsNoTracking().FirstOrDefaultAsync(t => t.id == id);
            if (template == null)
                return NotFound(new { message = "Template not found" });

            var dto = _mapper.Map<TemplateDto>(template);
            var serialized = JsonSerializer.Serialize(dto);
            await _cache.SetStringAsync(cacheKey, serialized, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return Ok(new { source = "database", data = dto });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
        }
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> CreateTemplate([FromBody] TemplateCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = 1; // luego se obtiene del JWT
            var entity = _mapper.Map<Template>(dto);
            entity.user_id = userId;
            entity.slug = dto.Template_Name.ToLower().Replace(" ", "-");
            entity.create_at = DateTime.UtcNow;

            await _context.templates.AddAsync(entity);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync("templates_all"); // Limpiar cache de lista

            var response = _mapper.Map<TemplateDto>(entity);
            return CreatedAtAction(nameof(GetTemplateById), new { id = entity.id }, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal error", detail = ex.Message });
        }
    }

    // PUT
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTemplate(int id, [FromBody] TemplateCreateDto dto)
    {
        try
        {
            var template = await _context.templates.FirstOrDefaultAsync(t => t.id == id);
            if (template == null)
                return NotFound(new { message = "Template not found" });

            _mapper.Map(dto, template); // AutoMapper update the fields 
            template.slug = dto.Template_Name.ToLower().Replace(" ", "-");
            template.create_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Clean caché
            await _cache.RemoveAsync($"template_{id}");
            await _cache.RemoveAsync("templates_all");

            var response = _mapper.Map<TemplateDto>(template);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal error", detail = ex.Message });
        }
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate(int id)
    {
        try
        {
            var template = await _context.templates.FirstOrDefaultAsync(t => t.id == id);
            if (template == null)
                return NotFound(new { message = "Template not found" });

            _context.templates.Remove(template);
            await _context.SaveChangesAsync();

            // Clean caché
            await _cache.RemoveAsync($"template_{id}");
            await _cache.RemoveAsync("templates_all");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal error", detail = ex.Message });
        }
    }
}
