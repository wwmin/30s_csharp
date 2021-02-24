<Query Kind="Statements">
  <Output>DataGrids</Output>
</Query>

//使用Linq to Rx
/*
LINQ to Rx的数据模型与普通IEnumerable<T>的模型在数学上是对偶的。在开始对拉集合进行迭代时，我们以“请给我一个迭代器”（调用GetEnumerator）开始，
然后重复“还有其他项吗？如果有，就给我”（调用MoveNext和Current）。LINQ to Rx则是反向的。
它不向迭代器发出请求，而是提供一个观察者。然后，它也不请求下一个项，而是通知你的代码是否准备好了一个项、是否有错误发生、是否到达了数据末端。
以下是涉及的两个接口的声明：
*/
public interface IObservable<T>
{
	IDisposable Subscribe(IObserver<T> observer);
}

public interface IObserver<T>
{
	void OnNext(T value);
	void OnCompleted();
	void OnException(Exception error);
}