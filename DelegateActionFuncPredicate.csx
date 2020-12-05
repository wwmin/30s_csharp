//delegate
public delegate int PerformCalculation(int x, int y);
public int DelegateMethod(int x, int y)
{
    "DelegateMethod".Dump();
    return x + y;
}
public int DelegateMethod2(int x, int y)
{
    "DelegateMethod2".Dump();
    return 2 * x + 2 * y;
}
PerformCalculation calc = new PerformCalculation(DelegateMethod);
//多播委托即对同一个委托进行多次+=订阅,但每次执行的结果都会被下次的执行结果所覆盖,最后得到的结果即订阅最后一个方法的值
calc += DelegateMethod2;
calc(1, 2).Dump();
calc(1, 2).Dump();
//Action
Action<string> action = new Action<string>(Display);
action("hello");
static void Display(string message)
{
    message.Dump("action");
}

//Func
Func<int, double> func = new Func<int, double>(CalculateHre);
func(50000).Dump("func");

static double CalculateHre(int basic)
{
    return (double)(basic * .4);
}

//Predicate
class Customer
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
List<Customer> customers = new List<Customer>();
customers.Add(new Customer { Id = 1, Name = "wwmin" });
customers.Add(new Customer { Id = 2, Name = "liyue" });
Predicate<Customer> hydCustomers = x => x.Id == 1;
Customer customer = customers.Find(hydCustomers)!;
customer.Name.Dump("predicate");