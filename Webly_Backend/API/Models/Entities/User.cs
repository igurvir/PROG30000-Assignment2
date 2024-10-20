namespace API.Models;
public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty; // Initialize to avoid null warning
    public string Name { get; set; } = string.Empty; // Initialize to avoid null warning
    public List<Image> Images { get; set; } = new List<Image>(); // Initialize to avoid null warning
}
