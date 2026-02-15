using Microsoft.AspNetCore.Identity;

namespace Umbra.School.Data;

public class ApplicationRole : IdentityRole
{
    public bool Enabled { get; set; } = true;
}