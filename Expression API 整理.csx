//Expression API 整理 //参考URL: https://docs.microsoft.com/zh-cn/dotnet/api/system.linq.expressions.expression?view=net-5.0
using System.Diagnostics;
using System.Linq.Expressions;
//1. 构造函数
//Expression()
//Expression(ExpressionType,Type)
//2. 属性
//CanReduce
//NodeType
//Type

//3. Expression 分类与操作运算符
//Expression 操作说明
//BinaryExpression 表示具有二进制运算符的表达式
//UnaryExpression 表示具有一元运算符的表达式
//BlockExpression 表示包含一个表达式序列的块，表达式中可定义变量
//ConditionalExpression 表示具有条件运算符的表达式
//ConstantExpression 表示具有常数值的表达式
//DefaultExpression 表示一个类型或空表达式的默认值
//DynamicExpression 表示一个动态操作
//GotoExpression 表示无条件跳转。 这包括返回语句，break 和 continue 语句以及其他跳转。
//IndexExpression 表示对一个属性或数组进行索引
//InvocationExpression 表示一个将委托或 lambda 表达式应用到一个自变量表达式列表的表达式
//LabelExpression 表示一个标签，可以放置在任何 Expression 上下文。 如果它跳转到，它会提供相应的值 GotoExpression。 否则，它接收中的值 DefaultValue。 如果 Type 等于 System.Void，则应提供任何值
//LambdaExpression 介绍 lambda 表达式。 它捕获一个类似于 .NET 方法主体的代码块
//ListInitExpression 表示具有集合初始值设定项的构造函数调用
//LoopExpression 表示无限循环。 可通过“中断”退出该循环
//MemberExpression 表示访问字段或属性
//MemberInitExpression 表示调用构造函数并初始化新对象的一个或多个成员
//MethodCallExpression 表示对静态方法或实例方法的调用
//NewArrayExpression 表示创建一个新数组，并可能初始化该新数组的元素
//NewExpression 表示一个构造函数调用
//ParameterExpression 表示一个命名的参数表达式
//SwitchExpression 表示通过将控制权传递给处理多个选择的控件表达式 SwitchCase
//TryExpression 表示一个 try/catch/finally/fault 块
//TypeBinaryExpression 表示表达式和类型之间的操作
/*
 * 定义扩展
 */
/// <summary>
/// 扩展IEnumerable ForEach
///<para>遍历IEnumerable 执行action</para>
/// </summary>
public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
{
    using IEnumerator<TSource> enumerator = source.GetEnumerator();
    while (enumerator.MoveNext())
    {
        TSource current = enumerator.Current;
        action(current);
    }
}
//4. 方法
//Accept(ExpressionVisitor) //调度到此节点类型的特定 Visit 方法。 例如，MethodCallExpression 调用 VisitMethodCall(MethodCallExpression)。

//示例用到的公共test class
public class Person
{
    public string Name { get; set; } = null!;
    public string GetName() => this.Name;
    public string GetOrSetName(string name)
    {
        if (this.Name == null) this.Name = name;
        return this.Name;
    }
}

//Add(Expression, Expression)    创建一个表示不进行溢出检查的算术加法运算的 BinaryExpression。
Expression sumExpr = Expression.Add(Expression.Constant(1), Expression.Constant(2));
Console.WriteLine(sumExpr.ToString());//(1+2)
Console.WriteLine(Expression.Lambda<Func<int>>(sumExpr).Compile()());//3



//Add(Expression, Expression, MethodInfo) 创建一个表示不进行溢出检查的算术加法运算的 BinaryExpression。 可指定实现方法。
public class Plus
{
    //下面表达式指定加法的实现
    public static int AddOne(int a, int b)
    {
        return a + b + 1;
    }
}
Expression sumExpr2 = Expression.Add(Expression.Constant(1), Expression.Constant(2), typeof(Plus).GetMethod("AddOne"));
Console.WriteLine(sumExpr2.ToString());
Console.WriteLine(Expression.Lambda<Func<int>>(sumExpr2).Compile()());

