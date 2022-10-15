using FoodService.Models;

namespace FoodService.Services.OrderService;

public interface IOrderService
{
    public Task SendOrder(GroupOrder order);
    public Task AddOrdersToList(GroupOrder groupOrder);
    public Task ServePreparedOrders(GroupOrder order);
    public Task MarkOrderAs(Order order, OrderStatusEnum orderStatus);
    public Task<IList<Order>?> CollectClientOrders(int clientId);
}