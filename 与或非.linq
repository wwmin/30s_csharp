<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	//表示 mo在Mode.x和Mode.y 时都是成立的
	Mode mo = Mode.x | Mode.y;
	mo.Dump("mo"); //3
	
	((mo & Mode.x) == Mode.x).Dump("(mo & Mode.x) == Mode.x");//true
	((mo & Mode.y) == Mode.y).Dump("(mo & Mode.y) == Mode.y");//true
	((mo & Mode.z) == Mode.z).Dump("(mo & Mode.z) == Mode.z");//false
	(mo == Mode.x).Dump("mo == Mode.x");
	
	//赋值操作
	mo &=~Mode.x;//判断Mode.x时为False
	((mo & Mode.x) == Mode.x).Dump("(mo & Mode.x) == Mode.x");//true
	
	mo |= Mode.x;//判断Mode.x时为True
	((mo & Mode.x) == Mode.x).Dump("(mo & Mode.x) == Mode.x");//false
}

//var a= 1;
//int b= a|1;

//int c= a& 1;
//int d= a^2;
//d.Dump();

//定义枚举值的时候要用2的n次幂, 因为与或操作是二进制的
enum Mode{
    x = 1,
    y = 2,
    z = 4
}
