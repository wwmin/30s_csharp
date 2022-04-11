<Query Kind="Program">
  <Output>DataGrids</Output>
  <Namespace>System.Text.Json</Namespace>
</Query>

void Main()
{
	//SortedList 参考:https://www.koderhq.com/tutorial/csharp/sortedlist/
	//Dictionary
	Dictionary<string, string> dics = new Dictionary<string, string>();
	dics.Add("3", "c");
	dics.Add("2", "b");
	dics.Add("1", "a");
	dics.Dump("Dictionary");
	//SortedDictionary
	SortedDictionary<string, string> sd = new SortedDictionary<string, string>();
	foreach (var dic in dics)
	{
		sd.Add(dic.Key, dic.Value);
	}
	sd.Dump("SortedDictionary");


	//Hashtable
	Hashtable ht = new Hashtable();
	ht.Add(1, "1");
	ht.Add(2, "2");
	ht.Add(3, "3");
	foreach (DictionaryEntry h in ht)
	{
		(h.Key + ":" + h.Value).Dump("HashTable:item");
	}

	//SortedList
	SortedList sl = new SortedList();
	sl.Add(2, "2");
	sl.Add(1, "1");
	foreach (DictionaryEntry item in sl)
	{
		(item.Key + ":" + item.Value).Dump("SortedList:DictionaryEntry");
	}
	SortedList<int[], int> sil = new SortedList<int[], int>(new MyComparer());

	sil.Add(new int[] { 1, 2 }, 1);
	sil.Add(new int[] { 2, 1 }, 3);
	sil.Keys.ToList().Select(a => string.Join(",", a)).Dump("SortedList<int[], int>"); ;
	introspectiveSort<int[]>(sil.Keys, sil.Comparer.Compare);
	//List
	List<int> li = new List<int>();
	li.Add(1);
	li.Add(2);
	li.Dump(typeof(List<int>).ToString());
	li.TryGetFirst(l => l > 0, out bool found).Dump("trygetfirst:li");
	li.Sort((a, b) => b - a);
	//string.Join(",",li).Dump(typeof(List<int>).ToString());

	//IOrderedEnumerable
	IOrderedEnumerable<int> sortedInt = li.OrderBy(l => l);

	sortedInt.TryGetFirst(l => l > 0, out bool f).Dump("OrderedEnumerable<int>");
}

public void introspectiveSort<T>(IList<T> keys, Comparison<T> comparer)
{
	if (keys.Count() > 1)
	{
		JsonSerializer.Serialize(keys).Dump();
	}
}

/// <summary>Utils</summary>
public static class Utils
{
	/// <summary>获取第一个符合条件的值</summary>
	public static TSource TryGetFirst<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out bool found)
	{
		//if (source is IOrderedEnumerable<TSource> orderedEnumerable)
		//{
		//	return orderedEnumerable.TryGetFirst(predicate, out found);//此处会导致无限n循环
		//}
		foreach (TSource item in source)
		{
			if (predicate(item))
			{
				found = true;
				return item;
			}
		}
		found = false;
#nullable disable
		return default;
#nullable enable
	}
}

//实现自定义比较器
public class MyComparer : IComparer<int[]>
{
	public int Compare(int[]? x, int[]? y) => y?[0] > x?[0] ? -1 : 1;
}