//AddAssign(Expression, Expression) 创建一个表示不进行溢出检查的加法赋值运算的 BinaryExpression
ParameterExpression variableExpr = Expression.Variable(typeof(int), "sampleVar");//the parameter expression is used to create a variable;
BlockExpression addAssignExpr = Expression.Block(
new ParameterExpression[] { variableExpr },
Expression.Assign(variableExpr, Expression.Constant(1)),
Expression.AddAssign(
    variableExpr,
    Expression.Constant(2)
    )
);
addAssignExpr.Expressions.ForEach(p => p.ToString().Dump());//(sampleVar = 1),(sampleVar +=2)
Console.WriteLine(Expression.Lambda<Func<int>>(addAssignExpr).Compile()());//3

//AddAssign(Expression, Expression, MethodInfo)  创建一个表示不进行溢出检查的加法赋值运算的 BinaryExpression。可指定实现
//参Add(Expression, Expression, MethodInfo)

//AddAssign(Expression, Expression, MethodInfo, LambdaExpression) 创建一个表示不进行溢出检查的加法赋值运算的 BinaryExpression。可指定实现, 可指定类型转换函数


//AddAssignChecked(Expression, Expression)    创建一个表示进行溢出检查的加法赋值运算的 BinaryExpression。
ParameterExpression variableExprWithChecked = Expression.Variable(typeof(int), "sampleVar");//the parameter expression is used to create a variable;
BlockExpression addAssignExprWithChecked = Expression.Block(
new ParameterExpression[] { variableExprWithChecked },
Expression.Assign(variableExprWithChecked, Expression.Constant(2147483647)),
Expression.AddAssign(
    variableExprWithChecked,
    Expression.Constant(1)
    )
);
addAssignExprWithChecked.Expressions.ForEach(p => p.ToString().Dump());//(sampleVar = 2147483647),(sampleVar +=1)
Console.WriteLine(Expression.Lambda<Func<int>>(addAssignExprWithChecked).Compile()());//-2147483648

//AddChecked(Expression, Expression)    创建一个表示进行溢出检查的算术加法运算的 BinaryExpression。
//同Add

//And(Expression, Expression)    创建一个表示按位 BinaryExpression 运算的 AND。
Expression andExprBinary = Expression.And(
    Expression.Constant(true),
    Expression.Constant(false)
);
Console.WriteLine(andExprBinary.ToString());//(True And False)
Expression.Lambda<Func<bool>>(andExprBinary).Compile()().Dump();//False

//AndAlso(Expression, Expression)    创建一个 BinaryExpression，它表示仅在第一个操作数的计算结果为 AND 时才计算第二个操作数的条件 true 运算。
Expression andAlsoExpr = Expression.AndAlso(
    Expression.Constant(false),
    Expression.Constant(true)
);
Console.WriteLine(andAlsoExpr.ToString());//(False AndAlso True)
Console.WriteLine(Expression.Lambda<Func<bool>>(andAlsoExpr).Compile()());//False

//AndAssign 创建一个表示按位 AND 赋值运算的 BinaryExpression。
ParameterExpression variableAndAssignExpr1 = Expression.Variable(typeof(bool), "sampleVar1");//the parameter expression is used to create a variable;
BlockExpression andAlsoAssignExpr = Expression.Block(
new ParameterExpression[] { variableAndAssignExpr1 },
Expression.Assign(variableAndAssignExpr1, Expression.Constant(true)),
Expression.AndAssign(
    variableAndAssignExpr1,
    Expression.Constant(false)
    )
);
andAlsoAssignExpr.Expressions.ForEach(p => p.ToString().Dump());//(sampleVar1 = True) , (sampleVar1 &&= False)
Console.WriteLine(Expression.Lambda<Func<bool>>(andAlsoAssignExpr).Compile()());//False


