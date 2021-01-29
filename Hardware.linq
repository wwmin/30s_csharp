<Query Kind="Statements">
  <Output>DataGrids</Output>
  <NuGetReference>System.Management</NuGetReference>
  <Namespace>System.Management</Namespace>
</Query>

//硬件相关

//CPU个数
var cpuInfo = SystemInfo.GetCpuInfo();
cpuInfo.Dump(nameof(SystemInfo.GetCpuInfo));


SystemInfo.QueryEnvironment("%Path%").Dump();
Environment.UserDomainName.Dump(nameof(Environment.UserDomainName));

$"CommandLine: {Environment.CommandLine}".Dump(nameof(Environment.CommandLine));

//String[] arguments = Environment.GetCommandLineArgs();
//Console.WriteLine("GetCommandLineArgs: {0}", String.Join(", ", arguments));

//  <-- Keep this information secure! -->
$"CurrentDirectory: {Environment.CurrentDirectory}".Dump(nameof(Environment.CurrentDirectory));

$"ExitCode: { Environment.ExitCode}".Dump(nameof(Environment.ExitCode));

$"HasShutdownStarted: {Environment.HasShutdownStarted}".Dump(nameof(Environment.HasShutdownStarted));

//  <-- Keep this information secure! -->
$"MachineName: {Environment.MachineName}".Dump(nameof(Environment.MachineName));

$"NewLine: {Environment.NewLine}  first line{Environment.NewLine}  second line{Environment.NewLine}  third line".Dump(nameof(Environment.NewLine));

$"OSVersion: {Environment.OSVersion.ToString()}".Dump(nameof(Environment.OSVersion));

$"StackTrace: '{Environment.StackTrace}'".Dump(nameof(Environment.StackTrace));

//  <-- Keep this information secure! -->
$"SystemDirectory: {Environment.SystemDirectory}".Dump(nameof(Environment.SystemDirectory));

$"TickCount: {Environment.TickCount}".Dump(nameof(Environment.TickCount));

//  <-- Keep this information secure! -->
$"UserDomainName: {Environment.UserDomainName}".Dump(nameof(Environment.UserDomainName));

$"UserInteractive: {Environment.UserInteractive}".Dump(nameof(Environment.UserInteractive));

//  <-- Keep this information secure! -->
$"UserName: {Environment.UserName}".Dump(nameof(Environment.UserName));

$"Version: {Environment.Version.ToString()}".Dump(nameof(Environment.Version));

$"WorkingSet: {Environment.WorkingSet}".Dump(nameof(Environment.WorkingSet));

//  No example for Exit(exitCode) because doing so would terminate this example.
String nl = Environment.NewLine;
//  <-- Keep this information secure! -->
String query = "My system drive is %SystemDrive% and my system root is %SystemRoot%";
string str = Environment.ExpandEnvironmentVariables(query);
$"ExpandEnvironmentVariables: {nl}  {str}".Dump(nameof(Environment.ExpandEnvironmentVariables) + "SystemDrive");

$"GetEnvironmentVariable: {nl}  My temporary directory is {Environment.GetEnvironmentVariable("TEMP")}.".Dump(nameof(Environment.GetEnvironmentVariable) + "TEMP");


IDictionary environmentVariables = Environment.GetEnvironmentVariables();
environmentVariables.Dump(nameof(Environment.GetEnvironmentVariables));
//(environmentVariables as Dictionary<string,string>)!.Select(p=>(p.Key??"")+":"+(p.Value??"")).Dump(nameof(Environment.GetEnvironmentVariables));
foreach (DictionaryEntry de in environmentVariables)
{
	//Console.WriteLine("  {0} = {1}", de.Key, de.Value);
}

$"GetFolderPath: {Environment.GetFolderPath(Environment.SpecialFolder.System)}".Dump(nameof(Environment.SpecialFolder.System));

String[] drives = Environment.GetLogicalDrives();
$"GetLogicalDrives: {String.Join(", ", drives)}".Dump(nameof(Environment.GetLogicalDrives));

