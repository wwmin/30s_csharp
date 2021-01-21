<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	// Create a data source by using a collection initializer.
	List<Student> students = new List<Student>
	{
	    new Student {First="aa", Last="b", ID=111, Scores= new List<int> {97, 92, 81, 60}},
	    new Student {First="aa", Last="a", ID=112, Scores= new List<int> {75, 84, 91, 39}},
	    new Student {First="bb", Last="d", ID=113, Scores= new List<int> {88, 94, 65, 91}},
	    new Student {First="bb", Last="e", ID=114, Scores= new List<int> {97, 89, 85, 82}},
	    new Student {First="bb", Last="f", ID=115, Scores= new List<int> {35, 72, 91, 70}},
	    new Student {First="cc", Last="g", ID=116, Scores= new List<int> {99, 86, 90, 94}},
	    new Student {First="cc", Last="h", ID=117, Scores= new List<int> {93, 92, 80, 87}},
	    new Student {First="cc", Last="i", ID=118, Scores= new List<int> {92, 90, 83, 78}},
	    new Student {First="cc", Last="j", ID=119, Scores= new List<int> {68, 79, 88, 92}},
	    new Student {First="cc", Last="k", ID=120, Scores= new List<int> {99, 82, 81, 79}},
	    new Student {First="cc", Last="l", ID=121, Scores= new List<int> {96, 85, 91, 60}},
	    new Student {First="cc", Last="m", ID=122, Scores= new List<int> {94, 92, 91, 91}}
	};
	
	//创建查询
	IEnumerable<string> studentQuery =
	    from student in students
	    where student?.Scores?[0] > 10 && student.Scores[3] < 100
	    let sum = student.Scores!.Sum()
	    orderby student.First ascending, sum descending//修改查询 排序, ','逗号跟的是次要排序,相当于ThenBy
	    select student.First + ":" + student.Last + ":" + sum;//投影或映射
	//执行查询
	"studentQuery".Dump("studentQuery");
	string.Join("\n", studentQuery).Dump();
	
	//对结果进行分组
	//组键可以是任何类型,如字符串、内置数值类型、用户定义的命名类型或匿名类型
	//按字符串分组
	var studentQuery2 =
	    from student in students
	    orderby student.Last?[0] ascending
	    group student by student.First?[0].ToString() into g
	    where (g.Key == "a" || g.Key == "b")
	    orderby g.Key
	    select g;
	//按布尔值分组
	var studentQuery3 =
	    from student in students
	    group student by student.Scores!.Average() >= 80;//pass or fail
	
	//按数值范围分组 其中使用let引入标识符
	var studentQuery4 =
	    from student in students
	    let avg = (int)student.Scores!.Average() //使用let引入标识符
	    group student by (avg / 10) into g
	    orderby g.Key
	    select g;
	//按复合键分组
	var studentQuery5 =
	    from student in students
	    group student by new { id = student.ID, name = student.First };
	//对分组执行子查询
	var studentQuery6 =
	    from student in students
	    group student by student.First into studentGroup
	    select new
	    {
	        name = studentGroup.Key,
	        HighestScore = (from student2 in studentGroup
	                        select student2.Scores?.Max()).Max()
	    };
	//显示结果
	ShowInfo(studentQuery2);
	"studentQuery3".Dump("studentQuery3");
	ShowInfo(studentQuery3);
	"studentQuery4".Dump("studentQuery4");
	ShowInfo(studentQuery4);
	"studentQuery5".Dump("studentQuery5");
	ShowSpecialInfo(studentQuery5);
	//每组里取最高分
	studentQuery6.Select(p => p.name + ":" + p.HighestScore).Dump("studentQuery6");
	//使用SelectMany返回铺平之后的结果
	var studentQuery7 = studentQuery2.Select(p => p.Select(x => x)).SelectMany(p => p);
	string.Join('\n', studentQuery7).Dump("studentQuery7");
	
	//OrderBy 是主要排序, ThenBy是次要排序
	IEnumerable<Student> queryStudentByOrder = students.Where(p => p.First!.StartsWith("a")).OrderByDescending(s => s.ID).ThenBy(p => p.Scores!.Sum());
	string.Join('-', queryStudentByOrder).Dump();
	
	
	//集运算 : Distinct,Except,Intersect,Union
	//1. Distinct 去重,只保留一个相同项
	string[] planets = { "Mercury", "Venus", "Venus", "Earth", "Mars", "Earth" };
	string.Join(',', planets.Distinct()).Dump("Distinct");
	
	//2. Except 差集,位于一个集合但不位于另一个集合的元素
	string[] planets1 = { "Mercury", "Venus", "Earth", "Jupiter" };
	string[] planets2 = { "Mercury", "Earth", "Mars", "Jupiter" };
	string.Join(',', planets1.Except(planets2)).Dump("Except");
	
	//3. Intersect 交集,同时出现再两个集合中的元素
	string.Join(',', planets1.Intersect(planets2)).Dump("Intersect");
	
	//4. Union  联合, 位于两个集合中任一集合的唯一的元素
	string.Join(',', planets1.Union(planets2)).Dump("Union");
	
	int[] nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
	//限定符运算: All,Any,Contains
	//1. All
	nums.All(p => p > 0).Dump("All");
	//2. Any
	nums.Any(p => p >= 9).Dump("Any");
	//3. Contains
	nums.Contains(1).Dump("Contains");
	
	//数据分区: Skip,SkipWhile,Take,TakeWhile
	//1. Skip
	string.Join(',', nums.Skip(1)).Dump("Skip");
	//2. SkipWhile
	string.Join(',', nums.SkipWhile((p, i) => p > 2 && i > 4)).Dump("SkipWhile");
	
	int[] amounts = { 5000, 2500, 9000, 8000, 6500, 4000, 1500, 5500 };
	var query = amounts.SkipWhile((amount, index) =>
	{
	    amount.Dump();
	    return amount > 5000;
	});
	string.Join(',', query).Dump();
	//amounts.SkipWhile(a=>a>5000).ToList().Dump();
	
	
	//显示信息
	void ShowInfo<T>(IEnumerable<IGrouping<T, Student>> group) where T : IEquatable<T> //此处为演示T的类型为值类型
	{
	    foreach (var studentGroup in studentQuery2)
	    {
	        studentGroup.Key.Dump();
	        foreach (Student student in studentGroup)
	        {
	            $"   {student.First},{student.Last}".Dump();
	        }
	    }
	}
	
	void ShowSpecialInfo<T>(IEnumerable<IGrouping<T, Student>> group) where T : class
	{
	    foreach (var studentGroup in studentQuery2)
	    {
	        studentGroup.Key.Dump();
	        foreach (Student student in studentGroup)
	        {
	            $"   {student.First},{student.Last}".Dump();
	        }
	    }
	}
	
	
	//zip
	string[] s1 = new string[] { "zero", "one", "two" };
	string[] s2 = new string[] { "0", "1", "2" };
	
	s1.Zip(s2, (a, b) => new { key = a, value = b }).Dump();//[{key:zero,value:0},{key:one,value:1},{key:two,value:2}]
	
	//Aggregate 自定义对集合元素的两个值的操作
	int[] ints = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
	ints.Aggregate((a, b) => a + b).Dump();
}

//Linq
public class Student
{
    public string? First { get; set; }
    public string? Last { get; set; }
    public int ID { get; set; }
    public List<int>? Scores;
    public override string ToString()
    {
        return $"First:{First} Last:{Last} ID:{ID} Scores:{(string.Join(',', Scores ?? new List<int>()))}";
    }
}