//ArrayAccess   创建一个用于访问数组的 IndexExpression。
//下面的代码示例演示如何使用方法更改多维数组中元素的值 ArrayAccess 。
ParameterExpression arrayExpr = Expression.Parameter(typeof(int[,]), "Array");
ParameterExpression firstIndexExpr = Expression.Parameter(typeof(int), "FirstIndex");
ParameterExpression secondIndexExpr = Expression.Parameter(typeof(int), "SecondIndex");
// The list of indexes.
List<Expression> indexes = new List<Expression> { firstIndexExpr, secondIndexExpr };
// This parameter represents the value that will be added to a corresponding array element.
ParameterExpression valueExpr = Expression.Parameter(typeof(int), "Value");
Expression arrayAccessExpr = Expression.ArrayAccess(
    arrayExpr,
    indexes
);
Expression<Func<int[,], int, int, int, int>> lambdaExpr =
    Expression.Lambda<Func<int[,], int, int, int, int>>(
        Expression.Assign(arrayAccessExpr, Expression.Add(arrayAccessExpr, valueExpr)),
        arrayExpr,
        firstIndexExpr,
        secondIndexExpr,
        valueExpr
);
Console.WriteLine(arrayAccessExpr.ToString());//Array[FirstIndex, SecondIndex]
Console.WriteLine(lambdaExpr.ToString());//(Array, FirstIndex, SecondIndex, Value) => (Array[FirstIndex, SecondIndex] = (Array[FirstIndex, SecondIndex] + Value))
int[,] sampleArray = {{10,  20,   30},
                      {100, 200, 300}};
Console.WriteLine(lambdaExpr.Compile().Invoke(sampleArray, 1, 1, 5));//205

//下面的代码示例演示如何使用方法更改数组元素的值 ArrayAccess 。
ParameterExpression arrayExprOne = Expression.Parameter(typeof(int[]), "Array");
ParameterExpression indexExprOne = Expression.Parameter(typeof(int), "Index");
ParameterExpression valueExprOne = Expression.Parameter(typeof(int), "Value");
Expression arrayAccessExprOne = Expression.ArrayAccess(
    arrayExprOne,
    indexExprOne
);
Expression<Func<int[], int, int, int>> lambdaExprOne = Expression.Lambda<Func<int[], int, int, int>>(
    Expression.Assign(arrayAccessExprOne, Expression.Add(arrayAccessExprOne, valueExprOne)),
    arrayExprOne,
    indexExprOne,
    valueExprOne
);
Console.WriteLine(arrayAccessExprOne.ToString());//Array[Index]
Console.WriteLine(lambdaExprOne.ToString());//(Array, Index, Value) => (Array[Index] = (Array[Index] + Value))
Console.WriteLine(lambdaExprOne.Compile().Invoke(new int[] { 10, 20, 30 }, 0, 5));//15



//ArrayIndex     创建一个表示应用数组索引运算符的 Expression。
string[,] gradeArray = { { "chemistry", "history", "mathematics" }, { "78", "61", "82" } };
Expression arrayExpression = Expression.Constant(gradeArray);

// Create a MethodCallExpression that represents indexing
// into the two-dimensional array 'gradeArray' at (0, 2).
// Executing the expression would return "mathematics".
MethodCallExpression methodCallExpression =
    Expression.ArrayIndex(
        arrayExpression,
        Expression.Constant(0),
        Expression.Constant(2));

Console.WriteLine(methodCallExpression.ToString());//value(System.String[,]).Get(0, 2)

//ArrayLength(Expression) 方法  创建一个 UnaryExpression，它表示获取一维数组的长度的表达式。
int[] gradeIntArray = { 1, 2, 3, 4 };
Expression arrayIntExpression = Expression.Constant(gradeIntArray);
var arrayIntLength = Expression.ArrayLength(arrayIntExpression);
arrayIntLength.ToString().Dump();//ArrayLength(value(System.Int32[]))

