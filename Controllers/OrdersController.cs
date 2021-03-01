using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DutchTree.Data;
using DutchTree.Data.Entities;
using DutchTree.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DutchTree.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository repository;
        private readonly IMapper mapper;
        private readonly UserManager<StoreUser> userManager;
        private readonly ILogger<OrdersController> logger;

        public OrdersController(
            IDutchRepository repository,
            IMapper mapper,
            UserManager<StoreUser> userManager,
            ILogger<OrdersController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var username = User.Identity?.Name;

                var orders = repository.GetAllOrdersByUser(username, includeItems);

                return Ok(mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(orders));
            }
            catch (Exception e)
            {
                logger.LogError($"Failed to get orders: {e}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var username = User.Identity?.Name;

                var order = repository.GetOrderByIdAndUsername(id, username);

                if (order != null) return Ok(mapper.Map<Order, OrderViewModel>(order));

                return NotFound();
            }
            catch (Exception e)
            {
                logger.LogError($"Failed to get order: {e}");
                return BadRequest("Failed to get order");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var order = mapper.Map<OrderViewModel, Order>(model);

                if (order.OrderDate == DateTime.MinValue)
                {
                    order.OrderDate = DateTime.UtcNow;
                }

                var user = await userManager.FindByNameAsync(User.Identity?.Name);
                order.User = user;

                repository.AddOrder(order);
                if (repository.SaveAll())
                {
                    return Created($"/api/orders/{order.Id}", mapper.Map<Order, OrderViewModel>(order));
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Failed to save order: {e}");
            }
            return BadRequest("Failed to save order");
        }
    }
}