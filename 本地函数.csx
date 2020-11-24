//本地函数
//本地函数是在编译时定义的方法
//Lambda 表达式是在运行时声明和分配的对象。
//示例1
public class C
{
    public void Main()
    {
        int result = add(100, 200);
        result.Dump();
        //本地函数 add
        int add(int a, int b) { return a + b; };
    }

    public void TestLambda()
    {
        //lambda
        Func<int, int, int> add = (int a, int b) => a + b;
        int result = add(100, 200);
        result.Dump();
    }
}

C c = new C();
c.Main();
c.TestLambda();

//示例2
//使用本地函数版本
public static uint LocFunFibonacci(uint n)
{
    return Fibonacci(n);
    static uint Fibonacci(uint num)
    {
        if (num == 0) return 0;
        if (num == 1) return 1;
        return checked(Fibonacci(num - 2) + Fibonacci(num - 1));
    }
}

//使用Lambda表达式版本
public static uint LambdaFibonacci(uint n)
{
    Func<uint, uint> Fibonacci = null;//这里必须明确赋值
    Fibonacci = num =>
    {
        if (num == 0) return 0;
        if (num == 1) return 1;
        return checked(Fibonacci(num - 2) + Fibonacci(num - 1));
    };
    return Fibonacci(n);
}

LocFunFibonacci(4).Dump();
LambdaFibonacci(4).Dump();