using Identity.Models;
using LegoBrickwell.Models;
using Microsoft.AspNetCore.Identity;

namespace LegoBrickwell.IdentityPolicy
{
    public class CustomUsernameEmailPolicy : UserValidator<AppUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            IdentityResult result = await base.ValidateAsync(manager, user);
            List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

            if (user.UserName == "google")
            {
                errors.Add(new IdentityError
                {
                    Description = "Google cannot be used as a user name"
                });
            }

            if (!user.Email.ToLower().EndsWith("@gmail.com") && !user.Email.ToLower().EndsWith("@yahoo.com") && !user.Email.ToLower().EndsWith("@outlook.com") && !user.Email.ToLower().EndsWith("@hotmail.com"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Only gmail.com, yahoo.com, outlook.com, hotmail.com are allowed"
                });
            }
            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}