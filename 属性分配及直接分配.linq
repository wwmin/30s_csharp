<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	int num = 1000_000_000;
	
	Stopwatch st = new Stopwatch();
	st.Start();
	for (int i = 0; i < num; i++)
	{
	    TestClass.Name = "value";
	}
	
	st.Stop();
	st.ElapsedMilliseconds.Dump("属性");
	
	st.Reset();
	st.Restart();
	for (int i = 0; i < num; i++)
	{
	    TestClass.SureName = "value";
	}
	st.Stop();
	st.ElapsedMilliseconds.Dump("字段");
}

public class TestClass
{
    public static string? Name { get; set; }//属性(自动属性)
    public static string? SureName;//字段
}
