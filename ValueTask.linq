<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

async Task Main()
{
	ValueTask<int> Foo()
	{
	    return ValueTask.FromResult(1);
	}
	
	async ValueTask<int> Caller()
	{
	    return await Foo();
	}
	
	var i = await Caller();
	
	i.Dump();
}
await Main();

// You can define other methods, fields, classes and namespaces here
