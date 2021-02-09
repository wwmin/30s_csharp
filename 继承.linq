<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

//C# 继承  base Virual Abstract Sealed 相关
//http://c.biancheng.net/csharp/60/
//Object 四个主要方法: Equals/GetHashCode/GetType/ToString

static void Main(string[] args)
{
	//base  在继承的关系中，子类如果需要调用父类中的成员可以借助 base 关键字来完成


	//virtual 是虚拟的含义，在 C# 语言中，默认情况下类中的成员都是非虚拟的，
	//通常将类中的成员定义成虚拟的，表示这些成员将会在继承后重写其中的内容。
	//virtual 关键字能修饰方法、属性、索引器以及事件等，用到父类的成员中。

	//abstract 关键字代表的是抽象的，
	//使用该关键字能修饰类和方法，修饰的方法被称为抽象方法、修饰的类被称为抽象类。
	//抽象方法是一种不带方法体的方法，仅包含方法的定义

	//通常抽象类会被其他类继承，并重写其中的抽象方法或者虚方法。

	//sealed 关键字的含义是密封的，使用该关键字能修饰类或者类中的方法，
	//修饰的类被称为密封类、修饰的方法被称为密封方法。
	//但是密封方法必须出现在子类中，并且是子类重写的父类方法，
	//即 sealed 关键字必须与 override 关键字一起使用。
	//密封类不能被继承，密封方法不能被重写。

	TestBase();
}

static void TestBase()
{
	//Person person = new Person();//虚方法不能实例化
	//Console.WriteLine("Person类的Print方法打印内容");
	//person.Print();
	Student student = new Student();
	Console.WriteLine("Student类的Print方法打印内容");
	student.Print();
	Teacher teacher = new Teacher();
	Console.WriteLine("Teacher类的Print方法打印内容");
	teacher.Print();
}

abstract class Person
{
	public Person(string name)
	{
		this.Name = name;
	}
	public int Id { get; set; }
	public string Name { get; set; }
	public virtual void Print()
	{
		Console.WriteLine("编号：" + Id);
		Console.WriteLine("姓名：" + Name);
	}
	public abstract void ShowMyInfo();//在各自子类中实现自己的实现方法
}

class Teacher : Person
{
	public Teacher(string name) : base(name)
	{

	}
	public string Title { get; set; }
	public string WageNo { get; set; }
	public override void Print()
	{
		base.Print();
		Console.WriteLine("职称：" + Title);
		Console.WriteLine("工资号：" + WageNo);
	}
	public sealed override void ShowMyInfo() => (this.Name + ":" + this.WageNo).Dump();
}
sealed class Student : Person
{
	public Student(string name) : base(name)
	{

	}
	public string Major { get; set; }
	public string Grade { get; set; }
	public override void Print()
	{
		base.Print();
		Console.WriteLine("专业：" + Major);
		Console.WriteLine("年级：" + Grade);
	}
	public override void ShowMyInfo() => (this.Name + "-" + this.Major).Dump();
}