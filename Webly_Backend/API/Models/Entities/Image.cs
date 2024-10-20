using API.Models;

public class Image
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty; // Initialize to avoid null warning
    public User User { get; set; } = new User(); // Initialize to avoid null warning
    public DateTime PostingDate { get; set; }
    public List<Tag> Tags { get; set; } = new List<Tag>(); // Initialize to avoid null warning
}
