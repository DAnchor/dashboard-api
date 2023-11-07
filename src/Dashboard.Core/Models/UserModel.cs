using Microsoft.AspNetCore.Identity;

namespace Dashboard.Core.Models;

public class UserModel : IdentityUser
{
    public bool IsActive { get; set; }
    public int Age { get; set; }
    public override string Email { get; set; }
    public override string UserName { get; set; }
    public string Address { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public UserModel() { }

    public UserModel(string address, int age, string email, string userName, string firstName, string lastName)
    {
        Address = address;
        Age = age;
        Email = email ?? throw new ArgumentNullException(nameof(email));
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
    }
}
