<Query Kind="Expression">
  <NuGetReference>Newtonsoft.Json.Bson</NuGetReference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

//我们通常只认为HTTP之传输少量的数据，但是HTTP 1.1规范中定了可以使用Transfer-Encoding：chunk的方式分块传输大量数据。
//所以再需要传输大量数据的情况下（尤其是做上下游系统数据集成分析类应用时）可以使用HTTP流式传输，并流式加载和处理他们。好处是占用内存资源大大减少，并可以处理大量数据传输。
async void Main()
{
	string year = "FY22";
	string accountIds = "123";
	//在main方法使用这个异步流返回的结果
	await foreach(var item in LoadApiData(year,accountIds)
	{
		//处理item
	}
}
//该方法使用了C# 8.0的   IAsyncEnumerable 异步流接口
//这里的作用就是为了异步返回流式读取到的JSON数据。
Private Async IAsyncEnumerable<string> mockHttpReadLargerJson(string year, string accountIds)
{
	String apiUrl = "http://salesforce/api?Year={year}&accountId={accountIds}";
	HttpClient client = New HttpClient();
	//获取响应流,该方法会很快返回,因为HTTP一旦准备好头部就会返回,不会等待所有数据完成后再返回
	Stream jsonResponse = Await client.GetStreamAsync(apiUrl);
	var reader = New StreamReader(jsonResponse);
	//Newtonsoft JSON读取器，它解决了JSON数组流式返回需要分析json格式的问题。
	var jr = New JsonTextReader(reader);
	JavaScriptSerializer serializer = new JavaScriptSerializer();
	while (jr.Read())
	{
		var item = serializer.Deserialize<DataModel>(jr);
		yield return item;
	}
}