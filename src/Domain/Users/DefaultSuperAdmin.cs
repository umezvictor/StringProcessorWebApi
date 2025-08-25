using Microsoft.AspNetCore.Identity;
using Shared.Enums;
namespace Domain.Users
{
    public static class DefaultSuperAdmin
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            var defaultUser = new User
            {
                UserName = "victorblaze@gmail.com",
                Email = "victorblaze@gmail.com",
                PhoneNumber = "081786666666",
                FirstName = "Victor",
                LastName = "Umezuruike",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,


            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, UserRoleType.SuperAdmin.ToString());
                }
            }


            var defaultUser2 = new User
            {
                UserName = "victorblaze2010@gmail.com",
                Email = "victorblaze2010@gmail.com",
                PhoneNumber = "081786666666",
                FirstName = "Victor",
                LastName = "Umezuruike",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,


            };
            if (userManager.Users.All(u => u.Id != defaultUser2.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser2.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser2, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser2, UserRoleType.SuperAdmin.ToString());
                }
            }
        }
    }
}
