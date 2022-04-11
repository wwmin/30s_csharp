<Query Kind="Program" />

void Main()
{
	getLocalIp().Dump();
	getRemoteIp().Dump();;
}
/// <summary>获取外网ip地址</summary>
public string getRemoteIp()
{
	return System.Net.NetworkInformation.NetworkInterface.
		GetAllNetworkInterfaces().Select(p => p.GetIPProperties())
		.SelectMany(p => p.UnicastAddresses)
		.Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
		!System.Net.IPAddress.IsLoopback(p.Address)).FirstOrDefault()?.Address.ToString();
}

/// <summary>获取本地ip地址 如:192.168.1.22</summary>
public string getLocalIp()
{
	return System.Net.NetworkInformation.NetworkInterface.
		GetAllNetworkInterfaces().Select(p => p.GetIPProperties())
		.SelectMany(p => p.UnicastAddresses)
		.Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
		p.IsDnsEligible == true && 
		p.PrefixOrigin == System.Net.NetworkInformation.PrefixOrigin.Dhcp)
		.FirstOrDefault()?.Address.ToString();
}

//查看本地已开放端口
//使用cmd
//>netstat -na
