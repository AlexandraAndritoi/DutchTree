using System;
using DutchTree.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DutchTree.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<StoreUser> userManager;

        public DutchSeeder(DutchContext context, IWebHostEnvironment webHostEnvironment, UserManager<StoreUser> userManager)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
        }

        public async Task Seed()
        {
            await context.Database.EnsureCreatedAsync();

            var user = await userManager.FindByEmailAsync("alexandra@dutchtree.com");

            if (user == null)
            {
                user = new StoreUser
                {
                    FirstName = "Alexandra",
                    LastName = "Andritoi",
                    Email = "alexandra@dutchtree.com",
                    UserName = "alexandra@dutchtree.com",
                };

                var result = await userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
            }

            if (context.Products.Any()) return;
            
            // Create sample data

            var filePath = Path.Combine(webHostEnvironment.ContentRootPath, "Data/art.json");
            var json = await File.ReadAllTextAsync(filePath);
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json).ToList();
            await context.Products.AddRangeAsync(products);

            var order = context.Orders.FirstOrDefault(_ => _.Id == 1);
            if(order != null)
            {
                order.User = user;
                order.Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Product = products.First(),
                        Quantity = 5,
                        UnitPrice = products.First().Price,
                    }
                };
            }

            await context.SaveChangesAsync();
        }
    }
}
