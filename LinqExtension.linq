<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

//LINQ扩展方法

void Main()
{
	#region test And
	{
		Expression<Func<string, bool>> where1 = s => s.StartsWith("a");
		Expression<Func<string, bool>> where2 = s => s.Length > 10;
		Func<string, bool> funcAnd = where1.And(where2).Compile();
		funcAnd("abcdefghijklmn").Dump("And");
		Func<string, bool> funcOr = where1.And(where2).Compile();
		funcOr("abc").Dump("Or");
	}
	#endregion
	#region test MaxOrDefault MinOrDefault
	{
		var str = Enumerable.Range(1, 10).OrderByDescending(p => p);
		var ms = str.AsQueryable().OrderByDescending(s => s).FirstOrDefault();
		ms.Dump("MaxOrDefault");
		
		var mins=str.AsQueryable().MinOrDefault();
		mins.Dump("MinOrDefault");
	}
	#endregion
}



/// <summary>LINQ扩展方法</summary>
public static class LinqExtension
{
	/// <summary>
	/// 与连接
	/// </summary>
	/// <typeparam name="T">类型</typeparam>
	/// <param name="left">左条件</param>
	/// <param name="right">右条件</param>
	/// <returns>新表达式</returns>
	public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
	{
		return CombineLambdas(left, right, ExpressionType.AndAlso);
	}

	/// <summary>
	/// 或连接
	/// </summary>
	/// <typeparam name="T">类型</typeparam>
	/// <param name="left">左条件</param>
	/// <param name="right">右条件</param>
	/// <returns>新表达式</returns>
	public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
	{
		return CombineLambdas(left, right, ExpressionType.Or);
	}

	private static Expression<Func<T, bool>> CombineLambdas<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right, ExpressionType expressionType)
	{
		var visitor = new SubstituteParameterVisitor
		{
			Sub = {
				[right.Parameters[0]] = left.Parameters[0]
			}
		};

		Expression body = Expression.MakeBinary(expressionType, left.Body, visitor.Visit(right.Body));
		return Expression.Lambda<Func<T, bool>>(body, left.Parameters[0]);
	}

	/// <summary>
	/// 取最大值
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="source"></param>
	/// <param name="selector"></param>
	/// <returns></returns>
	public static TResult MaxOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector) => source.Select(selector).OrderByDescending(_ => _).FirstOrDefault()!;

	/// <summary>
	/// 取最大值
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="source"></param>
	/// <param name="selector"></param>
	/// <param name="defaultValue"></param>
	public static TResult MaxOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, TResult defaultValue) => source.Select(selector).OrderByDescending(_ => _).FirstOrDefault() ?? defaultValue;

	/// <summary>
	/// 取最大值
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <param name="source"></param>
	/// <returns></returns>
	public static TSource MaxOrDefault<TSource>(this IQueryable<TSource> source) => source.OrderByDescending(_ => _).FirstOrDefault()!;

	/// <summary>
	/// 取最大值
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <param name="source"></param>
	/// <param name="defaultValue"></param>
	/// <returns></returns>
	public static TSource MaxOrDefault<TSource>(this IQueryable<TSource> source, TSource defaultValue) => source.OrderByDescending(_ => _).FirstOrDefault() ?? defaultValue;

	/// <summary>
	/// 取最小值
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="source"></param>
	/// <param name="selector"></param>
	/// <returns></returns>
	public static TResult MinOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector) => source.Select(selector).OrderBy(_ => _).FirstOrDefault()!;

	/// <summary>
	/// 取最小值
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="source"></param>
	/// <param name="selector"></param>
	/// <param name="defaultValue"></param>
	/// <returns></returns>
	public static TResult MinOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, TResult defaultValue) => source.Select(selector).OrderBy(_ => _).FirstOrDefault() ?? defaultValue;

	/// <summary>
	/// 取最小值
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <param name="source"></param>
	/// <returns></returns>
	public static TSource MinOrDefault<TSource>(this IQueryable<TSource> source) => source.OrderBy(_ => _).FirstOrDefault()!;

	/// <summary>
	/// 取最小值
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <param name="source"></param>
	/// <param name="defaultValue"></param>
	/// <returns></returns>
	public static TSource MinOrDefault<TSource>(this IQueryable<TSource> source, TSource defaultValue) => source.OrderBy(_ => _).FirstOrDefault() ?? defaultValue;
}

internal class SubstituteParameterVisitor : ExpressionVisitor
{
	internal Dictionary<Expression, Expression> Sub = new Dictionary<Expression, Expression>();
	protected override Expression VisitParameter(ParameterExpression node)
	{
		return Sub.TryGetValue(node, out var newValue) ? newValue : node;
	}
}
