<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	//反射获取对象的属性及其值
	MyClass mc = new MyClass();
	mc.RealName = "wwmin";
	mc.Age = 11;
	
	List<string> objectNameTypeValueList= new List<string>();
	#region 获取
	//反射
	Type t= mc.GetType();
	//获取所有公共属性
	PropertyInfo[] pArray = t.GetProperties();
	Array.ForEach<PropertyInfo>(pArray, p => {
		objectNameTypeValueList.Add(p.Name+"|"+p.PropertyType.Name+"|"+p.GetValue(mc));
	});
	objectNameTypeValueList.Dump("ObjectInfoList");
	#endregion
	
	#region 设置属性值
	mc.GetType().GetProperty(nameof(mc.RealName)).SetValue(mc,"测试反射设置属性值");
	mc.RealName.Dump("设置属性值");
	#endregion
	
	TestReflectCheckNullable();
	TestReflectCheckItemNullable();
}

#region 检查元素是否可空的反射API
/*
它提供来自反射成员的可空性信息和上下文：
ParameterInfo 参数
FieldInfo 字段
PropertyInfo 属性
EventInfo 事件
*/
void TestReflectCheckNullable()
{
	var example = new ExampleNullableClass();
	var nullablilityInfoContext = new NullabilityInfoContext();
	foreach (var propertyInfo in example.GetType().GetProperties())
	{
		var nullabilityInfo = nullablilityInfoContext.Create(propertyInfo);
		Console.WriteLine($"{propertyInfo.Name} property is {nullabilityInfo.WriteState}");
	}
}

/*
检查嵌套元素是否可为空的反射API
它允许您获取嵌套元素的可为空的信息, 您可以指定数组属性必须为非空，但元素可以为空，反之亦然。
*/
void TestReflectCheckItemNullable()
{
	Type exampleType = typeof(ExampleNullableItemClass);
	PropertyInfo notNullableArrayPI = exampleType.GetProperty(nameof(ExampleNullableItemClass.NotNullableArray))!;
	PropertyInfo nullableArrayPI = exampleType.GetProperty(nameof(ExampleNullableItemClass.NullableArray))!;

	NullabilityInfoContext nullabilityInfoContext = new();

	NullabilityInfo notNullableArrayNI = nullabilityInfoContext.Create(notNullableArrayPI);
	Console.WriteLine(notNullableArrayNI.ReadState);//NotNull
	Console.WriteLine(notNullableArrayNI.ElementType!.ReadState);//Nullable

	NullabilityInfo nullableArrayNI = nullabilityInfoContext.Create(nullableArrayPI);
	Console.WriteLine(nullableArrayNI.ReadState);//Nullable
	Console.WriteLine(nullableArrayNI.ElementType!.ReadState);//Nullable
}

#endregion

public class MyClass
{
	//静态字段 反射式不会遍历到
	public static string StrId = "12345";
	public string RealName{get;set;}
	public int Age{get;set;}
}

public class GitHubUser
{
	public string Name { get; set; }
	public string Bio { get; set; }
}

public class ExampleNullableClass
{
	public string? Name { get; set; }
	public string Value { get; set; }

	public GitHubUser gitHubUser { get; set; }
}

public class ExampleNullableItemClass
{
	public string?[] NotNullableArray { get; set; }
	public string?[]? NullableArray { get; set; }
}