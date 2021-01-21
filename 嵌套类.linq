<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	Container.Nested nest = new Container.Nested();
	nest.GetParentId().Dump();
	
	
	Container.Nested nest2 = new Container.Nested(new Container());
	nest2.GetParentId().Dump();
}

//嵌套类
public class Container
{
    public int Id { get; set; } = 1;
    public Container()
    {
        Id = 2;
    }
    public class Nested
    {
        private readonly Container? parent;
        public Nested()
        {

        }
        public Nested(Container parent)
        {
            this.parent = parent;
        }
        public int GetParentId()
        {
            return parent?.Id ?? 0;
        }
    }
}
