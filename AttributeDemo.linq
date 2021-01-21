<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	var ss = new SampleClass() { Id = 1 };
	
	PrintAuthorInfo(ss.GetType());
}

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = true)]
public class AuthorAttribute : System.Attribute
{
    public string name;
    public double version;

    public AuthorAttribute(string name)
    {
        this.name = name;
        version = 1.0;
    }
}

[Author("liyue")]
[Author("wwmin", version = 1.1)]
class SampleClass
{
    public int Id { get; set; }
}

private static void PrintAuthorInfo(System.Type t)
{
    $"Author infomation for {t}".Dump();
    if (t.IsDefined(typeof(AuthorAttribute), true))
    {
        var attributes = t.GetCustomAttributes<AuthorAttribute>().Select(p => p.name);
        string.Join(",", attributes).Dump();
    }
    //Using reflection.
    System.Attribute[] attrs = System.Attribute.GetCustomAttributes(t);//Reflect.
    foreach (System.Attribute attr in attrs)
    {
        if (attr is AuthorAttribute a)
        {
            $"{a.name},version {a.version:f}".Dump();
        }
    }
}
