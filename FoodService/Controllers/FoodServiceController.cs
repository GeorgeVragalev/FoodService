using FoodService.FoodService;
using FoodService.Models;
using FoodService.Models.Enum;
using FoodService.Services.OrderService;
using FoodService.Services.RestaurantService;
using Microsoft.AspNetCore.Mvc;

namespace FoodService.Controllers;

[ApiController]
[Route("/glovo")]
public class FoodServiceController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IRestaurantService _restaurantService;
    private readonly IGlovo _glovo;
    private static Mutex _mutex = new();

    public FoodServiceController(IOrderService orderService, IGlovo glovo, IRestaurantService restaurantService)
    {
        _orderService = orderService;
        _glovo = glovo;
        _restaurantService = restaurantService;
    }

    [HttpPost("/order")]
    public async Task TakeOrderFromClient([FromBody] GroupOrder order)
    {
        Console.WriteLine($"Group Order {order.Id} received with {order.Orders.Count} orders from client {order.ClientId}");
        await _orderService.AddOrdersToList(order);
        
        await _glovo.DistributeOrderToRestaurants(order);
    }
    
     [HttpPost("/serve")]
     public async Task ReceivePreparedOrder([FromBody] Order order)
     {
         Console.WriteLine($"Restaurant prepared foods from order {order.Id} from group order: {order.GroupOrderId}");
         
         //check if order is complete
         await _orderService.MarkOrderAs(order, OrderStatusEnum.Cooked);

         if (order.ClientId != null)
         {
             var groupOrder = await _orderService.CollectClientOrders(order.ClientId.Value);

             if (groupOrder!= null)
             {
                 await _orderService.ServePreparedOrders(groupOrder);
             }
         }
     }
    
    [HttpPost("/register")]
    public async Task RegisterRestaurant([FromBody] RestaurantData restaurantData)
    {
        await _restaurantService.RegisterRestaurant(restaurantData);
        Console.WriteLine($"Restaurant {restaurantData.RestaurantName} registered");
    }
    
    [HttpPost("/rating")]
    public async Task SubmitRating([FromBody] OrderRating orderRating)
    {
        var resturantUrl = await _restaurantService.GetRestaurantUrlById(orderRating.Order.RestaurantId);
        await _restaurantService.SubmitRating(orderRating, $"{resturantUrl}/rating");
    }
    
    [HttpGet("/menu")]
    public async Task<IList<RestaurantData>?> GetRestaurantData()
    {
        return await _restaurantService.GetRestaurantsData();
    }
}