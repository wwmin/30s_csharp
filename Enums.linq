<Query Kind="Program">
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Dynamic</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	{
		//根据id获取enum key
		Enum.Parse(typeof(RoleEnum), "1").Dump();
		((RoleEnum)Enum.Parse(typeof(RoleEnum),"1")).GetDescription().Dump();
		
		var allItems = EnumHelper.GetAllItems(typeof(RoleEnum));
		string.Join(Environment.NewLine, allItems.Select(i => (string)i.Text)).Dump("RoleEnumName");
		RoleEnum.超级管理员.GetDescription().Dump();
	}

	{
		//1、枚举创建，并赋值
		Permission permission = Permission.create | Permission.read | Permission.update | Permission.delete;
		Console.WriteLine("1、枚举创建，并赋值……");
		((int)permission).Dump("(int)permission)");
		permission.ToString().Dump("permission.ToString()");
		//比较判断
		((permission & Permission.create) == Permission.create).Dump();//true
		((permission & Permission.read) == Permission.read).Dump();//true
		((permission & Permission.update) == Permission.update).Dump();//true
		((permission & Permission.delete) == Permission.delete).Dump();//true

		//2、通过数字字符串转换
		permission = (Permission)Enum.Parse(typeof(Permission), "5");//create, read, update, delete
		Console.WriteLine("2、通过数字字符串转换……");
		permission.ToString().Dump("(Permission)Enum.Parse(typeof(Permission),\"5\")");//create, update
		((int)permission).Dump("(int)permission");

		//3、通过枚举名称字符串转换
		permission = (Permission)Enum.Parse(typeof(Permission), "update,delete,read", true);
		Console.WriteLine("3、通过枚举名称字符串转换……");
		permission.Dump("permission");

		//4、直接用数字强制转换
		permission = (Permission)7;
		Console.WriteLine("4、直接用数字强制转换……");
		permission.Dump("permission");

		//5、去掉一个枚举项
		permission = permission & ~Permission.read;
		Console.WriteLine("5、去掉一个枚举项……");
		permission.Dump("permission");

		//6、加上一个枚举项
		permission = permission | Permission.delete;
		Console.WriteLine("6、加上一个枚举项……");
		permission.Dump("permission");
		
		// 在数据库中判断 , 下面的sql语句同样可以判断多个权限
		//  "AND (@permission IS NULL OR @permission=0 OR permission &@permission =@permission)"
	}
}

public static class EnumHelper
{
	/// <summary>
	/// 	get enum description by name
	/// </summary>
	public static string GetDescription<T>(this T enumItemName) where T : IConvertible
	{
		if (enumItemName is null || enumItemName is not Enum) throw new ArgumentNullException(nameof(enumItemName));
		FieldInfo fi = enumItemName.GetType().GetField(enumItemName.ToString());
		DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
		if (attributes is not null && attributes.Length > 0)
		{
			return attributes[0].Description;
		}
		return enumItemName.ToString();
	}
	/// <summary>
	/// get all infomation of enum ,include value name description
	/// </summary>
	public static List<dynamic> GetAllItems(Type enumName)
	{
		List<dynamic> list = new List<dynamic>();
		FieldInfo[] fields = enumName.GetFields();
		foreach (FieldInfo field in fields)
		{
			if (!field.FieldType.IsEnum)
			{
				continue;
			}
			//get enum value
			int value = (int)enumName.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)!;
			string text = field.Name;
			string description = string.Empty;
			object[] array = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (array.Length > 0)
			{
				description = ((DescriptionAttribute)array[0]).Description;
			}
			else
			{
				description = "";
			}
			dynamic obj = new ExpandoObject();
			obj.Value = value;
			obj.Text = text;
			obj.Description = description;
			list.Add(obj);
		}
		return list;
	}

	//public static string GetDescriptionByName<T>(T enumItemName)
	//{
	//	if (enumItemName == null) throw new ArgumentNullException(nameof(enumItemName));
	//	FieldInfo fi = enumItemName.GetType().GetField(enumItemName.ToString()!)!;
	//	DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
	//	if (attributes != null && attributes.Length > 0)
	//	{
	//		return attributes[0].Description;
	//	}
	//	else
	//	{
	//		return enumItemName.ToString()!;
	//	}
	//}


}

//Flags关键字允许我们在使用.net 枚举变量时,使用多个组合值
//让枚举能参与二进制的与或运算
//.net 中枚举一般有两种用法, 
//一:表示唯一的元素序列; 
//2:表示多种复合状态. 这个时候一般为枚举加上[Flags]特性标记为位域, 这样可以用"或"运算符组合多个状态
[Flags]
public enum RoleEnum
{
	[Description("unknown")]
	未知 = 0,
	[Description("superadmin")]
	超级管理员 = 1,
	[Description("admin")]
	管理员 = 2,
	[Description("user")]
	普通用户 = 4,
	[Description("tempuser")]
	临时用户 = 8,
	[Description("audit")]
	审核员 = 16,
	[Description("examiner")]
	审查员 = 32
}
[Flags]
public enum Permission
{
	create = 1,
	read = 2,
	update = 4,
	delete = 8
}