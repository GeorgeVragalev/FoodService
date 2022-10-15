using System.Collections.Concurrent;
using System.Linq;
using FoodService.Helpers;
using FoodService.Models;

namespace FoodService.Repositories.OrderListRepository;

public class OrderListRepository : IOrderListRepository
{
    private readonly ConcurrentBag< Order> _orderList = new ConcurrentBag<Order>();

    public void AddOrderToList(Order order)
    {
        _orderList.Add( order);
        PrintConsole.Write($"Order {order.Id} added to list", ConsoleColor.DarkBlue);
    }

    public IList<Order> GetUnservedOrders()
    {
        var orders = _orderList.AsQueryable().Where(o => o.OrderStatusEnum == OrderStatusEnum.Cooked).ToList();
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

    public async Task MarkOrderAs(Order order, OrderStatusEnum orderStatus)
    {
        var orderInList = _orderList.AsQueryable().FirstOrDefault(o => o.Id == order.Id);
        if (orderInList != null) 
            orderInList.OrderStatusEnum = orderStatus;
    }

    public Task<IList<Order>> CollectClientOrders(int clientId)
    {
        var clientsOrders = _orderList.AsQueryable()
            .Where(o => o.ClientId == clientId);
        
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