//Assign    创建一个表示赋值运算的 BinaryExpression。
ParameterExpression variableAssignExpr = Expression.Variable(typeof(String), "sampleVar");
Expression assignSimpleExpr = Expression.Assign(
    variableAssignExpr,
    Expression.Constant("Hello World!")
    );
Expression blockAssignExpr = Expression.Block(
    new ParameterExpression[] { variableAssignExpr },
    assignSimpleExpr
    );
Console.WriteLine(assignSimpleExpr.ToString());//(sampleVar = "Hello World!")
Console.WriteLine(Expression.Lambda<Func<String>>(blockAssignExpr).Compile()());//Hello World!


//Bind   创建一个表示成员初始化的 MemberAssignment。
public class TestBindMember
{
    public int Num { get; set; }
}
var variableBindExpr = Expression.Parameter(typeof(int), "number");
var newTestBindMember = Expression.New(typeof(TestBindMember));
//MemberInfo testBindMemberInfo = testBindMember.GetType().GetMember("Num").FirstOrDefault()!;
var bindsExpr = new[] {
    Expression.Bind(typeof(TestBindMember).GetProperty("Num")!,variableBindExpr)
};
var bindsBody = Expression.MemberInit(newTestBindMember, bindsExpr);
var bindsFunc = Expression.Lambda<Func<int, TestBindMember>>(bindsBody, new[] { variableBindExpr }).Compile();
bindsBody.ToString().Dump();//new TestBindMember() {Num = number}
var testBindMember = bindsFunc(1111);
Console.WriteLine(testBindMember.GetType() == typeof(TestBindMember));//True
testBindMember.Num.Dump();//111


//Block    创建一个 BlockExpression。当执行块表达式时，它将返回块中最后一个表达式的值
ParameterExpression varExpr = Expression.Variable(typeof(int), "sampleVar");
BlockExpression blockAssignAddExpr = Expression.Block(
    new ParameterExpression[] { varExpr },
    Expression.Assign(varExpr, Expression.Constant(1)),
    Expression.Add(varExpr, Expression.Constant(5))
);
blockAssignAddExpr.Expressions.ForEach(p => p.ToString().Dump()); //(sampleVar = 1) , (sampleVar + 5)
var compileBlockExpr = Expression.Lambda<Func<int>>(blockAssignAddExpr).Compile()().Dump();//6

"创建块表达式".Dump();
Person personBlock = new Person();
//personBlock.Name = "wwmin";
Type[] types = new Type[1];
types[0] = typeof(string);
BlockExpression blockExpr = Expression.Block(
    Expression.Call(
        null,
        typeof(Console).GetMethod("Write", new Type[] { typeof(String) })!,
        Expression.Constant("Hello World!")
    )
);
blockExpr.Expressions.ForEach(p => p.ToString().Dump());//Write("Hello World!")
Expression.Lambda<Action>(blockExpr).Compile()();//Hello World!


//Break   创建一个表示 break 语句的 GotoExpression。结合Loop 和 IfThenElse 等循环判断语句
ParameterExpression valueBreak = Expression.Parameter(typeof(int), "value");
ParameterExpression resultBreak = Expression.Parameter(typeof(int), "result");
LabelTarget labelBreak = Expression.Label(typeof(int));
BlockExpression blockBreak = Expression.Block(
    new[] { resultBreak },
    Expression.Assign(resultBreak, Expression.Constant(1)),
        Expression.Loop(
            Expression.IfThenElse(
                Expression.GreaterThan(valueBreak, Expression.Constant(1)),
                Expression.MultiplyAssign(resultBreak,
                    Expression.PostDecrementAssign(valueBreak)),
                Expression.Break(labelBreak, resultBreak)
            ),
        labelBreak
    )
);
blockBreak.Expressions.ForEach(p =>
{
    if (p is LoopExpression loopExpression) loopExpression.Body.ToString().Dump();//IIF((value > 1), (result *= value--), break UnnamedLabel_0 (result))
    else p.ToString().Dump();//(result = 1)
});
Expression.Lambda<Func<int, int>>(blockBreak, valueBreak).Compile()(5).Dump(); //120


