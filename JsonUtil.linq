<Query Kind="Program">
  <Namespace>System.Text.Json</Namespace>
</Query>

void Main()
{
	var s = "[{\"hour\":0,\"duration\":\"0-1时\",\"weekday\":130.4,\"weekend\":130.4},{\"hour\":1,\"duration\":\"1-2时\",\"weekday\":128.8,\"weekend\":128.8},{\"hour\":2,\"duration\":\"2-3时\",\"weekday\":128,\"weekend\":128},{\"hour\":3,\"duration\":\"3-4时\",\"weekday\":129.2,\"weekend\":129.2},{\"hour\":4,\"duration\":\"4-5时\",\"weekday\":137.8,\"weekend\":137.8},{\"hour\":5,\"duration\":\"5-6时\",\"weekday\":166,\"weekend\":166},{\"hour\":6,\"duration\":\"6-7时\",\"weekday\":189.4,\"weekend\":189.4},{\"hour\":7,\"duration\":\"7-8时\",\"weekday\":191.4,\"weekend\":191.4},{\"hour\":8,\"duration\":\"8-9时\",\"weekday\":191,\"weekend\":191},{\"hour\":9,\"duration\":\"9-10时\",\"weekday\":175.6,\"weekend\":175.6},{\"hour\":10,\"duration\":\"10-11时\",\"weekday\":160.4,\"weekend\":160.4},{\"hour\":11,\"duration\":\"11-12时\",\"weekday\":160,\"weekend\":160},{\"hour\":12,\"duration\":\"12-13时\",\"weekday\":158.8,\"weekend\":158.8},{\"hour\":13,\"duration\":\"13-14时\",\"weekday\":157,\"weekend\":157},{\"hour\":14,\"duration\":\"14-15时\",\"weekday\":152.6,\"weekend\":152.6},{\"hour\":15,\"duration\":\"15-16时\",\"weekday\":153.2,\"weekend\":153.2},{\"hour\":16,\"duration\":\"16-17时\",\"weekday\":165,\"weekend\":165},{\"hour\":17,\"duration\":\"17-18时\",\"weekday\":186.4,\"weekend\":186.4},{\"hour\":18,\"duration\":\"18-19时\",\"weekday\":203.2,\"weekend\":203.2},{\"hour\":19,\"duration\":\"19-20时\",\"weekday\":209,\"weekend\":209},{\"hour\":20,\"duration\":\"20-21时\",\"weekday\":207.6,\"weekend\":207.6},{\"hour\":21,\"duration\":\"21-22时\",\"weekday\":197.2,\"weekend\":197.2},{\"hour\":22,\"duration\":\"22-23时\",\"weekday\":173.4,\"weekend\":173.4},{\"hour\":23,\"duration\":\"23-0时\",\"weekday\":143.4,\"weekend\":143.4}]";
	var ss = "{\"hour\":0,\"duration\":\"0-1时\",\"weekday\":130.4,\"weekend\":130.4}";
	//var d = JsonUtil.GetJsonObjectProperty<double>(ss, "weekday");
	//d.Dump();
	var data = JsonUtil.GetJsonArrayProperty<double>(s, "hour").ToHashSet<double>();
	data.Dump();
}

/// <summary>
/// json text util
/// </summary>
public static class JsonUtil
{
	/// <summary>
	/// 获取json对象的属性值
	/// </summary>
	/// <param name="jsonText"></param>
	/// <param name="propertyName"></param>
	/// <returns></returns>
	public static T GetJsonObjectProperty<T>(string jsonText, string propertyName)
	{
		if (string.IsNullOrWhiteSpace(jsonText)) return default;
		using JsonDocument json = JsonDocument.Parse(jsonText);
		json.RootElement.TryGetProperty(propertyName, out JsonElement value);
		return GetData<T>(value);
	}
	/// <summary>
	/// 获取json数组的属性值
	/// </summary>
	/// <param name="jsonText"></param>
	/// <param name="propertyName"></param>
	/// <returns></returns>
	public static List<T> GetJsonArrayProperty<T>(string jsonText, string propertyName)
	{
		if (string.IsNullOrWhiteSpace(jsonText)) return default;
		using (JsonDocument json = JsonDocument.Parse(jsonText))
		{
			var array = json.RootElement.EnumerateArray();
			var t = typeof(T);
			var data = array.Select(p => p.GetProperty(propertyName));
			return t.Name switch
			{
				"Int32" => data.Select(a => a.GetInt32()).Cast<T>().ToList(),
				"Double" => data.Select(a => a.GetDouble()).Cast<T>().ToList(),
				"String" => data.Select(a => a.GetString()).Cast<T>().ToList(),
				"Decimal" => data.Select(a => a.GetDecimal()).Cast<T>().ToList(),
				"DateTime" => data.Select(a => a.GetDateTime()).Cast<T>().ToList(),
				"Boolean" => data.Select(a => a.GetBoolean()).Cast<T>().ToList(),
				"Char" => data.Select(a => a.GetByte()).Cast<T>().ToList(),
				"Guid" => data.Select(a => a.GetGuid()).Cast<T>().ToList(),
				_ => null,
			};
		};
	}

	private static T GetData<T>(JsonElement json)
	{
		var t = typeof(T);
		return t.Name switch
		{
			"Int32" => (T)(object)json.GetInt32(),
			"Double" => (T)(object)json.GetDouble(),
			"String" => (T)(object)json.GetString(),
			"Decimal" => (T)(object)json.GetDecimal(),
			"DateTime" => (T)(object)json.GetDateTime(),
			"Boolean" => (T)(object)json.GetBoolean(),
			"Char" => (T)(object)json.GetByte(),
			"Guid" => (T)(object)json.GetGuid(),
			_ => default,
		};
	}
}
