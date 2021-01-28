<Query Kind="Program">
  <Output>DataGrids</Output>
  <Namespace>System.Runtime.InteropServices</Namespace>
</Query>

//GC 垃圾回收
//将C#的类型分为两类：一类实现了IDisposable，另一类则没有。前者我们定义为非普通类型，后者为普通类型。
//非普通类型包含了非托管资源，实现了IDisposable，但又包含有自身是托管资源，所以不普通，
//结论: 普通类型不需要手动清理，而非普通类型需要手动清理。并且不需要将不在使用的对象置为null,而静态字段需要
void Main()
{
	#region 普通class
	{
		var mc1 = new MyClass() { Name = "mc1" };
		var mc2 = new MyClass() { Name = "mc2" };

		mc1 = null;
	}
	#endregion
	#region 带有静态字段的class
	{
		var mc = new MyClass1() { Name = "mc" };
		//注意结果中, mc已被销毁,但mc中MyClass静态对象字段并没有被销毁
		/*上面的代码运行我们会发现，当mc被回收时，它的静态属性并没有被GC回收，
		而我们将MyClass终结器中的MyClass2=null的注释取消，
		再运行，当我们两次点击按钮7的时候，属性MyClass2才被真正的释放，
		因为第一次GC的时候只是在终结器里面将MyClass属性置为null，
		在第二次GC的时候才当作垃圾回收了，之所以静态变量不被释放(即使赋值为null也不会被编译器优化)，
		是因为类型的静态字段一旦被创建，就被作为“根”存在，基本上不参与GC，
		所以GC始终不会认为它是个垃圾，而非静态字段则不会有这样的问题。*/

		MyClass1.MyClass = null!;//此时静态变量才可回收
	}
	#endregion
	#region 显示释放资源
	{
		using (var mc = new MyClassWithDispose())
		{

		}
		//上面相当于
		MyClassWithDispose mcwd = null!;
		try
		{
			mcwd = new MyClassWithDispose();
		}
		finally
		{
			if (mcwd != null)
			{
				mcwd.Dispose();
			}
		}

	}
	#endregion
}

#region 普通对象
public class MyClass
{
	public string Name { get; set; } = string.Empty;
	~MyClass()
	{
		$"{Name}被销毁了".Dump(nameof(MyClass));
	}
}
#endregion

#region 含有静态字段的对象
//非显示释放
public class MyClass1
{
	public string Name { get; set; } = string.Empty;
	public static MyClass MyClass { get; set; } = new MyClass() { Name = $"在{nameof(MyClass1)}中的{nameof(MyClass)}" };
	~MyClass1()
	{
		$"{Name}被销毁了".Dump(nameof(MyClass1));
	}
}
#endregion

#region 显示释放资源
public class MyClassWithDispose : IDisposable
{
	/// <summary>模拟一个非托管资源</summary>
	private IntPtr NativeResource { get; set; } = Marshal.AllocHGlobal(100);
	/// <summary>模拟一个托管资源</summary>
	public Random ManagedResource { get; set; } = new Random();
	/// <summary>释放标记</summary>
	private bool disposed;

	/*这个析构方法更规范的说法叫做终结器，它的意义在于，如果我们忘记了显式调用Dispose方法，垃圾回收器在扫描内存的时候，会作为释放资源的一种补救措施。
	为什么加了析构方法就会有这种效果，我们知道在new对象的时候，CLR会为对象创建一块内存空间，一旦对象不再被引用，就会被垃圾回收器回收掉，
	对于没有实现IDisposable接口的类来说，垃圾回收时将直接回收掉这片内存空间，而对于实现了IDisposable接口的类来说，
	由于析构方法的存在，在创建对象之初，CLR会将该对象的一个指针放到终结器列表中，在GC回收内存之前，会首先将终结器列表中的指针放到一个freachable队列中，
	同时，CLR还会分配专门的内存空间来读取freachable队列，并调用对象的终结器，只有在这个时候，对象才被真正的被标识为垃圾，在下一次垃圾回收的时候才回收这个对象所占用的内存空间。
	那么，实现了IDisposable接口的对象在回收时要经过两次GC才能被真正的释放掉，因为GC要先安排CLR调用终结器，
	基于这个特点，如果我们显式调用了Dispose方法，那么GC就不会再进行第二次垃圾回收了，
	当然，如果忘记了Dispose，也避免了忘记调用Dispose方法造成的内存泄漏。
	*/
	/// <summary>为了防止忘记显示的调用Dispose方法</summary>
	~MyClassWithDispose()
	{
		//必须为false
		Dispose(false);
	}
	/*还有一点我们也注意到了，如果已经显式的调用了Dispose方法，那么隐式释放资源就再没必要运行了，GC的SuppressFinalize方法就是通知GC的这一点：*/
	/// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
	public void Dispose()
	{
		//必须为true
		Dispose(true);
		//通知垃圾回收器不再调用终结器
		GC.SuppressFinalize(this);

		$"{nameof(MyClassWithDispose)}被释放了".Dump(nameof(MyClassWithDispose));
	}
	/// <summary>非必须,只是为了更符合其他语言的规范</summary>
	public void Close()
	{
		Dispose();
	}
	/*为什么需要提供一个Dispose虚方法？
	我们注意到了，实现自Idisposable接口的Dispose方法并没有做实际的清理工作，而是调用了我们这个受保护的Dispose虚方法：
	之所以是虚方法，就是考虑到它如果被其他类继承时，子类也实现了Dispose模式，这个虚方法可以提醒子类，清理的时候要注意到父类的清理工作，
	即如果子类重新该方法，必须调用base.Dispose方法，
	如果不是虚方法，那么就很有可能让开发者在子类继承的时候忽略掉父类的清理工作，所以，基于继承体系的原因，我们要提供这样的一个虚方法。
	*/
	/// <summary>非密封类可重写的Dispose方法,方便子类继承时可重写</summary>
	protected virtual void Dispose(bool disposing)
	{
		if (disposed) return;
		//清理托管资源
		if (disposing)
		{
			if (ManagedResource != null)
			{
				ManagedResource = null!;
			}
		}
		//清理非托管资源
		if (NativeResource != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(NativeResource);
			NativeResource = IntPtr.Zero;
		}
		//告诉自己已经被释放
		disposed = true;
	}
}
#endregion

//参考: https://masuit.com/1202