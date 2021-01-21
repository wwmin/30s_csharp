<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	//使用Dictionary add方法初始化字典
	var students = new Dictionary<int, StudentName>(){
	{1,new StudentName{Name = "ww",ID =1}},
	{2,new StudentName{Name = "mm",ID=2}}
	};
	
	foreach (var index in Enumerable.Range(1, 2))
	{
	    $"student {index} is {students[index].Name} {students[index].ID}".Dump("student");
	}
	Console.WriteLine();
	
	//使用索引初始化字典
	var students2 = new Dictionary<int, StudentName>()
	{
	    [3] = new StudentName { Name = "ww2", ID = 3 },
	    [4] = new StudentName { Name = "mm2", ID = 4 }
	};
	foreach (var index in Enumerable.Range(3, students2.Count))
	{
	    $"student {index} is {students2[index].Name} {students2[index].ID}".Dump("student2");
	}
}

//Dictionary<TKey,TValue>
class StudentName
{
    public string? Name { get; set; }
    public int ID { get; set; }
}
