<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

//.net List去重
//问题描述
//对象集合
using System.Diagnostics.CodeAnalysis;

void Main()
{
	var persons = new List<Person>{
		new Person { Name = "ww",Age = 1},
		new Person { Name = "ll",Age = 2},
		new Person { Name = "ww",Age = 1},
		new Person { Name = "ll",Age = 2},
		new Person { Name = "ww",Age = 1},
	};
	//使用Distinct去重, 此时会无效, 因为比较的是引用地址和hashCode值:
	var distinctItems = persons.Distinct();//distinct 只能比较值类型,引用类型因为比较的是地址,地址不同,就认为对象不同, 可看一下,List<int>的distinct
	distinctItems.Dump();//仍然是那么多

	var ds = new List<int> { 1, 2, 3, 1, 2, 3 }.Distinct(); //distinct 只能比较值类型
	ds.Dump();//1,2,3

	var distinctPerosnWithComparer = persons.Distinct(new PersonItemComparer());
	distinctPerosnWithComparer.Dump("PersonItemComparer");//ww 1 , ll 2
	var personMaxs = new List<PersonMax>{
		new PersonMax { Name = "ww",Age = 1},
		new PersonMax { Name = "ll",Age = 2},
		new PersonMax { Name = "ww",Age = 1},
		new PersonMax { Name = "ll",Age = 2},
		new PersonMax { Name = "ww",Age = 1},
	};
	var pd = personMaxs.Distinct();
	pd.Dump();//ww:1,ll:2

	var p1 = new PersonMax { Name = "ww", Age = 1 };
	var p2 = new PersonMax { Name = "ww", Age = 1 };
	p1.Equals(p2).Dump();//True

	//方法3: linq => GroupBy (相当于只比较一个字段)
	var distinctPersonWithLinqGroup = persons.GroupBy(x => x.Name).Select(p => p.First());
	distinctPersonWithLinqGroup.Dump("distinctPersonWithLinqGroup");//ww 1 , ll 2
	var distinctPersonWithLinqMore = persons.DistinctBy(p => p.Name);
	distinctPersonWithLinqMore.Dump("distinctPersonWithLinqMore");//ww 1 , ll 2


	//比较值类型List
	var intList1 = new List<int> { 1, 2, 3 };
	var intList2 = new List<int> { 3, 2, 1 };
	(intList1 == intList2).Dump("List1 == List2");//false
	var s = intList1.Except(intList2);
	s.Count().Dump("Except");//count => 0
}

public class Person
{
	public string Name { get; set; } = string.Empty;
	public int Age { get; set; }
}

//方法1: 实现比较器
class PersonItemComparer : IEqualityComparer<Person>
{
	public bool Equals(Person? x, Person? y)
	{
		return x?.Name == y?.Name && x?.Age == y?.Age;
	}

	public int GetHashCode([DisallowNull] Person obj)
	{
		return obj.Name.GetHashCode() ^ obj.Age.GetHashCode();
	}
}

//方法2:
//实现对象比较, 重写Equals
//对象集合
public partial class PersonMax : Person
{
	//重写ToString 方法
	public override string ToString()
	{
		return Name + ":" + Age;
	}

	//重写Equals
	public override bool Equals(object? obj)
	{
		//return base.Equals(obj);
		if (obj is Person p)
		{
			return p.Name == Name && p.Age == Age;
		}
		else return false;
	}
	//重写GetHashCode 使其hashCode基于内部字段值的HashCode
	public override int GetHashCode()
	{
		//return base.GetHashCode();
		return Name.GetHashCode() ^ Age.GetHashCode();
	}
}
public static class Utils
{
	//方法4: 扩展Linq
	public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comaprer = null)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

		return _(); IEnumerable<TSource> _()
		{
			var knownKeys = new HashSet<TKey>(comaprer);
			foreach (var sourceItem in source)
			{
				if (knownKeys.Add(keySelector(sourceItem)))
					yield return sourceItem;
			}
		}
	}
}

