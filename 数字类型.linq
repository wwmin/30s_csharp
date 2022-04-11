<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	//Decimal
	decimal devidend = Decimal.One;
	devidend.Dump("Decimal.One");
	decimal divisor = 3;
	var d1 = devidend/divisor*divisor;
	d1.Dump("decimal division");
	Math.Round(d1,2).Dump("division");//1.00
	
	//Double
	double d = 1d;
	var d2 = d/(double)divisor*(double)divisor;
	Math.Round(d2,2).Dump("double");
	
	//关于小数末尾0处理结果
	(1.00m).Dump("decimal");//decimal => 1.00
	(1.00d).Dump("double");//double => 1
	(1.00f).Dump("float");//float => 1
	
	$"{Decimal.MinusOne},{Decimal.Zero},{Decimal.One},{Decimal.MinValue},{Decimal.MaxValue}".Dump("Decimal More");
	$"{Decimal.Multiply(4m,2m)}".Dump("Decimal.Multiply");
	
	var s=  "11";
	s.PadLeft(3,'0').Dump();//011
}
//参考: https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/builtin-types/integral-numeric-types
//整型数值类型
//整型数值类型 表示整数。 所有的整型数值类型均为值类型。 
//它们还是简单类型，可以使用文本进行初始化。 
//所有整型数值类型都支持算术、位逻辑、比较和相等运算符。

//整型类型的特征
/*
C# 类型/关键字	范围	大小	.NET 类型
sbyte	-128 到 127	8 位带符号整数	System.SByte
byte	0 到 255	无符号的 8 位整数	System.Byte
short	-32,768 到 32,767	有符号 16 位整数	System.Int16
ushort	0 到 65,535	无符号 16 位整数	System.UInt16
int	-2,147,483,648 到 2,147,483,647	带符号的 32 位整数	System.Int32
uint	0 到 4,294,967,295	无符号的 32 位整数	System.UInt32
long	-9,223,372,036,854,775,808 到 9,223,372,036,854,775,807	64 位带符号整数	System.Int64
ulong	0 到 18,446,744,073,709,551,615	无符号 64 位整数	System.UInt64
nint	取决于平台	带符号的 32 位或 64 位整数	System.IntPtr
nuint	取决于平台	无符号的 32 位或 64 位整数	System.UIntPtr
*/
//在除最后两行之外的所有表行中，最左侧列中的每个 C# 类型关键字都是相应 .NET 类型的别名。 
//关键字和 .NET 类型名称是可互换的。 例如，以下声明声明了相同类型的变量：
/*
int a = 123;
System.Int32 b = 123;
*/
//表的最后两行中的 nint 和 nuint 类型是本机大小的整数。 
//在内部它们由所指示的 .NET 类型表示，但在任意情况下关键字和 .NET 类型都是不可互换的。 
//编译器为 nint 和 nuint 的整数类型提供操作和转换，而不为指针类型 System.IntPtr 和 System.UIntPtr 提供。

//每个整型类型的默认值都为零 0。 除本机大小的类型外，每个整型类型都有 MinValue 和 MaxValue 常量，提供该类型的最小值和最大值。
//System.Numerics.BigInteger 结构用于表示没有上限或下限的带符号整数。

//整数文本
/*
整数文本可以是

十进制：不使用任何前缀
十六进制：使用 0x 或 0X 前缀
二进制：使用 0b 或 0B 前缀（在 C# 7.0 和更高版本中可用）
*/
/*
var decimalLiteral = 42;
var hexLiteral = 0x2A;
var binaryLiteral = 0b_0010_1010;
*/
//前面的示例还演示了如何将 _ 用作数字分隔符（从 C# 7.0 开始提供支持）。 可以将数字分隔符用于所有类型的数字文本。