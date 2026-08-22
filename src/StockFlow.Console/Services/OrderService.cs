using System.Numerics;
using StockFlow.Models;


namespace StockFlow.Service;

public class OrderService
{
    public void CheckoutBasket(
        List<Product> products,
        List<BasketItem> basketItems, 
        List<Order> orders)
    {   
        if (basketItems.Count == 0)
        {
            Console.WriteLine("The basket is empyt.");
            return;
        }
        //Check availability of product and stock quantity.
        foreach (BasketItem basketItem in basketItems)
        {
            Product? product = products.FirstOrDefault(product =>
            product.IsActive &&
            product.ProductId == basketItem.ProductId);

            if (product == null)
            {
                Console.WriteLine($"Cannot checkout. The {basketItem.ProductName} is no longer available.");
                return;
            }

            if (basketItem.Quantity > product.QuantityInStock)
            {
                Console.WriteLine($"Cannot checkout. Not enough stock for {product.Name}");
                return;
            }
        }

        int nextOrderId = orders.Count + 1;
        string orderNumber = $"ORD-{nextOrderId:000}";

        List<OrderItem> orderItems = new List<OrderItem>();

        foreach(BasketItem basketItem in basketItems)
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
        decimal totalAmount = CalculateOrderTotal(orderItems);

        Order order = new Order(
            nextOrderId,
            orderNumber,
            DateTime.Now,
            orderItems,
            totalAmount,
            "Pending Payment",
            "Unpaid"
        );

        foreach (BasketItem basketItem in basketItems)
        {
            Product? product = products.FirstOrDefault(product =>
            product.ProductId == basketItem.ProductId);

            if(product != null)
            {
                product.QuantityInStock -= basketItem.Quantity;
            }
        }

        orders.Add(order);
        basketItems.Clear();

        Console.WriteLine($"Order {order.OrderNumber} created successfully.");
        Console.WriteLine($"Total Amount: {order.TotalAmount:C}");
        Console.WriteLine($"Status: {order.OrderStatus}\n");

    }
    public void ViewOrders(List<Order> orders)
    {
        if(orders.Count == 0)
        {
            Console.WriteLine("There are no Order Available.");
            return;
        }

        Console.WriteLine("\nORDERS");
        Console.WriteLine("------");

        foreach(Order order in orders)
        {
            DisplayOrder(order);
        }
    }
    public decimal CalculateOrderTotal(List<OrderItem> orderItems)
    {
        decimal total = 0;

        foreach(OrderItem orderItem in orderItems)
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
        Console.WriteLine("Items:");

        foreach (OrderItem item in order.Items)
        {
            Console.WriteLine($"- {item.ProductName} @ {item.Quantity} x {item.UnitPrice:C} = {item.LineTotal:C}");
        }

        Console.WriteLine($"Total Amount: {order.TotalAmount:C}");
        Console.WriteLine("------");
    }
}