<Query Kind="Program">
  <Output>DataGrids</Output>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

//从MoreLinq库中学到的知识
void Main(string[] args)
{
	//GetAlphabets();
	//ReturnFuncOfFunc();
	//ImmutableListFunc();
	//ToStringWithFixedNum();
	//ZipLambda();
	//AggregateLambda();
	//IsTypeExpression();
	//OfTypeFunc();
	//WindowFunc();
	//AcquireTest();
	//AppendTest();
	//AppendManyTest();
	//PrependTest();
	AppendTwoAccumulatorTest();
}

//获取26个字母表
void GetAlphabets()
{
	var alphabets = Enumerable.Range(0, 26).Select(a => (char)('a' + a));
	//alphabets.Dump();
	//在Javascript中可使用如下方法
	//Array(26).fill (0).map((p,i)=> String.fromCharCode('a'.charCodeAt(0)+i))
	//const 和 readonly 的区别,readonly允许在构造器中改变它的状态(初始化),而const不行
}

//返回函数的函数
void ReturnFuncOfFunc()
{
	DelegateType affine1 = MakeAffine(1, 2);
	DelegateType affine2 = MA(1, 3);
	affine1(5).Dump();//1*5+2==7
	affine2(5).Dump();//1*5+3==8
}

//ImmutableList 不可变集合 , 与string类似, string也属于Immutable,但是有些许区别
void ImmutableListFunc()
{
	ImmutableList<string> emptyBusket = ImmutableList.Create<string>();
	var fruitBasket = emptyBusket.Add("apple");
	//每次修改都会创建一个新的集合
	//这就意味着要开辟新的内存，并且存在数据拷贝。程序本身的执行效率会下降同时GC压力会增大。
	emptyBusket.Dump();//empty
	fruitBasket.Dump();//["apple"]
}

//两个可遍历对象各自对应值操作
void ZipLambda()
{
	var i = Enumerable.Range(0, 26);
	var a = Enumerable.Range(0, 26).Select(a => (char)('a' + a));
	var zip = i.Zip(a, (us, them) => (Us: us, Them: them));
	string.Join("\t", zip.Select(z => z.Us + ":" + z.Them)).Dump("Zip");
}

//两个可遍历集合的复合操作,比较常用的是求和
void AggregateLambda()
{
	var i = Enumerable.Range(0, 26);
	var a = Enumerable.Range(0, 26).Select(a => (char)('a' + a));
	var zipAggregate = i.Zip(a, (us, them) => (Us: us, Them: them)).Aggregate(0, (x, y) => x + y.Us);
	zipAggregate.Dump();
}

//is type pattern
void IsTypeExpression()
{
	ArrayList al = new ArrayList() { 0, 1, 'a', "b" };
	StringBuilder res = new StringBuilder();
	foreach (var a in al)
	{
		if (a is { } ai and not string and not char and not 0)
		{
			res.Append("\t");
			res.Append(a);
		}
	}
	res.ToString().Trim().Dump();
}

void OfTypeFunc()
{
	ArrayList al = new ArrayList() { 0, 1, 'a', "b" };
	var ai = al.OfType<int>();
	ai.Aggregate((x, y) => x + y).Dump("OfType");
}

//window 一次选取若干个相同数量且连续的数据, 
//如同使用带有窗口的卡尺一次移动一个位置看到的连续数据的效果
void WindowFunc()
{
	var a = new List<int> { 1, 2, 3, 4, 5 };
	foreach (var t in a.Window(4))
	{
		var s = string.Join(',', t);
		s.Dump();
	}
}

void AppendTest()
{
	var head = new[] { "first", "second", "third", "four", "five" };
	var tail = "six";
	var whole = head.Append(tail);
	whole.Dump();
}

void PrependTest()
{
	var head = new[] { 1, 2 };
	var whole = head.Prepend(0);
	whole.Dump();
}

void AppendManyTest()
{
	var a = Enumerable.Range(0, 5);
	var b = Enumerable.Range(0, 5);
	var res = a.Aggregate(b.AsEnumerable(), (xs, s) => xs.Append(s));
	res.Dump();
}

