namespace API.Models;
public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty; 
    public string Name { get; set; } = string.Empty; 
    public List<Image> Images { get; set; } = new List<Image>(); 
}
