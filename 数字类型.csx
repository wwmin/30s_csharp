//Decimal
decimal devidend = Decimal.One;
devidend.Dump();
decimal divisor = 3;
var d1 = devidend/divisor*divisor;
d1.Dump();
Math.Round(d1,2).Dump();

//Double
double d = 1d;
var d2 = d/(double)divisor*(double)divisor;
Math.Round(d2,2).Dump();

//关于小数末尾0处理结果
(1.00m).Dump();//decimal
(1.00d).Dump();//double
(1.00f).Dump();//float

$"{Decimal.MinusOne},{Decimal.Zero},{Decimal.One},{Decimal.MinValue},{Decimal.MaxValue}".Dump();
$"{Decimal.Multiply(4m,2m)}".Dump();