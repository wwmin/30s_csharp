<Query Kind="Statements">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

string s1 = "wwmin";
string s2= "liyue";
string.Compare(s1,s2).Dump();

string str= string.Concat("w","e");
str.Dump();

string.Format("我是{0}",s1).Dump();
string.Join(',',new string[]{"w2","w2","m2","i2","23"}).Dump();

string ss = "WWMIN";
string.Equals(s1,s2,StringComparison.OrdinalIgnoreCase).Dump();

int i = s1.IndexOf("w");
i.Dump();
s1.ToCharArray().Dump();

string sequenceString = new string('\t',10);
(sequenceString+",创建重复n个字符").Dump();//                    ,创建重复n个字符; 注意前面有10ge空格