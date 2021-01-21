<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

//.net 中枚举一般有两种用法, 一:表示唯一的元素序列; 2:表示多种复合状态. 这个时候一般为枚举加上[Flags]特性标记为位域, 
//这样可以用"或"运算符组合多个状态

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

void Main()
{
	Permission permission = Permission.create | Permission.read | Permission.update | Permission.delete;
	Console.WriteLine("1、枚举创建，并赋值……");
	((int)permission).Dump("(int)permission)");
	permission.ToString().Dump("permission.ToString()");
	
	((permission & Permission.create) == Permission.create).Dump("(permission & Permission.create) == Permission.create");
	((permission & Permission.read) == Permission.read).Dump("(permission & Permission.read) == Permission.read");
	((permission & Permission.update) == Permission.update).Dump("(permission & Permission.update) == Permission.update");
	((permission & Permission.delete) == Permission.delete).Dump("(permission & Permission.delete) == Permission.delete");
	
	
	permission = (Permission)Enum.Parse(typeof(Permission),"5");//create, read, update, delete
	Console.WriteLine("2、通过数字字符串转换……");
	permission.ToString().Dump("(Permission)Enum.Parse(typeof(Permission),\"5\")");//create, update
	((int)permission).Dump("(int)permission");
	
	permission = (Permission)Enum.Parse(typeof(Permission),"update,delete,read",true);
	Console.WriteLine("3、通过枚举名称字符串转换……");
	permission.Dump("permission");
	
	permission = (Permission)7;
	Console.WriteLine("4、直接用数字强制转换……");
	permission.Dump("permission");
	
	permission = permission & ~Permission.read;
	Console.WriteLine("5、去掉一个枚举项……");
	permission.Dump("permission");
	
	permission = permission|Permission.delete;
	Console.WriteLine("6、加上一个枚举项……");
	permission.Dump("permission");
	 
	 //在数据库中判断 , 下面的sql语句同样可以判断多个权限
	 //"AND (@permission IS NULL OR @permission=0 OR permission &@permission =@permission)"
	 //如果没有得到得到非预期的值(值没有对应的成员),比如:
	 Days d= (Days)8;
	 Enum.IsDefined(typeof(Days),d).Dump("Enum.IsDefined(typeof(Days),d)");//false
	 //即使枚举没有值为0的成员,它的默认值永远都是0
	 default(Days).Dump("default(Days)");//0
	 
	 ApiStatus.OK.GetDescription().Dump();
	 
	 //枚举遍历
	 Enum.GetValues(typeof( ApiStatus)).AsQueryable().Dump();
}

public static class Utils
{
	public static string GetDescription(this Enum val)
	{
		var field = val.GetType().GetField(val.ToString());
		var customAttribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
		if (customAttribute == null) { return val.ToString(); }
		else return ((DescriptionAttribute)customAttribute).Description;
	}

}

[Flags]
public enum Permission{
    create = 1,
    read = 2,
    update = 4,
    delete = 8
}
 
 //c#枚举成员的类型默认是int类型的,通过继承可以声明枚举成员为其他类型,枚举类型一定是继承自byte,sbyte
 //,short,unshort,int,uint,long和ulong中的一种, 不能是其它类型
 public enum Days: byte{
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6,
    Sunday = 7
 }
 
 //枚举可以通过Description、Display等特性来为成员添加有用的辅助信息,比如:
 public enum ApiStatus{
    [Display(Description ="Display:成功",Name = "Ok")]
    [Description("成功")]
    OK = 200,
    [Description("资源未找到")]
    NotFount = 404,
    [Description("拒绝访问")]
    AccessDenied = 401
 }
 
