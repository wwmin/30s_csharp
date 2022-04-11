<Query Kind="Program" />

//位运算符和移位运算符
//参考: https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators
/*
说明
以下运算符使用整数类型或 char 类型的操作数执行位运算或移位运算：

   一元 ~（按位求补）运算符
   二进制 <<（向左移位）和 >>（向右移位）移位运算符
   二进制 &（逻辑 AND）、|（逻辑 OR）和 ^（逻辑异或）运算符
   
这些运算符是针对 int、uint、long 和 ulong 类型定义的。 如果两个操作数都是其他整数类型（sbyte、byte、short、ushort 或 char），它们的值将转换为 int 类型，这也是一个运算的结果类型。 如果操作数是不同的整数类型，它们的值将转换为最接近的包含整数类型。
&、| 和 ^ 运算符也是为 bool 类型的操作数定义的。 有关详细信息，请参阅布尔逻辑运算符。
位运算和移位运算永远不会导致溢出，并且不会在已检查和未检查的上下文中产生相同的结果。
其他:
Convert.ToString(112, toBase: 2),表示将数112转换成2进制,toBase有效值有:2,8,10,16
定义如下:
```
public static string ToString(int value, int toBase)
{
	if (toBase != 2 && toBase != 8 && toBase != 10 && toBase != 16)
	{
		throw new ArgumentException(SR.Arg_InvalidBase);
	}
	return ParseNumbers.IntToString(value, toBase, -1, ' ', 0);
}
```
*/

void Main()
{
	unary();
	left_shift();
	right_shift();
	logical_and();
	logical_exclusive_or();
	logical_or();
	compound_assignment();
	preecedence();
	shift_count();
}
//一元按位求补 位运算 ~ , ~还可以定义终结器(以前称为析构函数)
public void unary()
{
	uint a = 0b_0000_1111_0000_1111_0000_1111_0000_1100;
	uint b = ~a;
	Console.WriteLine(Convert.ToString(b, toBase: 2));
	// Output:
	// 11110000111100001111000011110011
}

//二元位运算 左位移
//左移运算会放弃超出结果类型范围的高阶位，并将低阶空位位置设置为零
//由于移位运算符仅针对 int、uint、long 和 ulong 类型定义，因此运算的结果始终包含至少 32 位。
//如果左侧操作数是其他整数类型（sbyte、byte、short、ushort 或 char），则其值将转换为 int 类型
public void left_shift()
{
	uint x = 0b_1100_1001_0000_0000_0000_0000_0001_0001;
	Console.WriteLine($"Before: {Convert.ToString(x, toBase: 2)}");

	uint y = x << 1;
	Console.WriteLine($" After:  {Convert.ToString(y, toBase: 2)}");
	// Output:
	// Before: 11001001000000000000000000010001
	// After:  10010000000000000000000100010000

	byte a = 0b_1111_0001;

	var b = a << 8;
	Console.WriteLine(b.GetType().ToString());
	Console.WriteLine($"Shifted byte: {Convert.ToString(b, toBase: 2)}");
	// Output:
	// System.Int32
	// Shifted byte: 1111000100000000
}

//二元位运算 右位移
//右移位运算会放弃低阶位
public void right_shift()
{
	uint x = 0b_1001;
	Console.WriteLine($"Before: {Convert.ToString(x, toBase: 2),4}");

	uint y = x >> 2;
	Console.WriteLine($"After:  {Convert.ToString(y, toBase: 2),4}");
	// Output:
	// Before: 1001
	// After:    10

	//如果左侧操作数的类型是 int 或 long，则右移运算符将执行 算术移位：左侧操作数的最高有效位（符号位）的值将传播到高顺序空位位置。 
	//也就是说，如果左侧操作数为非负，高顺序空位位置设置为零，如果为负，则将该位置设置为 1
	int a = int.MinValue;
	Console.WriteLine($"Before: {Convert.ToString(a, toBase: 2)}");

	int b = a >> 3;
	Console.WriteLine($"After:  {Convert.ToString(b, toBase: 2)}");
	// Output:
	// Before: 10000000000000000000000000000000
	// After:  11110000000000000000000000000000

	//如果左侧操作数的类型是 uint 或 ulong，则右移运算符执行逻辑移位：高顺序空位位置始终设置为零。
	uint c = 0b_1000_0000_0000_0000_0000_0000_0000_0000;
	Console.WriteLine($"Before: {Convert.ToString(c, toBase: 2),32}");

	uint d = c >> 3;
	Console.WriteLine($"After: {Convert.ToString(d, toBase: 2),32}");
	// Output:
	// Before: 10000000000000000000000000000000
	// After:     10000000000000000000000000000
}
//逻辑与
public void logical_and()
{
	uint a = 0b_1111_1000;
	uint b = 0b_1001_1101;
	uint c = a & b;
	Console.WriteLine(Convert.ToString(c, toBase: 2));
	// Output:
	// 10011000
}
//逻辑异或
public void logical_exclusive_or()
{
	uint a = 0b_1111_1000;
	uint b = 0b_0001_1100;
	uint c = a ^ b;
	Console.WriteLine(Convert.ToString(c, toBase: 2));
	// Output:
	// 11100100
}
//逻辑或
public void logical_or()
{
	uint a = 0b_1010_0000;
	uint b = 0b_1001_0001;
	uint c = a | b;
	Console.WriteLine(Convert.ToString(c, toBase: 2));
	// Output:
	// 10110001
}
//复合赋值
public void compound_assignment()
{
	uint INITIAL_VALUE = 0b_1111_1000;

	uint a = INITIAL_VALUE;
	a &= 0b_1001_1101;
	Display(a);  // output: 10011000

	a = INITIAL_VALUE;
	a |= 0b_0011_0001;
	Display(a);  // output: 11111001

	a = INITIAL_VALUE;
	a ^= 0b_1000_0000;
	Display(a);  // output: 1111000

	a = INITIAL_VALUE;
	a <<= 2;
	Display(a);  // output: 1111100000

	a = INITIAL_VALUE;
	a >>= 4;
	Display(a);  // output: 1111

	void Display(uint x) => Console.WriteLine($"{Convert.ToString(x, toBase: 2),8}");


	byte x = 0b_1111_0001;

	int b = x << 8;
	Console.WriteLine($"{Convert.ToString(b, toBase: 2)}");  // output: 1111000100000000

	x <<= 8;
	Console.WriteLine(x);  // output: 0
}
//位运算符优先级
public void preecedence()
{
	uint a = 0b_1101;
	uint b = 0b_1001;
	uint c = 0b_1010;

	uint d1 = a | b & c;
	Display(d1);  // output: 1101

	uint d2 = (a | b) & c;
	Display(d2);  // output: 1000

	void Display(uint x) => Console.WriteLine($"{Convert.ToString(x, toBase: 2),4}");
}
//移位运算符的移位计数
public void shift_count()
{
	int count1 = 0b_0000_0001;
	int count2 = 0b_1110_0001;

	int a = 0b_0001;
	Console.WriteLine($"{a} << {count1} is {a << count1}; {a} << {count2} is {a << count2}");
	// Output:
	// 1 << 1 is 2; 1 << 225 is 2

	int b = 0b_0100;
	Console.WriteLine($"{b} >> {count1} is {b >> count1}; {b} >> {count2} is {b >> count2}");
	// Output:
	// 4 >> 1 is 2; 4 >> 225 is 2
}

//枚举逻辑运算符
//运算符可重载性
