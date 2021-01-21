<Query Kind="Program">
  <Connection>
    <ID>54bf9502-9daf-4093-88e8-7177c129999f</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Provider>System.Data.SqlServerCe.4.0</Provider>
    <AttachFileName>&lt;ApplicationData&gt;\LINQPad\DemoDB.sdf</AttachFileName>
    <Persist>true</Persist>
  </Connection>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

#region Base64相关


void Main()
{
	"在输入框中输入要加解密的字符串".Dump();
	var s = Console.ReadLine();
	var d = Utils.EncodeBase64(s ?? "").Dump("EncodeBase64");
	Utils.DecodeBase64(d).Dump("DecodeBase64");
}

public static class Utils
{
	/// <summary>
	/// Base64加密
	/// </summary>
	/// <param name="codeName">加密采用的编码方式</param>
	/// <param name="source">待加密的明文</param>
	/// <returns></returns>
	public static string EncodeBase64(Encoding encodeType, string source)
	{
		string encode = string.Empty;
		byte[] bytes = encodeType.GetBytes(source);
		try
		{
			encode = Convert.ToBase64String(bytes);
		}
		catch
		{
			encode = source;
		}
		return encode;
	}

	/// <summary>
	/// Base64加密，采用utf8编码方式加密
	/// </summary>
	/// <param name="source">待加密的明文</param>
	/// <returns>加密后的字符串</returns>
	public static string EncodeBase64(this string source)
	{
		return EncodeBase64(Encoding.UTF8, source);
	}

	/// <summary>
	/// Base64解密
	/// </summary>
	/// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
	/// <param name="result">待解密的密文</param>
	/// <returns>解密后的字符串</returns>
	public static string DecodeBase64(Encoding encode, string result)
	{
		string decode = "";
		byte[] bytes = Convert.FromBase64String(result);
		try
		{
			decode = encode.GetString(bytes);
		}
		catch
		{
			decode = result;
		}
		return decode;
	}

	/// <summary>
	/// Base64解密，采用utf8编码方式解密
	/// </summary>
	/// <param name="result">待解密的密文</param>
	/// <returns>解密后的字符串</returns>
	public static string DecodeBase64(this string result)
	{
		return DecodeBase64(Encoding.UTF8, result);
	}
	#endregion
}



//javascript 对应加解密

/////解码
//function b64DecodeUnicode(str)
//{
//    return decodeURIComponent(atob(str).split('').map(function(c) {
//        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
//    }).join(''));
//}
/////编码
//function b64EncodeUnicode(str)
//{
//    return btoa(encodeURIComponent(str).replace(/% ([0 - 9A - F]{ 2})/ g, function(match, p1) {
//        return String.fromCharCode('0x' + p1);
//    }));
//}
