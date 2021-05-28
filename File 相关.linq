<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

//文件操作

//DriveInfo 查看计算机驱动器信息主要包括查看磁盘的空间、磁盘的文件格式、磁盘的卷标等
DriveInfo[] driveInfos = DriveInfo.GetDrives();
driveInfos.Dump("driveInfos");

DriveInfo driveInfo = new DriveInfo("D");
driveInfo.Dump("driveInfo");



//DirectoryInfo 文件夹操作
//创建文件夹
var tempFilePath = @"D:/temp" + DateTime.Today.ToFileTime();
DirectoryInfo directoryInfo = new DirectoryInfo(tempFilePath);
directoryInfo.Create();
directoryInfo.CreateSubdirectory("code-1");
directoryInfo.CreateSubdirectory("code-2");
//查看文件夹
IEnumerable<DirectoryInfo> dirs = directoryInfo.EnumerateDirectories();
dirs.Dump("IEnumerable<DirectoryInfo>");
//删除文件夹
//await Task.Delay(3000);
//await Task.Run(() => directoryInfo.Delete(true));//查看文件夹被创建,且3秒后被删除




//Directory 静态类与DirectoryInfo类似
//判断文件夹是否存在
if (Directory.Exists(tempFilePath))
{
	//Directory.Delete(tempFilePath);//存在则删除
}
//获取当前文件路径
Directory.GetCurrentDirectory().Dump("GetCurrentDirectory");
//遍历指定路径下所有文件夹
var allDirs = Directory.GetDirectories(tempFilePath, "*", SearchOption.AllDirectories);
allDirs.Dump("AllDires");
//遍历当前文件夹下所有文件
var allFiles = Directory.GetFiles(tempFilePath, "*.*", SearchOption.AllDirectories);
allFiles.Dump("AllFiles");
//文件夹和文件同时遍历
var allDireAndFileList = allDirs.Union(allFiles).OrderBy(d => d);
allDireAndFileList.Dump(nameof(allDireAndFileList));



//FileInfo 类 文件操作
Directory.CreateDirectory(tempFilePath);
FileInfo fileInfo = new FileInfo(tempFilePath + @"/test.txt");
if (!fileInfo.Exists)
{
	fileInfo.Create().Close();//创建文件
}
fileInfo.Attributes = FileAttributes.Normal;//设置文件属性
fileInfo.CopyTo(@"D:\test2021.1.1.txt", true);




