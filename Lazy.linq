<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	Author author = new Author();
	author.Blogs.Value.Dump();//需要值时的
	
	//懒加载实例
	StateManager sm = StateManager.Instance;
	sm.Age.Dump();
}

//Lazy 使用场景
//延迟初始化 是一种将对象的创建延迟到第一次需要用时的技术
public class Author
{
    public int Id { get; set; }
    public string? Name { get; set; }
    //    public List<Blog> Blogs{get;set;}//未使用Lazy时
    public Lazy<IList<Blog>> Blogs => new Lazy<IList<Blog>>(() => GetBlogDetailsForAuthor(this.Id));//使用Lazy初始化,实例化时需要Blogs.Value
    private IList<Blog> GetBlogDetailsForAuthor(int Id)
    {
        List<Blog> blogs = new List<Blog>
        {
            new Blog() { Id = 1, Title = "1", PublicationDate = DateTime.Now.AddDays(-1) },
            new Blog() { Id = 1, Title = "11", PublicationDate = DateTime.Now.AddDays(-2) },
            new Blog() { Id = 3, Title = "3", PublicationDate = DateTime.Now.AddDays(-3) }
        };
        return blogs.Where(p => p.Id == Id).ToList();
    }
}



public class Blog
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime PublicationDate { get; set; }
}

//懒加载实现
//不用Lazy的实现
public sealed class MyStateManager
{
    private MyStateManager() { }
    public static MyStateManager Instance
    {
        get
        {
            return Nested.obj;
        }
    }

    private class Nested
    {
        static Nested() { }
        internal static readonly MyStateManager obj = new MyStateManager();
    }
}

//使用Lazy的实现单例模式
public class StateManager
{
    public int Age { get; set; }
    private static readonly Lazy<StateManager> obj = new Lazy<StateManager>(() => new StateManager());
    private StateManager() { }
    public static StateManager Instance
    {
        get
        {
            return obj.Value;
        }
    }
}

//Lazy类实现原理
//public class Lazy<T>{
//    public T Value{
//        get{
//            if(_state != null){
//                return CreateValue();
//            }
//            return _value;
//        }
//    }
//}
