<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

//#r "nuget:BenchmarkDotNet.Annotations/0.12.1"
//#r "nuget:BenchmarkDotNet/0.12.1"
//using System;
#LINQPad optimize+     // Enable compiler optimizations
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

void Main()
{
	Util.AutoScrollResults = true;
	//var summary = BenchmarkRunner.Run(typeof().Assembly);
	var summary = BenchmarkRunner.Run(typeof(Test));
	//var summary = BenchmarkRunner.Run<Md5VsSha256>();
	var testListStructure = BenchmarkRunner.Run<ListCtor>();
}

public class HashAndMD5
{
    public string GetMD5(string input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        using (var md5 = MD5.Create())
        {
            var buffer = Encoding.UTF8.GetBytes(input);
            var hashResult = md5.ComputeHash(buffer);
            return BitConverter.ToString(hashResult).Replace("-", string.Empty);
        }
    }

    public string GetSHA1(string input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        using (var sha1 = SHA1.Create())
        {
            var buffer = Encoding.UTF8.GetBytes(input);
            var hashResult = sha1.ComputeHash(buffer);
            return BitConverter.ToString(hashResult).Replace("-", string.Empty);
        }
    }

}

[ShortRunJob]
//[SimpleJob(RuntimeMoniker.CoreRt50)]
//[MaxColumn]
//[MinColumn]
//[MemoryDiagnoser]
public class Test
{
    [Params("https://www.baidu.com/img/bd_logo1.png")]
    public string Content { get; set; } = string.Empty;

    public void TestMD5()
    {
        HashAndMD5 hashAndMD5 = new HashAndMD5();
        hashAndMD5.GetMD5(Content);
    }

    public void TestSHA1()
    {
        HashAndMD5 hashAndMD5 = new HashAndMD5();
        hashAndMD5.GetSHA1(Content);
    }
}

//[SimpleJob(RuntimeMoniker.CoreRt50)]
[MaxColumn]
[MinColumn]
[MemoryDiagnoser]
public class Md5VsSha256
{
    public const int N = 10000;
    public readonly byte[] data;

    public readonly SHA256 sha256 = SHA256.Create();
    public readonly MD5 md5 = MD5.Create();

    public Md5VsSha256()
    {
        data = new byte[N];
        new Random(42).NextBytes(data);
    }

    [Benchmark]
    public byte[] Sha256() => sha256.ComputeHash(data);

    [Benchmark]
    public byte[] Md5() => md5.ComputeHash(data);
}

[MaxColumn]
[MinColumn]
[MemoryDiagnoser]
public class ListCtor{

	[Params(10000)]
	public int initNum = default;
	public void TestListWithNoNum()
	{
		new List<int>() {};
	}

	public void TestListWithNum()
	{
		new List<int>(initNum) {};
	}
}