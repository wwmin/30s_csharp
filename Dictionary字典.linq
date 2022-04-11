<Query Kind="Program">
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Unicode</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	//使用Dictionary add方法初始化字典
	var students = new Dictionary<int, StudentName>(){
	{1,new StudentName{Name = "ww",ID =1}},
	{2,new StudentName{Name = "mm",ID=2}}
	};

	foreach (var index in Enumerable.Range(1, 2))
	{
		$"student {index} is {students[index].Name} {students[index].ID}".Dump("student");
	}
	Console.WriteLine();

	//使用索引初始化字典
	var students2 = new Dictionary<int, StudentName>()
	{
		[3] = new StudentName { Name = "ww2", ID = 3 },
		[4] = new StudentName { Name = "mm2", ID = 4 }
	};
	foreach (var index in Enumerable.Range(3, students2.Count))
	{
		$"student {index} is {students2[index].Name} {students2[index].ID}".Dump("student2");
	}

	//使用带有修改值监听的Dictionary
	var _dictionaryWrapper = new DictionaryWrapper<int, StudentName>(students);
	_dictionaryWrapper.OnValueChanged += new EventHandler<ValueChangedEventArgs<int, StudentName>>(OnConfigUsedChanged);
	_dictionaryWrapper[1] = new StudentName() { Name = "自定义1", ID = 1 };

	void OnConfigUsedChanged(object sender, ValueChangedEventArgs<int, StudentName> e)
	{
		var options = new JsonSerializerOptions()
		{
			Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
		};
		($"字典:{e.Key}的值发生变更,新值:{JsonSerializer.Serialize(e.NewValue, options)},原值:{JsonSerializer.Serialize(e.OldValue, options)},请注意...").Dump("字典值变更通知");
	}

}

//Dictionary<TKey,TValue>
class StudentName
{
	public string Name { get; set; }
	public int ID { get; set; }
}


public class ValueChangedEventArgs<TKey, TValue> : EventArgs
{
	public TKey Key { get; set; }
	public TValue NewValue { get; set; }
	public TValue OldValue { get; set; }
	public ValueChangedEventArgs(TKey key, TValue newValue, TValue oldValue)
	{
		Key = key;
		NewValue = newValue;
		OldValue = oldValue;
	}
}

/// <summary>
/// 自定义Dictionary
/// 修改value值具有通知事件	
/// </summary>
public class DictionaryWrapper<TKey, TValue>
{
	public object objLock = new object();

	private Dictionary<TKey, TValue> _dict;
	public event EventHandler<ValueChangedEventArgs<TKey, TValue>> OnValueChanged;
	public DictionaryWrapper(Dictionary<TKey, TValue> dict)
	{
		_dict = dict;
	}
	public TValue this[TKey key]
	{
		get => _dict[key];
		set
		{
			lock (objLock)
			{
				try
				{
					if (_dict.ContainsKey(key) && _dict[key] != null && !_dict[key].Equals(value))
					{
						OnValueChanged(this, new ValueChangedEventArgs<TKey, TValue>(key, value, _dict[key]));
					}
				}
				catch (Exception ex) when (_dict.Count() > 0)
				{
					Console.WriteLine($"检测值变更或触发值变更事件,发生未知异常{ex}");
				}
				finally
				{
					_dict[key] = value;
				}
			}
		}
	}
}
