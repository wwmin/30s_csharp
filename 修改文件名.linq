<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

static void Main(string[] args)
{
	Console.WriteLine("Hello World!");
	var currentDirectory = Directory.GetCurrentDirectory();
	var allFiles = Directory.GetFiles(currentDirectory).ToList();
	var allFilesWithSubFolder = Directory.GetFiles(currentDirectory, "*.*", SearchOption.AllDirectories);
	allFiles = allFiles.Where(p => Path.GetExtension(p) == ".mp4").ToList();
	foreach (var fileName in allFiles)
	{
		string tempName = Path.GetFileNameWithoutExtension(fileName);
		int startNameIndex = tempName.IndexOf("第");
		int lastNameIndex = tempName.LastIndexOf("集");
		if (startNameIndex > -1 && lastNameIndex > -1)
		{
			var numNameChars = tempName.ToArray()[(startNameIndex + 1)..lastNameIndex];
			Console.WriteLine(numNameChars);
			var numString = string.Join("", numNameChars);
			numString = numString.PadLeft(2, '0');
			numString = "第" + numString + "集";
			File.Move(fileName, Path.GetDirectoryName(fileName) + "\\" + numString + Path.GetExtension(fileName));
			Console.WriteLine(numString);
		}
	}
	Console.ReadKey();
}
// You can define other methods, fields, classes and namespaces here
