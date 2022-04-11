<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Namespace>System.Globalization</Namespace>
</Query>

//日期时间相关
DateTime.Now.ToUnixMillis().Dump();//1625903158623
DateTime.Now.ToDate().Dump();//2021-07-10 03:45:58

DateOnly dateOnly = new(2021,12,17);
dateOnly.Dump();

TimeOnly timeOnly=new(16,33,12);
timeOnly.ToString("hh:mm:ss").Dump();

DateOnly dateOnlyFromDate = DateOnly.FromDateTime(DateTime.Now);
dateOnlyFromDate.Dump();

TimeOnly timeOnlyFromDate = TimeOnly.FromDateTime(DateTime.Now);
timeOnlyFromDate.ToString("hh:mm:ss").Dump();


Console.BackgroundColor = ConsoleColor.White;
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("Hello DateTime!");

{
//一、定义
//DateTime 的定义，从定义可以知道，DateTime实现了IComparable、IConvertible、IEquatable、IFormattable、ISerializable。因此，DateTime可以让我们使用有关日期和时间的很多相关信息。
//public struct DateTime : IComparable, IComparable<DateTime>, IConvertible, IEquatable<DateTime>, IFormattable, System.Runtime.Serialization.ISerializable
}
{

//二、构造
DateTime d1 = new DateTime(2020, 7, 18);
DateTime d2 = new DateTime(2020, 7, 18, 23, 06, 20);
DateTime d3 = new DateTime(1595084858242000);
}

{
//三、静态字段
var maxValue = DateTime.MaxValue;
var minValue = DateTime.MinValue;
var unix = DateTime.UnixEpoch;
System.Console.WriteLine($"{maxValue} | {minValue} | {unix}");

System.Console.WriteLine(DateTime.Now.ToUnixTicks());
//在Javascript中引入时间
//var time = new Date().setTime(unix_ticks);
}

{
//四、方法
TimeSpan duration = new System.TimeSpan(30, 0, 0, 0);
DateTime newDate1 = DateTime.Now.Add(duration);
System.Console.WriteLine(newDate1.ToString());

DateTime today = DateTime.Now;
DateTime newDate2 = today.AddDays(30);
System.Console.WriteLine(newDate2.ToString());

string dateString = "2020-07-18 23:35:50";
DateTime dateTime1 = DateTime.Parse(dateString);

DateTime date1 = new DateTime(2020, 7, 18, 23, 36, 10);
DateTime date2 = new DateTime(2020, 7, 18, 23, 38, 10);
DateTime date3 = new DateTime(2020, 7, 18, 23, 40, 10);

TimeSpan diff1 = date2.Subtract(date1);
DateTime date4 = date3.Subtract(diff1);
TimeSpan diff2 = date3 - date2;
DateTime date5 = date2 - diff1;
System.Console.WriteLine($"{diff1} | {diff2} | {date4} | {date5}");

}


{
//五、属性
DateTime myDate = new DateTime(2020, 7, 18, 23, 44, 40);
int year = myDate.Year;
int month = myDate.Month;
int day = myDate.Day;
int hour = myDate.Hour;
int minute = myDate.Minute;
int second = myDate.Second;
int weekDay = (int)myDate.DayOfWeek;
string weekString = myDate.DayOfWeek.ToString();
System.Console.WriteLine($"{year} | {month} | {day} | {hour} | {minute} | {second} | {weekDay} | {weekString}");
}
{
//六、DateTimeKind
//DateTimeKind用来定义实例表示的时间是基于本地时间（LocalTime)、UTC时间（UTC）或是不指定（Unspecified）。
//在大多数情况下，我们定义时间就直接定义年月日时分秒，例如下面：
DateTime myDate = new DateTime(2020, 7, 19, 0, 16, 10);
//这种定义下，这个时间就是Unspecified的。
//在使用时，如果应用过程中不做时间转换，始终以这种方式用，那不会有任何问题。
//但在某些情况下，时间有可能会发生转换，例如跨国应用的时间处理，
//再例如MongoDB，在数据库保存数据时，强制使用UTC时间。这种情况下，处理时间就必须采用LocalTime或UTC时间：
DateTime myDateLocal = new DateTime(2020, 7, 19, 0, 19, 20, DateTimeKind.Local);
DateTime myDateUnspecified = new DateTime(2020, 7, 19, 0, 19, 20, DateTimeKind.Local);
//否则，在时间类型不确定的情况下，时间转换会出问题
DateTime myDate1 = new DateTime(2020, 7, 19, 0, 12, 10);
var date1 = myDate1.ToLocalTime();
System.Console.WriteLine(date1.ToString());//2020/7/19 8:12:10

var date2 = myDate.ToUniversalTime();
Console.WriteLine(date2.ToString());//2020/7/18 16:16:10
									//当使用ToLocalTime方法时，Unspecified时间会认为自己是UTC时间，
									//而当使用ToUniversalTime时，Unspecified时间又会认为自己是LocalTime时间，导致时间上的转换错误

}

{
//七、时间对象的加减及比较
DateTime date1 = new DateTime(2020, 7, 14);
TimeSpan timeSpan = new TimeSpan(10, 5, 5, 2);
DateTime addResult = date1 + timeSpan;
DateTime subtractResult = date1 - timeSpan;

DateTime date2 = new DateTime(2020, 7, 14);
DateTime date3 = new DateTime(2020, 7, 15);
bool isEqual = date2 == date3;
System.Console.WriteLine(isEqual);
}
{
//八、日期的格式化
//相关表达式图：https://mmbiz.qpic.cn/sz_mmbiz_png/pR5ChhudOtyoUJMKxdnDzPg5FSouh3emzVaAYrXEfBbBQ3FfnfJTLdtCvKIz7Ao8je91au1CYvsBzmBxmmkmfQ/640?wx_fmt=png&tp=webp&wxfrom=5&wx_lazy=1&wx_co=1
var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
System.Console.WriteLine(now);
}
{
//九、阴历
//DateTime本身依赖于日历Calendar类。Calendar是一个抽象类，
//在System.Globalization命名空间下，也在System.Runtime.dll中。
//而在Calendar类下面，提供了很多不同类型的日历。跟我们有关系的，是中国的阴历ChineseLunisolarCalendar。
Calendar calendar = new ChineseLunisolarCalendar();
DateTime d = new DateTime(2020, 06, 29, calendar);
System.Console.WriteLine(d);//2020-7-19 00:00:00
							//经常看阴历的伙伴们会看出一点问题：今天是阴历5月24，为什么这儿写的是6月24呢？这个是因为今天闰4月，所以，阴历5月实际是这一个阴历年的第6个月。
							//那如何判断哪个月是否闰月呢？
bool is_leapYear = calendar.IsLeapYear(2020);
bool is_leapMonth = calendar.IsLeapMonth(2020, 5);
bool is_leapDay = calendar.IsLeapDay(2020, 5, 26);
//公历转阴历：
DateTime date = DateTime.Now;
int year = calendar.GetYear(date);
int month = calendar.GetMonth(date);
int day = calendar.GetDayOfMonth(date);
}
Console.ResetColor();
Console.BackgroundColor = ConsoleColor.Black;
        }



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

public static class DateTimeUtil
{
	//C#时间到JavaScript时间的转换
	public static long ToUnixTicks(this DateTime time)
	{
		return (long)TimeSpan.FromTicks(time.Ticks - DateTime.UnixEpoch.Ticks).TotalMilliseconds - TimeZoneInfo.Local.GetUtcOffset(time).Hours * 60 * 60 * 1000;
	}
}