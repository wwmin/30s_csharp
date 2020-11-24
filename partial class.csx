public class BaseInfo
{
    protected static readonly DateTime globalStartTime;
    static BaseInfo()//静态构造函数, 只在实例化该类前执行依次
    {
        globalStartTime = DateTime.Now;
    }
    public BaseInfo(int id) => Id = id;
    //    {
    //        Id = id;
    //    }
    public int Id { get; set; }
}
interface IDoWord
{
    void DoWork();
}
//分别给部分类加特性,相当于对同一个类加两个特性
[SerializableAttribute]
public partial class Employee : BaseInfo
{
    public string CardId { get; set; }
    public Employee(int id) : base(id)
    {
        CardId = "0_" + id.ToString(); ;
    }
    public void DoWork()
    {
        $"Employee do work elapsedTime {DateTime.Now - globalStartTime}".Dump();
    }
}
[Obsolete]//废弃的标识
public partial class Employee : IDoWord
{
    public void GoToLunch()
    {
        $"Employee (cardId:{CardId}) go to lunch".Dump();
    }

    public override string ToString()
    {
        return $"{CardId}";
    }

}

Employee e = new Employee(1);

e.GoToLunch();
e.Id.Dump();
e.DoWork();
e.ToString().Dump();