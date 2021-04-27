<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Order</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

//Span
//此案例对比了string , Regex , Span , StringBuilder 方法的性能差异
void Main()
{
	Util.AutoScrollResults = true;
	//LinqPad: 如果测试需要将linqpad的/o- 模式改为 /o+
	//DonNet: 先发布,在调用. 即:dotnet build -c Release , 然后 dotnet yourproject.dll
	BenchmarkRunner.Run<FilterCodeBlocksBenchmarks>();
	//结果: (此结果使用的是较大文件测试出的,与本文给出的结果可能存在量级上的差异,也可能会存在测试数据量太小导致的结果不一样)
	// |                                Method |        Mean |     Error |      StdDev | Rank |      Gen 0 |      Gen 1 |      Gen 2 |   Allocated |
	// |-------------------------------------- |------------:|----------:|------------:|-----:|-----------:|-----------:|-----------:|------------:|
	// | FilterCodeBlockBySpanAndStringBuilder |    579.5 μs |  11.45 μs |    22.60 μs |    1 |   215.8203 |   214.8438 |   214.8438 |  1512.75 KB |
	// |                FilterCodeBlockByRegex |  5,216.7 μs |  36.13 μs |    30.17 μs |    2 |   164.0625 |   164.0625 |   164.0625 |   700.22 KB |
	// |      FilterCodeBlockBySpanAndToString | 25,345.8 μs | 882.51 μs | 2,489.15 μs |    3 |  1531.2500 |  1468.7500 |  1468.7500 | 32814.82 KB |
	// |               FilterCodeBlockByString | 42,285.1 μs | 844.19 μs | 1,237.41 μs |    4 | 10846.1538 | 10538.4615 | 10384.6154 | 62713.91 KB |
}

public class FilterCodeBlocks
{
	private static string _startTag = "<pre";
	private static string _endTag = "</pre>";
	private static int _startTagLength => _startTag.Length;
	private static int _endTagLength => _endTag.Length;
	private static Regex _codeTag = new Regex("(<pre(.*?)>)(.|\n)*?(</pre>)");

	public string FilterCodeBlockByString(string content)
	{
		var result = "";
		while (true)
		{
			var startPos = content.IndexOf(_startTag, StringComparison.CurrentCulture);
			if (startPos == -1) break;
			var content2 = content.Substring(startPos + _startTagLength, content.Length - startPos - _startTagLength);
			var endPos = content2.IndexOf(_endTag, StringComparison.CurrentCulture);
			result += content.Substring(0, startPos);
			content = content2.Substring(endPos + _endTagLength, content2.Length - endPos - _endTagLength);
		}
		result += content;
		return result;
	}

	public string FilterCodeBlockBySpanAndToString(ReadOnlySpan<char> content)
	{
		string result = "";
		ReadOnlySpan<char> contentSpan2 = new ReadOnlySpan<char>();
		int startPos = 0;
		int endPos = 0;

		ReadOnlySpan<char> startTagSpan = _startTag.AsSpan();
		ReadOnlySpan<char> endTagSpan = _endTag.AsSpan();
		while (true)
		{
			startPos = content.IndexOf(startTagSpan);
			if (startPos == -1) break;
			contentSpan2 = content.Slice(startPos + _startTagLength, content.Length - startPos - _startTagLength);
			endPos = contentSpan2.IndexOf(endTagSpan);
			result += content.Slice(0, startPos).ToString();
			content = contentSpan2.Slice(endPos + _endTagLength, contentSpan2.Length - endPos - _endTagLength);
		}
		result += content.ToString();
		return result;
	}

	public string FilterCodeBlockBySpanAndStringBuilder(ReadOnlySpan<char> content)
	{
		StringBuilder result = new StringBuilder(content.Length);
		ReadOnlySpan<char> contentSpan2 = new ReadOnlySpan<char>();
		int startPos = 0;
		int endPos = 0;

		ReadOnlySpan<char> startTagSpan = _startTag.AsSpan();
		ReadOnlySpan<char> endTagSpan = _endTag.AsSpan();
		while (true)
		{
			startPos = content.IndexOf(startTagSpan);
			if (startPos == -1) break;
			contentSpan2 = content.Slice(startPos + _startTagLength, content.Length - startPos - _startTagLength);
			endPos = contentSpan2.IndexOf(endTagSpan);
			result.Append(content.Slice(0, startPos));
			content = contentSpan2.Slice(endPos + _endTagLength, contentSpan2.Length - endPos - _endTagLength);
		}
		result.Append(content);
		return result.ToString();
	}

	public string FilterCodeBlockByRegex(string content)
	{
		return _codeTag.Replace(content, string.Empty);
	}
}

