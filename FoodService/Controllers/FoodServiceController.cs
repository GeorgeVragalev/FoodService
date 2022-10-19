using FoodService.FoodService;
using FoodService.Models;
using FoodService.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace FoodService.Controllers;

[ApiController]
[Route("/glovo")]
public class FoodServiceController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IGlovo _glovo;
    private static Mutex _mutex = new();

    public FoodServiceController(IOrderService orderService, IGlovo glovo)
    {
        _orderService = orderService;
        _glovo = glovo;
    }

    [HttpPost("/order")]
    public async Task TakeOrderFromClient([FromBody] GroupOrder order)
    {
        Console.WriteLine($"Group Order {order.Id} received with {order.Orders.Count} orders from client {order.ClientId}");
        //send order to DiningHall
        //todo glovo send order to dedicated restaurant 
        await _orderService.AddOrdersToList(order);
        
        await _glovo.DistributeOrderToRestaurants(order);
        
        //prepare foods
        foreach (var o in order.Orders)
        {
            await _orderService.MarkOrderAs(o, OrderStatusEnum.Cooked);
        }

        var clientOrders = await _orderService.CollectClientOrders(order.Orders[0].ClientId);
        if (clientOrders != null)
        {
            _mutex.WaitOne();
            var preparedOrder = new GroupOrder()
            {
                Id = order.Id,
                Orders = clientOrders,
                ClientId = order.ClientId
            };
            _mutex.ReleaseMutex();
            
            await _orderService.ServePreparedOrders(preparedOrder);
            
            //Mark orders as served
            foreach (var o in clientOrders)
            {
                await _orderService.MarkOrderAs(o, OrderStatusEnum.Served);
            }
        }
    }
    
     // [HttpPost("/serve")]
     // public Task ReceivePreparedOrder([FromBody] Order order)
     // {
     //     Console.WriteLine($"Restaurant prepared foods from order  {order.Id} from group order: {order.GroupOrderId}");
     //     return Task.CompletedTask;
     //     //todo send order to client service check if group order is prepared or not
     // }
    
    [HttpPost("/register")]
    public Task RegisterRestaurant([FromBody] RestaurantData restaurantData)
    {
        //todo register restaurant in repository and store menus
        Console.WriteLine($"Restaurant {restaurantData.RestaurantName} registered");
        return Task.CompletedTask;
    }
    
    //rating send from client to restaurant
    [HttpPost("/rating")]
    public Task SubmitRating([FromBody] RestaurantData restaurantData)
    {
        //todo register restaurant in repository and store menus
        Console.WriteLine($"Restaurant {restaurantData.RestaurantName} registered");
        return Task.CompletedTask;
    }
    
    [HttpGet]
    public ContentResult Get()
    {
        return Content("Hi");
    }
}