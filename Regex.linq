<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var url = "https://mi.com/swagger.json";
	UrlDiscern(url).Dump("url");
}


/// <summary>
/// 识别urlStr是否是网络路径
/// </summary>
/// <param name="urlStr"></param>
/// <returns></returns>
public static bool UrlDiscern(string urlStr)=>Regex.IsMatch(urlStr, @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?");