<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

async Task Main()
{
	//namespace System.Threading.Tasks
	
	//从 ValueTask => Task
	async Task<int> ShowTaskAsync(){
	    return await ValueTask.FromResult(1).AsTask();
	}
	
	(await ShowTaskAsync()).Dump();
}

await Main();
// You can define other methods, fields, classes and namespaces here
