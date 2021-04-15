<Query Kind="Program">
  <Output>DataGrids</Output>
  <NuGetReference>System.Collections.Immutable</NuGetReference>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>System.Security</Namespace>
</Query>

//一: 不可变优点
//集合共享安全，从不被改变
//访问集合时，不需要锁集合（线程安全）
//修改集合不担心旧集合被改变
//书写更简洁，函数式风格。 var list = ImmutableList.Empty.Add(10).Add(20).Add(30);
//保证数据完整性，安全性
//二: 不可变对象缺点
//不可变本身的优点即是缺点，当每次对象/集合操作都会返回个新值。而旧值依旧会保留一段时间，这会使内存有极大开销，也会给GC造成回收负担，性能也比可变集合差的多。
void Main()
{
	//不可变对象 immutable
	#region 不可变对象 string int

	{
		var str = "mushroomsir";
		var newstr = str.Substring(0, 6);
		newstr.Dump();
		str.Dump();
		//C# 中的string是不可变的,Substring(0,6)返回的是一个新的字符串值,而原字符串在共享域中是不变的
		//StringBuilder是可变的

		var age = 18;
		age = 2;
		//int 在内存中是不可修改的,age=2时会在栈中开辟新值并赋给age变量
	}
	#endregion

	#region 不可变集合
	//.NET提供的常用数据结构

	//ImmutableStack
	//ImmutableQueue
	//ImmutableList
	//ImmutableHashSet
	//ImmutableSortedSet
	//ImmutableDictionary<K, V>
	//ImmutableSortedDictionary<K, V>
	ImmutableList<int> il = ImmutableList<int>.Empty;
	il = il.Add(1);
	il = il.AddRange(new[] { 1, 2, 3 });
	il.Dump();//1 1 2 3
			  //跟string和StringBuild一样，Net提供的不可变集合也增加了批量操作的API，用来避免大量创建对象：
	var ilBuilder = il.ToBuilder();
	ilBuilder.Add(10);
	ilBuilder.Add(11);
	il = ilBuilder.ToImmutable();
	il.Dump();//1 1 2 3 10 11
	#endregion

	ImmutableList<Person> pl = ImmutableList<Person>.Empty;
	pl = pl.Add(new Person("wwmin"));
	pl[0].ChangeName("liyue");
	pl.Dump();//[0] {Name:"wwmin"}
}



#region 不可变对象
//可变对象在多线程并发中共享是存在问题的
class Contact
{
	public string Name { get; set; }
	public string Address { get; set; }
	public Contact(string contactName, string contactAddress)
	{
		Name = contactName;
		Address = contactAddress;
	}
}

//不可变对象, 将创建对象每次都创建一个新值,保证数据的一致性
class Contact2
{
	public string Name { get; set; }
	public string Address { get; set; }
	public Contact2(string contactName, string contactAddress)
	{
		Name = contactName;
		Address = contactAddress;
	}

	public static Contact2 CreateContact(string name, string address)
	{
		return new Contact2(name, address);
	}
}
#endregion

public class Person
{
	public string Name { get; }
	public Person(string name)
	{
		Name = name;
	}
	//保证对象不变性
	public Person ChangeName(string name)
	{
		return new Person(name);
	}
}

