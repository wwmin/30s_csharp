using System.Diagnostics;
int num = 1000_000_000;
public class TestClass
{
    public static string? Name { get; set; }
    public static string? SureName;
}

Stopwatch st = new Stopwatch();
st.Start();
for (int i = 0; i < num; i++)
{
    TestClass.Name = "value";
}

st.Stop();
st.ElapsedMilliseconds.Dump();

st.Reset();
st.Restart();
for (int i = 0; i < num; i++)
{
    TestClass.SureName = "value";
}
st.Stop();
st.ElapsedMilliseconds.Dump();