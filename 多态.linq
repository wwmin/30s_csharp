<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

//多态: 
//方法一: 子类可以替换父类 3个条件(机制): 
//1.第一个语法机制是编程语言要支持父类对象可以引用子类对象
//2.第二个语法机制是编程语言要支持继承
//3.第三个语法机制是编程语言要支持子类可以重写（override）父类中的方法
//方法二: 使用接口类实现
//方法三: 使用duck-typing实现(动态语言的实现,将具有相同的方法的对象传入到一个方法中,该方法中能使用特定的方法即可)
void Main()
{
	{
		//1.父类对象可以引用子类对象
		DynamicArray dynamicArray = new SortedDynamicArray();
		Test(dynamicArray);//1,3,5
	}


	{
		IIterator arrayIterator = new MyArray(new List<string>() { "a", "b" });
		print(arrayIterator);

		IIterator linkedListIterator = new MyLinkArray(new List<string>() { "A", "B" });
		print(linkedListIterator);
	}
}

#region 使用继承+方法重写 实现多态
public static void Test(DynamicArray dynamicArray)
{
	dynamicArray.Add(5);
	dynamicArray.Add(1);
	dynamicArray.Add(3);
	for (int i = 0; i < dynamicArray.size; i++)
	{
		dynamicArray[i].Dump();
	}
}


public class DynamicArray
{
	private static int DEFAULT_CAPACITY = 2;
	public int size { get; set; }
	protected int capacity = DEFAULT_CAPACITY;
	protected int[] elements = new int[DEFAULT_CAPACITY];

	public int this[int index]
	{

		get => elements[index];
		set => elements[index] = value;
	}

	public virtual void Add(int e)
	{
		ensureCapacity();
		elements[size++] = e;
	}

	protected void ensureCapacity()
	{
		if (size >= capacity - 1)
		{
			capacity *= 2;
			//如果数组满了就扩容
			var newArray = new int[capacity];
			Array.Copy(elements, newArray, size);
			elements = newArray;
		}
	}
}

//2.继承
public class SortedDynamicArray : DynamicArray
{
	//3.子类重写父类方法
	public override void Add(int e)
	{
		ensureCapacity();
		int i;
		for (i = size - 1; i >= 0; --i)
		{
			if (elements[i] > e)
			{
				elements[i + 1] = elements[i];
			}
			else
			{
				break;
			}
		}
		elements[i + 1] = e;
		++size;
	}
}

#endregion

#region 使用接口类实现多态
public static void print(IIterator iterator)
{
	while (iterator.hasNext())
	{
		iterator.next().Dump();
	}
}

public interface IIterator
{
	bool hasNext();
	string next();
	bool remove(string value);
}
public class MyArray : IIterator
{
	protected List<string> arr { get; set; }
	public int currentIndex { get; set; } = 0;


	public MyArray(List<string> list)
	{
		arr = list;
	}
	public bool hasNext()
	{
		return arr.Count() > currentIndex;
	}

	public string next()
	{
		"".Dump("MyArray");
		return arr[currentIndex++];
	}

	public bool remove(string value)
	{
		return arr.Remove(value);
	}
}

public class MyLinkArray : IIterator
{
	protected List<string> arr { get; set; }
	public int currentIndex { get; set; }
	public MyLinkArray(List<string> list)
	{
		arr = list;
	}

	public bool hasNext()
	{
		return arr.Count() > currentIndex;
	}

	public string next()
	{
		"".Dump("MyLinkArray");
		return arr[currentIndex++];
	}

	public bool remove(string value)
	{
		return arr.Remove(value);
	}
}
#endregion