using StockFlow.Models;
using StockFlow.Repositories;

namespace StockFlow.Services;

public class OrderService
{
    private readonly ProductRepository _productRepository;
    private readonly OrderRepository _orderRepository;
    private readonly OrderItemRepository _orderItemRepository;
    private readonly StockMovementRepository _stockMovementRepository;
    private readonly StockMovementService _stockMovementService;

    public OrderService(
        ProductRepository productRepository,
        OrderRepository orderRepository,
        OrderItemRepository orderItemRepository,
        StockMovementRepository stockMovementRepository,
        StockMovementService stockMovementService
        
    )
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _stockMovementRepository = stockMovementRepository;
        _stockMovementService  = stockMovementService;
    }

    public void CheckoutBasket(List<BasketItem> basketItems)
    {
        List<Product> products = _productRepository.GetActiveProducts();

        if (basketItems.Count == 0)
        {
            Console.WriteLine("The basket is empty.");
            return;
        }

        // Checks if each basket item is still available and has enough stock.
        foreach (BasketItem basketItem in basketItems)
        {
            Product? product = products.FirstOrDefault(product =>
                product.IsActive &&
                product.ProductId == basketItem.ProductId
            );

            if (product == null)
            {
                Console.WriteLine($"Cannot checkout. {basketItem.ProductName} is no longer available.");
                return;
            }

            if (basketItem.Quantity > product.QuantityInStock)
            {
                Console.WriteLine($"Cannot checkout. Not enough stock for {product.Name}.");
                return;
            }
        }

        List<OrderItem> orderItems = new List<OrderItem>();

        // Generates the next business-facing order number.
        string orderNumber = GenerateNextOrderNumber();

        foreach (BasketItem basketItem in basketItems)
        {
            OrderItem orderItem = new OrderItem(

                basketItem.ProductId,
                basketItem.ProductCode,
                basketItem.ProductName,
                basketItem.Quantity,
                basketItem.UnitPrice
            );

            orderItems.Add(orderItem);
        }

        // Calculates the total amount from all order items.
        decimal totalAmount = CalculateOrderTotal(orderItems);

        // Creates the order using an object initializer.
        Order order = new Order
        {
            OrderNumber = orderNumber,
            OrderDate = DateTime.Now,
            Items = orderItems,
            TotalAmount = totalAmount,
            OrderStatus = "Pending Payment",
            PaymentStatus = "Unpaid"
        };

        // Saves the order summary to SQLite.
        _orderRepository.AddOrder(order);

        // Gets the saved order so we can use the SQLite-generated OrderId.
        Order? savedOrder = _orderRepository.FindOrderByNumber(order.OrderNumber);

        if (savedOrder == null)
        {
            Console.WriteLine("Order was not saved properly.");
            return;
        }

        // Saves each order item using the saved OrderId.
        foreach (OrderItem orderItem in orderItems)
        {
            orderItem.OrderId = savedOrder.OrderId;

            _orderItemRepository.AddOrderItem(
                savedOrder.OrderId,
                orderItem
            );
        }

        // Updates product stock and records stock-out movement.
        foreach (BasketItem basketItem in basketItems)
        {
            Product? product = products.FirstOrDefault(product =>
                product.ProductId == basketItem.ProductId
            );

            if (product != null)
            {
                int stockBefore = product.QuantityInStock;

                product.QuantityInStock -= basketItem.Quantity;

                int stockAfter = product.QuantityInStock;

                // Saves the updated product stock to SQLite after checkout deduction.
                _productRepository.UpdateProduct(product);

                _stockMovementService.RecordSaleStockOut(
                    product,
                    basketItem.Quantity,
                    stockBefore,
                    stockAfter,
                    order.OrderNumber
                );
            }
        }

        basketItems.Clear();

        Console.WriteLine($"Order {order.OrderNumber} created successfully.");
        Console.WriteLine($"Total Amount: {order.TotalAmount:C}");
        Console.WriteLine($"Status: {order.OrderStatus}\n");
    }

    public void ViewOrders()
    {
        // Reloads latest orders from SQLite.
        List<Order> orders = _orderRepository.GetAllOrders();

        if (orders.Count == 0)
        {
            Console.WriteLine("There are no orders available.");
            return;
        }

        Console.WriteLine("\nORDERS");
        Console.WriteLine("------");

        foreach (Order order in orders)
        {
            order.Items = _orderItemRepository.GetOrderItemsByOrderId(order.OrderId);
            DisplayOrder(order);
        }
    }

    public decimal CalculateOrderTotal(List<OrderItem> orderItems)
    {
        decimal total = 0;

        foreach (OrderItem orderItem in orderItems)
        {
            total += orderItem.LineTotal;
        }

        return total;
    }

    public void DisplayOrder(Order order)
    {
        Console.WriteLine($"Order ID: {order.OrderId}");
        Console.WriteLine($"Order Number: {order.OrderNumber}");
        Console.WriteLine($"Order Date: {order.OrderDate}");
        Console.WriteLine($"Order Status: {order.OrderStatus}");
        Console.WriteLine($"Payment Status: {order.PaymentStatus}");
        Console.WriteLine("Items:");

        foreach (OrderItem item in order.Items)
        {
            Console.WriteLine($"- {item.ProductName} @ {item.Quantity} x {item.UnitPrice:C} = {item.LineTotal:C}");
        }

        Console.WriteLine($"Total Amount: {order.TotalAmount:C}");
        Console.WriteLine("------");
    }

    private string GenerateNextOrderNumber()
    {
        // Reads saved orders from SQLite.
        List<Order> savedOrders = _orderRepository.GetAllOrders();

        if (savedOrders.Count == 0)
        {
            return "ORD-001";
        }

        // Uses the highest existing OrderId to create the next order number.
        int nextOrderNumber = savedOrders.Max(order => order.OrderId) + 1;

        return $"ORD-{nextOrderNumber:D3}";
    }
}                       