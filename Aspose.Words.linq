<Query Kind="Program">
  <NuGetReference>Aspose.Words</NuGetReference>
  <Namespace>Aspose.Words</Namespace>
  <Namespace>Aspose.Words.Drawing</Namespace>
  <Namespace>Aspose.Words.Tables</Namespace>
</Query>

void Main()
{
	//CreateSimpleDocx();
	CreateDocByTemplate();
}
public static string docPath = @"D:\temp\";
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

	string fileName = "综合能源规划设计报告模板";
	string extention = ".docx";
	Document doc = new Document(docPath + fileName + extention);
	Aspose.Words.DocumentBuilder builder = new DocumentBuilder(doc);
	//Console.WriteLine(builder);
	doc.Range.Replace(new Regex("{name}"), "wwmin");

	doc.Range.Replace(new Regex("{dateTime}"), DateTime.Today.ToShortDateString());
	//builder.MoveToDocumentEnd();
	//builder.Writeln("Hello World");

	//replaceImage(doc, builder);
	replaceTable(doc);
	doc.Save(docPath + fileName + DateTime.Now.Ticks + extention);
	"done".Dump();
}

private void addImg(Document doc)
{
	Shape shape = new Shape(doc, ShapeType.Image);
	shape.ImageData.SetImage(docPath + @"img\map.png");
	shape.Width = 100;
	shape.Height = 100;
	doc.FirstSection.Body.FirstParagraph.AppendChild(shape);
}

//查找图片并替换
private void replaceImage(Document doc, DocumentBuilder builder)
{
	var shapes = doc.GetChildNodes(NodeType.Shape, true).Select(s => (Shape)s).ToList();
	shapes.Select(s => ((Shape)s).AlternativeText).Dump();
	//在word模板中给图片添加替换文字的方式使其具有特定名称,可查找
	var mapImg = shapes.Find(p => p.AlternativeText == "map");
	builder.MoveTo(mapImg);
	builder.InsertImage(docPath + @"img\map.png", mapImg.Width, mapImg.Height);
	mapImg.ParentNode.RemoveChild(mapImg);
	//shapes.Dump();
}

//给定的table插入字段
private void replaceTable(Document doc)
{
	var tables = doc.GetChildNodes(NodeType.Table, true).ToList();
	var table1 = tables[1] as Table;
	var r1 = table1.Rows[1];
	var cell1=r1.Cells[1];
	cell1.FirstParagraph.Remove();
	Paragraph p = new Paragraph(doc);
	p.AppendChild(new Run(doc,"测试11"));
	cell1.AppendChild(p);
}