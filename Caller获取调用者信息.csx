using System.Linq.Expressions;
using System.Runtime.CompilerServices;

//利用Caller特性和可选参数获得调用者的信息,包括:CallerMemberName,CallerLineNumber,CallerFilePath
public void Log(string msg, [CallerMemberName] string? name = null, [CallerLineNumber] int line = -1,
[CallerFilePath] string? path = null)
{
    msg.Dump($"log: line={line},path={path},name={name}");
}

void MyMethod()
{
    Log("Test Caller");
}
MyMethod();