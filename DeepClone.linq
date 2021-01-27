<Query Kind="Program">
  <Namespace>System.Runtime.Serialization.Formatters.Binary</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	var s = new Student() { Id = 1, Name = "wwmin" };
	//var sc = SerializeHelper.Serializable(s);
	//var b = Convert.FromBase64String(sc);
	//System.Text.Encoding.Default.GetString(b).Dump();
	var sc=SerializeHelper.DeepClone(s).Dump();
	Object.ReferenceEquals(s,sc).Dump();
	
	var sc2 = s.Clone();
	Object.ReferenceEquals(s, sc2).Dump();
}

public class SerializeHelper
{
	public static string Serializable(object target)
	{
		using MemoryStream stream = new MemoryStream();
		new BinaryFormatter().Serialize(stream, target);
		return Convert.ToBase64String(stream.ToArray());
	}

	public static T Derializable<T>(string target)
	{
		byte[] targetArray = Convert.FromBase64String(target);
		using MemoryStream stream = new MemoryStream(targetArray);
		return (T)new BinaryFormatter().Deserialize(stream);
	}

	public static T DeepClone<T>(T t)
	{
		if (t == null) return default!;
		return Derializable<T>(Serializable(t));
	}
}
[Serializable]
class Student : ICloneable
{
	public int Id { get; set; }
	public string? Name { get; set; }

	public object Clone()
	{
		return new Student() { Id = this.Id, Name = this.Name };
	}
}
