using System.Diagnostics;
using System.Linq.Expressions;
var watch = new Stopwatch();
//解析表达式树
{
    //Create the expression tree
    Expression<Func<int, bool>> exprTree = num => num < 5;
    //Func<int, bool> fn = num => num < 4;

    //Decompose the expression tree.
    ParameterExpression param = (ParameterExpression)exprTree.Parameters[0];
    BinaryExpression operation = (BinaryExpression)exprTree.Body;
    ParameterExpression left = (ParameterExpression)operation.Left;
    ConstantExpression right = (ConstantExpression)operation.Right;

    $"Decomposed expression: {param.Name} => {left.Name} {operation.NodeType} {right.Value}".Dump();

    //编译表达式树
    Func<int, bool> result = exprTree.Compile();
    result(4).Dump();
    //可以合并成一个
    exprTree.Compile()(5).Dump();
}


//------------------------------------------------------------------------------------------------//
//通过API创建表达式树
//简单创建 num => num < 5
{
    ParameterExpression numParam = Expression.Parameter(typeof(int),"num");
    ConstantExpression five = Expression.Constant(5,typeof(int));
    BinaryExpression numLessThanFive = Expression.LessThan(numParam,five);
    Expression<Func<int,bool>> lambda = Expression.Lambda<Func<int,bool>>(numLessThanFive,new ParameterExpression[]{numParam});
    lambda.Dump();
    lambda.Compile()(4).Dump("简单创建");
}
//复杂创建
{
    //Creating a parameter expression.
    ParameterExpression value = Expression.Parameter(typeof(int), "value");
    //Creating an expression to hold a local variable.
    ParameterExpression result = Expression.Parameter(typeof(int), "result");
    //Creating a label to jump to from a loop
    LabelTarget label = Expression.Label(typeof(int));
    //Creating a method body.
    BlockExpression block = Expression.Block(
        //Adding a local variable.
        new[] { result },
        //Assigning a constant to a local variable: result = 1
        Expression.Assign(result, Expression.Constant(1)),
            //Adding a loop.
            Expression.Loop(
                    //Adding a conditional block info the loop.
                    Expression.IfThenElse(
                                //Condition: value >1
                                Expression.GreaterThan(value, Expression.Constant(1)),
                                    //If true: result *= value --
                                    Expression.MultiplyAssign(result, Expression.PostDecrementAssign(value)),
                                            //If false,exit the loop and go to the label
                                            Expression.Break(label, result)
             ),
            //Label to jump to.
            label
        )
    );
    int factorial = Expression.Lambda<Func<int, int>>(block, value).Compile()(5);
    factorial.Dump("factorial");
}



//------------------------------------------------------------------------------------------------//
//more test
public class Claptrap
{
    public int Level { get; set; }

    public void LevelUp(int diff)
    {
        Level += diff;
    }
}
private const int Count = 1_000_000;
private const int Diff = 100;

private MethodInfo _methodInfo;
private Action<Claptrap, int> _func;
private PropertyInfo _propertyInfo;
{
    _methodInfo = typeof(Claptrap).GetMethod(nameof(Claptrap.LevelUp))!;
    nameof(_methodInfo).Dump();
    var instance = Expression.Parameter(typeof(Claptrap), "c");
    var levelP = Expression.Parameter(typeof(int), "1");
    var callExpression = Expression.Call(instance, _methodInfo, levelP);
    var lambdaExpression = Expression.Lambda<Action<Claptrap, int>>(callExpression, instance, levelP);
    lambdaExpression.Dump();
    _func = lambdaExpression.Compile();
    var claptrap = new Claptrap();
    claptrap.Level.Dump();
    _func.Invoke(claptrap, 4);
    claptrap.Level.Dump();
}
//------------------------------------------------------------------------------------------------//
{
    _propertyInfo = typeof(Claptrap).GetProperty(nameof(Claptrap.Level))!;
    var instance = Expression.Parameter(typeof(Claptrap), "c");
    var levelProperty = Expression.Property(instance, _propertyInfo);
    var levelP = Expression.Parameter(typeof(int), "1");
    var addAssignExpression = Expression.AddAssign(levelProperty, levelP);
    var lambdaExpression = Expression.Lambda<Action<Claptrap, int>>(addAssignExpression, instance, levelP);
    _func = lambdaExpression.Compile();

    var claptrap = new Claptrap();
    //run reflection

    watch.Start();
    for (int i = 0; i < Count; i++)
    {
        var value = (int)_propertyInfo.GetValue(claptrap)!;
        _propertyInfo.SetValue(claptrap, value + Diff);
    }
    watch.Stop();
    watch.Elapsed.Ticks.Dump("run reflect elapsed");
    watch.Reset();
    claptrap.Level.Dump("claptrap.Level");

    //run expression
    watch.Start();
    for (int i = 0; i < Count; i++)
    {
        _func.Invoke(claptrap, Diff);
    }
    watch.Stop();
    watch.Elapsed.Ticks.Dump("run expression elapsed");
    watch.Reset();
    claptrap.Level.Dump("claptrap.Level");

    //run Directly
    watch.Start();
    for (int i = 0; i < Count; i++)
    {
        claptrap.Level += Diff;
    }
    watch.Stop();
    watch.Elapsed.Ticks.Dump("run directly elapsed");
    watch.Reset();
    claptrap.Level.Dump();
}