<Query Kind="Program" />

void Main()
{
	string appContextDir = AppContext.BaseDirectory;
	string domainDir = AppDomain.CurrentDomain.BaseDirectory;

	string assemblyDir = GetAssemblyDir(Assembly.GetExecutingAssembly());
	string codeBaseDir = GetCodeBaseDir(Assembly.GetExecutingAssembly(),"Users");

	var newPath = new Uri(new Uri(codeBaseDir), @"Data/").LocalPath;

	appContextDir.Dump();//C:\Users\wweim\AppData\Local\Temp\LINQPad6\_vqhdsvtk\shadow-1
	domainDir.Dump(); //C:\Users\wweim\AppData\Local\Temp\LINQPad6\_vqhdsvtk\shadow-1
	assemblyDir.Dump();//C:\Users\wweim\AppData\Local\Temp\LINQPad6\_vqhdsvtk\azglmr\
	codeBaseDir.Dump();//C:\
	newPath.Dump();//C:\Data\
}

private string GetAssemblyDir(Assembly assembly)
{
	Uri uri = new Uri(assembly.Location);
	return Path.GetDirectoryName(uri.LocalPath) + Path.DirectorySeparatorChar;
}

private string GetCodeBaseDir(Assembly assembly,string subString)
{
	Uri uri = new Uri(assembly.Location);
	string mainFolder = Path.GetDirectoryName(uri.LocalPath)?.Substring(0, uri.LocalPath.IndexOf(subString, StringComparison.Ordinal));
	return mainFolder;
}

// You can define other methods, fields, classes and namespaces here
