using Interfaces.Interfaces;

namespace Interfaces.Models;

public class Person : IPerson
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string StreetAddress { get; set; } = null!;
}
