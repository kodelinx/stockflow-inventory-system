using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Transactions;
using StockFlow.Models;

namespace StockFlow.Services;

public class InventoryService
{
    public void ViewProducts(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("No products to view.");
            return;
        }

        Console.WriteLine("\n\b Product Inventory");
        Console.WriteLine("------------------------");

        foreach (Product product in products)
        {
            Console.WriteLine($"\nProduct ID: {product.ProductId}");
            Console.WriteLine($"Product Code: {product.ProductCode}");
            Console.WriteLine($"Name: {product.Name}");
            Console.WriteLine($"Category: {product.Category}");
            Console.WriteLine($"Unit Price: {product.UnitPrice}");
            Console.WriteLine($"Quantity in Stock: {product.QuantityInStock}");
            Console.WriteLine($"Reorder Level: {product.ReorderLevel}");
            Console.WriteLine($"Active:  {product.IsActive}\n");
            Console.WriteLine("------------------------"); 
        }
        Console.WriteLine("");
    }
    public void AddProduct(List<Product> products)
    {
        Console.WriteLine("Enter the Product Details\n");

        int productId = products.Count + 1;

        string productCode = $"PRD-{productId:000}";

        Console.Write("Name: ");
        string? name = Console.ReadLine() ?? "";

        Console.WriteLine("Category: ");
        string? category = Console.ReadLine() ?? "";

        Console.WriteLine("Unit Price: ");
        decimal unitPrice = Convert.ToDecimal(Console.ReadLine());

        Console.WriteLine("Quantity in Stock: ");
        int quantityInStock = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Reorder Level: ");
        int reorderLevel = Convert.ToInt32(Console.ReadLine());

        //create a Product object and initialize from data provided
        Product product = new Product(
            productId, 
            productCode, 
            name, 
            category, 
            unitPrice, 
            quantityInStock,
            reorderLevel,
            true);

        products.Add(product);

        Console.WriteLine($"Product {product.ProductCode} has been added successfully");
    }

    public void SearchProduct(List<Product> products)
    {
        if(products.Count == 0)
        {
            Console.WriteLine("There are no products available to search.");
            return;
        }
        Console.Write("Search Product Name  OR Code: ");
        string? searchInput = Console.ReadLine() ?? "";

        List<Product> matchingProducts = products.Where(product => product.IsActive  && 
            (
                product.Name.Contains(searchInput, StringComparison.OrdinalIgnoreCase) ||
                product.ProductCode.Contains(searchInput, StringComparison.OrdinalIgnoreCase)
            )
        ).ToList();

        if(matchingProducts == null)
        {
            Console.WriteLine("Product is not found.\n");
            return;
        }

        Console.WriteLine("\nProduct found");
        ViewProducts(matchingProducts);
    }
    public void UpdateProduct(List<Product> products)
    {
        if(products.Count == 0)
        {
            Console.WriteLine("There are no products available to update");
        }

        Console.Write("Enter Product Code to update: ");
        string productCode = Console.ReadLine() ?? "";

        Product? product = products.FirstOrDefault(product => product.IsActive &&(
            product.ProductCode.Contains(productCode, StringComparison.OrdinalIgnoreCase)
        ));

        if(product == null)
        {
            Console.WriteLine("The product is not found.");
            return;
        }
        
        DisplayProduct(product);

        while (true)
        {
            Console.WriteLine("\n(1) Name\n(2) Category\n(3) Unit Price\n(4) Quantity in Stock\n(5) Reorder Level\n");
            Console.Write("Enter the field you want to update: ");
            int field = Convert.ToInt32(Console.ReadLine());

            if(field == 1)
            {
                Console.Write("Enter NEW name: ");
                string newName = Console.ReadLine() ?? "";
                product?.Name = newName;
                break;
            }
            else if(field == 2)
            {
                Console.Write("Enter NEW category: ");
                string newCategory = Console.ReadLine() ?? "";
                product?.Category = newCategory;
                break;
            }
            else if(field == 3)
            {
                Console.Write("Enter NEW unit price: ");
                decimal newUnitPrice = Convert.ToDecimal(Console.ReadLine());
                product?.UnitPrice = newUnitPrice;
                break;
            }
            else if(field == 4)
            {
                Console.Write("Enter NEW quantity: ");
                int newQuantity = Convert.ToInt32(Console.ReadLine());
                product?.QuantityInStock = newQuantity;
                break;
            }
            else if(field == 5)
            {
                Console.Write("Enter NEW level: ");
                int newLevel = Convert.ToInt32(Console.ReadLine());
                product?.ReorderLevel = newLevel;
                break;
            }
            else
            {
                Console.WriteLine("Input is incorrect.");
            }
        }
        

    }

    //Soft deletion of product. It doesn't totally remove the product but disables it temporarily. 
    public void DeativateProduct(List<Product> products)
    {
        if(products.Count == 0)
        {
            Console.WriteLine("There are no products available to delete.\n");
            return;
        }

        Console.Write("Enter Code to deativate: ");
        string? codeInput = Console.ReadLine() ?? "";

        Product? product = products.FirstOrDefault(product => product.IsActive && (
            product.ProductCode.Contains(codeInput, StringComparison.OrdinalIgnoreCase)
        ));

        if(product == null)
        {
            Console.WriteLine("The product is not found.");
            return;
        }

        product.IsActive = false;

        Console.WriteLine($"Product {product.ProductCode} has been deativated successfully.");

    }
    public void DeleteProduct(List<Product> products)
    {
        if(products.Count == 0)
        {
            Console.WriteLine("There are no products available to delete.\n");
            return;
        }

        Console.Write("Enter the Code of the Product to Delete: ");
        string? codeInput = Console.ReadLine() ?? "";

        Product? product = products.FirstOrDefault(product => product.IsActive && (
            product.ProductCode.Contains(codeInput, StringComparison.OrdinalIgnoreCase)
        ));

        if(product == null)
        {
            Console.WriteLine("The product is not found.");
            return;
        }

        Console.WriteLine($"Product {product.ProductCode} has been deleted successfully.");

        products.Remove(product);
    }
    public void DisplayProduct(Product product)
    {
        Console.WriteLine($"\nProduct ID: {product.ProductId}");
        Console.WriteLine($"Product Code: {product.ProductCode}");
        Console.WriteLine($"Name: {product.Name}");
        Console.WriteLine($"Category: {product.Category}");
        Console.WriteLine($"Unit Price: {product.UnitPrice}");
        Console.WriteLine($"Quantity in Stock: {product.QuantityInStock}");
        Console.WriteLine($"Reorder Level: {product.ReorderLevel}");
        Console.WriteLine($"Active:  {product.IsActive}\n");
    }
}