//系统信息
public static class SystemInfo
{
	private static List<ManagementBaseObject>? _cpuObjects;
	private static readonly PerformanceCounter PcCpuLoad; //CPU计数器 
	/// <summary>
	/// 获取CPU核心数 
	/// </summary>
	public static int ProcessorCount { get; }
	static SystemInfo()
	{
		//初始化CPU计数器 
		PcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total")
		{
			MachineName = "."
		};
		PcCpuLoad.NextValue();

		//CPU个数 
		ProcessorCount = Environment.ProcessorCount;
	}
	/// <summary>
	/// 获取环境变量
	/// </summary>
	/// <param name="type">环境变量名</param>
	/// <returns></returns>
	public static string QueryEnvironment(string type) => Environment.ExpandEnvironmentVariables(type);

	/// <summary>
	/// 获取CPU占用率 %
	/// </summary>
	public static float CpuLoad => PcCpuLoad.NextValue();


	/// <summary>
	/// 获取CPU信息
	/// </summary>
	/// <returns>CPU信息</returns>
	public static List<CpuInfo> GetCpuInfo()
	{
		try
		{
			using var mos = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
			using var moc = mos.Get();
			_cpuObjects ??= moc.AsParallel().Cast<ManagementBaseObject>().ToList();
			float temperature = GetCPUTemperature(out string ex);
			if (!string.IsNullOrEmpty(ex)) ex.Dump(nameof(GetCPUTemperature));
			return _cpuObjects.Select(mo => new CpuInfo
			{
				CpuLoad = CpuLoad,
				NumberOfLogicalProcessors = ProcessorCount,
				CurrentClockSpeed = mo.Properties["CurrentClockSpeed"].Value.ToString(),
				Manufacturer = mo.Properties["Manufacturer"].Value.ToString(),
				MaxClockSpeed = mo.Properties["MaxClockSpeed"].Value.ToString(),
				Type = mo.Properties["Name"].Value.ToString(),
				DataWidth = mo.Properties["DataWidth"].Value.ToString(),
				DeviceID = mo.Properties["DeviceID"].Value.ToString(),
				NumberOfCores = Convert.ToInt32(mo.Properties["NumberOfCores"].Value),
				Temperature = temperature
			}).ToList();

		}
		catch (Exception)
		{
			return new List<CpuInfo>();
		}
	}

	/// <summary>
	/// 获取CPU温度
	/// </summary>
	/// <returns>CPU温度</returns>
	public static float GetCPUTemperature(out string ex)
	{
		ex = string.Empty;
		try
		{
			string str = "";
			using var mos = new ManagementObjectSearcher(@"root\WMI", "select * from MSAcpi_ThermalZoneTemperature");
			var moc = mos.Get();
			foreach (var mo in moc)
			{
				str += mo.Properties["CurrentTemperature"].Value.ToString();
			}

			//这就是CPU的温度了
			float temp = (float.Parse(str) - 2732) / 10;
			return (float)Math.Round(temp, 2);
		}
		catch (Exception e)
		{
			ex = e.Message;
			return 0;
		}
	}
}

#nullable disable
/// <summary>
/// CPU模型
/// </summary>
public class CpuInfo
{
	/// <summary>
	/// 设备ID
	/// </summary>
	public string DeviceID { get; set; }

	/// <summary>
	/// CPU型号 
	/// </summary>
	public string Type { get; set; }

	/// <summary>
	/// CPU厂商
	/// </summary>
	public string Manufacturer { get; set; }

	/// <summary>
	/// CPU最大睿频
	/// </summary>
	public string MaxClockSpeed { get; set; }

	/// <summary>
	/// CPU的时钟频率
	/// </summary>
	public string CurrentClockSpeed { get; set; }

	/// <summary>
	/// CPU核心数
	/// </summary>
	public int NumberOfCores { get; set; }

	/// <summary>
	/// 逻辑处理器核心数
	/// </summary>
	public int NumberOfLogicalProcessors { get; set; }

	/// <summary>
	/// CPU使用率
	/// </summary>
	public double CpuLoad { get; set; }

	/// <summary>
	/// CPU位宽
	/// </summary>
	public string DataWidth { get; set; }

	/// <summary>
	/// 核心温度
	/// </summary>
	public double Temperature { get; set; }
}
#nullable enable


