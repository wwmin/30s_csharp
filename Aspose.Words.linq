<Query Kind="Program">
  <NuGetReference>Aspose.Words</NuGetReference>
  <Namespace>Aspose.Words</Namespace>
</Query>

void Main()
{
	//CreateSimpleDocx();
	CreateDocByTemplate();
}

//创建简单的word
private void CreateSimpleDocx()
{
	Document doc = new Document();
	DocumentBuilder builder = new DocumentBuilder(doc);
	builder.MoveToDocumentEnd();
	builder.Font.LocaleId = 1031;

	builder.Writeln("hello world");
	var output = doc.Save(@"D:\temp\test.docx");
	output.Dump();
}

//从模板中创建word
private void CreateDocByTemplate()
{
	string docPath = @"D:\temp\";
	string fileName = "模板";
	string extention = ".docx";
	Document doc = new Document(docPath + fileName + extention);
	Aspose.Words.DocumentBuilder builder = new DocumentBuilder(doc);
	doc.Range.Replace(@"{name}","wwmin",false,true);

	doc.Range.Replace(@"{dateTime}",DateTime.Today.ToShortDateString(),false,true);
	builder.MoveToDocumentEnd();
	builder.Writeln("Hello World");
	//doc.Document.Dump();
	doc.Save(docPath + fileName+DateTime.Now.Ticks + extention);
}