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

// Create a data source by using a collection initializer.
static List<Student> students = new List<Student>
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
    where student?.Scores?[0] > 90 && student.Scores[3] < 80
    orderby student.Last ascending//修改查询 排序
    select student.First +":"+ student.Last;//投影或映射
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
ShowInfo(studentQuery2);
"studentQuery3".Dump("studentQuery3");
ShowInfo(studentQuery3);
"studentQuery4".Dump("studentQuery4");
ShowInfo(studentQuery4);
"studentQuery5".Dump("studentQuery5");
ShowSpecialInfo(studentQuery5);
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


//OrderBy 是主要排序, ThenBy是次要排序
IEnumerable<Student> queryStudentByOrder = students.Where(p=>p.First!.StartsWith("a")).OrderByDescending(s => s.ID).ThenBy(p => p.Scores!.Sum());
string.Join('-', queryStudentByOrder).Dump();