void AppendTwoAccumulatorTest()
{
	var a = new[] { 1, 2 };
	var res = a.Aggregate(1, (x, y) => x + y, 2, (x, y) => x * y, (x, y) => x * y);
	//(1+1+2)*(2+1+2)
	res.Dump();
}

//以固定位数,转换string
void ToStringWithFixedNum()
{
	var s = 10;
	s.ToString("000", CultureInfo.InvariantCulture).Dump("ToString with fixed Num");
}
//返回函数的函数的定义
delegate int DelegateType(int x);
static DelegateType MakeAffine(int a, int b)
{
	return delegate (int x)
	{
		return a * x + b;
	};
}

static DelegateType MA(int a, int b) => (int x) => a * x + b;

//确保所有disposable对象都能正确关闭，即使中间会有失败的
void AcquireTest()
{

	Disposable a = null!;
	Disposable b = null!;
	Disposable c = null!;

	var allocators = MoreEnumerable.From(() => a = new Disposable(),
										 () => b = new Disposable(),
										 () => throw new TestException(),
										 () => c = new Disposable());

	try
	{
		allocators.Acquire();
	}
	catch (AggregateException ex)
	{
		ex.Dump(nameof(TestException));
	}
}
class Disposable : IDisposable
{
	public bool Disposed { get; private set; }
	public void Dispose()
	{
		nameof(Disposable).Dump();
		Disposed = true;
	}
}
sealed class TestException : System.Exception { };

//--------------------------------------------------------------------------------------------------------------------------------------//
public static class MoreEnumerable
{
	public static TSource[] Acquire<TSource>(this IEnumerable<TSource> source)
		  where TSource : IDisposable
	{
		if (source == null) throw new ArgumentNullException(nameof(source));

		var disposables = new List<TSource>();
		try
		{
			disposables.AddRange(source);
			return disposables.ToArray();
		}
		catch
		{
			foreach (var disposable in disposables)
				disposable.Dispose();
			throw;
		}
	}

	public static IEnumerable<T> Evaluate<T>(this IEnumerable<Func<T>> functions) =>
	from f in functions ?? throw new ArgumentNullException(nameof(functions))
	select f();

	public static IEnumerable<T> From<T>(params Func<T>[] functions)
	{
		if (functions == null) throw new ArgumentNullException(nameof(functions));
		return Evaluate(functions);
	}

	public static IEnumerable<T> Append<T>(this IEnumerable<T> head, T tail)
	{
		if (head == null) throw new ArgumentNullException(nameof(head));
		return head is PendNode<T> node ? node.Concat(tail) : PendNode<T>.WithSource(head).Concat(tail);
	}

	public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource value)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		return source is PendNode<TSource> node ? node.Prepend(value) : PendNode<TSource>.WithSource(source).Prepend(value);
	}

	public static IEnumerable<IList<TSource>> Window<TSource>(this IEnumerable<TSource> source, int size)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
		return _();
		IEnumerable<IList<TSource>> _()
		{
			using var iter = source.GetEnumerator();
			var window = new TSource[size];
			int i;
			for (i = 0; i < size && iter.MoveNext(); i++)
			{
				window[i] = iter.Current;
			}
			if (i < size)
				yield break;

			while (iter.MoveNext())
			{
				var newWindow = new TSource[size];
				Array.Copy(window, 1, newWindow, 0, size - 1);
				newWindow[size - 1] = iter.Current;

				yield return window;
				window = newWindow;
			}
			yield return window;

		}
	}

	public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TResult>(
		   this IEnumerable<T> source,
		   TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
		   TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
		   Func<TAccumulate1, TAccumulate2, TResult> resultSelector)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
		if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
		if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

		var a1 = seed1;
		var a2 = seed2;

		foreach (var item in source)
		{
			a1 = accumulator1(a1, item);
			a2 = accumulator2(a2, item);
		}

		return resultSelector(a1, a2);
	}

	public static TSource AggregateRight<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (source == null) throw new ArgumentNullException(nameof(func));
		var list = source.ToListLike();
		if (list.Count == 0) throw new InvalidOperationException("Sequence contains no elements.");
		return AggregateRightImpl(list, list[list.Count - 1], func, list.Count - 1);
	}

	static TResult AggregateRightImpl<TSource, TResult>(IListLike<TSource> list, TResult accumulator, Func<TSource, TResult, TResult> func, int i)
	{
		while (i-- > 0)
		{
			accumulator = func(list[i], accumulator);
		}
		return accumulator;
	}
}

