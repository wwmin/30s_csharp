<Query Kind="Program">
  <Namespace>System.Text.Json</Namespace>
</Query>

void Main()
{
	List<double> data = new List<double> {1.0,2.0,3.0,4.0,5.0,6.0,7.0};
	byte[] bd = ConvertDoubleListToBytes(data);
 	List<double> ld=ConvertBytesToDoubleList(bd);
	ld.Dump();
}

//将List<double> 转为 byte[]
static byte[] ConvertDoubleListToBytes(List<double> matrix)
{
	if (matrix == null) return new byte[0];
	using MemoryStream stream = new MemoryStream();
	BinaryWriter bw = new BinaryWriter(stream);
	foreach (var d in matrix)
	{
		bw.Write(d);
	}
	return stream.ToArray();
}
//将byte[] 转为 List<double>
static List<double> ConvertBytesToDoubleList(byte[] matrix){
	if(matrix == null) return null;
	List<double> result = new List<double>();
	using var br = new BinaryReader(new MemoryStream(matrix));
	var ptCount = matrix.Length/8;
	for (int i = 0; i < ptCount; i++)
	{
		result.Add(br.ReadDouble());
	}
	return result;
}