//Call    创建一个 MethodCallExpression。
public class SmapleClass
{
    public int AddIntegers(int arg1, int arg2)
    {
        return arg1 + arg2;
    }
}
Expression callExpr = Expression.Call(
    Expression.New(typeof(SmapleClass)),
    typeof(SmapleClass).GetMethod("AddIntegers", new Type[] { typeof(int), typeof(int) })!,
    Expression.Constant(1),
    Expression.Constant(2)
    );
callExpr.ToString().Dump();//new SmapleClass().AddIntegers(1, 2)
Expression.Lambda<Func<int>>(callExpr).Compile()().Dump();//3
//创建不带参数调用的表达式
var callSampleStringExpr = Expression.Call(Expression.Constant("sample string"), typeof(String).GetMethod("ToUpper", new Type[] { })!);
Expression.Lambda<Func<string>>(callSampleStringExpr).Compile()().Dump();//SAMPLE STRING

//Catch  (类似与 TryCatch)  创建一个表示 catch 语句的 CatchBlock。
TryExpression tryCatchExpr =
        Expression.TryCatch(
            Expression.Block(
                Expression.Throw(Expression.Constant(new DivideByZeroException())),
                Expression.Constant("try block")
            ),
            Expression.Catch(
                typeof(DivideByZeroException),
                Expression.Constant("catch block")
            )
        );
Expression.Lambda<Func<string>>(tryCatchExpr).Compile()().Dump();//catch block

//ClearDebug   创建一个用于清除序列点的 DebugInfoExpression。 //TODO

//Coalesce (?? 运算符)   创建一个表示合并运算的 BinaryExpression。
ParameterExpression varCoalesceExpr = Expression.Variable(typeof(string), "sampleVar");

Expression callPersonNameExpr = Expression.Call(
    Expression.New(typeof(Person)),
    typeof(Person).GetMethod("GetName")!
    );

var assignCoalesceNullExpr = Expression.Assign(varCoalesceExpr, Expression.Coalesce(callPersonNameExpr, Expression.Constant("??赋值成功")));

var blockCoalesceExpr = Expression.Block(new ParameterExpression[] { varCoalesceExpr }, assignCoalesceNullExpr);
blockCoalesceExpr.Expressions.ForEach(p => p.ToString().Dump());     //(sampleVar = (new Person().GetName() ?? "??赋值成功"))
Expression.Lambda<Func<string>>(blockCoalesceExpr).Compile()().Dump();   // ??赋值成功

//Condition    创建一个表示条件语句的 ConditionalExpression。
int conditionNum = 100;
Expression conditionExpr = Expression.Condition(
    Expression.Constant(conditionNum > 10),
    Expression.Constant("num is greater than 10"),
    Expression.Constant("num is smaller than 10")
    );
conditionExpr.ToString().Dump();//IIF(True, "num is greater than 10", "num is smaller than 10")
Expression.Lambda<Func<string>>(conditionExpr).Compile()().Dump();//num is greater than 10

//Constant    创建一个 ConstantExpression。
//创建一个可以为null的常量表达式
Expression constantNullalbeExpr = Expression.Constant(null, typeof(int?));
var coaleasceExpr = Expression.Coalesce(constantNullalbeExpr, Expression.Constant(123456));
var varCoalesceNullableExpr = Expression.Variable(typeof(int), "sampleVar");
var assignNullableExpr = Expression.Assign(varCoalesceNullableExpr, coaleasceExpr);
var blockAssignNullableExpr = Expression.Block(new ParameterExpression[] { varCoalesceNullableExpr }, assignNullableExpr);
blockAssignNullableExpr.Expressions.ForEach(p => p.ToString().Dump());//(sampleVar = (null ?? 123456))
Expression.Lambda<Func<int>>(blockAssignNullableExpr).Compile()().Dump();//123456


