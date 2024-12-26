using E_Recrutement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;

namespace E_Recrutement.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
               .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // Seed Sectors
            modelBuilder.Entity<Sector>().HasData(
                new Sector { Id = 1, Name = "Sector 1", Slug = "sector-1", Disable = false },
                new Sector { Id = 2, Name = "Sector 2", Slug = "sector-2", Disable = false }
                // Add more sectors as needed
            );

            // Seed Provinces
            modelBuilder.Entity<Province>().HasData(
                new Province { Id = 1, Name = "Province 1", Slug = "province-1", SectorId = 1, Disable = false, Popular = 0 },
                new Province { Id = 2, Name = "Province 2", Slug = "province-2", SectorId = 2, Disable = false, Popular = 0 }
                // Add more provinces as needed
            );

            // Seed ContractTypes
            modelBuilder.Entity<ContractType>().HasData(
                new ContractType { Id = 1, Name = "Contract Type 1", Slug = "contract-type-1", Disable = false },
                new ContractType { Id = 2, Name = "Contract Type 2", Slug = "contract-type-2", Disable = false }
                // Add more contract types as needed
            );

            // Seed Profils
            modelBuilder.Entity<Profil>().HasData(
                new Profil { Id = 1, Name = "Profil 1", Slug = "profil-1", SectorId = 1, Disable = false, Popular = 0 },
                new Profil { Id = 2, Name = "Profil 2", Slug = "profil-2", SectorId = 2, Disable = false, Popular = 0 }
                // Add more profils as needed
            );


            // Seed Offer data for Morocco
            //modelBuilder.Entity<Offer>().HasData(
            //  new Offer { Id = 1, UserId = Guid.NewGuid(), Name = "Software Engineer", SectorId = 1, Description = "Job description for Software Engineer position.", CreateDate = DateTime.Now, ProvinceId = 6, MinSalary = 5000, MaxSalary = 8000, ContractType = ContractType.CDI },
            //new Offer { Id = 2, UserId = Guid.NewGuid(), Name = "Marketing Manager", SectorId = 2, Description = "Job description for Marketing Manager position.", CreateDate = DateTime.Now, ProvinceId = 6, MinSalary = 6000, MaxSalary = 9000, ContractType = ContractType.CDI }
            // Add more offer data as needed
            //);
            // Seed Offer data

        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> AppUsers { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<CV> CVs { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Profil> Profils { get; set; }  
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<Skill> Skills { get; set; }

    }
}
    

