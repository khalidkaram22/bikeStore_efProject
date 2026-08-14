using BikeStoreEF.Data;
using BikeStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeStoreEF
{
    internal class Program
    {
        static void Main(string[] args)
        {


            using ApplicationDbContext dbContext = new ApplicationDbContext() ;

            bool exit = false;

            while (!exit)
            {
                Console.Clear();

                Console.WriteLine("======================================");
                Console.WriteLine("        BikeStore EF Core Tasks");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Retrieve all categories");
                Console.WriteLine("2. Retrieve first product");
                Console.WriteLine("3. Retrieve product by ID");
                Console.WriteLine("4. Retrieve products by model year");
                Console.WriteLine("5. Retrieve customer by ID");
                Console.WriteLine("6. Retrieve product names with brand names");
                Console.WriteLine("7. Count products in a category");
                Console.WriteLine("8. Calculate total list price of a category");
                Console.WriteLine("9. Calculate average product price");
                Console.WriteLine("10. Retrieve completed orders");
                Console.WriteLine("0. Exit");
                Console.WriteLine("======================================");

                Console.Write("Choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid choice!");
                    Console.ReadKey();
                    continue;
                }

                Console.Clear();

                switch (choice)
                {
                    case 1:
                        GetAllCategories(dbContext);
                        break;

                    case 2:
                        GetFirstProduct(dbContext);
                        break;

                    case 3:

                        Console.Write("enter id : ");
                        int id = Convert.ToInt32(Console.ReadLine()) ;

                        GetProductById(dbContext , id);
                        break;

                    case 4:

                        Console.Write("enter id : ");
                        short modelYear = Convert.ToInt16(Console.ReadLine());

                        GetProductsByModelYear(dbContext , modelYear);
                        break;

                    case 5:
                        Console.Write("enter id : ");

                        int cid = Convert.ToInt32(Console.ReadLine());

                        GetCustomerById(dbContext , cid);
                        break;

                    case 6:
                        GetProductsWithBrands(dbContext);
                        break;

                    case 7:
                        CountProductsInCategory(dbContext);
                        break;

                    case 8:
                        TotalListPriceByCategory(dbContext);
                        break;

                    case 9:
                        AverageProductPrice(dbContext);
                        break;

                    case 10:
                        GetCompletedOrders(dbContext);
                        break;

                    case 0:
                        exit = true;
                        continue;

                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();





            }






        }

        static void GetAllCategories(ApplicationDbContext context)
        {
            var categories = context.Categories.ToList();
            foreach (var category in categories) {
                Console.WriteLine($"category name :{category.CategoryName} id : {category.CategoryId}");
            }
        }

        static void GetFirstProduct(ApplicationDbContext context)
        {
            var product = context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefault();

            Console.WriteLine($"name : {product.ProductName} list price : {product.ListPrice} id : {product.ProductId} brand : {product.Brand.BrandName} category {product.Category.CategoryName} model year : {product.ModelYear} stock : {product.Stocks.Count} OrderItems : {product.OrderItems.Count}");
        }

        static void GetProductById(ApplicationDbContext context , int id)
        {
            var product = context.Products
             .Include(p => p.Brand)
              .Include(p => p.Category)
             .FirstOrDefault(x => x.ProductId == id);

            Console.WriteLine($"name : {product.ProductName} list price : {product.ListPrice} id : {product.ProductId} brand : {product.Brand.BrandName} category {product.Category.CategoryName} model year : {product.ModelYear} stock : {product.Stocks.Count} OrderItems : {product.OrderItems.Count}");
        }

        static void GetProductsByModelYear(ApplicationDbContext context , short modelYear)
        {
            var products = context.Products
                .Include(p => p.Brand)
              .Include(p => p.Category)
                .Where(p => p.ModelYear == modelYear)
                .ToList();


            foreach (var product in products)
            {

                Console.WriteLine("===================================");
                Console.WriteLine($"name : {product.ProductName} list price : {product.ListPrice} id : {product.ProductId} brand : {product.Brand.BrandName} category {product.Category.CategoryName} model year : {product.ModelYear} stock : {product.Stocks.Count} OrderItems : {product.OrderItems.Count}");

            }
        }

        static void GetCustomerById(ApplicationDbContext context , int id)
        {

            var customer = context.Customers
                .Include(c=>c.Orders)
                .FirstOrDefault(c => c.CustomerId == id);

            Console.WriteLine($"name : {customer.FirstName} {customer.LastName} , Email: {customer.Email} ,city {customer.City}, phone {customer.Phone} , state: {customer.State} , street : {customer.Street}, zip code :{customer.ZipCode} ,orders  {customer.Orders}");


            Console.WriteLine("===================================");

            Console.WriteLine("Orders:");

            foreach (var order in customer.Orders)
            {
                Console.WriteLine($"Order ID      : {order.OrderId}");
                Console.WriteLine($"Order Status  : {order.OrderStatus}");
                Console.WriteLine($"Order Date    : {order.OrderDate}");
                Console.WriteLine($"Required Date : {order.RequiredDate}");
                Console.WriteLine($"Shipped Date  : {order.ShippedDate}");
                Console.WriteLine($"Store ID      : {order.StoreId}");
                Console.WriteLine($"Staff ID      : {order.StaffId}");
                Console.WriteLine(new string('-', 40));
            }


        }

        static void GetProductsWithBrands(ApplicationDbContext context)
        {
            var products = context.Products
                .Select (p => new {p.ProductName , brandName = p.Brand.BrandName})
                .ToList();


            foreach (var product in products) {

                Console.WriteLine("===================================");
                Console.WriteLine($"product name : {product.ProductName} || brand name : {product.brandName}"); }

        }

        static void CountProductsInCategory(ApplicationDbContext context)
        {
            var categories = context.Categories.Select(c => new
            {
                c.CategoryName ,
                productCount = c.Products.Count()
            })
                .ToList();


            foreach (var c in categories) { Console.WriteLine($"name : {c.CategoryName} || count : {c.productCount}"); }
           
        }

        static void TotalListPriceByCategory(ApplicationDbContext context)
        {
            var categories = context.Categories
                .Select(c=> new {
                    CategoryName = c.CategoryName,
                    TotalPrice = c.Products .Sum(p=> p.ListPrice) 

                });

            foreach (var category in categories)
            {
                Console.WriteLine("================================");
                Console.WriteLine($"Category : {category.CategoryName} || Total Price : {category.TotalPrice:C}");
            }
        }

        static void AverageProductPrice(ApplicationDbContext context)
        {
            var avg = context.Products
                .Average(p => p.ListPrice);

            Console.WriteLine($"{avg}");
        }

        static void GetCompletedOrders(ApplicationDbContext context)
        {
            var orders = context.Orders
                .Include(o=> o.Customer)
                .Include(o => o.Staff)
                .Include(o => o.Store)
                .Where(o => o.OrderStatus == 1).ToList();

            foreach (var order in orders) { Console.WriteLine($"orderID : {order.OrderId} ,customer id : {order.CustomerId}, customer name {order.Customer.FirstName}{order.Customer.LastName},store name: {order.Store.StoreName},store email :{order.Store.Email} ,staff id : {order.Staff.StaffId} ,staff name: {order.Staff.FirstName}{order.Staff.LastName} ,orderDate : {order.OrderDate} ,RequiredDate: {order.RequiredDate} , ShippedDate :{order.ShippedDate}"); }

        }

    }
}

