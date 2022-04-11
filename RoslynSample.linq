<Query Kind="Program">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
</Query>

void Main()
{
	//ShowCustomWalker();
	ShowCustomRewriter();
}
#region CSharpSyntaxWalker
//演示按步访问示例
public void ShowCustomWalker()
{
	var tree = CSharpSyntaxTree.ParseText(@"
	public class MyClass
        {
            public void MyMethod()
            {
            }
			//带有一个参数的构造函数
            public void MyMethod(int n)
            {
            }
	public class MyOtherClass{
		public void MyMethod(int n){}
	}
	");
	var walker = new CustomWalker();
	"".Dump("CustomWalker");
	walker.Visit(tree.GetRoot());
	/*
	CompilationUnit
	ClassDeclaration
	    MethodDeclaration
	        PredefinedType
	        ParameterList
	        Block
	    MethodDeclaration
	        PredefinedType
	        ParameterList
	            Parameter
	                PredefinedType
        Block
	*/

	var deeperWalker = new CustomDeeperWalker();
	"".Dump("DeepWalker");
	deeperWalker.Visit(tree.GetRoot());

	var classMethodWalker = new ClassMethodWalker();
	classMethodWalker.Visit(tree.GetRoot());
}
//自定义按步浏览访问
public class CustomWalker : CSharpSyntaxWalker
{
	static int Tabs = 0;
	public override void Visit(SyntaxNode node)
	{
		Tabs += 2;
		var indents = new string('\t', Tabs);
		Console.WriteLine(indents + node.Kind());
		base.Visit(node);
		Tabs -= 2;
	}
}

//自定义深度按步浏览访问
public class CustomDeeperWalker : CSharpSyntaxWalker
{
	static int Tabs = 0;
	//NOTE: Make sure you invoke the base constructor with 
	//the correct SyntaxWalkerDepth. Otherwise VisitToken()
	//will never get run.
	public CustomDeeperWalker() : base(SyntaxWalkerDepth.Token)
	{

	}
	public override void Visit(SyntaxNode node)
	{
		Tabs += 2;
		var indents = new string('\t', Tabs);
		Console.WriteLine(indents + node.Kind());
		base.Visit(node);
		Tabs -= 2;
	}

	public override void VisitToken(SyntaxToken token)
	{
		var indents = new string('\t', Tabs);
		Console.WriteLine(indents + token);
		base.VisitToken(token);
	}
}

//自定义访问类和方法
public class ClassMethodWalker : CSharpSyntaxWalker
{
	string className = string.Empty;
	public override void VisitClassDeclaration(ClassDeclarationSyntax node)
	{
		className = node.Identifier.ToString();
		base.VisitClassDeclaration(node);
	}

	public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
	{
		string methodName = node.Identifier.ToString();
		Console.WriteLine(className + ' ' + methodName + " parameterLength:" + node.ParameterList.Parameters.Count());
		base.VisitMethodDeclaration(node);
	}
}
#endregion

#region CSharpSyntaxRewriter
public void ShowCustomRewriter()
{
	var tree = CSharpSyntaxTree.ParseText(@"
	public class Sample{
		public void Foo(){
			Console.WriteLine();
			;
		}
	}
	");
	var rewriter = new EmptyStatementRemoval();
	var result = rewriter.Visit(tree.GetRoot());
	Console.WriteLine(result.ToFullString());
}
//去除多余的分号
public class EmptyStatementRemoval : CSharpSyntaxRewriter
{
	public override SyntaxNode VisitEmptyStatement(EmptyStatementSyntax node)
	{
		return null;
	}
}
#endregion