//Continue    创建一个表示 continue 语句的 GotoExpression。
void ContinueSample()
{
    LabelTarget breakLabel = Expression.Label();
    LabelTarget continueLabel = Expression.Label();
    Expression continueExpr = Expression.Continue(continueLabel);
    ParameterExpression count = Expression.Parameter(typeof(int));
    Expression loopExpr = Expression.Loop(
        Expression.Block(
            Expression.IfThen(
                Expression.GreaterThan(count, Expression.Constant(3)),
                Expression.Break(breakLabel)
            ),
            Expression.PreIncrementAssign(count),
            Expression.Call(
                null,
                typeof(Console).GetMethod("WriteLine", new Type[] { typeof(String) })!,
                Expression.Constant("Loop")
                ),
            continueExpr,
            Expression.PreDecrementAssign(count)
        ),
        breakLabel,
        continueLabel
    );
    Expression.Lambda<Action<int>>(loopExpr, count).Compile()(1);
}
ContinueSample();//Loop Loop Loop

//Convert     创建一个表示类型转换运算的 UnaryExpression。
void ConvertSample()
{
    Expression convertExpr = Expression.Convert(
                                    Expression.Constant(5.5),
                                    typeof(Int16)
                                );
    convertExpr.ToString().Dump();//Convert(5.5, Int16)
    Expression.Lambda<Func<Int16>>(convertExpr).Compile()().Dump();//5
}
ConvertSample();

//ConvertChecked  (类似与 Convert)  创建一个 UnaryExpression，它表示在目标类型发生溢出时引发异常的转换运算。

//DebugInfo  创建一个具有指定跨度的 DebugInfoExpression。//TODO

//Decrement  创建一个 UnaryExpression，它表示按 1 递减表达式值。
void DecrementSample()
{
    double num = 5.5;
    Expression decrementExpr = Expression.Decrement(Expression.Constant(num));
    decrementExpr.ToString().Dump();//Decrement(5.5)
    Expression.Lambda<Func<double>>(decrementExpr).Compile()().Dump();//4.5
}
DecrementSample();

//Default(Type)  创建一个 DefaultExpression，Type 属性设置为指定类型。
void DefaultSample()
{
    Expression defaultExpr = Expression.Default(typeof(byte));
    defaultExpr.ToString().Dump();//default(Byte)
    Expression.Lambda<Func<byte>>(defaultExpr).Compile()().Dump();//0
}
DefaultSample();

//Divide  创建一个表示算术除法运算的 BinaryExpression。
void DivideSample()
{
    Expression divideExpr = Expression.Divide(Expression.Constant(10.0), Expression.Constant(4.0));
    divideExpr.ToString().Dump();//(10 / 4)
    Expression.Lambda<Func<double>>(divideExpr).Compile()().Dump();//2.5
}
DivideSample();

//DivideAssign (类似与Divide) 创建一个表示不进行溢出检查的除法赋值运算的 BinaryExpression。

//Dynamic  创建一个表示动态操作的 DynamicExpression。 //TODO

//ElementInit  创建 ElementInit。
void ElementInitSample()
{
    string tree = "maple";
    MethodInfo addMethod = typeof(Dictionary<int, string>).GetMethod("Add")!;
    ElementInit elementInit = Expression.ElementInit(addMethod, Expression.Constant(tree.Length), Expression.Constant(tree));
    elementInit.ToString().Dump();//Void Add(Int32, System.String)(5, "maple")
}
ElementInitSample();

//Empty 创建具有 Void 类型的空表达式。 
//如果需要表达式，但不需要执行任何操作，则可以使用空表达式。 例如，你可以使用空表达式作为块表达式中的最后一个表达式。 在这种情况下，块表达式的返回值为 void。
void EmptySample()
{
    DefaultExpression emptyExpr = Expression.Empty();
    var emptyBlock = Expression.Block(emptyExpr);//return value is void
    emptyBlock.Expressions.ForEach(p => p.ToString().Dump());//default(Void)
}
EmptySample();

