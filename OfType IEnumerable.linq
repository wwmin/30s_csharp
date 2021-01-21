<Query Kind="Statements">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

string[] ss = new string[]{"111","222","333"};
var sList = ss.OfType<string>().ToList().Dump();

object[] oo = new object[]{1,2,3,"4","5","6",11.1,22.2};
oo.OfType<int>().ToList().Dump();
oo.OfType<string>().ToList().Dump();
oo.OfType<double>().ToList().Dump();