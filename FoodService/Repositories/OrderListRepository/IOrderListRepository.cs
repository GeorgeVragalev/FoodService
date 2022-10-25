
using FoodService.Models;
using FoodService.Models.Enum;

namespace FoodService.Repositories.OrderListRepository;

public interface IOrderListRepository
{
    public Task AddOrderToList(Order order);
    public IList<Order> GetUnservedOrders();
    public Task CleanServedOrders();
    public Task MarkOrderAs(Order order, OrderStatusEnum orderStatus);
    public Task<IList<Order>> CollectClientOrders(int? clientId);
}