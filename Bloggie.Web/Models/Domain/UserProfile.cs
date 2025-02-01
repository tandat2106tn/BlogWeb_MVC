using System.ComponentModel.DataAnnotations;

public class UserProfile
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(450)]  // Match IdentityUser Id length
    public string UserId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string FullName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    [StringLength(200)]
    public string Address { get; set; }
    
    [StringLength(20)]
    public string PhoneNumber { get; set; }
    
    [StringLength(500)]
    public string Education { get; set; }
    
    [StringLength(1000)]
    public string WorkExperience { get; set; }
    
    [StringLength(500)]
    public string Skills { get; set; }
}