<Query Kind="Statements">
  <Output>DataGrids</Output>
</Query>

DateTime.Now.ToUnixMillis().Dump();//1625903158623
DateTime.Now.ToDate().Dump();//2021-07-10 03:45:58

public static class Util
{
	/// <summary>
	/// 扩展日期: 获取日期的(自1970-1-1 00:00:00开始的)Unix毫秒数
	/// <para>通常用于Java或JavaScript中时间戳比较</para>
	/// </summary>
	public static long ToUnixMillis(this DateTime date)
	{
		DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		TimeSpan diff = date.ToUniversalTime() - origin;
		return (long)diff.TotalMilliseconds;
	}
	
	/// <summary>
	///  使用toString将DateTime格式化
	/// </summary>
	public static string ToDate(this DateTime date){
		return date.ToString("yyyy-MM-dd hh:mm:ss");
	}
}
