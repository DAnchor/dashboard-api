using Dashboard.Core.Models;

namespace Dashboard.TestHelpers;

public static class UserHelper
{
    public static UserModel GetUser()
    {
        return new UserModel
        {
            Id = "9f44cb79-821d-4b42-8550-4ea01569d6b6",
            FirstName = "Aoife",
            LastName = "McCarthy",
            UserName = "aoife_mccarthy",
            Email = "aoife_mccarthy@dashboard.com",
            Address = "Contae Bhaile Átha Cliath, Éireann",
            IsActive = true
        };
    }
}