//Equal  创建一个表示相等比较的 BinaryExpression。
void EqualSample()
{
    Expression equalExpr = Expression.Equal(Expression.Constant(1, typeof(int)), Expression.Constant(2, typeof(int)));
    equalExpr.ToString().Dump();//(1 == 2)
    Expression.Lambda<Func<bool>>(equalExpr).Compile()().Dump();//False
}
EqualSample();

//ExclusiveOr    创建一个表示按位 BinaryExpression 运算的 XOR。
void ExclusiveOrSample()
{
    Expression exclusiveOrExpr = Expression.ExclusiveOr(Expression.Constant(5), Expression.Constant(3));
    // The XOR operation is performed as follows:
    // 101 xor 011 = 110
    exclusiveOrExpr.ToString().Dump();//(5 ^ 3)
    Expression.Lambda<Func<int>>(exclusiveOrExpr).Compile()().Dump();//6
}
ExclusiveOrSample();

//ExclusiveOr    创建一个表示按位 BinaryExpression 运算的 XOR。
void ExclusiveOrAssignSample()
{
    ParameterExpression varsample = Expression.Variable(typeof(int), "sampleVar");
    Expression assignExpr = Expression.Assign(varsample, Expression.Constant(5));
    Expression exclusiveOrAssignExpr = Expression.ExclusiveOrAssign(varsample, Expression.Constant(3));
    // The XOR operation is performed as follows:
    // 101 xor 011 = 110
    exclusiveOrAssignExpr.ToString().Dump();//(sampleVar ^= 3)
    var blockExpr = Expression.Block(new ParameterExpression[] { varsample }, assignExpr, exclusiveOrAssignExpr);
    Expression.Lambda<Func<int>>(blockExpr).Compile()().Dump();//6
}
ExclusiveOrAssignSample();

//Field     创建一个表示访问字段的 MemberExpression。
public class TestFieldClass
{
    public int num = 10;
}
void FieldSample()
{
    TestFieldClass obj = new TestFieldClass();
    Expression fieldExpr = Expression.Field(Expression.Constant(obj), "num");
    fieldExpr.ToString().Dump();//value(Program+TestFieldClass).num
    Expression.Lambda<Func<int>>(fieldExpr).Compile()().Dump();//10

    //给字段赋值
    var assignExpr = Expression.Assign(fieldExpr, Expression.Constant(20));
    //带有返回值
    var blockWithReturnExpr = Expression.Block(fieldExpr, assignExpr);
    Expression.Lambda<Func<int>>(blockWithReturnExpr).Compile()().Dump("带有返回值的赋值操作");
    //不带有返回值
    var blockExpr = Expression.Block(fieldExpr, assignExpr, Expression.Empty());
    Expression.Lambda<Action>(blockExpr).Compile()();//20
    obj.num.Dump();
}
FieldSample();

//GetActionType(Type[])    创建一个 Type 对象，该对象表示具有特定类型参数的泛型 System.Action 委托类型。//TODO

//GetDelegateType(Type[])  获取一个 Type 对象，表示具有特定类型参数的泛型 System.Func 或 System.Action 委托类型。//TODO

//GetFuncType(Type[])   创建一个 Type 对象，该对象表示System.Func具有特定类型参数的泛型 system.exception 委托类型。 最后一个类型参数指定已创建委托的返回类型。//TODO

//GOTO    创建一个表示“go to”语句的 GotoExpression。
void GotoSample()
{
    LabelTarget returnTarget = Expression.Label();
    BlockExpression blockExpr = Expression.Block(
        Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) })!, Expression.Constant("GoTo")),
        Expression.Goto(returnTarget),
        Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) })!, Expression.Constant("Other Work")),
        Expression.Label(returnTarget)
    );
    Expression.Lambda<Action>(blockExpr).Compile()();//GoTo
}
GotoSample();

