<Query Kind="Statements">
  <Output>DataGrids</Output>
</Query>

DateTime.Now.ToUnixMillis().Dump();

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
}
