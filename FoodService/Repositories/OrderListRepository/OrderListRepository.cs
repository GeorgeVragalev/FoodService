using System.Collections.Concurrent;
using System.Linq;
using FoodService.Helpers;
using FoodService.Models;
using FoodService.Models.Enum;

namespace FoodService.Repositories.OrderListRepository;

public class OrderListRepository : IOrderListRepository
{
    private readonly ConcurrentBag< Order> _orderList = new ConcurrentBag<Order>();
    private static Mutex _mutex = new();

    public Task AddOrderToList(Order order)
    {
        _mutex.WaitOne();
        _orderList.Add( order);
        PrintConsole.Write($"Order {order.Id} added to list", ConsoleColor.DarkBlue);
        _mutex.ReleaseMutex();
        return Task.CompletedTask;
    }

    public IList<Order> GetUnservedOrders()
    {
        _mutex.WaitOne();
        var orders = _orderList.AsQueryable().Where(o => o.OrderStatusEnum == OrderStatusEnum.Cooked).ToList();
        _mutex.ReleaseMutex();
        return orders;
    }

    public Task CleanServedOrders()
    {
        var orders = _orderList.Where(o => o.OrderStatusEnum == OrderStatusEnum.Served).ToList();
        if (orders.Count != 0)
        {
            orders.Clear();
        }
        return Task.CompletedTask;
    }

    public Task MarkOrderAs(Order order, OrderStatusEnum orderStatus)
    {
        _mutex.WaitOne();
        var orderInList = _orderList.AsQueryable().FirstOrDefault(o => o.Id == order.Id);
        if (orderInList != null) 
            orderInList.OrderStatusEnum = orderStatus;
        _mutex.ReleaseMutex();
        return Task.CompletedTask;
    }

    public Task<IList<Order>> CollectClientOrders(int? clientId)
    {
        _mutex.WaitOne();
        var clientsOrders = _orderList.AsQueryable()
            .Where(o => o.ClientId == clientId);
        _mutex.ReleaseMutex();
        foreach (var order in clientsOrders)
        {
            if (order.OrderStatusEnum != OrderStatusEnum.Cooked)
            {
                return Task.FromResult<IList<Order>>(null);
            }
        }
        
        return Task.FromResult<IList<Order>>(clientsOrders.ToList());
    }
}