using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DutchTree.Data;
using DutchTree.Data.Entities;
using DutchTree.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTree.Controllers
{
    [Route("/api/orders/{orderId}/items")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemsController : Controller
    {
        private readonly IDutchRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<OrderItemsController> logger;

        public OrderItemsController(IDutchRepository repository, IMapper mapper, ILogger<OrderItemsController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            try
            {
                var username = User.Identity?.Name;

                var order = repository.GetOrderByIdAndUsername(orderId, username);

                if (order != null) return Ok(mapper.Map<ICollection<OrderItem>, ICollection<OrderItemViewModel>>(order.Items));

                return NotFound();
            }
            catch (Exception e)
            {
                logger.LogError($"Failed to get order: {e}");
                return BadRequest("Failed to get order");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int orderId, int id)
        {
            try
            {
                var username = User.Identity?.Name;

                var order = repository.GetOrderByIdAndUsername(orderId, username);

                if (order == null) return NotFound();

                var orderItem = order.Items.FirstOrDefault(_ => _.Id == id);

                if(orderItem == null) return NotFound();

                return Ok(mapper.Map<OrderItem, OrderItemViewModel>(orderItem));
            }
            catch (Exception e)
            {
                logger.LogError($"Failed to get order: {e}");
                return BadRequest("Failed to get order");
            }
        }
    }
}
