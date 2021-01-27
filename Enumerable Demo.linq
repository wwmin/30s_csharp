<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	Enumerable.Range(1, 4).Count(n => Enumerable.Range(1, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)).Dump();
	
	Enumerable.Range(1, 5).Count(n => n / 2 > 0).Dump();
	Enumerable.Repeat(1, 4).Dump();
	
	SortedList sl = new SortedList();
	sl.Add(2, "wwmin");
	sl.Add(1, "liyue");
	//sl.Add(1,"ww");
	sl.Dump();
	
	sl.Keys.Dump();
	
	SortedSet<int> ss = new SortedSet<int>();
	ss.Add(2);
	ss.Add(2);
	ss.Add(1);
	ss.Add(1);
	
	ss.Dump();
	
	var s = Enumerable.Range(1, 10);
	
	
	s.SingleOrDefaultLocal(p => p == 3).Dump();
	s.TryGetFirst(p => p == 2, out bool found).Dump();
	ss.SingleOrDefaultLocal(p => p == 1).Dump();
	
	//使用实现IEnumerable和IEnumerator的类
	Cars cars = new Cars();
	foreach (var car in cars)
	{
	    car.Name.Dump("Car");
	}
	
	//使用yield 实现IEnumerable
	IEnumerable<Car> MyCars(string name = "")
	{
	    yield return new Car("长城", 1951);
	    if (name == "tsl") yield break;
	    yield return new Car("Ford", 1992);
	    yield return new Car("Fiat", 1988);
	    yield return new Car("Buick", 1932);
	    yield return new Car("Toyota", 1932);
	    yield return new Car("Dodge", 1999);
	    yield return new Car("Honda", 1977);
	}
	
	void ShowCars(string name = "")
	{
	    foreach (var car in MyCars(name))
	    {
	        car.Name.Dump();
	    }
	}
	ShowCars();
	"tsl yield break".Dump("yield break");
	ShowCars("tsl");
	
	"使用yield 实现Cars类".Dump("使用yield 实现Cars类");
	CarsY carsY = new CarsY();
	foreach (var car in carsY)
	{
	    ((Car)car).Name.Dump("CarsY");
	}
}


//手动实现IEnumerator 和 IEnumerable
public class Cars : IEnumerable<Car>
{

#nullable disable
    private readonly Car[] carList;
#nullable enable
    public Cars()
    {
        carList = new Car[6]{
           new Car("Ford", 1992),
           new Car("Fiat", 1988),
           new Car("Buick", 1932),
           new Car("Toyota",1932),
           new Car("Dodge",1999),
           new Car("Honda",1977)
        };
    }
    private class MyEnumerator<Car> : IEnumerator<Car>
    {
        public Car[] carList;
        int position = -1;

        public MyEnumerator(Car[] list)
        {
            carList = list;
        }
        private IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }
        public bool MoveNext()
        {
            position++;
            return (position < carList.Length);
        }
        public void Reset()
        {
            position = -1;
        }

        public void Dispose()
        {
            if (this.GetEnumerator() is IDisposable disposable)
            {
                //                disposable.Dispose();
                disposable = null!;
                disposable.Dump();
            }
        }

        public Car Current
        {
            get
            {
                try
                {
                    return carList[position]!;
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current => throw new NotImplementedException();
    }
    public IEnumerator<Car> GetEnumerator()
    {
        return new MyEnumerator<Car>(carList);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new MyEnumerator<Car>(carList);
    }
}

public class Car
{
    public Car(string name, int age)
    {
        this.Name = name;
        this.Age = age;
    }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
//使用yield 实现Cars类
public class CarsY : IEnumerable<Car>
{
    private readonly Car[] carList;
    public CarsY()
    {
        carList = new Car[6]{
           new Car("Ford", 1992),
           new Car("Fiat", 1988),
           new Car("Buick", 1932),
           new Car("Toyota",1932),
           new Car("Dodge",1999),
           new Car("Honda",1977)
        };
    }
    public IEnumerator<Car> GetEnumerator()
    {
        for (int i = 0; i < carList.Length; i++)
        {
            yield return carList[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}


public static class Utils
{
	//实现SingleOrDefault
	public static TSource SingleOrDefaultLocal<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		using (IEnumerator<TSource> enumerator = source.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TSource current = enumerator.Current;
				if (predicate(current))
				{
					while (enumerator.MoveNext())
					{
						if (predicate(enumerator.Current))
						{
							throw new Exception("source has more item");
						}
					}
					return current;
				}
			}
		}
#nullable disable
		return default;
#nullable enable
	}

	//实现FirstOrDefault
	public static TSource TryGetFirst<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out bool found)
	{
		//    IOrderedEnumerable<TSource> orderedEnumerable = source as IOrderedEnumerable<TSource>;
		//if (source is IOrderedEnumerable<TSource> orderedEnumerable)
		//{
		//	return orderedEnumerable.TryGetFirst(predicate, out found);
		//}
		foreach (TSource item in source)
		{
			if (predicate(item))
			{
				found = true;
				return item;
			}
		}
		found = false;
#nullable disable
		return default;
#nullable enable
	}

}