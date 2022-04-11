<Query Kind="Program">
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Windows</Namespace>
</Query>

void Main()
{
	//读取剪切板text内容
	var str = Clipboard.GetText(TextDataFormat.Text);
	var obj = Clipboard.GetDataObject();
	str.Dump();
	var i = str.IndexOf("{");
	str = i > 0 ? str[..i] : str;
	str = str.ToLower().Trim().Replace(" ", "_");
	str.Dump();
	//处理完数据之后将数据copy到剪切板
	Clipboard.SetText(str);
}
