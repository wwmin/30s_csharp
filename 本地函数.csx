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

//本地函数及异常
//迭代器的主体是延迟执行的,所以仅在枚举返回的序列时才显示异常,而非在调用迭代器方法时
//示例3
int[] list = Enumerable.Range(1, 6).ToArray<int>();
//var result = Filter(list, num => num % 2 > 0);
var result = Filter(list, null!);

string.Join(',', result).Dump();
static IEnumerable<T> Filter<T>(IEnumerable<T> source, Func<T, bool> predicate)
{
    if (source == null) throw new ArgumentNullException(nameof(source));
    if (source == null) throw new ArgumentNullException(nameof(predicate));
    //同步写法, 异常将发生在调用result时
    //    foreach (var element in source)
    //    {
    //        if (predicate(element))
    //            yield return element;
    //    }

    //本地函数写法, 异常将发生在调用Filter函数时发生, 相当于异常提前了
    IEnumerable<T> Iterator()
    {
        foreach (var element in source)
        {
            if (predicate(element))
                yield return element;
        }
    }
    return Iterator();
}

//总结:本地函数是方法中的方法,但它又仅仅是方法中的方法,它还可以出现在
//构造函数、属性访问器、事件访问器等等成员中;本地函数在功能上类似与Lambda表达式,
//但它比Lambda表达式更加方便和清晰,在分配和性能上也比Lambda表达式略占优势
//本地函数支持泛型和作为迭代器实现. 本地函数还有助于迭代器方法和异步方法中立即显示异常