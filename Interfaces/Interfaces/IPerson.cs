namespace Interfaces.Interfaces;

public interface IPerson
{
    Guid Id { get; set;  }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Email { get; set; }
    string StreetAddress { get; set; }
}
