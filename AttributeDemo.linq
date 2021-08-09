<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	PrintAuthorInfo(typeof(SampleClass));
	//已启用提示
	TestObsolete();
}

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)]
public class AuthorAttribute : System.Attribute
{
	public string name;
	public double version;

	public AuthorAttribute(string name)
	{
		this.name = name;
		version = 1.0;
	}

}

[Author("liyue")]
[Author("wwmin", version = 1.1)]
class SampleClass
{
	public int Id { get; set; }

	[Conditional("DEBUG")]
	[Author("ceshi")]
	public void ShowInfo()
	{
		Console.WriteLine(nameof(Id) + ":" + Id);
	}
}

private static void PrintAuthorInfo(System.Type t)
{
	//创建类的实例
	var instance = Activator.CreateInstance(t);
	//instance.Id = 1;
	//instance.ShowInfo();
	//获取类下的所有方法
	var methods = t.GetMethods().Where(m => m.IsDefined(typeof(AuthorAttribute),true)).ToList();
	foreach (MethodInfo method in methods)
	{
		//获取方法上的特性
		var atts = method.GetCustomAttributes<AuthorAttribute>();
		foreach (var att in atts)
		{
			att.name.Dump();
		}
		//执行有AuthorAttribute特性的方法
		method.Invoke(instance, new object[] {});
	}
	$"Author infomation for {t}".Dump();
	if (t.IsDefined(typeof(AuthorAttribute), true))
	{
		var attributes = t.GetCustomAttributes<AuthorAttribute>().Select(p => p.name);
		string.Join(",", attributes).Dump();
	}
	//Using reflection.
	System.Attribute[] attrs = System.Attribute.GetCustomAttributes(t);//Reflect.
	foreach (System.Attribute attr in attrs)
	{
		if (attr is AuthorAttribute a)
		{
			$"{a.name},version {a.version:f}".Dump();
		}
	}
}

[Obsolete("已弃用")]
private static void TestObsolete(){
	Console.WriteLine("弃用特性");
}
/*
预定义特性（Attribute）
.Net 框架提供了三种预定义特性：
•	AttributeUsage
•	Conditional
•	Obsolete
*/

/*
1. AttributeUsage
预定义特性 AttributeUsage 描述了如何使用一个自定义特性类。它规定了特性可应用到的项目的类型。
规定该特性的语法如下：
[AttributeUsage(validon,AllowMultiple = allowmultiple,Inherited = inherited)]
其中：
•	参数 validon 规定特性可被放置的语言元素。它是枚举器 AttributeTargets 的值的组合。默认值是 AttributeTargets.All。
•	参数 allowmultiple（可选的）为该特性的 AllowMultiple 属性（property）提供一个布尔值。如果为 true，则该特性是多用的。默认值是 false（单用的）。
•	参数 inherited（可选的）为该特性的 Inherited 属性（property）提供一个布尔值。如果为 true，则该特性可被派生类继承。默认值是 false（不被继承）。
 
 
[AttributeUsage(AttributeTargets.Class |
AttributeTargets.Constructor |
AttributeTargets.Field |
AttributeTargets.Method |
AttributeTargets.Property,
AllowMultiple = true)]


2. Conditional
这个预定义特性标记了一个条件方法，其执行依赖于指定的预处理标识符。
它会引起方法调用的条件编译，取决于指定的值，比如 Debug 或 Trace。



static void Main(string[] args)
{
	DebugTest.alert("inmain"); 只有VS在Debug的时候才会调用这个方法

	DebugTest.alert2("inmain2");
	Console.ReadKey();
}


[Conditional("DEBUG")]
public static void alert(string msg)
{
	Console.WriteLine(msg);
}

3. Obsolete
[Obsolete(
   message, 提示类方法过去的文字提示内容
   iserror      false, 方法还能用.ture 那么编译器就会报错
)]
[Obsolete("方法已过时", false)]//如果是true，那么编译器会报错

		public static void alert2(string msg)
{
	Console.WriteLine(msg);
}
*/
