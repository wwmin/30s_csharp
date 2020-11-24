//迭代器方法活get访问器的返回类型可以是IEnumerable、IEnumerable<T>
//或IEnumerator、IEnumerator<T>
//可以使用yield break语句终止迭代
//示例1
IEnumerable SomeNumbers(int firstNumber, int lastNumber)
{
    for (int number = firstNumber; number <= lastNumber; number++)
    {
        yield return number;
    }
}
foreach (int number in SomeNumbers(3, 5))
{
    number.ToString().Dump();
}

//创建集合类
//示例2
class DaysOfTheWeek : IEnumerable
{
    private readonly string[] days = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
    public IEnumerator GetEnumerator()
    {
        for (int index = 0; index < days.Length; index++)
        {
            yield return days[index];
        }
    }
}

DaysOfTheWeek days = new DaysOfTheWeek();
foreach (string day in days)
{
    day.Dump();
}

//示例3
class Zoo : IEnumerable
{
    private readonly List<Animal> animals = new List<Animal>();
    public void AddMammal(string name)
    {
        animals.Add(new Animal { Name = name, Type = Animal.TypeEnum.Mammal });
    }

    public void AddBird(string name)
    {
        animals.Add(new Animal { Name = name, Type = Animal.TypeEnum.Bird });
    }

    public IEnumerator GetEnumerator()
    {
        foreach (Animal theAnimal in animals)
        {
            yield return theAnimal.Name;
        }
    }

    public IEnumerable Mammals
    {
        get { return AnimalsForType(Animal.TypeEnum.Mammal); }
    }

    public IEnumerable Birds
    {
        get { return AnimalsForType(Animal.TypeEnum.Bird); }
    }

    private IEnumerable AnimalsForType(Animal.TypeEnum type)
    {
        foreach (Animal theAnimal in animals)
        {
            if (theAnimal.Type == type)
            {
                yield return theAnimal.Name;
            }
        }
    }

    private class Animal
    {
        public enum TypeEnum { Bird, Mammal }
        public string Name { get; set; }
        public TypeEnum Type { get; set; }
    }
}

Zoo theZoo = new Zoo();
theZoo.AddMammal("Whale");
theZoo.AddMammal("Rhinoceros");
theZoo.AddBird("Penguin");
theZoo.AddBird("Warbler");
foreach (string name in theZoo)
{
    name.Dump("all animals");
}
foreach (string name in theZoo.Mammals)
{
    name.Dump("mammals");
}
foreach (string name in theZoo.Birds)
{
    name.Dump("birds");
}

//示例4
class Stack<T> : IEnumerable<T>
{
    private readonly T[] values = new T[100];
    private int top = 0;
    public void Push(T t)
    {
        values[top] = t;
        top++;
    }
    public T Pop()
    {
        top--;
        return values[top];
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int index = top - 1; index >= 0; index--)
        {
            yield return values[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerable<T> TopToBottom
    {
        get { return this; }
    }

    public IEnumerable<T> BottomToTop
    {
        get
        {
            for (int index = 0; index <= top - 1; index++)
            {
                yield return values[index];
            }
        }
    }

    public IEnumerable<T> TopN(int itemsFromTop)
    {
        int startIndex = itemsFromTop >= top ? 0 : top - itemsFromTop;
        for (int index = top - 1; index >= startIndex; index--)
        {
            yield return values[index];
        }
    }
}

Stack<int> theStack = new Stack<int>();
for (int number = 0; number < 9; number++)
{
    theStack.Push(number);
}

foreach (int number in theStack)
{
    number.Dump("the stack list");
}

foreach (int number in theStack.TopToBottom)
{
    number.Dump("top to bottom");
}
foreach (int number in theStack.BottomToTop)
{
    number.Dump("bottom to top");
}

foreach (int number in theStack.TopN(4))
{
    number.Dump("top n");
}