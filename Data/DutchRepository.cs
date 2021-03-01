using DutchTree.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DutchTree.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext context;
        private readonly ILogger<DutchRepository> logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems = true)
        {
            if (includeItems)
            {
                return context.Orders
                    .Include(_ => _.Items)
                    .ThenInclude(_ => _.Product)
                    .ToList();
            }
            return context.Orders.ToList();
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems = true)
        {
            if (includeItems)
            {
                return context.Orders
                    .Where(_ => _.User.UserName == username)
                    .Include(_ => _.Items)
                    .ThenInclude(_ => _.Product)
                    .ToList();
            }
            return context.Orders.Where(_ => _.User.UserName == username).ToList();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                logger.LogInformation("GetAllProducts was called");

                return context.Products.OrderBy(_ => _.Title).ToList();
            }
            catch(Exception e)
            {
                logger.LogError($"Failed to get all products: {e}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return context.Products.Where(_ => _.Category == category).ToList();
        }

        public Order GetOrderById(int id)
        {
            return context.Orders
                .Include(_ => _.Items)
                .ThenInclude(_ => _.Product)
                .FirstOrDefault(_ => _.Id == id);
        }

        public Order GetOrderByIdAndUsername(int id, string username)
        {
            return context.Orders
                .Include(_ => _.Items)
                .ThenInclude(_ => _.Product)
                .FirstOrDefault(_ => _.Id == id && _.User.UserName == username);
        }

        public void AddEntity(object model)
        {
            context.Add(model);
        }

        public bool SaveAll()
        {
            return context.SaveChanges() > 0;
        }

        public void AddOrder(Order order)
        {
            foreach(var item in order.Items)
            {
                item.Product = context.Products.Find(item.Product.Id);
            }
            AddEntity(order);
        }
    }
}
