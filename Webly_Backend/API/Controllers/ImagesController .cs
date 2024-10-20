using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Models.Persistence;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ImagesController(AppDbContext context)
    {
        _context = context;
    }

    // GET /api/images
    [HttpGet]
    public async Task<IActionResult> GetImages(int page = 1)
    {
        const int pageSize = 10;
        var images = await _context.Images.OrderByDescending(i => i.PostingDate)
                                          .Skip((page - 1) * pageSize)
                                          .Take(pageSize)
                                          .ToListAsync();

        var totalImages = await _context.Images.CountAsync();
        var totalPages = (int)Math.Ceiling(totalImages / (double)pageSize);

        var response = new
        {
            meta = new { totalPages, totalImages },
            data = images.Select(i => new { i.Id, i.Url, username = i.User.Name }),
            links = new
            {
                first = Url.Action(nameof(GetImages), new { page = 1 }),
                prev = page > 1 ? Url.Action(nameof(GetImages), new { page = page - 1 }) : null,
                next = page < totalPages ? Url.Action(nameof(GetImages), new { page = page + 1 }) : null,
                last = Url.Action(nameof(GetImages), new { page = totalPages })
            }
        };

        return Ok(response);
    }

    // GET /api/images/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetImageById(Guid id)
    {
        var image = await _context.Images.Include(i => i.User)
                                         .Include(i => i.Tags)
                                         .FirstOrDefaultAsync(i => i.Id == id);

        if (image == null)
        {
            return NotFound();
        }

        var response = new
        {
            id = image.Id,
            url = image.Url,
            userName = image.User.Name,
            userId = image.User.Id,
            tags = image.Tags.Select(t => t.Text).ToList()
        };

        return Ok(response);
    }

    // GET /api/images/byTag?tag=cars
    [HttpGet("byTag")]
    public async Task<IActionResult> GetImagesByTag(string tag, int page = 1)
    {
        const int pageSize = 10;
        var images = await _context.Images.Include(i => i.Tags)
                                          .Where(i => i.Tags.Any(t => t.Text == tag))
                                          .OrderByDescending(i => i.PostingDate)
                                          .Skip((page - 1) * pageSize)
                                          .Take(pageSize)
                                          .ToListAsync();

        if (!images.Any())
        {
            return NotFound();
        }

        var totalImages = await _context.Images.CountAsync(i => i.Tags.Any(t => t.Text == tag));
        var totalPages = (int)Math.Ceiling(totalImages / (double)pageSize);

        var response = new
        {
            meta = new { totalPages, totalImages },
            data = images.Select(i => new { i.Id, i.Url, username = i.User.Name }),
            links = new
            {
                first = Url.Action(nameof(GetImagesByTag), new { tag, page = 1 }),
                prev = page > 1 ? Url.Action(nameof(GetImagesByTag), new { tag, page = page - 1 }) : null,
                next = page < totalPages ? Url.Action(nameof(GetImagesByTag), new { tag, page = page + 1 }) : null,
                last = Url.Action(nameof(GetImagesByTag), new { tag, page = totalPages })
            }
        };

        return Ok(response);
    }

    // GET /api/images/populartags
    [HttpGet("populartags")]
    public async Task<IActionResult> GetPopularTags()
    {
        var tags = await _context.Tags.GroupBy(t => t.Text)
                                      .OrderByDescending(g => g.Count())
                                      .Take(5)
                                      .Select(g => new { tag = g.Key, count = g.Count() })
                                      .ToListAsync();

        return Ok(tags);
    }
}
