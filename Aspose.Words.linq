<Query Kind="Program">
  <NuGetReference>Aspose.Words</NuGetReference>
  <Namespace>Aspose.Words</Namespace>
  <Namespace>Aspose.Words.Drawing</Namespace>
  <Namespace>Aspose.Words.Tables</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
	//CreateSimpleDocx();
	//CreateDocByTemplate();
	//InsertNode();
	AsposeWordHelper.GetAncestorInBody(
}
public static string docPath = @"D:\temp\";
private void InsertNode()
{
	string fileName = "test.docx";
	Document doc = new Document(docPath + fileName);
	DocumentBuilder builder = new DocumentBuilder(doc);

	var device_invest_name = "test";
	Paragraph pg = (Paragraph)doc.GetChildNodes(NodeType.Paragraph, true).FirstOrDefault(p => p.Range.Text.Contains(device_invest_name));
	var font =  ((Run)pg.Runs.First()).Font;
	var runs = pg.Runs;
	builder.MoveTo(pg);
	for (int i = 0; i < 3; i++)
	{
		var p = builder.InsertParagraph();
		Run run = new Run(doc, "测试插入" + i);
		//run.Font.Color= Color.Blue;
		//run.Font.Style = style;
		run.Font.Bidi = font.Bidi;
		run.Font.Color=font.Color;
		run.Font.Bold = font.Bold;
		run.Font.BoldBi=font.BoldBi;
		run.Font.Italic = font.Italic;
		run.Font.ItalicBi= font.ItalicBi;
		run.Font.NoProofing = font.NoProofing;
		run.Font.Outline = font.Outline;
		run.Font.Scaling = font.Scaling;
		run.Font.Shadow = font.Shadow;
		run.Font.Size=font.Size;
		run.Font.SizeBi =font.SizeBi;
		run.Font.SmallCaps= font.SmallCaps;
		run.Font.Spacing = font.Spacing;
		run.Font.Style=font.Style;
		run.Font.TextEffect = font.TextEffect;
		p.AppendChild(run);
	}


	doc.Save(docPath + DateTime.Now.Ticks + fileName);
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

	string fileName = "test";
	string extention = ".docx";
	Document doc = new Document(docPath + fileName + extention);
	Aspose.Words.DocumentBuilder builder = new DocumentBuilder(doc);

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
	var cell1 = r1.Cells[1];
	cell1.FirstParagraph.Remove();
	Paragraph p = new Paragraph(doc);
	p.AppendChild(new Run(doc, "测试11"));
	cell1.AppendChild(p);
}

/// <summary>
/// AsposeWordHelper
/// </summary>
public static class AsposeWordHelper
{
	#region Text Methods
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
	#endregion
	#region Paragraph Methods
	/// <summary>
	/// 向节点后面插入段落
	/// </summary>
	/// <param name="documentBuilder"></param>
	/// <param name="refNode"></param>
	/// <param name="text"></param>
	public static void InsertParagraphAfter(this DocumentBuilder documentBuilder, Node refNode, string text)
	{
		var doc = documentBuilder.Document;
		if (documentBuilder.CurrentNode != refNode)
		{
			documentBuilder.MoveTo(refNode);
		}
		var p = documentBuilder.InsertParagraph();
		Run run = new Run(doc, text);
		if (refNode is Paragraph)
		{
			var font = ((Run)((Paragraph)refNode).Runs.FirstOrDefault())?.Font;
			if (font != null)
			{
				run.Font.Bidi = font.Bidi;
				run.Font.Color = font.Color;
				run.Font.Bold = font.Bold;
				run.Font.BoldBi = font.BoldBi;
				run.Font.Italic = font.Italic;
				run.Font.ItalicBi = font.ItalicBi;
				run.Font.NoProofing = font.NoProofing;
				run.Font.Outline = font.Outline;
				run.Font.Scaling = font.Scaling;
				run.Font.Shadow = font.Shadow;
				run.Font.Size = font.Size;
				run.Font.SizeBi = font.SizeBi;
				run.Font.SmallCaps = font.SmallCaps;
				run.Font.Spacing = font.Spacing;
				run.Font.Style = font.Style;
				run.Font.TextEffect = font.TextEffect;
			}
		}
		p.AppendChild(run);
	}
	#endregion
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
		//remove all content from the cloned row's cells. This makes the row ready for new content to be inserted into.
		foreach (Cell cell in cloneRow.Cells)
		{
			cell.RemoveAllChildren();
		}
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
	#region Picture Methods
	/// <summary>
	/// 在特定paragraph下添加图片
	/// </summary>
	/// <param name="paragraph">word段落</param>
	/// <param name="imagePath">图片全路径</param>
	/// <param name="width">图片宽</param>
	/// <param name="height">图片高</param>
	public static void AddImage(this Paragraph paragraph, string imagePath, int width = 100, int height = 100)
	{
		Document doc = (Document)paragraph.Document;
		Shape shape = new Shape(doc, ShapeType.Image);
		shape.ImageData.SetImage(imagePath);
		shape.Width = width;
		shape.Height = height;
		paragraph.AppendChild(shape);
	}
	/// <summary>
	/// 图片替换
	/// </summary>
	/// <param name="doc"></param>
	/// <param name="imageAlternativeText">图片中的替换文字,作为识别该图片用</param>
	/// <param name="imagePath">图片路径</param>
	/// <param name="documentBuilder"></param>
	/// <returns></returns>
	public static bool ReplaceImage(this Document doc, string imageAlternativeText, string imagePath, DocumentBuilder documentBuilder = null)
	{
		if (documentBuilder == null) documentBuilder = new DocumentBuilder(doc);
		var shapes = doc.GetChildNodes(NodeType.Shape, true).Select(s => (Shape)s).ToList();
		var imageShape = shapes.FirstOrDefault(s => s.AlternativeText == imageAlternativeText);
		if (imageShape == null) return false;
		documentBuilder.MoveTo(imageShape);
		documentBuilder.InsertImage(imagePath, imageShape.Width, imageShape.Height);
		imageShape.ParentNode.RemoveChild(imageShape);
		return true;
	}
	#endregion
	#region Node Methods
	public static Node GetAncestorInBody(Node startNode){
		while (startNode.ParentNode.NodeType!= NodeType.Body)
		{
			startNode = startNode.ParentNode;
		}
		return startNode;
	}
	#endregion
}