<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

//加密相关
void Main()
{
	//crypto
	TestRandomNumberGenerator();
}

private void TestRandomNumberGenerator(){
	/*
	您可以从密码安全伪随机数生成器 (CSPNG) 轻松生成随机值序列。
	它对于以下场景中很有用：
	密钥生成
	随机数
	某些签名方案中的盐
	*/
	// Fills an array of 300 bytes with a cryptographically strong random sequence of values.
	// GetBytes(byte[] data);
	// GetBytes(byte[] data, int offset, int count)
	// GetBytes(int count)
	// GetBytes(Span<byte> data)
	byte[] bytes = RandomNumberGenerator.GetBytes(300);
	Console.WriteLine(bytes.Length);
	Console.WriteLine(Convert.ToBase64String(bytes));//生成字母字符串
	//Console.WriteLine(Encoding.UTF8.GetString(bytes));//此方法转string 出现乱码
	//Console.WriteLine(BitConverter.ToString(bytes));//生成短线-相隔的两位字符拼接串
}