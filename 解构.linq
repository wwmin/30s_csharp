<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

//数据解构 tuple/class/record
void Main()
{
	"-----------------------------------testTuple".Dump();
	//tuple
	testTuple();
	"-----------------------------------testDecontructClass".Dump();
	//class
	testDecontructClass();
	"-----------------------------------testDecontructClass".Dump();
	//record
	testRecord();
}
#region 元组
public void testTuple()
{
	//结构元组,获取所有元组
	var (firstName, lastName, age) = GetPerson();
	firstName.Dump(nameof(firstName));
	lastName.Dump(nameof(lastName));
	age.Dump(nameof(age));
	//使用弃元
	var (_, _, age1) = GetPerson();
	age1.Dump(nameof(age1));
	//使用元组整体获取,基于元组指定了返回别名
	var person = GetPerson();
	person.firstName.Dump(nameof(person.firstName));
	person.lastName.Dump(nameof(person.lastName));
	person.age.Dump(nameof(person.age));
	//使用元组整体获取, 没有别名时是Item1-ItemN表示各个值
	var person2 = GetPersonWithNoAliasName();
	person2.Item1.Dump(nameof(person2.Item1));
	person2.Item2.Dump(nameof(person2.Item2));
	person2.Item3.Dump(nameof(person2.Item3));
	//person2.Item4.Dump(nameof(person2.Item4));//不存在Item4时编译器自动提示
}

public (string firstName, string lastName, int age) GetPerson()
{
	var firstName = "w";
	var lastName = "m";
	var age = 22;
	return (firstName, lastName, age);
}

public (string, string, int) GetPersonWithNoAliasName()
{
	var firstName = "w";
	var lastName = "m";
	var age = 22;
	return (firstName, lastName, age);
}
#endregion
#region 解构类
public void testDecontructClass()
{
	var person = new Person { FirstName = "ww", LastName = "min", Age = 33 };
	{
		var (firstName, lastName, age) = person;
		firstName.Dump(nameof(firstName));
		lastName.Dump(nameof(lastName));
		age.Dump(nameof(age));
	}
	{
		var (firstName, age) = person;
		(firstName + age).Dump("重载的解构");
	}
	{
		var person2 = new Person2 { FirstName = "ww", LastName = "min", Age = 33 };
		//person2.Deconstruct(out string firstName, out string lastName, out int age);
		(string firstName, string lastName, int age) = person2;
		(firstName, lastName, age).Dump("静态扩展的解构");

	}
}
public class Person
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public int Age { get; set; }
	//解构
	public void Deconstruct(out string firstName, out string lastName, out int age)
	{
		firstName = FirstName;
		lastName = LastName;
		age = Age;
	}
	//解构可重载
	public void Deconstruct(out string firstName, out int age) => (firstName, age) = (FirstName, Age);
}

//通过静态扩展方法的方式给类的实例添加Deconstruct()方法
public class Person2
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public int Age { get; set; }
}
public static class PersonExtensions
{
	public static void Deconstruct(this Person2 person, out string firstName, out string lastName, out int age) => (firstName, lastName, age) = (person.FirstName, person.LastName, person.Age);
}
#endregion
#region 解构record
//记录类型既可以和类一样有具名成员，也可以和元组一样有基于成员顺序的可解构特性，不需要单独定义 Deconstruct() 方法。
record PersonRecord(string FirstName, string LastName, int age);

public void testRecord()
{
	var personRecord = new PersonRecord("ww", "min", 44);
	var (firstName, lastName, age) = personRecord;
	(firstName, lastName, age).Dump("解构record");
}
#endregion