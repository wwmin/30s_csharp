<Query Kind="Program" />

//二进制
void Main()
{
	//二进制与int之间转换
	string a = "00000010100101000001111010011100";
	a.Dump();
	a.Length.Dump();//32
	var ai = Convert.ToInt32(a,2);
	var aii = Convert.ToUInt32(a,2);
	ai.Dump();//43261596
	aii.Dump();//43261596
	
	"再转回 二进制".Dump();
	var ais = Convert.ToString(ai,toBase:2);
	ais.Length.Dump();//26
	ais.Dump();//10100101000001111010011100  比原始少了前导0

	/*
	整数文本
整数文本可以是

十进制：不使用任何前缀
十六进制：使用 0x 或 0X 前缀
二进制：使用 0b 或 0B 前缀（在 C# 7.0 和更高版本中可用）
下面的代码演示每种类型的示例：
	*/
	//十进制
	int decimalLiteral = 42;
	//十六进制
	int hexLiteral = 0x2A;
	//二进制表示的证书
	int binaryLiteral = 0b_0010_1010;
	decimalLiteral.Dump();
	hexLiteral.Dump();
	binaryLiteral.Dump();
}



