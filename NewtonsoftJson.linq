<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	var a = new List<S>();
	a.Add(new S(){Id = 1, Name = "wwmin"});
	a.Add(new S(){Id = 2, Name = "liyue"});
	
	var ss="["+ JsonConvert.SerializeObject(a) + "]";
	ss.Dump();
}

//#r "nuget:Newtonsoft.Json/12.0.3"

//using Newtonsoft.Json;

public class S{
 public int Id { get; set; }
 public string Name { get; set; }
}
