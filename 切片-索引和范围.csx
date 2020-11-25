//切片
var arr = Enumerable.Range(1, 10).ToArray();
var all = arr[..];
string.Join(',', all).Dump();
var first3 = arr[0..3];//提取前3个
string.Join(',', first3).Dump();

var last3 = arr[^3..];//提取最后3个
string.Join(',', last3).Dump();

var last2 = arr[^3..^1];//提取倒数第三个和第二个
string.Join(',', last2).Dump();

var midd3 = arr[4..7];//提取中间的3个
string.Join(',', midd3).Dump();

//Index结构体和Hat运算符(^)
int[] arr2 = new int[] { 1, 3, 5, 7, 9 };
var x1 = arr2[4];
var x2 = arr2[arr2.Length - 1];
var x3 = arr2[^1];

//使用Index
Index lastItem = new Index(4, false);//第二个参数bool fromEnd
var x4 = arr2[lastItem];
Index lastItem2 = new Index(1, true);
var x5 = arr2[lastItem2];

var lastItem3 = ^1;
var x6 = arr2[lastItem3];
$"{x1}-{x2}-{x3}-{x4}-{x5}-{x6}".Dump();

//使用Range
var array = new string[]{
    "Item0",
    "Item1",
    "Item2",
    "Item3",
    "Item4",
    "Item5",
    "Item6",
    "Item7",
    "Item8",
    "Item9"
};

List<string> allItems = new List<string>();
foreach (var item in array[1..6])
{
    allItems.Add(item);
}

string.Join(',', allItems).Dump();

//将范围作为变量
Range range = 1..9;
string.Join(',', array[range]).Dump();

//Range使用范围: Array,String,Span<T>,ReadOnlySpan<T>. 不能用在List或IEnumerable<T>上
//但是list有GetRange(index,count)方法
//Enumerable有Range方法

//list中的GetRange(index,count)
var listRange = allItems.GetRange(1, 4);
string.Join(',', listRange).Dump();
//Enumerable中的Range
var enumerableRange = Enumerable.Range(1, 4);
enumerableRange.Dump();

//string中使用Range
string s = "0123456789";
string r = s[1..3];
r.Dump();

//Span中Range
void TestSpanRange()
{
    Span<string> ss = new Span<string>(allItems.ToArray());
    string.Join(',', ss[1..4].ToArray()).Dump();
}
TestSpanRange();
//ReadOnlySpan<T>中Range
void TestReadOnlyRange()
{
    ReadOnlySpan<string> ss = new ReadOnlySpan<string>(allItems.ToArray());
    string.Join(',', ss[1..4].ToArray()).Dump();
}
TestReadOnlyRange();


//原理
public class S
{
//    public static T[] GetSubArray</*[Nullable(2)]*/ T>(T[] array, Range range)
//    {
//        ValueTask<int, int> offsetAndLength = range.GetOffsetAndLength(array.Length)!;
//        int item1 = offsetAndLength.Item1;
//        int item2 = offsetAndLength.Item2;
//        T[] array3 = new T[item2];
//        Buffer.MemoryCopy<T>(Unsafe.As<byte, T>(array3.GetRawSzArrayData()), Unsafe.Add<T>(Unsafe.As<byte, T>(array.GetRawSzArrayData()), item), (ulong)item2);
//        return array3;
//    }

//    public readonly struct Range : IEquatable<Range>
//    {
//        public Index Start { get; }
//        public Index End { get; }

//        public Range(Index start, Index end)
//        {
//            this.Start = start;
//            this.End = end;
//        }

//        public ValueTuple<int, int> GetOffsetAndLength(int length)
//        {
//            Index start = this.Start;
//            int num;
//            if (start.IsFromEnd)
//            {
//                num = length - start.Value;
//            }
//            else
//            {
//                num = start.Value;
//            }
//            Index end = this.End;
//            int num2;
//            if (end.IsFromEnd)
//            {
//                num2 = length - end.Value;
//            }
//            else
//            {
//                num2 = end.Value;
//            }
//            return new ValueTuple<int, int>(num, num2 - num);
//        }
//    }
}
