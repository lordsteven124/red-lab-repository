using Microsoft.AspNetCore.Identity;
using ProductManagement.API.Models;

namespace ProductManagement.API.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create roles
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create default admin user with simple password
            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User"
                };

                var result = await userManager.CreateAsync(adminUser, "admin");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine("✅ Admin user created: admin@example.com / admin");
                }
                else
                {
                    // Log errors if user creation fails
                    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    foreach (var error in result.Errors)
                    {
                        logger.LogError(error.Description);
                    }
                }
            }

            // Seed sample products if none exist
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Nombre = "Laptop Dell XPS 13",
                        Descripcion = "Laptop ultrabook con procesador Intel i7",
                        Precio = 1299.99m,
                        Estado = true,
                        UsuarioCreacion = "system",
                        FechaCreacion = DateTime.UtcNow
                    },
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Nombre = "iPhone 15 Pro",
                        Descripcion = "Smartphone Apple con cámara profesional",
                        Precio = 999.99m,
                        Estado = true,
                        UsuarioCreacion = "system",
                        FechaCreacion = DateTime.UtcNow
                    },
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Nombre = "Samsung Galaxy S24",
                        Descripcion = "Teléfono Android con IA integrada",
                        Precio = 899.99m,
                        Estado = true,
                        UsuarioCreacion = "system",
                        FechaCreacion = DateTime.UtcNow
                    }
                };

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
                Console.WriteLine("✅ Sample products created");
            }
        }
    }
}