public abstract class PendNode<T> : IEnumerable<T>
{
	public static PendNode<T> WithSource(IEnumerable<T> source) => new Source(source);

	public PendNode<T> Prepend(T item) => new Item(item, isPrepend: true, next: this);

	public PendNode<T> Concat(T item) => new Item(item, isPrepend: false, next: this);

	sealed class Item : PendNode<T>
	{
		public T Value { get; }
		public bool IsPrepend { get; }
		public int ConcatCount { get; }
		public PendNode<T> Next { get; }

		public Item(T item, bool isPrepend, PendNode<T> next)
		{
			if (next == null) throw new ArgumentNullException(nameof(next));
			Value = item;
			IsPrepend = isPrepend;
			ConcatCount = next is Item nextItem ? nextItem.ConcatCount + (isPrepend ? 0 : 1) : 1;
			Next = next;
		}
	}

	sealed class Source : PendNode<T>
	{
		public IEnumerable<T> Value { get; }
		public Source(IEnumerable<T> source) => Value = source;
	}

	public IEnumerator<T> GetEnumerator()
	{
		var i = 0;
		T[]? concats = null;//Array for > 4 concatenations
		var concat1 = default(T);
		var concat2 = default(T);
		var concat3 = default(T);
		var concat4 = default(T);

		var current = this;
		for (; current is Item item; current = item.Next)
		{
			if (item.IsPrepend)
			{
				yield return item.Value;
			}
			else
			{
				if (concats == null)
				{
					if (i == 0 && item.ConcatCount > 4)
					{
						concats = new T[item.ConcatCount];
					}
					else
					{
						switch (i++)
						{
							case 0: concat1 = item.Value; break;
							case 1: concat2 = item.Value; break;
							case 2: concat3 = item.Value; break;
							case 3: concat4 = item.Value; break;
							default: throw new IndexOutOfRangeException();
						}
						continue;
					}
				}
				concats[i++] = item.Value;
			}
		}
		var source = (Source)current;
		foreach (var item in source.Value)
		{
			yield return item;
		}
		if (concats == null)
		{
			if (i == 4) { yield return concat4!; i--; }
			if (i == 3) { yield return concat3!; i--; }
			if (i == 2) { yield return concat2!; i--; }
			if (i == 1) { yield return concat1!; i--; }
			yield break;
		}
		for (i--; i >= 0; i--)
		{
			yield return concats[i];
		}
	}
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public interface IListLike<out T>
{
	int Count { get; }
	T this[int index] { get; }
}
public static class ListLike
{
	public static IListLike<T> ToListLike<T>(this IEnumerable<T> source) => source switch
	{
		null => throw new ArgumentNullException(nameof(source)),
		IList<T> list => new List<T>(list),
		IReadOnlyList<T> list => new ReadOnlyList<T>(list),
		_ => null!
	};

	sealed class List<T> : IListLike<T>
	{
		readonly IList<T> _list;
		public List(IList<T> list) => _list = list ?? throw new ArgumentNullException(nameof(list));
		public int Count => _list.Count;
		public T this[int index] => _list[index];
	}
	sealed class ReadOnlyList<T> : IListLike<T>
	{
		readonly IReadOnlyList<T> _list;
		public ReadOnlyList(IReadOnlyList<T> list) => _list = list ?? throw new ArgumentNullException(nameof(list));
		public int Count => _list.Count;
		public T this[int index] => _list[index];
	}
}