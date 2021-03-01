using DutchTree.Data.Entities;
using System.Collections.Generic;

namespace DutchTree.Data
{
    public interface IDutchRepository
    {
        IEnumerable<Order> GetAllOrders(bool includeItems = true);
        IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems = true);
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        Order GetOrderById(int id);
        Order GetOrderByIdAndUsername(int id, string username);
        void AddEntity(object model);
        bool SaveAll();
        void AddOrder(Order order);
    }
}