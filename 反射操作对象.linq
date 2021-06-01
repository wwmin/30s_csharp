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
}

public class MyClass{
	//静态字段 反射式不会遍历到
	public static string StrId = "12345";
	public string RealName{get;set;}
	public int Age{get;set;}
}

// You can define other methods, fields, classes and namespaces here
