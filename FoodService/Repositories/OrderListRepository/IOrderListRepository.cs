
using FoodService.Models;

namespace FoodService.Repositories.OrderListRepository;

public interface IOrderListRepository
{
    public void AddOrderToList(Order order);
    public IList<Order> GetUnservedOrders();
    public Task CleanServedOrders();
    public Task MarkOrderAs(Order order, OrderStatusEnum orderStatus);
    public Task<IList<Order>> CollectClientOrders(int clientId);
}