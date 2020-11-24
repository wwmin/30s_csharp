using System.Collections.Generic;

Enumerable.Range(1,4).Count(n=>Enumerable.Range(1,(int)Math.Sqrt(n)-1).All(i=>n%i>0)).Dump();

Enumerable.Range(1,5).Count(n=>n/2>0).Dump();
Enumerable.Repeat(1,4).Dump();

SortedList sl = new SortedList();
sl.Add(2,"wwmin");
sl.Add(1,"liyue");
//sl.Add(1,"ww");
sl.Dump();

sl.Keys.Dump();

SortedSet<int> ss = new SortedSet<int>();
ss.Add(2);
ss.Add(2);
ss.Add(1);
ss.Add(1);

ss.Dump();