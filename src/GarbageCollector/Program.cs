


internal class Program
{
    static Customer customer; // her zaman ulaşılabilir olduğu için program kapana kadar ulaşılabilir olacak.
    private static void Main(string[] args)
    {
        Run();

        GC.Collect();
        GC.WaitForPendingFinalizers();


    }
    static void Run()
    {
        double totalOrderPrice = CreateShortLiveOrders();

        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine(new string('-', 25));

        customer = CreatePersistentCustomers(); //referance olduğu için finalizer çalışmaz.
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
    static double CreateShortLiveOrders()
    {
        double totalPrice = 0;
        for (int i = 0; i < 3; i++)
        {
            Order order = new Order($"Order {i + 1}", 100 + i * 50);
            totalPrice += order.Price;
        }
        return totalPrice;
    }
    static Customer CreatePersistentCustomers()
    {
        Customer customer = new("John Doe");
        customer.Add(new Order("123", 2150));
        customer.Add(new Order("456", 789789));

        return customer;
    }
}

class Order
{
    public Order(string id, double price)
    {
        Id = id;
        Price = price;
    }

    public string Id { get; private set; }
    public double Price { get; private set; }

    ~Order()
    {
        Console.WriteLine($"{Id} ' li sipariş için finalizer çalıştı.");
    }

}
class Customer
{
    public string Name { get; private set; }
    private List<Order> orders = new List<Order>();

    public Customer(string name)
    {
        Name = name;
    }

    public void Add(Order o)
    {
        orders.Add(o);
    }
    ~Customer()
    {
        Console.WriteLine($"{Name} müşterisi için finalizer çalıştı.");
    }
}
