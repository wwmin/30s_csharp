//获取对象属性
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection.Emit;

static int length = 10000000;
public class People
{
    public string? Name { get; set; }
}

//方法1. 直接调用
private static void Directly()
{
    People people = new People { Name = "wwmin" };
    Stopwatch stopwatch = Stopwatch.StartNew();
    for (int i = 0; i < length; i++)
    {
        object value = people.Name;
    }
    stopwatch.Stop();
    $"Directly: {stopwatch.ElapsedMilliseconds}ms".Dump("1");
}

Directly();

//方法2. 反射
private static void Reflection()
{
    People people = new People { Name = "wwmin" };
    Type type = typeof(People);
    PropertyInfo property = type.GetProperty("Name")!;
    Stopwatch stopwatch = Stopwatch.StartNew();
    for (int i = 0; i < length; i++)
    {
        object value = property.GetValue(people)!;
    }
    stopwatch.Stop();
    $"Reflection: {stopwatch.ElapsedMilliseconds}ms".Dump("2");
}
Reflection();

//方法3. 动态构建Lambda
private static void Lambda()
{
    People people = new People();
    Type type = typeof(People);
    var parameter = Expression.Parameter(type, "m");//参数m
    PropertyInfo property = type.GetProperty("Name")!;
    Expression expProperty = Expression.Property(parameter, property.Name);//取参数的属性m.Name
    var propertyDelegateExpression = Expression.Lambda(expProperty, parameter);//变成表达式m=>m.Name
    var propertyDelegate = (Func<People, object>)propertyDelegateExpression.Compile();//编译成委托
    Stopwatch stopwatch = Stopwatch.StartNew();
    for (int i = 0; i < length; i++)
    {
        object value = propertyDelegate.Invoke(people);
    }
    stopwatch.Stop();
    $"Lambda:{stopwatch.ElapsedMilliseconds}".Dump("3");
}
Lambda();

//方法4. 委托调用
//在C#中，可读属性都有一个对应的get_XXX()的方法，可以通过调用这个方法来取得对应属性的值。
//可以使用System.Delegate.CreateDelegate创建一个委托来调用这个方法。
delegate object MemberGetDelegate(People p);
private static void Delegate()
{
    People people = new People { Name = "wwmin" };
    Type type = typeof(People);
    PropertyInfo property = type.GetProperty("Name")!;
    MemberGetDelegate memberGet = (MemberGetDelegate)System.Delegate.CreateDelegate(typeof(MemberGetDelegate),
    property.GetGetMethod()!);
    Stopwatch stopwatch = Stopwatch.StartNew();
    for (int i = 0; i < length; i++)
    {
        object value = memberGet(people);
    }
    stopwatch.Stop();
    $"Delegate: {stopwatch.ElapsedMilliseconds}".Dump("4");
}
Delegate();

//方法4-1. 封装委托调用
//封装委托调用方法
public class PropertyValue<T>
{
    private static readonly ConcurrentDictionary<string, MemberGetDelegate> _memberGetDelegate = new ConcurrentDictionary<string, MemberGetDelegate>();
    delegate object MemberGetDelegate(T obj);
    public PropertyValue(T obj)
    {

        Target = obj;
    }
    public T Target { get; private set; }
    public object Get(string name)
    {
        MemberGetDelegate memberGet = _memberGetDelegate.GetOrAdd(name, BuildDelegate);
        return memberGet(Target);
    }
    private MemberGetDelegate BuildDelegate(string name)
    {
        Type type = typeof(T);
        PropertyInfo? property = type.GetProperty(name);
        return (MemberGetDelegate)System.Delegate.CreateDelegate(typeof(MemberGetDelegate), property?.GetGetMethod()!);
    }
}

private static void DelegatePackage()
{
    People people = new People { Name = "wwm" };
    PropertyValue<People> propertyValue = new PropertyValue<People>(people);
    Stopwatch stopwatch = Stopwatch.StartNew();
    for (int i = 0; i < length; i++)
    {
        object value = propertyValue.Get("Name");
    }
    stopwatch.Stop();
    $"DelegatePackage: {stopwatch.ElapsedMilliseconds}".Dump("4-1");
}
DelegatePackage();

//方法5. Emit
//Emit可以在运行时动态生成代码，我们可以动态构建一个方法，在这个方法里面调用取属性值的方法
private static void Emit()
{
    People people = new People { Name = "Wayne" };
    Type type = typeof(People);
    var property = type.GetProperty("Name");
    DynamicMethod method = new DynamicMethod("GetPropertyValue", typeof(object), new Type[] { type }, true);
    ILGenerator il = method.GetILGenerator();
    il.Emit(OpCodes.Ldarg_0);
    il.Emit(OpCodes.Callvirt, property?.GetGetMethod()!);

    if (property != null && property.PropertyType.IsValueType)
    {
        il.Emit(OpCodes.Box, property.PropertyType);//值类型需要装箱，因为返回类型是object
    }
    il.Emit(OpCodes.Ret);
    if (method.CreateDelegate(typeof(Func<People, object>)) is not Func<People, object> fun) return;
    Stopwatch stopwatch = Stopwatch.StartNew();

    for (int i = 0; i < length; i++)
    {
        object value = fun.Invoke(people);
    }
    stopwatch.Stop();
    $"Emit:{stopwatch.ElapsedMilliseconds}ms".Dump();
}
Emit();