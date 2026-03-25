using System.ComponentModel.DataAnnotations;

namespace LuxeFemStore.Models;

public class User
{
    [Key] // This makes the ID the primary key in the database
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}