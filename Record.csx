//使用快速定义结构可简化为: public record Person(string FirstName,string LastName);
public record Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Person() { }
    public Person(string firstName, string lastName) => (FirstName, LastName) = (firstName, lastName);//位置构造函数
    public void Deconstruct(out string firstName, out string lastName) => (firstName, lastName) = (FirstName, LastName);//位置析构函数
}

public record Student : Person
{
    public int Num { get; set; }
}
var p1 = new Person() { FirstName = "w", LastName = "m" };
var p2 = new Person() { FirstName = "l", LastName = "y" };
(p1.Equals(p2)).Dump(); //使用record类型 可以认为是值类型, 比较的时候会比较值\
//使用析构
var (fn1, ln1) = p1;
var (fn2, ln2) = p2;
(fn1 + ln1).Dump();
(fn2 + ln2).Dump();


Student student = new Student();
Student s1 = student with { Num = 1 }; //结合with 快速定义差异化对象
s1.ToString().Dump();
Object.ReferenceEquals(student, s1).Dump("Object.ReferenceEquals(obj1,obj2)");//False


//快速定义record
//public record LogBase(string Name, string Content);
