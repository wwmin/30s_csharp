<Query Kind="Statements">
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http.Json</Namespace>
</Query>

//使用Parallel 多线程执行
var userHandlers = new[]
{
	"users/okyrylchuk",
	"users/jaredpar",
	"users/davidfowl"
};

using HttpClient client = new()
				 {
					 BaseAddress = new Uri("https://api.github.com"),
				 };
client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("DotNet", "6"));

ParallelOptions options = new()
{
	MaxDegreeOfParallelism = 3
};
await Parallel.ForEachAsync(userHandlers, options, async (uri, token) =>
{
	//如果参数为null，则报错
	ArgumentNullException.ThrowIfNull(uri);
	var user = await client.GetFromJsonAsync<GitHubUser>(uri, token);
	Console.WriteLine($"Name: {user.Name}\nBio: {user.Bio}\n");
});

public class GitHubUser
{
	public string Name { get; set; }
	public string Bio { get; set; }
}

// Output:
// Name: David Fowler
// Bio: Partner Software Architect at Microsoft on the ASP.NET team, Creator of SignalR
// 
// Name: Oleg Kyrylchuk
// Bio: Software developer | Dotnet | C# | Azure
// 
// Name: Jared Parsons
// Bio: Developer on the C# compiler