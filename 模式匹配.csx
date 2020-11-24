//switch 模式匹配
int Match(int v)
{
    int i = v switch
    {
        > 3 => 5,
        < 3 and > 1 => 6,
        < 3 and > -5 => 7,
        < 3 => 8,
        _ => 9
    };
    return i;
}
//Match(1).Dump();
//Match(3).Dump();
//Match(5).Dump();
//模式匹配嵌套
int MatchMore(int m)
{
    return m switch
    {
        < 0 => Match(m) switch
        {
            0 => MatchMore(m),
            _ => Match(m)
        },
        _ => 0
    };
}

MatchMore(-1);

//属性匹配
Type type = Type.GetType("System.String")!;
if (type is not null and { FullName: "System.String" })
{
    "It's type is System.String.".Dump();
}

//int32
int i = 1;
Type t = i.GetType();
if (t is not null and { FullName: "System.Int32" })
{
    $"It's type is {t.FullName}".Dump();
}

//
int j = 1;
if (j is int and > 0)
{
    j.Dump("j is int and >0");
}