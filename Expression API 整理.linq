<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

//Expression API 整理 //参考URL: https://docs.microsoft.com/zh-cn/dotnet/api/system.linq.expressions.expression?view=net-5.0
//using System.Diagnostics;
//using System.Linq.Expressions;

void Main()
{
	//Add(Expression, Expression)    创建一个表示不进行溢出检查的算术加法运算的 BinaryExpression。
	void AddSample()
	{
		Expression sumExpr = Expression.Add(Expression.Constant(1), Expression.Constant(2));
		Console.WriteLine(sumExpr.ToString());//(1+2)
		Console.WriteLine(Expression.Lambda<Func<int>>(sumExpr).Compile()());//3
		Expression sumExpr2 = Expression.Add(Expression.Constant(1), Expression.Constant(2), typeof(Plus).GetMethod("AddOne"));
		Console.WriteLine(sumExpr2.ToString());
		Console.WriteLine(Expression.Lambda<Func<int>>(sumExpr2).Compile()());
	}
	AddSample();

	//AddAssign(Expression, Expression) 创建一个表示不进行溢出检查的加法赋值运算的 BinaryExpression
	void AddAssignSample()
	{
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
	}
	AddAssignSample();

	//AddAssign(Expression, Expression, MethodInfo)  创建一个表示不进行溢出检查的加法赋值运算的 BinaryExpression。可指定实现
	//参Add(Expression, Expression, MethodInfo)

	//AddAssign(Expression, Expression, MethodInfo, LambdaExpression) 创建一个表示不进行溢出检查的加法赋值运算的 BinaryExpression。可指定实现, 可指定类型转换函数


	//AddAssignChecked(Expression, Expression)    创建一个表示进行溢出检查的加法赋值运算的 BinaryExpression。
	void AddAssignCheckedSample()
	{
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
	}
	AddAssignCheckedSample();

	//AddChecked(Expression, Expression)    创建一个表示进行溢出检查的算术加法运算的 BinaryExpression。
	//同Add

	//And(Expression, Expression)    创建一个表示按位 BinaryExpression 运算的 AND。
	void AndSample()
	{
		Expression andExprBinary = Expression.And(
		Expression.Constant(true),
		Expression.Constant(false)
		);
		Console.WriteLine(andExprBinary.ToString());//(True And False)
		Expression.Lambda<Func<bool>>(andExprBinary).Compile()().Dump();//False
	}
	AndSample();

	//AndAlso(Expression, Expression)    创建一个 BinaryExpression，它表示仅在第一个操作数的计算结果为 AND 时才计算第二个操作数的条件 true 运算。
	void AndAlsoSample()
	{
		Expression andAlsoExpr = Expression.AndAlso(
			Expression.Constant(false),
			Expression.Constant(true)
		);
		Console.WriteLine(andAlsoExpr.ToString());//(False AndAlso True)
		Console.WriteLine(Expression.Lambda<Func<bool>>(andAlsoExpr).Compile()());//False
	}
	AndAlsoSample();

	//AndAssign 创建一个表示按位 AND 赋值运算的 BinaryExpression。
	void AndAssignSample()
	{
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
	}
	AndAssignSample();



	//ArrayAccess   创建一个用于访问数组的 IndexExpression。
	//下面的代码示例演示如何使用方法更改多维数组中元素的值 ArrayAccess 。
	void ArrayAccessSample()
	{
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
		Console.WriteLine(lambdaExpr.ToString());
		//(Array, FirstIndex, SecondIndex, Value) => (Array[FirstIndex, SecondIndex] = (Array[FirstIndex, SecondIndex] + Value))
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

	}
	ArrayAccessSample();


	//ArrayIndex     创建一个表示应用数组索引运算符的 Expression。
	void ArrayIndexSample()
	{
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
	}
	ArrayIndexSample();


	//ArrayLength(Expression) 方法  创建一个 UnaryExpression，它表示获取一维数组的长度的表达式。
	void ArrayLengthSample()
	{
		int[] gradeIntArray = { 1, 2, 3, 4 };
		Expression arrayIntExpression = Expression.Constant(gradeIntArray);
		var arrayIntLength = Expression.ArrayLength(arrayIntExpression);
		arrayIntLength.ToString().Dump();//ArrayLength(value(System.Int32[]))
	}
	ArrayLengthSample();

	//Assign    创建一个表示赋值运算的 BinaryExpression。
	void AssignSample()
	{
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
	}
	AssignSample();

	//Bind
	void BindSample()
	{
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
	}
	BindSample();


	//Block    创建一个 BlockExpression。当执行块表达式时，它将返回块中最后一个表达式的值
	void BlockSample()
	{
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
	}
	BlockSample();


	//Break   创建一个表示 break 语句的 GotoExpression。结合Loop 和 IfThenElse 等循环判断语句
	void BreakSample()
	{
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
			if (p is LoopExpression loopExpression) loopExpression.Body.ToString().Dump();
			//IIF((value > 1), (result *= value--), break UnnamedLabel_0 (result))
			else p.ToString().Dump();//(result = 1)
		});
		Expression.Lambda<Func<int, int>>(blockBreak, valueBreak).Compile()(5).Dump(); //120
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
	}
	BreakSample();

	//Catch  (类似与 TryCatch)  创建一个表示 catch 语句的 CatchBlock。
	void CatchSample()
	{
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
	}
	CatchSample();

	//ClearDebug   创建一个用于清除序列点的 DebugInfoExpression。 //TODO

	//Coalesce (?? 运算符)   创建一个表示合并运算的 BinaryExpression。
	void CoalesceSample()
	{
		ParameterExpression varCoalesceExpr = Expression.Variable(typeof(string), "sampleVar");

		Expression callPersonNameExpr = Expression.Call(
			Expression.New(typeof(Person)),
			typeof(Person).GetMethod("GetName")!
			);

		var assignCoalesceNullExpr = Expression.Assign(varCoalesceExpr, Expression.Coalesce(callPersonNameExpr, Expression.Constant("??赋值成功")));

		var blockCoalesceExpr = Expression.Block(new ParameterExpression[] { varCoalesceExpr }, assignCoalesceNullExpr);
		blockCoalesceExpr.Expressions.ForEach(p => p.ToString().Dump());     //(sampleVar = (new Person().GetName() ?? "??赋值成功"))
		Expression.Lambda<Func<string>>(blockCoalesceExpr).Compile()().Dump();   // ??赋值成功
	}
	CoalesceSample();

	//Condition    创建一个表示条件语句的 ConditionalExpression。
	void ConditionSample()
	{
		int conditionNum = 100;
		Expression conditionExpr = Expression.Condition(
			Expression.Constant(conditionNum > 10),
			Expression.Constant("num is greater than 10"),
			Expression.Constant("num is smaller than 10")
			);
		conditionExpr.ToString().Dump();//IIF(True, "num is greater than 10", "num is smaller than 10")
		Expression.Lambda<Func<string>>(conditionExpr).Compile()().Dump();//num is greater than 10
	}
	ConditionSample();

	//Constant    创建一个 ConstantExpression。
	//创建一个可以为null的常量表达式
	void ConstantSample()
	{
		Expression constantNullalbeExpr = Expression.Constant(null, typeof(int?));
		var coaleasceExpr = Expression.Coalesce(constantNullalbeExpr, Expression.Constant(123456));
		var varCoalesceNullableExpr = Expression.Variable(typeof(int), "sampleVar");
		var assignNullableExpr = Expression.Assign(varCoalesceNullableExpr, coaleasceExpr);
		var blockAssignNullableExpr = Expression.Block(new ParameterExpression[] { varCoalesceNullableExpr }, assignNullableExpr);
		blockAssignNullableExpr.Expressions.ForEach(p => p.ToString().Dump());//(sampleVar = (null ?? 123456))
		Expression.Lambda<Func<int>>(blockAssignNullableExpr).Compile()().Dump();//123456
	}
	ConstantSample();


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

		lambdaExpr.ToString().Dump("Lambda");//arg => (arg + 1)
		Console.WriteLine(lambdaExpr.Compile().DynamicInvoke(1));//2
	}
	LambdaSample();

	//LeftShift  创建一个表示按位左移(<<)运算的 BinaryExpression。
	//按位左移:将第一个操作数向左移动第二个操作数指定的位数，空出的位置补0。
	//解释:左移相当于乘. 左移一位相当于乘2;左移两位相当于乘4;左移三位相当于乘8。x<<1=x*2
	//同理:右移相当于整除. 右移一位相当于除以2;右移两位相当于除以4;右移三位相当于除以8。x>>1=x/2

	void LeftShiftSample()
	{
		ParameterExpression paramExpr = Expression.Parameter(typeof(int), "arg");
		ParameterExpression paramExpr2 = Expression.Parameter(typeof(int), "num");
		LambdaExpression lambdaExpr = Expression.Lambda(
			Expression.LeftShift(
				paramExpr,
				paramExpr2
			),
			new List<ParameterExpression>() { paramExpr, paramExpr2 }
		 );
		lambdaExpr.ToString().Dump();//(arg, num) => (arg << num)
		lambdaExpr.Compile().DynamicInvoke(1, 2).Dump("LeftShift");//4
	}
	LeftShiftSample();

	//LeftShiftAssign    创建一个表示按位左移赋值(<<=)运算的 BinaryExpression。
	void LeftShiftAssignSample()
	{
		ParameterExpression varsample = Expression.Variable(typeof(int), "sampleVar");
		Expression assignExpr = Expression.Assign(varsample, Expression.Constant(1));
		Expression exclusiveOrAssignExpr = Expression.LeftShiftAssign(varsample, Expression.Constant(3));
		exclusiveOrAssignExpr.ToString().Dump();//(sampleVar <<= 3)
		var blockExpr = Expression.Block(new ParameterExpression[] { varsample }, assignExpr, exclusiveOrAssignExpr);
		Expression.Lambda<Func<int>>(blockExpr).Compile()().Dump("LeftShiftAssign");//8
	}
	LeftShiftAssignSample();

	//LessThan   创建一个表示“小于”数值比较的 BinaryExpression。
	void LessThanSample()
	{
		//静态编译方式
		Expression lessThanExpr = Expression.LessThan(
			Expression.Constant(42),
			Expression.Constant(42)
		);
		Expression.Lambda<Func<bool>>(lessThanExpr).Compile()().Dump("LessThan");//false

		//动态参数方式
		ParameterExpression paramNum1 = Expression.Parameter(typeof(int), "num1");
		ParameterExpression paramNum2 = Expression.Parameter(typeof(int), "num2");
		LambdaExpression lambdaExpr = Expression.Lambda(
			Expression.LessThan(
				paramNum1,
				paramNum2
			),
			new List<ParameterExpression>() { paramNum1, paramNum2 }
		 );
		lambdaExpr.ToString().Dump();//(num1, num2) => (num1 < num2)
		lambdaExpr.Compile().DynamicInvoke(1, 3).Dump("LessThanDynamic");//true
	}
	LessThanSample();

	//LessThanOrEqual      创建一个表示“小于或等于”数值比较的 BinaryExpression。
	void LessThanOrEqualSample()
	{
		//静态编译方式
		Expression lessThanExpr = Expression.LessThanOrEqual(
			Expression.Constant(42),
			Expression.Constant(42)
		);
		Expression.Lambda<Func<bool>>(lessThanExpr).Compile()().Dump("LessThan");//true

		//动态参数方式
		ParameterExpression paramNum1 = Expression.Parameter(typeof(int), "num1");
		ParameterExpression paramNum2 = Expression.Parameter(typeof(int), "num2");
		LambdaExpression lambdaExpr = Expression.Lambda(
			Expression.LessThanOrEqual(
				paramNum1,
				paramNum2
			),
			new List<ParameterExpression>() { paramNum1, paramNum2 }
		 );
		lambdaExpr.ToString().Dump();//(num1, num2) => (num1 <= num2)
		lambdaExpr.Compile().DynamicInvoke(1, 3).Dump("LessThanDynamic");//true
	}
	LessThanOrEqualSample();

	//ListBind    创建一个 MemberListBinding 对象。
	void ListBindSample()
	{
		MethodInfo addMethod = typeof(List<int>).GetMethod("Add")!;
		ElementInit e1 = Expression.ElementInit(addMethod, Expression.Constant(1));
		ElementInit e2 = Expression.ElementInit(addMethod, Expression.Constant(3));
		var variableBindExpr = (new ElementInit[] { e1, e2 }).AsEnumerable();

		Expression testExpr = Expression.MemberInit(
			Expression.New(typeof(TestListBindClass)),
			new List<MemberBinding> {
				Expression.ListBind(typeof(TestListBindClass).GetMember("Nums")[0],variableBindExpr)
			}
		);
		var res = Expression.Lambda<Func<TestListBindClass>>(testExpr).Compile()();
		res.ToString().Dump("ListBind");
	}
	ListBindSample();

	//ListInit     创建一个 ListInitExpression。
	void ListInitSample()
	{
		string tree1 = "maple";
		string tree2 = "oak";

		System.Reflection.MethodInfo addMethod = typeof(Dictionary<int, string>).GetMethod("Add")!;

		// Create two ElementInit objects that represent the
		// two key-value pairs to add to the Dictionary.
		System.Linq.Expressions.ElementInit elementInit1 =
			System.Linq.Expressions.Expression.ElementInit(
				addMethod,
				System.Linq.Expressions.Expression.Constant(tree1.Length),
				System.Linq.Expressions.Expression.Constant(tree1));
		System.Linq.Expressions.ElementInit elementInit2 =
			System.Linq.Expressions.Expression.ElementInit(
				addMethod,
				System.Linq.Expressions.Expression.Constant(tree2.Length),
				System.Linq.Expressions.Expression.Constant(tree2));

		// Create a NewExpression that represents constructing
		// a new instance of Dictionary<int, string>.
		System.Linq.Expressions.NewExpression newDictionaryExpression =
			System.Linq.Expressions.Expression.New(typeof(Dictionary<int, string>));

		// Create a ListInitExpression that represents initializing
		// a new Dictionary<> instance with two key-value pairs.
		System.Linq.Expressions.ListInitExpression listInitExpression =
			System.Linq.Expressions.Expression.ListInit(
				newDictionaryExpression,
				elementInit1,
				elementInit2);

		Console.WriteLine(listInitExpression.ToString());
	}
	ListInitSample();

	//Loop     创建一个 LoopExpression。
	void LoopSample()
	{
		ParameterExpression value = Expression.Parameter(typeof(int), "value");
		// Creating an expression to hold a local variable.
		ParameterExpression result = Expression.Parameter(typeof(int), "result");
		// Creating a label to jump to from a loop.
		LabelTarget label = Expression.Label(typeof(int));
		// Creating a method body.
		BlockExpression block = Expression.Block(
			new[] { result },
			Expression.Assign(result, Expression.Constant(1)),
				Expression.Loop(
				   Expression.IfThenElse(
					   Expression.GreaterThan(value, Expression.Constant(1)),
					   Expression.MultiplyAssign(result,
						   Expression.PostDecrementAssign(value)),
					   Expression.Break(label, result)
				   ),
			   label
			)
		);
		// Compile and run an expression tree.
		int factorial = Expression.Lambda<Func<int, int>>(block, value).Compile()(5);
		Console.WriteLine(factorial);//120
	}
	LoopSample();

	//MakeBinary    通过调用适当的工厂方法来创建一个 BinaryExpression。
	void MakeBinarySample()
	{
		System.Linq.Expressions.BinaryExpression binaryExpression =
			System.Linq.Expressions.Expression.MakeBinary(
				System.Linq.Expressions.ExpressionType.Subtract,
				System.Linq.Expressions.Expression.Constant(53),
				System.Linq.Expressions.Expression.Constant(14));

		Console.WriteLine(binaryExpression.ToString());//(53 - 14)
	}
	MakeBinarySample();

	//MakeCatchBlock    创建一个表示具有指定元素的 catch 语句的 CatchBlock。 //TODO

	//MakeDynamic    创建一个表示动态操作的 DynamicExpression。//TODO

	//MakeGoto    创建一个 GotoExpression，它表示指定的 GotoExpressionKind 的跳转。 也可以指定在跳转时传递给标签的值。//TODO

	//MakeIndex   创建一个 IndexExpression，它表示访问对象中的索引属性。//TODO

	//MakeMemberAccess    创建一个表示访问字段或属性的 MemberExpression。//TODO

	//MakeTry     创建一个表示具有指定元素的 try 块的 TryExpression。//TODO

	//MakeUnary   通过调用适当的工厂方法来创建一个 UnaryExpression。  //TODO

	//MemberBind  创建一个表示递归初始化某个成员的成员的 MemberMemberBinding。//TODO

	//MemberInit  表示一个表达式，该表达式创建新对象并初始化该对象的一个属性。
	void MemberInitSample()
	{
		// This expression creates a new TestMemberInitClass object
		// and assigns 10 to its num property.
		Expression testExpr = Expression.MemberInit(
			Expression.New(typeof(TestFieldClass)),
			new List<MemberBinding>() {
			Expression.Bind(typeof(TestFieldClass).GetMember("num")[0], Expression.Constant(10))
			}
		);

		var tree = Expression.Lambda<Func<TestFieldClass>>(testExpr).Compile();
		var test = tree();
		Console.WriteLine(test.num);//10
	}
	MemberInitSample();

	//Modulo   创建一个表示算术余数运算的 BinaryExpression。
	void ModuloSample()
	{
		Expression sumExpr = Expression.Modulo(Expression.Constant(3), Expression.Constant(2));
		Console.WriteLine(sumExpr.ToString());//(3%2)
		Expression.Lambda<Func<int>>(sumExpr).Compile()().Dump("ModuloAssign 余数运算");//1
	}
	ModuloSample();

	//ModuloAssign   创建一个表示余数赋值运算的 BinaryExpression。
	void ModuloAssignSample()
	{
		ParameterExpression variableExpr = Expression.Variable(typeof(int), "sampleVar");
		BlockExpression block = Expression.Block(
			new ParameterExpression[] { variableExpr },
			Expression.Assign(variableExpr, Expression.Constant(3)),
			Expression.ModuloAssign(
				variableExpr,
				Expression.Constant(2)
			)
		);
		block.Expressions.ForEach(p => p.ToString().Dump());//(sampleVar = 3)  | (sampleVar %= 2)
		Expression.Lambda<Func<int>>(block).Compile()().Dump("ModuloAssign 余数赋值运算");//1
	}
	ModuloAssignSample();

	//Multiply   创建一个表示不进行溢出检查的算术乘法运算的 BinaryExpression。
	void MultiplySample()
	{
		Expression expr = Expression.Multiply(Expression.Constant(1), Expression.Constant(2));
		Expression.Lambda<Func<int>>(expr).Compile()().Dump("Multiply");//2
	}
	MultiplySample();

	//MultiplyAssign  创建一个表示不进行溢出检查的乘法赋值运算的 BinaryExpression。 同AddAssign

	//Negate     创建一个表示算术求反运算的 UnaryExpression。
	void NegateSample()
	{
		Expression negateExpr = Expression.Negate(Expression.Constant(5));
		Expression.Lambda<Func<int>>(negateExpr).Compile()().Dump("Negate");//-5
	}
	NegateSample();

	//New   创建一个 NewExpression。
	void NewSample()
	{
		NewExpression newDictionaryExpression = Expression.New(typeof(Dictionary<int, string>));
		newDictionaryExpression.ToString().Dump("New");//new Dictionary`2()

		NewExpression newListExpression = Expression.New(typeof(List<int>));
		newListExpression.ToString().Dump("New");//new List`1()
	}
	NewSample();

	//NewArrayBounds   创建一个表示创建具有指定秩的数组的 NewArrayExpression。
	void NewArrayBoundsSample()
	{
		NewArrayExpression newArrayExpression = Expression.NewArrayBounds(
				typeof(string),
				Expression.Constant(3),
				Expression.Constant(2)
			);
		newArrayExpression.ToString().Dump("NewArrayBounds");//new System.String[,](3, 2)
	}
	NewArrayBoundsSample();

	//NewArrayInit    创建一个表示创建一维数组并使用元素列表初始化该数组的 NewArrayExpression。
	void NewArrayInitSample()
	{
		List<Expression> trees = new List<Expression>()     {
			Expression.Constant("oak"),
		  	Expression.Constant("fir"),
		  	Expression.Constant("spruce"),
		  	Expression.Constant("alder")
		  };

		NewArrayExpression newArrayExpression = Expression.NewArrayInit(typeof(string), trees);
		newArrayExpression.ToString().Dump("NewArrayInit");
	}
	NewArrayInitSample();

	//Not   创建一个表示按位求补运算的 UnaryExpression。
	void NotSample()
	{
		Expression notExpr = Expression.Not(Expression.Constant(true));
		notExpr.ToString().Dump("NotExpr");//Not(true)
		Expression.Lambda<Func<bool>>(notExpr).Compile()().Dump("Not");//false
	}
	NotSample();

	//NotEqual   创建一个表示不相等比较的 BinaryExpression。
	void NotEqualSample()
	{
		Expression notEqualExpr = Expression.NotEqual(Expression.Constant(1), Expression.Constant(2));
		notEqualExpr.ToString().Dump("NotEqualExpr");//(1!=2)
		Expression.Lambda<Func<bool>>(notEqualExpr).Compile()().Dump("NotEqual");//true
	}
	NotEqualSample();

	//OnesComplement    返回表示一的补数的表达式。//TODO

	//Or  创建一个表示按位 BinaryExpression 运算的 OR。
	void OrSample()
	{
		Expression orExpr = Expression.Or(Expression.Constant(true), Expression.Constant(false));
		orExpr.ToString().Dump("OrExpr");//(True Or False)
		Expression.Lambda<Func<bool>>(orExpr).Compile()().Dump("Or");//True
	}
	OrSample();

	//OrAssign   创建一个表示按位 OR 赋值运算的 BinaryExpression。
	void OrAssignSample()
	{
		ParameterExpression variableExpr = Expression.Variable(typeof(bool), "sampleVar");
		BlockExpression block = Expression.Block(
			new ParameterExpression[] { variableExpr },
			Expression.Assign(variableExpr, Expression.Constant(true)),
			Expression.OrAssign(
				variableExpr,
				Expression.Constant(false)
			)
		);
		block.Expressions.ForEach(p => p.ToString().Dump());//(sampleVar = True)  | (sampleVar ||= False)
		Expression.Lambda<Func<bool>>(block).Compile()().Dump("OrAssign 按位Or赋值运算");//True
	}
	OrAssignSample();

	//OrElse    创建一个 BinaryExpression，它表示仅在第一个操作数的计算结果为 OR 时才计算第二个操作数的条件 false 运算。
	void OrElseSample()
	{
		Expression orElseExpr = Expression.OrElse(
				Expression.Constant(false),
				Expression.Constant(true)
		);
		orElseExpr.ToString().Dump("OrElse");//(False OrElse True)
		Expression.Lambda<Func<bool>>(orElseExpr).Compile().Invoke().Dump("OrElse");//true
	}
	OrElseSample();

	//Parameter   创建一个 ParameterExpression 节点，该节点可用于标识表达式树中的参数或变量。
	void ParameterSample()
	{
		ParameterExpression param = Expression.Parameter(typeof(int));
		MethodCallExpression methodCall = Expression.Call(
			typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) })!,
			param
		);
		Expression.Lambda<Action<int>>(
			methodCall,
			new ParameterExpression[] { param }
		).Compile()(10);//10
	}
	ParameterSample();

	//PostDecrementAssign    创建一个 UnaryExpression，它表示将原始表达式递减 1 之后再进行表达式赋值。
	void PostDecrementAssignSample()
	{
		ParameterExpression variableExpr = Expression.Variable(typeof(int), "sampleVar");
		BlockExpression block = Expression.Block(
			new ParameterExpression[] { variableExpr },
			Expression.PostDecrementAssign(variableExpr),
			Expression.PostDecrementAssign(variableExpr)
		);
		block.Expressions.ForEach(p => p.ToString().Dump());//sampleVar--  | sampleVar--
		Expression.Lambda<Func<int>>(block).Compile()().Dump("PostDecrementAssign");//-1
	}
	PostDecrementAssignSample();

	//PostIncrementAssign    创建一个 UnaryExpression，它表示将原始表达式递增 1 之后再进行表达式赋值。同PostDecrementAssign

	//Power   创建一个表示对数进行幂运算的 BinaryExpression。
	void PowerSample()
	{
		var powerExpr = Expression.Power(Expression.Constant(2d), Expression.Constant(2d));
		powerExpr.ToString().Dump("PowerExpr");//(2**2)
		Expression.Lambda<Func<double>>(powerExpr).Compile()().Dump("Power");//2
	}
	PowerSample();

	//PowerAssign  创建一个 BinaryExpression，它表示对表达式求幂并将结果赋回给表达式。  同AddAssign

	//PreDecrementAssign    创建一个 UnaryExpression，它将表达式递减 1 并将结果赋回给表达式。同PostDecrementAssign

	//PreIncrementAssign    创建一个 UnaryExpression，它将表达式递增 1 并将结果赋回给表达式。同PostDecrementAssign

	//Property   创建一个表示访问属性的 MemberExpression。
	void PropertySample()
	{
		TestBindMember obj = new TestBindMember();
		obj.Num = 10;//定义class时, 带有{get;set;}的是属性, 不带的是字段
		Expression propertyExpr = Expression.Property(
			Expression.Constant(obj),
			"Num"
		);
		Expression.Lambda<Func<int>>(propertyExpr).Compile()().Dump("Property");//10
	}
	PropertySample();

	//PropertyOrField   创建一个表示访问属性或字段的 MemberExpression。
	void PropertyOrFieldSample()
	{
		TestFieldClass obj = new TestFieldClass();
		obj.num = 10;//定义class时, 带有{get;set;}的是属性, 不带的是字段
		Expression propertyExpr = Expression.PropertyOrField(
			Expression.Constant(obj),
			"num"
		);
		Expression.Lambda<Func<int>>(propertyExpr).Compile()().Dump("PropertyOrField");//10
	}
	PropertyOrFieldSample();

	//Quote  [引用] 创建一个表示具有类型 UnaryExpression 的常量值的表达式的 Expression。
	void QuoteSample()
	{
		TestFieldClass obj = new TestFieldClass();
		obj.num = 10;//定义class时, 带有{get;set;}的是属性, 不带的是字段
		Expression propertyExpr = Expression.PropertyOrField(
			Expression.Constant(obj),
			"num"
		);

		var lambda = Expression.Lambda<Func<int>>(propertyExpr);
		var res = lambda.Compile()().Dump();
		Expression quote = Expression.Quote(lambda);
		quote.ToString().Dump();//() => value(UserQuery+TestFieldClass).num
	}
	QuoteSample();

	//Reduce   将此节点简化为更简单的表达式。 如果 CanReduce 返回 true，则它应返回有效的表达式。 此方法可以返回本身必须简化的另一个节点。//TODO

	//ReduceAndCheck  同Reduce

	//ReduceExtensions   将表达式简化为已知节点类型（即非 Extension 节点）或仅在此类型为已知类型时返回表达式。//TODO

	//ReferenceEqual    创建一个表示引用相等比较的 BinaryExpression。//TODO

	//ReferenceNotEqual   同ReferenceEqual

	//Rethrow   创建一个 UnaryExpression，它表示重新引发异常。//TODO

	//Return    创建一个表示 return 语句的 GotoExpression。
	void ReturnSample()
	{
		LabelTarget returnTarget = Expression.Label();
		BlockExpression blockExpr = Expression.Block(
			Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) })!, Expression.Constant("Return")),
			Expression.Return(returnTarget),
			Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) })!, Expression.Constant("Other Work")),
			Expression.Label(returnTarget)
		);
		Expression.Lambda<Action>(blockExpr).Compile()();//Return
	}
	ReturnSample();

	//RightShift    创建一个表示按位右移运算的 BinaryExpression。同 LeftShift

	//RightShiftAssign  创建一个表示按位右移赋值运算的 BinaryExpression。 同LeftShiftAssign

	//RuntimeVariables  创建 RuntimeVariablesExpression的实例。//TODO

	//Subtract  创建一个表示不进行溢出检查的算术减法运算的 BinaryExpression。 同Add

	//SubtractAssign  创建一个表示不进行溢出检查的减法赋值运算的 BinaryExpression。  同AddAssign

	//Switch    创建一个表示 SwitchExpression 语句的 switch。
	void SwitchSample()
	{
		ConstantExpression switchValue = Expression.Constant(2);
		ParameterExpression pamaramNum = Expression.Parameter(typeof(int), "num");
		SwitchExpression switchExpr = Expression.Switch(
			pamaramNum,
			new SwitchCase[] {
				Expression.SwitchCase(
					Expression.Call(
						null,
						typeof(Console).GetMethod("WriteLine", new Type[] { typeof(String) })!,
						Expression.Constant("First")
					),
					Expression.Constant(1)
				),
				Expression.SwitchCase(
					Expression.Call(
						null,
						typeof(Console).GetMethod("WriteLine", new Type[] { typeof(String) })!,
						Expression.Constant("Second")
					),
					Expression.Constant(2)
				)
			}
		);
		LambdaExpression lambda = Expression.Lambda(switchExpr, new List<ParameterExpression>() { pamaramNum });
		lambda.ToString().Dump("Switch");//() => switch (2) { ... }
		lambda.Compile().DynamicInvoke(1);
	}
	SwitchSample();

	//SwitchCase   创建要在 SwitchCase 对象中使用的 SwitchExpression 对象。  同Switch

	//SymbolDocument  创建 SymbolDocumentInfo的实例。
	void SymbolDocumentSample()
	{
		var symbol = Expression.SymbolDocument("sss");
		symbol.ToString().Dump("Symbolocument");//System.Linq.Expressions.SymbolDocumentInfo
	}
	SymbolDocumentSample();

	//Throw  创建一个 UnaryExpression，它表示引发异常。
	void ThrowSample()
	{
		TryExpression tryCatchExpr = Expression.TryCatch(
		Expression.Block(
				Expression.Throw(Expression.Constant(new DivideByZeroException())),
				Expression.Constant("Try block")
			),
			Expression.Catch(
				typeof(DivideByZeroException),
				Expression.Constant("Catch block")
			)
		);
		Expression.Lambda<Func<string>>(tryCatchExpr).Compile()().Dump("throw");
	}
	ThrowSample();

	//ToString   返回 Expression 的的文本化表示形式。见其他ToString

	//TryCatch  创建一个表示 try 块的 TryExpression，该 try 块包含任意数量的 catch 语句，但不包含 fault 和 finally 块。
	void TryCatchSample()
	{
		TryExpression tryCatchExpr = Expression.TryCatch(
			Expression.Block(
				//Expression.Throw(Expression.Constant(new DivideByZeroException())),
				Expression.Throw(Expression.Constant(new ArgumentNullException())),
				Expression.Constant("Try block")
			),
			Expression.Catch(
				typeof(DivideByZeroException),
				Expression.Constant("Catch block" + typeof(DivideByZeroException))
			),
			Expression.Catch(
				typeof(ArgumentNullException),
				Expression.Constant("Catch block:" + typeof(ArgumentNullException))
			)
		);
		Expression.Lambda<Func<string>>(tryCatchExpr).Compile()().Dump("TryCatch");//Catch block:System.ArgumentNullException
	}
	TryCatchSample();

	//TryCatchFinally    创建一个表示 try 块的 TryExpression，该 try 块包含任意数量的 catch 语句和一个 finally 块。
	void TryCatchFinallySample()
	{
		TryExpression tryCatchExpr = Expression.TryCatchFinally(
			Expression.Block(
				Expression.Throw(Expression.Constant(new DivideByZeroException())),
				Expression.Constant("Try block")
			),
			Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) })!, Expression.Constant("Finally block")),//Finally block | 
			Expression.Catch(
				typeof(DivideByZeroException),
				Expression.Constant("Catch block")
			)
		);
		Expression.Lambda<Func<string>>(tryCatchExpr).Compile()().Dump("TryCatchFinally");//Catch block
	}
	TryCatchFinallySample();

	//TryFault   创建一个表示 try 块的 TryExpression，该 try 块包含一个 fault 块，但不包含 catch 语句。
	void TryFaultSample()
	{
		TryExpression tryCatchExpr = Expression.TryFault(
			Expression.Block(
				Expression.Throw(Expression.Constant(new DivideByZeroException())),
				Expression.Constant("Try block")
			),
			Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) })!, Expression.Constant("Fault block"))//Finally block | 
		);
		//Expression.Lambda<Func<string>>(tryCatchExpr).Compile()().Dump("TryFault");//TryFault报错未捕捉
	}
	TryFaultSample();

	//TryFinally   创建一个表示 try 块的 TryExpression，该 try 块包含一个 finally 块，但不包含 catch 语句。 同TryFault

	//TryGetActionType  创建一个 Type 对象，它表示具有特定类型参数的泛型 System.Action 委托类型。//TODO

	//TryGetFuncType   创建一个 Type 对象，它表示具有特定类型参数的泛型 System.Func 委托类型。 最后一个类型参数指定已创建委托的返回类型。//TODO

	//TypeAs   创建一个表示显式引用或装箱转换的 UnaryExpression（如果转换失败，则提供 null）。
	void TypeAsSample()
	{
		UnaryExpression typeAsExpression = Expression.TypeAs(Expression.Constant(3, typeof(int)), typeof(int?));
		typeAsExpression.ToString().Dump("TypeAs");//(3 As Nullable`1)
	}
	TypeAsSample();
	
	//TypeEqual   创建一个比较运行时类型标识的 TypeBinaryExpression。//TODO
	
	//TypeIs   创建一个 TypeBinaryExpression。
	void TypeIsSample(){
		TypeBinaryExpression typeBindaryExpression = Expression.TypeIs(Expression.Constant("www"),typeof(int));
		typeBindaryExpression.ToString().Dump("TypeIs");
	}
	TypeIsSample();
	
	//UnaryPlus    创建一个表示一元正运算的 UnaryExpression。
	void UnaryPlusSample(){
		UnaryExpression ue = Expression.UnaryPlus(Expression.Constant(1,typeof(int)));
		ue.ToString().Dump("UnaryPlus");//+1
	}
	UnaryPlusSample();

	//Unbox    创建一个表示显式取消装箱的 UnaryExpression。//TODO

	//Variable   创建一个 ParameterExpression 节点，该节点可用于标识表达式树中的参数或变量。
	void VariableSampel()
	{
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
	}
	VariableSampel();
	
	//VisitChildren   简化节点，然后对简化的表达式调用访问者委托。 该方法在节点不可简化时引发异常。//TODO
}

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
public static class Utils
{

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



//Add(Expression, Expression, MethodInfo) 创建一个表示不进行溢出检查的算术加法运算的 BinaryExpression。 可指定实现方法。
public class Plus
{
	//下面表达式指定加法的实现
	public static int AddOne(int a, int b)
	{
		return a + b + 1;
	}
}


//Bind   创建一个表示成员初始化的 MemberAssignment。
public class TestBindMember
{
	public int Num { get; set; }
}


//Call    创建一个 MethodCallExpression。
public class SmapleClass
{
	public int AddIntegers(int arg1, int arg2)
	{
		return arg1 + arg2;
	}
}

//Field     创建一个表示访问字段的 MemberExpression。
public class TestFieldClass
{
	public int num = 10;
}

//ListBind
class TestListBindClass
{
	public TestListBindClass()
	{
		Nums = new List<int>();
	}
	public List<int> Nums { get; set; }
}
