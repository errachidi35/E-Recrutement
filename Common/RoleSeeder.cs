using E_Recrutement.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Recrutement.Common
{
    public class RoleSeeder
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleSeeder(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync("ADMIN"))
            {
                var adminRole = new Role
                {
                    Name = "ADMIN",
                    Description = "Administrator role"
                };

                await _roleManager.CreateAsync(adminRole);
            }

            if (!await _roleManager.RoleExistsAsync("CANDIDATE"))
            {
                var userRole = new Role
                {
                    Name = "CANDIDATE",
                    Description = "Candidate role"
                };

                await _roleManager.CreateAsync(userRole);
            }

            if (!await _roleManager.RoleExistsAsync("RECRUITER"))
            {
                var userRole = new Role
                {
                    Name = "RECRUITER",
                    Description = "Recruiter role"
                };

                await _roleManager.CreateAsync(userRole);
            }

            // Add more roles as needed
        }
    }

}
