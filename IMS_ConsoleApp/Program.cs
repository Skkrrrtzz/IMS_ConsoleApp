namespace IMS_ConsoleApp
{
    // Product class to represent items in inventory
    public class Product(int id, string name, int quantity, decimal price)
    {
        public int ProductId { get; set; } = id;
        public string Name { get; set; } = name;
        public int QuantityInStock { get; set; } = quantity;
        public decimal Price { get; set; } = price;

        // Calculate total value for this product
        public decimal TotalValue => QuantityInStock * Price;
    }

    // Manages inventory operations
    public class InventoryManager
    {
        private List<Product> products = [];

        // Add a new product to inventory
        public bool AddProduct(Product product)
        {
            // Validate product ID is unique and positive
            if (product.ProductId <= 0 || products.Any(p => p.ProductId == product.ProductId))
            {
                return false;
            }

            // Validate price and quantity are non-negative
            if (product.Price < 0 || product.QuantityInStock < 0)
            {
                return false;
            }

            products.Add(product);
            return true;
        }

        // Remove a product by ID
        public bool RemoveProduct(int productId)
        {
            var product = products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                products.Remove(product);
                return true;
            }
            return false;
        }

        // Update product quantity
        public bool UpdateProduct(int productId, int newQuantity)
        {
            if (newQuantity < 0) return false;

            var product = products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                product.QuantityInStock = newQuantity;
                return true;
            }
            return false;
        }

        // List all products in inventory
        public void ListProducts()
        {
            if (products.Count == 0)
            {
                Console.WriteLine("Inventory is empty.");
                return;
            }

            Console.WriteLine("\nCurrent Inventory:");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("ID\tName\t\tQuantity\tPrice\t\tTotal Value");
            Console.WriteLine("-----------------------------------------------------------");

            foreach (var product in products.OrderBy(p => p.ProductId))
            {
                Console.WriteLine($"{product.ProductId}\t{product.Name}\t\t{product.QuantityInStock}\t\t{product.Price:C}\t\t{product.TotalValue:C}");
            }
        }

        // Calculate total value of all inventory
        public decimal GetTotalValue()
        {
            return products.Sum(p => p.TotalValue);
        }
    }

    // User interface for the inventory system
    internal class Program
    {
        private static InventoryManager inventory = new();

        private static void Main(string[] args)
        {
            Console.WriteLine("Retail Store Inventory Management System");
            Console.WriteLine("----------------------------------------");

            while (true)
            {
                DisplayMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProductUI();
                        break;

                    case "2":
                        RemoveProductUI();
                        break;

                    case "3":
                        UpdateProductUI();
                        break;

                    case "4":
                        inventory.ListProducts();
                        break;

                    case "5":
                        Console.WriteLine($"\nTotal Inventory Value: {inventory.GetTotalValue():C}");
                        break;

                    case "6":
                        Console.WriteLine("Exiting the system...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("\nMain Menu");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. Remove Product");
            Console.WriteLine("3. Update Product Quantity");
            Console.WriteLine("4. List All Products");
            Console.WriteLine("5. Show Total Inventory Value");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice (1-6): ");
        }

        private static void AddProductUI()
        {
            Console.WriteLine("\nAdd New Product");
            Console.WriteLine("----------------");

            try
            {
                Console.Write("Enter Product ID (positive integer): ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Enter Product Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Quantity in Stock (non-negative integer): ");
                int quantity = int.Parse(Console.ReadLine());

                Console.Write("Enter Price (non-negative number): ");
                decimal price = decimal.Parse(Console.ReadLine());

                var product = new Product(id, name, quantity, price);

                if (inventory.AddProduct(product))
                {
                    Console.WriteLine("Product added successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to add product. Check your inputs (ID must be unique and positive, quantity and price must be non-negative).");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter numbers where required.");
            }
        }

        private static void RemoveProductUI()
        {
            Console.WriteLine("\nRemove Product");
            Console.WriteLine("--------------");

            try
            {
                Console.Write("Enter Product ID to remove: ");
                int id = int.Parse(Console.ReadLine());

                if (inventory.RemoveProduct(id))
                {
                    Console.WriteLine("Product removed successfully!");
                }
                else
                {
                    Console.WriteLine("Product not found or invalid ID.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid product ID.");
            }
        }

        private static void UpdateProductUI()
        {
            Console.WriteLine("\nUpdate Product Quantity");
            Console.WriteLine("-----------------------");

            try
            {
                Console.Write("Enter Product ID to update: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Enter New Quantity (non-negative integer): ");
                int quantity = int.Parse(Console.ReadLine());

                if (inventory.UpdateProduct(id, quantity))
                {
                    Console.WriteLine("Product quantity updated successfully!");
                }
                else
                {
                    Console.WriteLine("Product not found or invalid quantity.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter valid numbers.");
            }
        }
    }
}