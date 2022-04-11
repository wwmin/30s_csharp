<Query Kind="Statements" />

//var basePath = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
//var basePath = Environment.CurrentDirectory;
//var basePath = AppDomain.CurrentDomain.BaseDirectory;
//var basePath = Process.GetCurrentProcess()?.MainModule?.FileName;
var basePath = $@"D:/";
Console.WriteLine(basePath);
FileUtil.WatchFiles(basePath, "*.txt");
Console.Read();

internal static class FileUtil
{
	private static FileSystemWatcher? s_watcher;


	/// <summary>
	/// file change watcher
	/// </summary>
	/// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation</param>
	/// <param name="filter">The type of files to watch. For example, "*.txt" watches for changes to all text files.</param>
	public static void WatchFiles(string path, string filter)
	{
		if (!Directory.Exists(path))
		{
			throw new FileNotFoundException();
		}
		s_watcher = new FileSystemWatcher(path, filter)
		{
			IncludeSubdirectories = true
		};
		s_watcher.Created += OnFileChanged;
		s_watcher.Changed += OnFileChanged;
		s_watcher.Deleted += OnFileChanged;
		s_watcher.Renamed += OnFileRenamed;
		s_watcher.EnableRaisingEvents = true;
		Console.WriteLine("watching file changes ...");
	}

	public static void OnFileChanged(object sender, FileSystemEventArgs e)
	{
		Console.WriteLine($"file {e.Name} {e.ChangeType}");
	}

	public static void OnFileRenamed(object sender, RenamedEventArgs e)
	{
		Console.WriteLine($"file {e.OldName} {e.ChangeType} {e.Name}");
	}
}
