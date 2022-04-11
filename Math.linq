<Query Kind="Program">
  <Namespace>System.Numerics</Namespace>
</Query>

void Main()
{
	var d =16;
	var s= Math.Sqrt(d);
	s.Dump();
	var sq = Sqrt(d);
	sq.Dump();
	
	//.NET 6 引入了用于处理2的幂的新方法
	//IsPow2 evaluates whether the specified Int32 value is a power of two.
	//'IsPow2' 判断指定值是否为 2 的幂。
	BitOperations.IsPow2(128).Dump();//True
	
	//'RoundUpToPowerOf2' 将指定值四舍五入到 2 的幂。
	BitOperations.RoundUpToPowerOf2(200).Dump();//256
}

/// <summary>开平方根(牛顿迭代法)</summary>
public static decimal Sqrt(decimal c){
	if(c<0)return decimal.MinValue;
	decimal e= 1e-50m;
	decimal x=c;
	decimal y =(x+c/x)/2;
	while (Math.Abs(x-y)>e)
	{
		x=y;
		y=(x+c/x)/2;
	}
	return x;
}