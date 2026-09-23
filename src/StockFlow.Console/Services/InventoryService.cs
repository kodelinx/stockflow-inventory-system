using StockFlow.Models;
using StockFlow.Utilities;
using StockFlow.Repositories;

namespace StockFlow.Services;


public class InventoryService
{
    private readonly InputValidationService _inputValidationService;
    private readonly ProductManager _productManager;
    private readonly ProductRepository _productRepository;

    public InventoryService(
        InputValidationService inputValidationService,
        ProductManager productManager,
        ProductRepository productRepository
    )
    {
        _inputValidationService = inputValidationService;
        _productManager = productManager;
        _productRepository = productRepository;
    }
    public void ViewProducts()
    {
        List<Product> products = _productRepository.GetAllProducts();
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
            Console.WriteLine($"Unit Price: {product.UnitPrice:F2}");
            Console.WriteLine($"Quantity in Stock: {product.QuantityInStock}");
            Console.WriteLine($"Reorder Level: {product.ReorderLevel}");
            Console.WriteLine($"Active:  {product.IsActive}\n");
            Console.WriteLine("------------------------"); 
        }
        Console.WriteLine("");
    }
    public void AddProduct()
    {
        Console.WriteLine("Enter the Product Details\n");

        string productCode = GenerateNextProductCode();
        string name = _inputValidationService.GetRequiredText("Name: ");
        string category = _inputValidationService.GetRequiredText("Category: ");
        decimal unitPrice = _inputValidationService.GetValidDecimal("Unit Price: ", 0.00m, 1000000m);
        int quantityInStock = _inputValidationService.GetValidInt("Quantity in Stock: ", 0, 1000000);
        int reorderLevel = _inputValidationService.GetValidInt("Reorder Level: ", 0, 1000000);


        //create a Product object and initialize from data provided
        Product product = _productManager.CreateProduct(
            //productId,
            productCode, 
            name, 
            category, 
            unitPrice, 
            quantityInStock,
            reorderLevel
            );
            
        //product.ProductId = nextProductId;

        //products.Add(product);
        _productRepository.AddProduct(product);

        Console.WriteLine($"Product {product.ProductCode} has been added successfully");
    }
    private string GenerateNextProductCode()
    {
        List<Product> products = _productRepository.GetAllProducts();

        if (products.Count == 0)
        {
            return "PRD-001";
        }

        int nextProductNumber =
            products.Max(product => product.ProductId) + 1;

        return $"PRD-{nextProductNumber:D3}";
    }
    public void SearchProduct()
    {
        List<Product> products = _productRepository.GetActiveProducts();
        if(products.Count == 0)
        {
            Console.WriteLine("There are no products available to search.");
            return;
        }

        string searchInput = _inputValidationService.GetRequiredText("Search Product Name OR Code: ");

        List<Product> matchingProducts = products.Where(product => product.IsActive  && 
            (
                product.Name.Contains(searchInput, StringComparison.OrdinalIgnoreCase) ||
                product.ProductCode.Contains(searchInput, StringComparison.OrdinalIgnoreCase)
            )
        ).ToList();

        if(matchingProducts.Count == 0)
        {
            Console.WriteLine("Product is not found.\n");
            return;
        }

        Console.WriteLine("\nProduct found");

        foreach (Product matchingProduct in matchingProducts)
        {
            DisplayProduct(matchingProduct);
        }
    }
    public void UpdateProduct()
    {
        /*List<Product> products = _productRepository.GetActiveProducts();
        if(products.Count == 0)
        {
            Console.WriteLine("There are no products available to update");
        }*/

        string productCode = _inputValidationService.GetRequiredText("Enter Product Code to update: ");

        Product? product = _productRepository.FindProductByCode(productCode);

        /*Product? product = products.FirstOrDefault(product => product.IsActive &&(
            product.ProductCode.Contains(productCode, StringComparison.OrdinalIgnoreCase)
        ));*/

        if(product == null)
        {
            Console.WriteLine("The product is not found.");
            return;
        }
        
        DisplayProduct(product);

        while (true)
        {
            Console.WriteLine("\n(1) Name\n(2) Category\n(3) Unit Price\n(4) Quantity in Stock\n(5) Reorder Level\n");
            int field = _inputValidationService.GetValidInt("Enter the field you want to update: ", 1, 5);

            if(field == 1)
            {
                string newName = _inputValidationService.GetRequiredText("Enter NEW name: ");
                product.Name = newName;
                break;
            }
            else if(field == 2)
            {
                string newCategory = _inputValidationService.GetRequiredText("Enter NEW category: ");
                product.Category = newCategory;
                break;
            }
            else if(field == 3)
            {
                decimal newUnitPrice = _inputValidationService.GetValidDecimal("Enter NEW unit price: ", 0, 1000000);
                product.UnitPrice = newUnitPrice;
                break;
            }
            else if(field == 4)
            {
                int newQuantity = _inputValidationService.GetValidInt("Enter NEW quantity: ", 0, 1000000);
                product.QuantityInStock = newQuantity;
                break;
            }
            else if(field == 5)
            {
                int newLevel = _inputValidationService.GetValidInt("Enter NEW reorder level: ", 0, 1000000);
                product.ReorderLevel = newLevel;
                break;
            }
            else
            {
                Console.WriteLine("Input is incorrect.");
            }
        }
        
        // Saves the updated product to SQLite.
        _productRepository.UpdateProduct(product);

        Console.WriteLine("Product updated successfully.\n");
    }

    //Soft deletion of product. It doesn't totally remove the product but disables it temporarily. 
    public void DeactivateProduct()
    {
        List<Product> products = _productRepository.GetActiveProducts();
        if(products.Count == 0)
        {
            Console.WriteLine("There are no products available to rectivate.\n");
            return;
        }

        string productCode = _inputValidationService.GetRequiredText("Enter the Product Code to deactivate: ");

        Product? product = _productRepository.FindProductByCode(productCode);

        if(product == null)
        {
            Console.WriteLine("The product is not found.");
            return;
        }

        _productRepository.DeactivateProduct(productCode);

        Console.WriteLine($"Product {product.ProductCode} has been deativated successfully.");
    }

    public void ReactivateProduct()
    {
        List<Product> products = _productRepository.GetAllProducts();

        if(products.Count == 0)
        {
            Console.WriteLine("There are no products available to reactivate");
            return;
        }

        string productCode = _inputValidationService.GetRequiredText("Enter product code to reactivate: ");

        Product? product = _productRepository.FindProductByCode(productCode);

        if (product == null)
        {
            Console.WriteLine("The product is is not existing in the inventory.");
            return;
        }
        
        if (product.IsActive)
        {
            Console.WriteLine("The product is alreaddy active.");
            return;
        }

        _productRepository.ReactivateProduct(productCode);

        Console.WriteLine($"{product.Name} has been reactivated successfully.\n");

    }
    public void DeleteProduct()
    {
        List<Product> products = _productRepository.GetAllProducts();

        if(products.Count == 0)
        {
            Console.WriteLine("There are no products available to delete.\n");
            return;
        }

        Console.Write("Enter the Code of the Product to Delete: ");
        string? productCode = Console.ReadLine() ?? "";

        Product? product = _productRepository.FindProductByCode(productCode);

        if(product == null)
        {
            Console.WriteLine("The product is not found.");
            return;
        }

        _productRepository.DeleteProduct(productCode);

        products.Clear();
        products.AddRange(_productRepository.GetActiveProducts());

        Console.WriteLine($"Product {product.ProductCode} has been deleted successfully.");
    }
    public void DisplayProduct(Product product)
    {
        Console.WriteLine($"\nProduct ID: {product.ProductId}");
        Console.WriteLine($"Product Code: {product.ProductCode}");
        Console.WriteLine($"Name: {product.Name}");
        Console.WriteLine($"Category: {product.Category}");
        Console.WriteLine($"Unit Price: {product.UnitPrice:F2}");
        Console.WriteLine($"Quantity in Stock: {product.QuantityInStock}");
        Console.WriteLine($"Reorder Level: {product.ReorderLevel}");
        Console.WriteLine($"Active:  {product.IsActive}\n");
    }
}