//GreaterThan     创建一个表示“大于”数值比较的 BinaryExpression。
void GreaterThanSample()
{
    Expression greaterThanExpr = Expression.GreaterThan(Expression.Constant(42), Expression.Constant(45));
    Expression.Lambda<Func<bool>>(greaterThanExpr).Compile()().Dump();//False
}
GreaterThanSample();

//GreaterThanOrEqual   创建一个表示“大于或等于”数值比较的 BinaryExpression。
void GreaterThanOrEqualSample()
{
    Expression greaterThanOrEqualExpr = Expression.GreaterThanOrEqual(Expression.Constant(42), Expression.Constant(42));
    Expression.Lambda<Func<bool>>(greaterThanOrEqualExpr).Compile()().Dump();//True
}
GreaterThanOrEqualSample();

//IfThen     创建一个 ConditionalExpression，它表示带 if 语句的条件块。
void IfThenSample()
{
    bool test = true;
    Expression ifThenExpr = Expression.IfThen(Expression.Constant(test),
                                Expression.Call(
                                    null,
                                    typeof(Console).GetMethod("WriteLine", new Type[] { typeof(String) })!,
                                    Expression.Constant("The condition is true.")
                                    )
                             );
    Expression.Lambda<Action>(ifThenExpr).Compile()();//The condition is true.
}
IfThenSample();

//IfThenElse   创建一个 ConditionalExpression，它表示带 if 和 else 语句的条件块。
void IfThenElseSample()
{
    bool test = false;
    Expression ifThenElseExpr = Expression.IfThenElse(
        Expression.Constant(test),
        Expression.Call(
            null,
            typeof(Console).GetMethod("WriteLine", new Type[] { typeof(String) })!,
            Expression.Constant("The condition is true.")
        ),
        Expression.Call(
            null,
            typeof(Console).GetMethod("WriteLine", new Type[] { typeof(String) })!,
            Expression.Constant("The condition is false.")
        )
    );
    Expression.Lambda<Action>(ifThenElseExpr).Compile()();//The condition is false.
}
IfThenElseSample();

//Increment (同Decrement) 创建一个 UnaryExpression，它表示按 1 递增表达式值。 

//Invoke  创建 InvocationExpression。
void InvokeSample()
{
    //下面的示例演示如何使用 Invoke(Expression, Expression[]) 方法创建一个 InvocationExpression ，该对象表示调用具有指定自变量的 lambda 表达式。
    Expression<Func<int, int, bool>> largeSumTest = (num1, num2) => (num1 + num2) > 1000;
    InvocationExpression invocationExpression = Expression.Invoke(largeSumTest, Expression.Constant(100), Expression.Constant(900));
    invocationExpression.ToString().Dump();//Invoke((num1, num2) => ((num1 + num2) > 1000), 100, 900)
}
InvokeSample();

//IsFalse   返回表达式的计算结果是否为 false。 //TODO

//IsTrue    返回表达式的计算结果是否为 true。  //TODO

//Label     创建一个表示标签的 LabelTarget。
void LabelSample()
{
    LabelTarget returnTarget = Expression.Label();
    BlockExpression blockExpr =
        Expression.Block(
            Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) })!, Expression.Constant("Return")),
            Expression.Return(returnTarget),
            Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) })!, Expression.Constant("Other Work")),
            Expression.Label(returnTarget)
        );
    Expression.Lambda<Action>(blockExpr).Compile()();//Return
}
LabelSample();

//Lambda   创建一个表示 Lambda 表达式的表达式树。
void LambdaSample()
{
    ParameterExpression paramExpr = Expression.Parameter(typeof(int), "arg");
    LambdaExpression lambdaExpr = Expression.Lambda(
        Expression.Add(
            paramExpr,
            Expression.Constant(1)
        ),
        new List<ParameterExpression>() { paramExpr }
    );

    Console.WriteLine(lambdaExpr);//arg => (arg + 1)
    Console.WriteLine(lambdaExpr.Compile().DynamicInvoke(1));//2
}
LambdaSample();

//LeftShift  创建一个表示按位左移运算的 BinaryExpression。
