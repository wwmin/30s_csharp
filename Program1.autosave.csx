public List<double> GetCountNum(int year)
{
    List<double> list = new List<double>();
    for (int i = 0; i < year; i++)
    {
        list.Add(0);
    }
    return list;
}
var list = GetCountNum(4);
list[0] = 1d;
list[0].Dump();
        