[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public class FilterCodeBlocksBenchmarks
{

	public static string s = "<p>我们通过IndexWriterConfig 可以设置IndexWriter的属性，" +
							   "已达到我们希望构建索引的需求，这里举一些属性，这些属性可以影响到IndexWriter写入索引的速度：" +
							   "</p>\n<div class=\"cnblogs_code\">\n<pre>IndexWriterConfig.setRAMBufferSizeMB" +
							   "(<span style=\"color: #0000ff;\">double</span><span style=\"color: #000000;\">);" +
							   "\nIndexWriterConfig.setMaxBufferedDocs(</span><span style=\"color: #0000ff;\">int</span><span " +
							   "style=\"color: #000000;\">);\nIndexWriterConfig.setMergePolicy(MergePolicy)</span></pre>\n</div>\n<p>" +
							   "setRAMBufferSizeMB()&nbsp;是设置";

	FilterCodeBlocks FilterCodeBlocks = new FilterCodeBlocks();
	[Benchmark]
	public void FilterCodeBlockByString()
	{
		FilterCodeBlocks.FilterCodeBlockByString(s);
	}
	[Benchmark]
	public void FilterCodeBlockBySpanAndToString()
	{
		FilterCodeBlocks.FilterCodeBlockBySpanAndToString(s);
	}

	[Benchmark]
	public void FilterCodeBlockBySpanAndStringBuilder()
	{
		FilterCodeBlocks.FilterCodeBlockBySpanAndStringBuilder(s);
	}


	[Benchmark]
	public void FilterCodeBlockByRegex()
	{
		FilterCodeBlocks.FilterCodeBlockByRegex(s);
	}
}

/*
Span< T > : 结构体，值类型 。相当于C++ 中的指针，它是一段连续内存的引用，也就是一段连续内存的首地址。有了Span< T >，我们就可以不在unsafe的代码块中写指针了。Span< char > 相对于 string 也就具有很大的性能优势。
举个栗子： string.Substring() 函数，实际上是在堆中额外创建了一个新的 string 对象，把字符 copy 过去，再返回这个对象的引用。而相对应的 Span< T > 的Slice() 函数则是直接在内存中返回子串的首地址引用，此过过程几乎不分配内存，并且十分高效。
后面的优化也是使用Span< T > 的Slice() 代替了 string 的SubString() 。
简单看下 Span< T > 的源码，就可以窥见 Span< T > 的奥秘：
public readonly ref partial struct Span<T>
{
	/// <summary>A byref or a native ptr.</summary>
	internal readonly ByReference<T> _pointer;
	/// <summary>The number of elements this Span contains.</summary>

	private readonly int _length;
         
        ....
        
        public Span(T[] array)
	{
		if (array == null)
		{
			this = default;
			return; // returns default
		}
		if (default(T) == null && array.GetType() != typeof(T[]))
			ThrowHelper.ThrowArrayTypeMismatchException();

		_pointer = new ByReference<T>(ref Unsafe.As<byte, T>(ref array.GetRawSzArrayData()));
		_length = array.Length;
	}
}

Span< T > 内部主要就是一个ByReference< T > 类型的对象，实际上就是ref T: 一个类型的引用，它和C 的int* char* 如出一折。Span < T > 也就是建立 ref 的基础上。
限定长度: _length ，就像 C 中定义指针，在使用前需要 malloc 或者 alloc 分配固定长度的内存。关于Span< T > 更多详细知识：
https://msdn.microsoft.com/en-us/magazine/mt814808.aspx
*/
/*
Span< T > 的特色
虽然Span< T > 的性能十分出色 ，但是 string 有太多完善的接口，string 是为了简化你的代码让你更加舒服的使用字符串，所以牺牲了性能。因此 在对计算机消耗要求十分的严苛的情况下，尝试使用Span< T > ,大多数情况下，简短的string 已经能满足需求。我的认知下的Span< T >的特色：

Span< T >的定义方法多种多样，可以直接 ( i ) 像定义数组那样 : Span<int> a = new int[10]; ( ii ) 在构造函数中直接传入 数组（指针+长度）Span<T> a = new Span<T>(T[]),Span<T> a = new Span<T>(void*,length) ; ( iii )可以直接在栈中分配内存：Span<char> a = stackalloc char[10]; 在C# 8.0中才可以，这样的写法真是高大上。

Span< T > 只能存在于栈中，而不能放在堆中。因为 ( i ) GC 在堆中很难跟踪这些指针， ( ii ) 在堆中会出现多线程， 如果两个线程的两个Span< T >指向了同一个地址，那就糟了。

可以使用 Memory< T > 代替 Span< T >在堆中使用。

所有 string 的接口都可以用 Span< char > 来实现，这似乎又回到了原始的C语言时代。

Span < T > 有个兄弟叫 ReadOnlySpan< T > 。
*/