var arr = Enumerable.Range(1, 10).ToArray();

var first3 = arr[0..3];//提取前3个
first3.Dump();

var last3 = arr[^3..];//提取最后3个
last3.Dump();

var last2 = arr[^3..^1];//提取倒数第三个和第二个
last2.Dump();

var midd3 = arr[4..7];//提取中间的3个
midd3.Dump();

//原理

public static T[] GetSubArray</*[Nullable(2)]*/ T>(T[] array, Range range)
{
    ValueTask<int, int> offsetAndLength = range.GetOffsetAndLength(array.Length)!;
    int item1 = offsetAndLength.Item1;
    int item2 = offsetAndLength.Item2;
    T[] array3 = new T[item2];
    Buffer.MemoryCopy<T>(Unsafe.As<byte, T>(array3.GetRawSzArrayData()), Unsafe.Add<T>(Unsafe.As<byte, T>(array.GetRawSzArrayData()), item), (ulong)item2);
    return array3;
}

public readonly struct Range : IEquatable<Range>
{
    public Index Start { get; }
    public Index End { get; }

    public Range(Index start, Index end)
    {
        this.Start = start;
        this.End = end;
    }

    public ValueTuple<int, int> GetOffsetAndLength(int length)
    {
        Index start = this.Start;
        int num;
        if (start.IsFromEnd)
        {
            num = length - start.Value;
        }
        else
        {
            num = start.Value;
        }
        Index end = this.End;
        int num2;
        if (end.IsFromEnd)
        {
            num2 = length - end.Value;
        }
        else
        {
            num2 = end.Value;
        }
        return new ValueTuple<int, int>(num, num2 - num);
    }
}