<Query Kind="Program">
  <Connection>
    <ID>9e9bb15b-e9d1-4d55-b6b8-166107188e3f</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DisplayName>localhost_sqlserver</DisplayName>
    <Database>Eletcric_2</Database>
  </Connection>
  <Output>DataGrids</Output>
</Query>

//对象自动使用索引器

void Main()
{
	var p = new Person();
	p.SetInstance(p);
	p["Name"] = "wwmin";
	p["Age"] = 22;
	//p["Name"].Dump();
	Console.WriteLine(p["Name"]);
	Console.WriteLine(p["Age"]);
}

public class Person : Indexed<Person>
{
	public string Name { get; set; }
	public int Age { get; set; }
}

public abstract class Indexed<T>
{
	T _t;
	//public Indexed(T t)
	//{
	//    _t = t;
	//}
	public void SetInstance(T t)
	{
		_t = t;
	}
	public dynamic this[string s]
	{
		get
		{
			Type type = typeof(T);
			var parameter = Expression.Parameter(type, "m");//参数m
			PropertyInfo property = type.GetProperty(s)!;
			if (property == null) return null!;
			Expression expProperty = Expression.Property(parameter, property.Name);
			var propertyDelegateExpression = Expression.Lambda(expProperty, parameter);
			var pType = property.PropertyType;
			var typeName = pType.FullName;
			switch (typeName)
			{
				case "System.Int32":
					{
						var propertyDelegate = (Func<T, int>)propertyDelegateExpression.Compile();
						int value = propertyDelegate.Invoke(_t);
						return value;
					}
				case "System.Double":
					{
						var propertyDelegate = (Func<T, double>)propertyDelegateExpression.Compile();
						double value = propertyDelegate.Invoke(_t);
						return value;
					}
				case "System.String":
					{
						var propertyDelegate = (Func<T, string>)propertyDelegateExpression.Compile();
						string value = propertyDelegate.Invoke(_t);
						return value;
					}
				case "System.Single":
					{
						var propertyDelegate = (Func<T, Single>)propertyDelegateExpression.Compile();
						Single value = propertyDelegate.Invoke(_t);
						return value;
					}
				case "System.Decimal":
					{
						var propertyDelegate = (Func<T, decimal>)propertyDelegateExpression.Compile();
						decimal value = propertyDelegate.Invoke(_t);
						return value;
					}
				case "System.Boolean":
					{
						var propertyDelegate = (Func<T, bool>)propertyDelegateExpression.Compile();
						bool value = propertyDelegate.Invoke(_t);
						return value;
					}
				default:
					return default!;
			}

		}
		set
		{
			//动态构建Lambda
			Type type = typeof(Person);
			ParameterExpression parameter = Expression.Parameter(type, "m");//参数m

			PropertyInfo property = type.GetProperty(s)!;
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
						propertyDelegate.Invoke(_t);
					}
					break;
				case "System.Double":
					{
						var d = value;//value.ToString().ToDouble();
						var body = Expression.Assign(expProperty, Expression.Constant(d));
						LambdaExpression propertyDelegateExpression = Expression.Lambda(body, parameter);
						var propertyDelegate = (Func<T, double>)propertyDelegateExpression.Compile();
						propertyDelegate.Invoke(_t);
					}
					break;
				case "System.String":
					{
						var d = value;//value.ToString();
						var body = Expression.Assign(expProperty, Expression.Constant(d));
						LambdaExpression propertyDelegateExpression = Expression.Lambda(body, parameter);
						var propertyDelegate = (Func<T, string>)propertyDelegateExpression.Compile();
						propertyDelegate.Invoke(_t);
					}
					break;
				case "System.Single":
					{
						var d = value;//value.ToString().ToFloat();
						var body = Expression.Assign(expProperty, Expression.Constant(d));
						LambdaExpression propertyDelegateExpression = Expression.Lambda(body, parameter);
						var propertyDelegate = (Func<T, float>)propertyDelegateExpression.Compile();
						propertyDelegate.Invoke(_t);
					}
					break;
				case "System.Decimal":
					{
						var d = value;//value.ToString().ToDecimal();
						var body = Expression.Assign(expProperty, Expression.Constant(d));
						LambdaExpression propertyDelegateExpression = Expression.Lambda(body, parameter);
						var propertyDelegate = (Func<T, decimal>)propertyDelegateExpression.Compile();
						propertyDelegate.Invoke(_t);
					}
					break;
				case "System.Boolean":
					{
						var d = value;//value.ToString().ToBoolean();
						var body = Expression.Assign(expProperty, Expression.Constant(d));
						LambdaExpression propertyDelegateExpression = Expression.Lambda(body, parameter);
						var propertyDelegate = (Func<T, bool>)propertyDelegateExpression.Compile();
						propertyDelegate.Invoke(_t);
					}
					break;
				default:
					break;
			};
		}
	}
}
