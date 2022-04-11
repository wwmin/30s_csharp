<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	//查看当前线程ID
	Environment.ProcessId.Dump();
	//查看当前线程路径
	Environment.ProcessPath.Dump();
	//获取所有进程
	void GetAllProcesses()
	{
	    //获取该系统下所有进程
	    Process[] processes = Process.GetProcesses();
	    //    foreach (var process in processes)
	    //    {
	    //        process.ProcessName.Dump();
	    //    }
	    string.Join<String>(",", processes.Select(p => p.ProcessName)).Dump();
	}
	//GetAllProcesses();
	
	//打开特定进程
	void StartNewProcess(string processName)
	{
	    Process p = new Process();//创建Process 类的对象
	    p.StartInfo.FileName = processName;
	    p.Start();//启动进程
	}
	//StartNewProcess("notepad");//此名称即可打开notepad进程,即打开记事本程序
	
	Thread.Sleep(100);
	//停止特定进程
	bool StopProcess(string processName)
	{
	    Process[] processes = Process.GetProcessesByName(processName);
	    if (processes.Length == 0) return false;
	    try
	    {
	        foreach (var p in processes)
	        {
	            //判断是否处于运行状态
	            if (!p.HasExited)
	            {
	                //关闭进程
	                p.Kill();
	                p.Close();
	                p.Dispose();
	            }
	        }
	        return true;
	    }
	    catch (Exception)
	    {
	
	        throw;
	    }
	}
	//StopProcess("notepad").Dump("关闭特定进程");//所有记事本进程都会被关闭
	
	//打开另一个cmd进程,然后控制其运行,并获取该控制台的输出内容
	void StartOtherCmdWindowAndReadInfo(string file)
	{
	    using (Process proc = new Process())
	    {
	        Stopwatch stopwatch = new Stopwatch();
	        stopwatch.Start();
	        Console.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ":" + file);
	        proc.StartInfo.WorkingDirectory = file;
	        proc.StartInfo.FileName = file + "command.bat";
	        //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//这里设置DOS窗口不显示，经实践可行
	        //proc.StartInfo.CreateNoWindow = true;//不显示程序窗口
	
	        proc.StartInfo.UseShellExecute = false;//是否使用操作系统shell启动
	        proc.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
	        proc.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
	        proc.StartInfo.RedirectStandardError = true;//重定向标准错误输出
	        proc.Start();//启动程序
	                     //proc.StandardInput.WriteLine("exit");//向cmd窗口写入命令
	        proc.StandardInput.AutoFlush = true;
	        proc.WaitForExit();
	        stopwatch.Stop();
	        Console.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ":" + file + ",共耗时:" + stopwatch.ElapsedMilliseconds / 1000d + "s");
	        var msg = proc.StandardOutput.ReadToEnd();
	        FileStream newFs = new FileStream(file + "Output/output.txt", FileMode.Append);
	        StreamWriter sw = new StreamWriter(newFs, Encoding.ASCII);
	        sw.Write(DateTime.Now);
	        sw.Write(msg);
	        sw.Flush();
	        sw.Close();
	        newFs.Close();
	        proc.Close();
	    };
	}
	
	PingServicecs.Test("192.168.9.161").Dump();//打印ping的输出信息
}

//调用ping.exe服务
public static class PingServicecs
{
    private const int TIME_OUT = 100;
    private const int PACKET_SIZE = 512;
    private const int TRY_TIMES = 2;
    //检查时间的正则
    private static Regex _reg = new Regex(@"时间=(.*?)ms", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    /// <summary>
    /// 结果集
    /// </summary>
    /// <param name="stringcommandLine">字符命令行</param>
    /// <param name="packagesize">丢包大小</param>
    /// <returns></returns>
    public static string LauchPing(string stringcommandLine, int packagesize)
    {
        Process process = new Process();
        process.StartInfo.Arguments = stringcommandLine;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.FileName = "ping.exe";
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.Start();
        return process.StandardOutput.ReadToEnd();//返回结果

    }
    /// <summary>
    /// 转换字节
    /// </summary>
    /// <param name="strBuffer">缓冲字符</param>
    /// <param name="packetSize">丢包大小</param>
    /// <returns></returns>
    private static float ParseResult(string strBuffer, int packetSize)
    {
        if (strBuffer.Length < 1)
            return 0.0F;
        MatchCollection mc = _reg.Matches(strBuffer);
        if (mc == null || mc.Count < 1 || mc[0].Groups == null)
            return 0.0F;
        if (!int.TryParse(mc[0].Groups[1].Value, out int avg))
            return 0.0F;
        if (avg <= 0)
            return 1024.0F;
        return (float)packetSize / avg * 1000 / 1024;
    }
    /// <summary>
    /// 通过Ip或网址检测调用Ping 返回 速度
    /// </summary>,
    /// <param name="strHost"></param>
    /// <returns></returns>
    public static string Test(string strHost, int trytimes, int PacketSize, int TimeOut)
    {
        return LauchPing(string.Format("{0} -n {1} -l {2} -w {3}", strHost, trytimes, PacketSize, TimeOut), PacketSize);
    }
    /// <summary>
    /// 地址
    /// </summary>
    /// <param name="strHost"></param>
    /// <returns></returns>
    public static string Test(string strHost)
    {
        return LauchPing(string.Format("{0} -n {1} -l {2} -w {3}", strHost, TRY_TIMES, PACKET_SIZE, TIME_OUT), PACKET_SIZE);
    }
}
