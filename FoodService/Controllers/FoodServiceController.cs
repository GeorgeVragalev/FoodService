using FoodService.Models;
using FoodService.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace FoodService.Controllers;

[ApiController]
[Route("/glovo")]
public class FoodServiceController : ControllerBase
{
    private readonly IOrderService _orderService;

    public FoodServiceController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("/order")]
    public async void TakeOrderFromClient([FromBody] GroupOrder order)
    {
        Console.WriteLine($"Group Order {order.Id} received with {order.Orders.Count} orders from client {order.ClientId}");
        //send order to DiningHall
        //todo glovo send order to dedicated restaurant 
        await _orderService.AddOrdersToList(order);
        
        //prepare foods
        foreach (var o in order.Orders)
        {
            await _orderService.MarkOrderAs(o, OrderStatusEnum.Cooked);
        }

        var clientOrders = await _orderService.CollectClientOrders(order.Orders[0].ClientId);
        if (clientOrders != null)
        {
            var preparedOrder = new GroupOrder()
            {
                Id = order.Id,
                Orders = clientOrders,
                ClientId = order.ClientId
            };
            await _orderService.ServePreparedOrders(preparedOrder);
            
            //Mark orders as served
            foreach (var o in clientOrders)
            {
                await _orderService.MarkOrderAs(o, OrderStatusEnum.Served);
            }
        }
    }
    
    // [HttpPost]
    // public void ReceivePreparedOrder([FromBody] Order order)
    // {
    //     Console.WriteLine("Order "+ order.Id+" received");
    //     //todo send order to client service 
            //check if group order is prepared or not
    // }
    
    [HttpPost("/register")]
    public void RegisterRestaurant([FromBody] RestaurantData restaurantData)
    {
        //todo register restaurant in repository and store menus
        Console.WriteLine($"Restaurant {restaurantData.RestaurantName} registered");
    }
    
    //rating send from client to restaurant
    [HttpPost("/rating")]
    public void SubmitRating([FromBody] RestaurantData restaurantData)
    {
        //todo register restaurant in repository and store menus
        Console.WriteLine($"Restaurant {restaurantData.RestaurantName} registered");
    }
    
    [HttpGet]
    public ContentResult Get()
    {
        return Content("Hi");
    }
}