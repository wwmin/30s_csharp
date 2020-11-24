using System.Runtime.InteropServices;

void Main()
{

    var nums = Enumerable.Range(1, 5).ToArray<int>();
    var span = new Span<int>(nums);
    span.ToArray().Dump();
}

void TestAsSpan()
{
    var word = "10+10";
    var splitIndex = word.IndexOf("+");
    //var s = word.Substring(0,2);//没有span截取string需要使用substring
    var sp = word.AsSpan();
    var num1 = int.Parse(word.AsSpan(0, splitIndex));
    var num2 = int.Parse(word.AsSpan(splitIndex));
    var sum = num1 + num2;
    sum.Dump();
}

void TestListSpan()
{
    List<string> list = Enumerable.Range(1, 5).Select(p => p.ToString()).ToList();
    var span = CollectionsMarshal.AsSpan(list);
    var str = span.Slice(0, span.Length);
    str.ToArray().Dump();
}

Main();
TestAsSpan();
TestListSpan();
