using API.Models;

public class Image
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty; 
    public User User { get; set; } = new User(); 
    public DateTime PostingDate { get; set; }
    public List<Tag> Tags { get; set; } = new List<Tag>(); 
}
