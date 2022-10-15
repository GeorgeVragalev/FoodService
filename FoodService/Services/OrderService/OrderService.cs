using System.Text;
using FoodService.Helpers;
using FoodService.Models;
using FoodService.Repositories.OrderListRepository;
using FoodService.Services.RestaurantService;
using Newtonsoft.Json;

namespace FoodService.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IRestaurantService _restaurantService;
    private readonly IOrderListRepository _orderListRepository;

    public OrderService(IRestaurantService restaurantService, IOrderListRepository orderListRepository)
    {
        _restaurantService = restaurantService;
        _orderListRepository = orderListRepository;
    }

    public async Task SendOrder(GroupOrder order)
    {
        try
        {
            var json = JsonConvert.SerializeObject(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // var url = Settings.Settings.GlovoUrl;
            using var client = new HttpClient();

            // await client.PostAsync(url, data);
            PrintConsole.Write($"Order {order.Id} with {order.Orders.Count} orders sent to glovo", ConsoleColor.Green);
        }
        catch (Exception e)
        {
            PrintConsole.Write(Thread.CurrentThread.Name + " Failed to send order id: " + order.Id,
                ConsoleColor.DarkRed);
        }
    }

    public async Task AddOrdersToList(GroupOrder groupOrder)
    {
        foreach (var order in groupOrder.Orders)
        {
            _orderListRepository.AddOrderToList(order);
        }
    }
    
    public async Task ServePreparedOrders(GroupOrder order)
    {
        try
        {
            var json = JsonConvert.SerializeObject(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = Settings.Settings.ClientUrl+"/client";
            using var client = new HttpClient();

            await client.PostAsync(url, data);
            PrintConsole.Write($"Order {order.Id} with {order.Orders.Count} orders sent to client", ConsoleColor.Green);
        }
        catch (Exception e)
        {
            PrintConsole.Write(Thread.CurrentThread.Name + " Failed to send order id: " + order.Id,
                ConsoleColor.DarkRed);
        }
    }

    public async Task MarkOrderAs(Order order, OrderStatusEnum orderStatus)
    {
        await _orderListRepository.MarkOrderAs(order, orderStatus);
    }

    public async Task<IList<Order>?> CollectClientOrders(int clientId)
    {
        var clientsOrders = await _orderListRepository.CollectClientOrders(clientId);

        if (clientsOrders == null)
        {
            return null;
        }

        return clientsOrders;
    }
}