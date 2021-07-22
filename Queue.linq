<Query Kind="Program" />

void Main()
{
	//Queue q = new Queue();

	Queue<string> q = new Queue<string>();
	
	q.Enqueue("A");
	q.Enqueue("M");
	q.Enqueue("G");
	q.Enqueue("W");

	var qe= q.GetEnumerator();
	qe.MoveNext();
	qe.MoveNext();
	qe.Current.Dump("使用计数器获取值");
	
	q.Dump();
	"".Dump();
	q.Enqueue("V");
	q.Enqueue("H");
	q.Dump();

	"removing some values".Dump();
	string ch = q.Dequeue();
	$"The removed value: {ch}".Dump();
	ch = (string)q.Dequeue();
	$"The removed value: {ch}".Dump();
	q.Dump();
	var tryDequeue = q.TryDequeue(out string tryDequeueString);
	if (tryDequeue) tryDequeueString.Dump("尝试移出第一个元素");

	q.Contains("A").Dump("Contains A");
	q.ToArray().Dump("Queue To Array");
	q.Peek().Dump("返回第一个元素,不移除该元素");
	q.Clear();
	if (q.TryPeek(out string res)) res.Dump("尝试返回第一个元素,不移除该元素");
}