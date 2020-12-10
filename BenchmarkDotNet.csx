#r "nuget:BenchmarkDotNet.Annotations/0.12.1"
#r "nuget:BenchmarkDotNet/0.12.1"
using System;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
public class Md5VsSha256
{
    private const int N = 10000;
    private readonly byte[] data;

    private readonly SHA256 sha256 = SHA256.Create();
    private readonly MD5 md5 = MD5.Create();

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

public static void Main(string[] args)
{
    var summary = BenchmarkRunner.Run(typeof().Assembly);
}