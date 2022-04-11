<Query Kind="Program" />

//char 操作
void Main()
{
	var s = "Wwmin  --== 12345";
	int n=s.Length;
	for (int i = 0; i < n; i++)
	{
		char c = s[i];
		if(char.IsLetterOrDigit(c)){
			char.ToLower(c).Dump();
		}
		
		if(char.IsNumber(c)){
			c.Dump("IsNumber");
		}
	
	}
	nameof(char.IsSeparator).Dump();
	nameof(char.IsAscii).Dump();
	nameof(char.IsControl).Dump();
}