//File 静态类, 文件操作, 功能类似与FileInfo
if (File.Exists(@"D:\test2021.1.1.txt"))
{
	File.Delete(@"D:\test2021.1.1.txt");
}
using (FileStream fs = File.Open(tempFilePath + "/test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
{   //创建文件
	using StreamReader sr = new StreamReader(fs, Encoding.UTF8);
	var txt = sr.ReadToEnd();
	txt.Dump("txt");
	string stringData = txt + $"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} \t\rHello World!";
	using StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
	sw.Write(stringData);
	sw.WriteLine();
}




//Path 静态类, 文件按路径操作
Console.WriteLine("请输入一个文件路径：");
string readPath = @"D:\temp";//string.Empty;
while (string.IsNullOrWhiteSpace(readPath))
{
	readPath = Console.ReadLine()!;
	if (readPath != null)
	{
		Console.WriteLine("不包含扩展名的文件名：" + Path.GetFileNameWithoutExtension(readPath));
		Console.WriteLine("文件扩展名：" + Path.GetExtension(readPath));
		Console.WriteLine("文件全名：" + Path.GetFileName(readPath));
		Console.WriteLine("文件路径：" + Path.GetDirectoryName(readPath));
	}
}

string.Join(",", Path.GetInvalidPathChars()).Dump(nameof(Path.GetInvalidPathChars));
string.Join(",", Path.GetInvalidFileNameChars()).Dump(nameof(Path.GetInvalidFileNameChars));



//Stream 在计算机编程中，流就是一个类的对象，很多文件的输入输出操作都以类的成员函数的方式来提供。

//StreamReader 类用于从流中读取字符串。它继承自 TextReader 类。
using (StreamReader sr = new StreamReader(tempFilePath + "/test.txt"))
{
	while (sr.Peek() != -1)
	{
		//读取文件中的一行字符
		string str = sr.ReadLine()!;
		Console.WriteLine(str);
	}
}

Path.GetRandomFileName().Dump();

//StreamWriter类主要用于向流中写入数据 (会清空所有文本)
//using (StreamWriter sw = new StreamWriter(tempFilePath + "/test.txt"))
//{

//sw.WriteLine("\r\n");
//sw.WriteLine("*****************");
//}

//FileStream 类主要用于文件的读写，不仅能读写普通的文本文件，还可以读取图像文件、声音文件等不同格式的文件。
/*
在创建 FileStream 类的实例时还会涉及多个枚举类型的值， 包括 FileAccess、FileMode、FileShare、FileOptions 等。

FileAccess 枚举类型主要用于设置文件的访问方式，具体的枚举值如下。
Read：以只读方式打开文件。
Write：以写方式打开文件。
ReadWrite：以读写方式打开文件。

FileMode 枚举类型主要用于设置文件打开或创建的方式，具体的枚举值如下。
CreateNew：创建新文件，如果文件已经存在，则会抛出异常。
Create：创建文件，如果文件不存在，则删除原来的文件，重新创建文件。
Open：打开已经存在的文件，如果文件不存在，则会抛出异常。
OpenOrCreate：打开已经存在的文件，如果文件不存在，则创建文件。
Truncate：打开已经存在的文件，并清除文件中的内容，保留文件的创建日期。如果文件不存在，则会抛出异常。
Append：打开文件，用于向文件中追加内容，如果文件不存在，则创建一个新文件。

FileShare 枚举类型主要用于设置多个对象同时访问同一个文件时的访问控制，具体的枚举值如下。
None：谢绝共享当前的文件。
Read：允许随后打开文件读取信息。
ReadWrite：允许随后打开文件读写信息。
Write：允许随后打开文件写入信息。
Delete：允许随后删除文件。
Inheritable：使文件句柄可由子进程继承。

FileOptions 枚举类型用于设置文件的高级选项，包括文件是否加密、访问后是否删除等，具体的枚举值如下。
WriteThrough：指示系统应通过任何中间缓存、直接写入磁盘。
None：指示在生成 System.IO.FileStream 对象时不应使用其他选项。
Encrypted：指示文件是加密的，只能通过用于加密的同一用户账户来解密。
DeleteOnClose：指示当不再使用某个文件时自动删除该文件。
SequentialScan：指示按从头到尾的顺序访问文件。
RandomAccess：指示随机访问文件。
Asynchronous：指示文件可用于异步读取和写入。
*/
using (FileStream fs = new FileStream(tempFilePath + "/test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
{
	string msg = "wwmin";
	//将字符串转换为字节数组
	byte[] bytes = Encoding.UTF8.GetBytes(msg);
	//设定文本的开始位置为文件的末尾, 等效于 FileStream(path,FileMode.Append,FileAccess.ReadWrite)
	fs.Position = fs.Length;
	//向文件中写入字节数组
	fs.Write(bytes, 0, bytes.Length);
	//刷新缓冲区
	fs.Flush();
	fs.Close();
}


//BinaryReader 读取二进制文件
using (FileStream fs = new FileStream(tempFilePath + "/test.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
{
	using BinaryReader br = new BinaryReader(fs, Encoding.UTF8, true);
	StringBuilder sb = new StringBuilder();
	//方式1: 使用Read()逐个字符读取文件内容
	int a = br.Read();
	while (a != -1)
	{
		sb.Append((char)a);
		a = br.Read();
	}
	sb.Dump("BinaryReader");

	fs.Position = 0;//将读取游标重置为0, 防止读取位置不正确(在第二次读取的时候要注意)

	//方式2: 使用Read(bytes,0,length)一次性读完数据

	long length = fs.Length;
	byte[] bytes = new byte[length];
	br.Read(bytes, 0, bytes.Length);
	string str = Encoding.Default.GetString(bytes);//将字节数组转为字符串
	str.Dump("Read(bytes,0,length)");
}




//BinaryWriter 写入二进制文件
using (FileStream fs = new FileStream(tempFilePath + "/test.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
{
	using BinaryWriter bw = new BinaryWriter(fs);
	fs.Position = fs.Length;
	bw.Write("这是BinaryWriter写入的数据");
	fs.Position = 0;
	//读取内容 直接使用fs读取
	long length = fs.Length;
	byte[] bytes = new byte[fs.Length];
	fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
	string str = Encoding.Default.GetString(bytes);//将字节数组转为字符串
	str.Dump("Read Write");
}

/****************************************文件帮助类*****************************************/
{
	var dirName = @"D:/data/db/";
	if (!File.Exists(dirName + "Eletcric_2_20210116.bak")) return;
	using (FileStream fs = new FileStream(dirName + "Eletcric_2_20210116.bak", FileMode.Open, FileAccess.Read))
	{
		fs.CopyToFile(dirName + "1.bak");
		fs.GetFileMD5().Dump(nameof(FileUtil.GetFileMD5)); ;
	}
}


//文件帮助类
public static class FileUtil
{
	/// <summary>
	/// 以文件流的形式复制大文件
	/// </summary>
	/// <param name="fs">源</param>
	/// <param name="dest">目标地址</param>
	/// <param name="bufferSize">缓冲区大小，默认8MB</param>
	public static void CopyToFile(this Stream fs, string dest, int bufferSize = 1024 * 8 * 1024)
	{
		using var fsWriter = new FileStream(dest, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		byte[] buf = new byte[bufferSize];
		int len;
		while ((len = fs.Read(buf, 0, buf.Length)) != 0)
		{
			fsWriter.Write(buf, 0, len);
		}
	}

	/// <summary>
	/// 以文件流的形式复制大文件(异步方式)
	/// </summary>
	/// <param name="fs">源</param>
	/// <param name="dest">目标地址</param>
	/// <param name="bufferSize">缓冲区大小，默认8MB</param>
	public static async void CopyToFileAsync(this Stream fs, string dest, int bufferSize = 1024 * 8 * 1024)
	{
		using var fsWriter = new FileStream(dest, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		byte[] buf = new byte[bufferSize];
		int len;
		await Task.Run(() =>
		{
			while ((len = fs.Read(buf, 0, buf.Length)) != 0)
			{
				fsWriter.Write(buf, 0, len);
			}
		}).ConfigureAwait(true);
	}


	/// <summary>
	/// 将内存流转储成文件
	/// </summary>
	/// <param name="ms"></param>
	/// <param name="filename"></param>
	public static void SaveFile(this MemoryStream ms, string fileName)
	{
		using var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		byte[] buffer = ms.ToArray();//转化为byte格式存储
		fs.Write(buffer, 0, buffer.Length);
		fs.Flush();
	}

	/// <summary>
	/// 计算文件的 MD5 值
	/// </summary>
	/// <param name="fs">源文件流</param>
	/// <returns>MD5 值16进制字符串</returns>
	public static string GetFileMD5(this FileStream fs) => HashFile(fs, "md5");
	/// <summary>
	/// 计算文件的 sha1 值
	/// </summary>
	/// <param name="fs">源文件流</param>
	/// <returns>sha1 值16进制字符串</returns>
	public static string GetFileSha1(this Stream fs) => HashFile(fs, "sha1");

	/// <summary>
	/// 计算文件的哈希值
	/// </summary>
	/// <param name="fs">被操作的源数据流</param>
	/// <param name="algo">加密算法</param>
	/// <returns>哈希值16进制字符串</returns>
	private static string HashFile(Stream fs, string algo)
	{
		HashAlgorithm crypto = algo switch
		{
			"sha1" => new SHA1CryptoServiceProvider(),
			_ => new MD5CryptoServiceProvider(),
		};
		byte[] retVal = crypto.ComputeHash(fs);

		StringBuilder sb = new StringBuilder();
		foreach (var t in retVal)
		{
			sb.Append(t.ToString("x2"));
		}
		return sb.ToString();
	}
}
