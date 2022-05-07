using Microsoft.AspNetCore.Identity;

namespace DocAppLibrary.Entities;

public class User : IdentityUser
{
    public int? Uid { get; set; }
}