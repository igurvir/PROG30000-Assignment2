using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Models.Persistence;
using Microsoft.EntityFrameworkCore;
using API.Models.Helpers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // POST /api/users
    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] User user)
    {
        if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email))
        {
            return BadRequest(new { errors = new[] { new { status = "400", title = "Invalid data", detail = "Name and email are required." } } });
        }

        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        {
            return BadRequest(new { errors = new[] { new { status = "400", title = "Email already exists", detail = "This email already exists in the database." } } });
        }

        user.Id = Guid.NewGuid();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // GET /api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _context.Users.Include(u => u.Images)
                                       .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        var response = new
        {
            id = user.Id,
            username = user.Name,
            email = user.Email,
            imagesUrls = user.Images.OrderByDescending(i => i.PostingDate)
                                     .Take(10)
                                     .Select(i => i.Url)
                                     .ToList()
        };

        return Ok(response);
    }

    // GET /api/users/{id}/images
    [HttpGet("{id}/images")]
    public async Task<IActionResult> GetUserImages(Guid id, int page = 1)
    {
        const int pageSize = 10;
        var user = await _context.Users.Include(u => u.Images)
                                       .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        var images = user.Images.OrderByDescending(i => i.PostingDate)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

        var totalImages = user.Images.Count;
        var totalPages = (int)Math.Ceiling(totalImages / (double)pageSize);

        var response = new
        {
            meta = new { totalPages, totalImages },
            data = images.Select(i => new { i.Id, i.Url }),
            links = new
            {
                first = Url.Action(nameof(GetUserImages), new { id, page = 1 }),
                prev = page > 1 ? Url.Action(nameof(GetUserImages), new { id, page = page - 1 }) : null,
                next = page < totalPages ? Url.Action(nameof(GetUserImages), new { id, page = page + 1 }) : null,
                last = Url.Action(nameof(GetUserImages), new { id, page = totalPages })
            }
        };

        return Ok(response);
    }

    // DELETE /api/users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await _context.Users.Include(u => u.Images)
                                       .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        _context.Images.RemoveRange(user.Images);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // POST /api/users/{id}/image
    [HttpPost("{id}/image")]
    public async Task<IActionResult> AddImageToUser(Guid id, [FromBody] string imageUrl)
    {
        var user = await _context.Users.Include(u => u.Images).FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            return NotFound(new { errors = new[] { new { status = "404", title = "User Not Found", detail = "The specified user does not exist." } } });
        }

        if (string.IsNullOrEmpty(imageUrl))
        {
            return BadRequest(new { errors = new[] { new { status = "400", title = "Invalid Data", detail = "Image URL is required." } } });
        }

        // Create a new Image entity
        var newImage = new Image
        {
            Id = Guid.NewGuid(),
            Url = imageUrl,
            PostingDate = DateTime.UtcNow,
            User = user
        };

        
        var tags = ImageHelper.GetTags(imageUrl).ToList();
        foreach (var tagText in tags)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Text == tagText) ?? new Tag { Id = Guid.NewGuid(), Text = tagText };
            newImage.Tags.Add(tag);
        }

        // Add the new image to the user's list of images and save changes
        user.Images.Add(newImage);
        _context.Images.Add(newImage);
        await _context.SaveChangesAsync();

        // Return the updated user object including the last 10 images
        var response = new
        {
            id = user.Id,
            username = user.Name,
            email = user.Email,
            imagesUrls = user.Images.OrderByDescending(i => i.PostingDate)
                                     .Take(10)
                                     .Select(i => i.Url)
                                     .ToList()
        };

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
    }
}
