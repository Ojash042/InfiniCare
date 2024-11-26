using System.ComponentModel.DataAnnotations;

namespace Infinicare_Ojash_Devkota.Models;

public class UserCredentials {
    [Key]
    public Guid UserId { get; set; } 
    public string UserName { get; set; }= String.Empty;
    public string UserPassword { get; set; } = String.Empty;    
    public required UserDetails UserDetails { get; set; }
}