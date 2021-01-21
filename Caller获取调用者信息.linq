<Query Kind="Program">
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	//利用Caller特性和可选参数获得调用者的信息,包括:CallerMemberName,CallerLineNumber,CallerFilePath
	void Log(string msg, [CallerMemberName] string? name = null, [CallerLineNumber] int line = -1,
	[CallerFilePath] string? path = null)
	{
	    msg.Dump($"log: line={line},path={path},name={name}");
	}
	
	void MyMethod()
	{
	    Log("Test Caller");
	}
	MyMethod();
}

// You can define other methods, fields, classes and namespaces here
