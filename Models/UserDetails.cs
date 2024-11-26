using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infinicare_Ojash_Devkota.Models;

public class UserDetails { 
    [Key]
    public Guid UserId { get; set; }
    public String AccountNumber {get; set;} = string.Empty; 
    public String FirstName { get; set; } = string.Empty;    
    public String? MiddleName { get; set; }
    public String LastName { get; set; } = String.Empty;
    public String Country { get; set; } = string.Empty;
    public required UserCredentials UserCredentials { get; set; }
}