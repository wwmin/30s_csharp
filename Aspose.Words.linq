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

/// <summary>
/// AsposeWordHelper
/// </summary>
public static class AsposeWordHelper
{
	/// <summary>
	/// 文字替换帮助
	/// </summary>
	/// <param name="doc"></param>
	/// <param name="oldString">自动使用{}扩起来</param>
	/// <param name="newString"></param>
	public static void ReplaceHelper(this Document doc, string oldString, string newString)
	{
#pragma warning disable CS0618 // 类型或成员已过时
		doc.Range.Replace(R("{" + oldString + "}"), newString ?? string.Empty);
#pragma warning restore CS0618 // 类型或成员已过时
	}

	/// <summary>
	/// 创建正则表达式
	/// </summary>
	/// <param name="pattern"></param>
	/// <returns></returns>
	private static Regex R(string pattern)
	{
		return new Regex(pattern);
	}
	#region Table Methods
	/// <summary>
	/// 获取所有table
	/// </summary>
	/// <param name="doc"></param>
	/// <returns></returns>
	public static List<Table> GetAllTables(this Document doc)
	{
		NodeCollection tables = doc.GetChildNodes(NodeType.Table, true);
		return tables.Select(t => (Table)t).ToList();
	}

	/// <summary>
	/// 根据索引获取table
	/// </summary>
	/// <param name="doc"></param>
	/// <param name="index"></param>
	/// <returns></returns>
	public static Table GetTableByIndex(this Document doc, int index)
	{
		Table table = (Table)doc.GetChild(NodeType.Table, index, true);
		return table;
	}

	/// <summary>
	/// 根据表格title获取该table
	/// </summary>
	/// <param name="doc"></param>
	/// <param name="title"></param>
	/// <returns></returns>
	public static Table GetTableByTitle(this Document doc, string title)
	{
		return doc.GetAllTables().Where(p => p.Title == title).FirstOrDefault();
	}

	/// <summary>
	/// clone 表格最后一行
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	public static Row CloneLastRow(this Table table)
	{
		Row cloneRow = (Row)table.LastRow.Clone(true);
		return cloneRow;
	}

	/// <summary>
	/// 向表格最后添加一行
	/// </summary>
	/// <param name="table"></param>
	/// <param name="row"></param>
	/// <returns></returns>
	public static Row AppendRow(this Table table, Row row)
	{
		Row r = (Row)table.AppendChild(row);
		return r;
	}

	/// <summary>
	/// clone表格最后一行后添加到表格最后,返回添加的行
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	public static Row CloneLastRowAndAppend(this Table table)
	{
		Row r = (Row)table.AppendChild(table.CloneLastRow());
		return r;
	}
	/// <summary>
	/// remove表格的某一行row
	/// </summary>
	/// <param name="table"></param>
	/// <param name="index"></param>
	public static void RemoveRowByIndex(this Table table, int index)
	{
		table.Rows.RemoveAt(index);
	}

	/// <summary>
	/// 表格添加cell内容
	/// </summary>
	/// <param name="row"></param>
	/// <param name="col">当前行的第i列</param>
	/// <param name="text">替换文本</param>
	public static void AppendCellText(this Row row, int col, string text)
	{
		Cell cell = row.Cells[col];
		var rs = cell.FirstParagraph.Runs;

		if (rs.FirstOrDefault() is not Run rf)
		{
			rf = new Run(row.Document, text);
			rf.Text = text;
			cell.FirstParagraph.Runs.Add(rf);
		}
		else
		{
			rf.Text = text;
		}
	}
	#endregion
}