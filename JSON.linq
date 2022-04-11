<Query Kind="Program">
  <Namespace>System.Web</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Unicode</Namespace>
</Query>

void Main()
{
	var data = "%7B%22ua%22%3A%22Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20Win64%3B%20x64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F91.0.4472.124%20Safari%2F537.36%20Edg%2F91.0.864.67%22%2C%22browser%22%3A%7B%22name%22%3A%22Edge%22%2C%22version%22%3A%2291.0.864.67%22%2C%22major%22%3A%2291%22%7D%2C%22engine%22%3A%7B%22name%22%3A%22Blink%22%2C%22version%22%3A%2291.0.4472.124%22%7D%2C%22os%22%3A%7B%22name%22%3A%22Windows%22%2C%22version%22%3A%2210%22%7D%2C%22device%22%3A%7B%7D%2C%22cpu%22%3A%7B%22architecture%22%3A%22amd64%22%7D%2C%22visitorId%22%3A%22c8578aef7925e6d67655b715b4420137%22%2C%22screen%22%3A%7B%22value%22%3A%5B900%2C1440%5D%2C%22duration%22%3A0%7D%7D";
	var a = HttpUtility.UrlDecode(data);
	JsonSerializerOptions option = new JsonSerializerOptions()
	{
		AllowTrailingCommas = true,
		PropertyNameCaseInsensitive = true,
		IncludeFields = true
	};
	var uaInfo = JsonSerializer.Deserialize<UAInfo>(a, option);
	a.Dump();
	//uaInfo.Dump();
	
	parseJSON(a);

	//转换汉字及转义字符 
	// 使用此可将汉字及转义字符都正确输出 Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
	// 使用此只能正确转义汉字             Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
	string sa = "王为民->wwmin";
	var s = JsonSerializer.Serialize(sa, new JsonSerializerOptions() {
		Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
	});
	s.Dump();//"王为民-\u003Ewwmin"
	
	var so = new { name = "王为民->wwmin" };
	var so2s = JsonSerializer.Serialize(so, new JsonSerializerOptions() {
		Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
	});
	so2s.Dump();//{"name":"王为民->wwmin"}
	
}

private void parseJSON(string json)
{
	var options = new JsonDocumentOptions
	{
		AllowTrailingCommas = true
	};
	using (JsonDocument document = JsonDocument.Parse(json,options))
	{
		var visitorId = document.RootElement.GetProperty("visitorId");
		visitorId.GetString().Dump();
	}


}

/// <summary>
/// UAInfo
/// </summary>
public class UAInfo
{
	/// <summary>
	/// UA
	/// </summary>
	public string UA { get; set; }
	/// <summary>
	/// Browser
	/// </summary>
	public BrowserInfo Browser { get; set; }
	/// <summary>
	/// Engine
	/// </summary>
	public NameVersionClass Engine { get; set; }
	/// <summary>
	/// OS
	/// </summary>
	public NameVersionClass OS { get; set; }

	/// <summary>
	/// Cpu
	/// </summary>
	public CpuInfo CPU { get; set; }
	/// <summary>
	/// VisitorId
	/// </summary>
	public string VisitorId { get; set; }
	/// <summary>
	/// Screen
	/// </summary>
	public ScreenInfo Screen { get; set; }
}

/// <summary>
/// NameVersionClass
/// </summary>
public class NameVersionClass
{
	/// <summary>
	/// Name
	/// </summary>
	public string Name { get; set; }
	/// <summary>
	/// Version
	/// </summary>
	public string Version { get; set; }
}
/// <summary>
/// BrowserInfo
/// </summary>
public class BrowserInfo : NameVersionClass
{
	/// <summary>
	/// 主要版本
	/// </summary>
	public string Major { get; set; }
}
/// <summary>
/// 计算机设备信息
/// </summary>
public class DeviceInfo
{
	/// <summary>
	/// Model
	/// </summary>
	public string Model { get; set; }
	/// <summary>
	/// Type
	/// </summary>
	public string Type { get; set; }
	/// <summary>
	/// Vendor
	/// </summary>
	public string Vendor { get; set; }
}


/// <summary>
/// CpuInfo
/// </summary>
public class CpuInfo
{
	/// <summary>
	/// Architecture
	/// </summary>
	public string Architecture { get; set; }
}
/// <summary>
/// ScreenInfo
/// </summary>
public class ScreenInfo
{
	/// <summary>
	/// Value
	/// </summary>
	public int[] Value { get; set; }
	/// <summary>
	/// Duration
	/// </summary>
	public int Duration { get; set; }
}