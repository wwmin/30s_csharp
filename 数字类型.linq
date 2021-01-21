<Query Kind="Statements">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

//Decimal
decimal devidend = Decimal.One;
devidend.Dump("Decimal.One");
decimal divisor = 3;
var d1 = devidend/divisor*divisor;
d1.Dump("decimal division");
Math.Round(d1,2).Dump("division");

//Double
double d = 1d;
var d2 = d/(double)divisor*(double)divisor;
Math.Round(d2,2).Dump("double");

//关于小数末尾0处理结果
(1.00m).Dump("decimal");//decimal
(1.00d).Dump("double");//double
(1.00f).Dump("float");//float

$"{Decimal.MinusOne},{Decimal.Zero},{Decimal.One},{Decimal.MinValue},{Decimal.MaxValue}".Dump("Decimal More");
$"{Decimal.Multiply(4m,2m)}".Dump("Decimal.Multiply");