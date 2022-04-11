<Query Kind="Program" />

//PriorityQueue .NET 6 新增数据结构
void Main()
{
	PriorityQueue<string, int> priorityQueue = new();
	priorityQueue.Enqueue("Second", 2);
	priorityQueue.Enqueue("Fourth", 4);
	priorityQueue.Enqueue("Third 1", 3);
	priorityQueue.Enqueue("Third 2", 3);
	priorityQueue.Enqueue("Third 3", 3);
	priorityQueue.Enqueue("First", 1);
	
	var mini_item = priorityQueue.Peek();
	mini_item.Dump("mini_item");
	
	var count=priorityQueue.Count;
	count.Dump("count");
	
	var enqueueDequeue = priorityQueue.EnqueueDequeue("Four",4);
	enqueueDequeue.Dump("EnqueueDequeue");

	var queueList = new List<(string, int)>(){("five",5),("six",6)};
	priorityQueue.EnqueueRange(queueList);
	
	priorityQueue.
	
	while (priorityQueue.Count > 0)
	{
		string item = priorityQueue.Dequeue();
		Console.WriteLine(item);
	}

	// Output:
	// First
	// Second
	// Third 2
	// Third 1
	// Fourth

}