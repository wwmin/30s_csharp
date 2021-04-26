<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var p = new Person();
	p.SetObjectValue(nameof(p.Name), "wwmin");
	p.SetObjectValue(nameof(p.Age),22);
	p.Name.Dump();
	p.Age.Dump();
}

public class Person
{
	public string Name { get; set; }
	public int Age{get;set;}
	public string Address { get; set; }
}

public static class Util
{
	public static void SetObjectValue<T>(this T obj, string name, dynamic value)
	{
		//动态构建Lambda
		Type type = typeof(T);
		ParameterExpression parameter = Expression.Parameter(type, "m");//参数m

		PropertyInfo property = type.GetProperty(name)!;
		if (property == null) return;
		Expression expProperty = Expression.Property(parameter, property.Name);//取参数的属性m.Hours
		var pType = property.PropertyType;
		var typeName = pType.FullName;
		switch (typeName)
		{
			case "System.Int32":
				{
					var d = value;//value.ToString().ToInt();
					var body = Expression.Assign(expProperty, Expression.Constant(d));
					LambdaExpression propertyDelegateExpression = Expression.Lambda(body, parameter);
					var propertyDelegate = (Func<T, int>)propertyDelegateExpression.Compile();
					propertyDelegate.Invoke(obj);
				}
				break;
			case "System.Double":
				{
					var d = value;//value.ToString().ToDouble();
					var body = Expression.Assign(expProperty, Expression.Constant(d));
					LambdaExpression propertyDelegateExpression = Expression.Lambda(body, parameter);
					var propertyDelegate = (Func<T, double>)propertyDelegateExpression.Compile();
					propertyDelegate.Invoke(obj);
				}
				break;
			case "System.String":
				{
					var d = value;//value.ToString();
					var body = Expression.Assign(expProperty, Expression.Constant(d));
					LambdaExpression propertyDelegateExpression = Expression.Lambda(body, parameter);
					var propertyDelegate = (Func<T, string>)propertyDelegateExpression.Compile();
					propertyDelegate.Invoke(obj);
				}
				break;
			case "System.Single":
				{
					var d = value;//value.ToString().ToFloat();
					var body = Expression.Assign(expProperty, Expression.Constant(d));
					LambdaExpression propertyDelegateExpression = Expression.Lambda(body, parameter);
					var propertyDelegate = (Func<T, float>)propertyDelegateExpression.Compile();
					propertyDelegate.Invoke(obj);
				}
				break;
			case "System.Decimal":
				{
					var d = value;//value.ToString().ToDecimal();
					var body = Expression.Assign(expProperty, Expression.Constant(d));
					LambdaExpression propertyDelegateExpression = Expression.Lambda(body, parameter);
					var propertyDelegate = (Func<T, decimal>)propertyDelegateExpression.Compile();
					propertyDelegate.Invoke(obj);
				}
				break;
			case "System.Boolean":
				{
					var d = value;//value.ToString().ToBoolean();
					var body = Expression.Assign(expProperty, Expression.Constant(d));
					LambdaExpression propertyDelegateExpression = Expression.Lambda(body, parameter);
					var propertyDelegate = (Func<T, bool>)propertyDelegateExpression.Compile();
					propertyDelegate.Invoke(obj);
				}
				break;
			default:
				break;
		};
	}

	/// <summary>
	/// string => int
	/// <para>使用int.TryParse</para> 
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static int ToInt(this string str)
	{
		if (string.IsNullOrEmpty(str)) return 0;
		int.TryParse(str, out int res);
		return res;
	}

	/// <summary>
	/// string => double
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static double ToDouble(this string str)
	{
		if (string.IsNullOrEmpty(str)) return 0d;
		double.TryParse(str, out double d);
		return d;
	}

	/// <summary>
	/// string => float
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static float ToFloat(this string str)
	{
		if (string.IsNullOrEmpty(str)) return 0;
		float.TryParse(str, out float d);
		return d;
	}


	/// <summary>
	/// string => decimal
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static decimal ToDecimal(this string str)
	{
		if (string.IsNullOrEmpty(str)) return 0;
		decimal.TryParse(str, out decimal d);
		return d;
	}


	/// <summary>
	/// string => decimal
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static bool ToBoolean(this string str)
	{
		if (string.IsNullOrEmpty(str)) return false;
		if (str == "1") return true;
		if (str == "0") return false;
		var d = Convert.ToBoolean(str);
		return d;
	}
}
