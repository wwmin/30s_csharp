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

//3. 方法
//Accept(ExpressionVisitor) //调度到此节点类型的特定 Visit 方法。 例如，MethodCallExpression 调用 VisitMethodCall(MethodCallExpression)。
//

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
var variableBindExpr = Expression.Parameter(typeof(int),"number");
var newTestBindMember = Expression.New(typeof(TestBindMember));
//MemberInfo testBindMemberInfo = testBindMember.GetType().GetMember("Num").FirstOrDefault()!;
var bindsExpr = new[] {
    Expression.Bind(typeof(TestBindMember).GetProperty("Num")!,variableBindExpr)
};
var bindsBody = Expression.MemberInit(newTestBindMember,bindsExpr);
var bindsFunc = Expression.Lambda<Func<int,TestBindMember>>(bindsBody,new[] { variableBindExpr }).Compile();
bindsBody.ToString().Dump();//new TestBindMember() {Num = number}
var testBindMember = bindsFunc(1111);
Console.WriteLine(testBindMember.GetType() == typeof(TestBindMember));//True
testBindMember.Num.Dump();//111




//扩展IEnumerable ForEach
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