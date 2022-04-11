<Query Kind="Program">
  <NuGetReference>Aspose.Words</NuGetReference>
  <Namespace>Aspose.Words</Namespace>
  <Namespace>Aspose.Words.Drawing</Namespace>
  <Namespace>Aspose.Words.Tables</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Text.Json</Namespace>
</Query>

void Main()
{
	//TestOldTableHorizontalMerge();
	//TestOldTableVerticalMerge();
	//TestOldTableRangeMerge();
	//TestInsertTable();
	TestOperateTable();
}
public static string docPath = @"D:\temp\";
private void InsertNode()
{
	string fileName = "test.docx";
	Document doc = new Document(docPath + fileName);
	DocumentBuilder builder = new DocumentBuilder(doc);

	var device_invest_name = "test";
	Paragraph pg = (Paragraph)doc.GetChildNodes(NodeType.Paragraph, true).FirstOrDefault(p => p.Range.Text.Contains(device_invest_name));
	var font = ((Run)pg.Runs.First()).Font;
	var runs = pg.Runs;
	builder.MoveTo(pg);
	for (int i = 0; i < 3; i++)
	{
		var p = builder.InsertParagraph();
		Run run = new Run(doc, "测试插入" + i);
		//run.Font.Color= Color.Blue;
		//run.Font.Style = style;
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

//表格合并 - 新创建
private void TestNewTableMerge()
{
	string fileName = "test.docx";

	Document doc = new Document();
	DocumentBuilder builder = new DocumentBuilder(doc);

	builder.InsertCell();
	builder.CellFormat.VerticalMerge = CellMerge.First;
	builder.Write("Text in merged cells");

	builder.InsertCell();
	builder.CellFormat.VerticalMerge = CellMerge.None;
	builder.Write("Text in one cell");
	builder.EndRow();

	builder.InsertCell();
	builder.CellFormat.VerticalMerge = CellMerge.Previous;

	builder.InsertCell();
	builder.CellFormat.VerticalMerge = CellMerge.None;
	builder.Write("Text in another cell");
	builder.EndRow();

	builder.InsertCell();
	builder.CellFormat.VerticalMerge = CellMerge.Previous;

	builder.InsertCell();
	builder.CellFormat.VerticalMerge = CellMerge.None;
	builder.Write("Text in another 2 cell");
	builder.EndRow();

	builder.EndTable();

	doc.Save(docPath + DateTime.Now.Ticks + fileName);
}

//表格水平合并
private void TestOldTableHorizontalMerge()
{
	string fileName = "test.docx";
	Document doc = new Document(docPath + fileName);
	var table = doc.GetTableByIndex(0);
	table.HorizontalMerge(1, 0, 1);
	doc.Save(docPath + DateTime.Now.Ticks + fileName);
}

//表格垂直合并
private void TestOldTableVerticalMerge()
{
	string fileName = "test.docx";
	Document doc = new Document(docPath + fileName);
	var table = doc.GetTableByIndex(0);
	table.VerticalMerge(1, 0, 1);
	doc.Save(docPath + DateTime.Now.Ticks + fileName);
}

private void TestOldTableRangeMerge()
{
	string fileName = "test.docx";
	Document doc = new Document(docPath + fileName);
	var table = doc.GetTableByIndex(0);
	table.RangeMerge(0, 1, 1, 2);
	doc.Save(docPath + DateTime.Now.Ticks + fileName);
}

//insert table
private void TestInsertTable()
{
	string fileName = "test.docx";
	Document doc = new Document(docPath + fileName);
	var table = doc.GetTableByIndex(1);
	var rows = table.Rows;
	var r1 = rows[0];
	var cells = r1.Cells;
	Cell cell = (Cell)cells.LastOrDefault();
	var c2 = (Cell)cell.Clone(true);
	c2.AppendCellText("row 1,Cell 2 text");
	r1.AppendChild(c2);
	var c3 = (Cell)cell.Clone(false);
	c3.AppendCellText("row 1,Cell 3 text");
	r1.AppendChild(c3);
	doc.Save(docPath + DateTime.Now.Ticks + fileName);
}

private void TestOperateTable()
{
	string fileName = "test.docx";
	Document doc = new Document(docPath + fileName);
	var table = doc.GetTableByIndex(1);

	var a = "[{\"sectionName\":\"项目基本信息\",\"dataList\":[{\"name\":\"项目名称\",\"unit\":null,\"value\":[\"天津生态城综合能源项目\",\"天津生态城综合能源项目-1\",\"天津生态城综合能源项目-2\",\"天津生态城综合能源项目-3\"]},{\"name\":\"项目描述\",\"unit\":null,\"value\":[null,null,null,null]},{\"name\":\"项目地址\",\"unit\":null,\"value\":[\"天津市滨海新区顺吉路\",\"天津市滨海新区航苑路\",\"天津市滨海新区航苑路\",\"天津市滨海新区航苑路\"]}]},{\"sectionName\":\"可再生能源\",\"dataList\":[{\"name\":\"气温平均值\",\"unit\":\"℃\",\"value\":[\"12.8\",\"12.8\",\"12.8\",\"12.8\"]},{\"name\":\"日照平均值\",\"unit\":\"MJ/㎡\",\"value\":[\"170.78\",\"170.78\",\"170.78\",\"170.78\"]},{\"name\":\"风速平均值\",\"unit\":\"m/s\",\"value\":[\"3.01\",\"3.01\",\"3.01\",\"3.01\"]}]},{\"sectionName\":\"能耗分析\",\"dataList\":[{\"name\":\"用电总量\",\"unit\":\"kWh\",\"value\":[\"731584\",\"731584\",\"731584\",\"731584\"]},{\"name\":\"用热总量\",\"unit\":\"kWh\",\"value\":[\"411756.8\",\"411756.8\",\"411756.8\",\"411756.8\"]},{\"name\":\"用冷总量\",\"unit\":\"kWh\",\"value\":[\"766612\",\"766612\",\"766612\",\"766612\"]}]},{\"sectionName\":\"经济模型\",\"dataList\":[{\"name\":\"电价类型\",\"unit\":null,\"value\":[\"单一\",\"单一\",\"单一\",\"单一\"]},{\"name\":\"电价\",\"unit\":\"元/kWh\",\"value\":[\"0.82\",\"0.82\",\"0.82\",\"0.82\"]}]},{\"sectionName\":\"综合能源规划配置\",\"dataList\":[{\"name\":\"溴冷机\",\"unit\":\"\",\"value\":[\"550000\",\"550000\",\"550000\",\"550000\"]},{\"name\":\"地源热泵\",\"unit\":\"\",\"value\":[\"15394.60\",\"8624.20\",\"80600\",\"80600\"]},{\"name\":\"风机\",\"unit\":\"\",\"value\":[\"60000\",\"60000\",\"60000\",\"150000\"]},{\"name\":\"热电联产\",\"unit\":\"\",\"value\":[\"12000\",\"12000\",\"12000\",\"12000\"]},{\"name\":\"光伏逆变器\",\"unit\":\"\",\"value\":[\"1980\",\"1980\",\"1980\",\"1980\"]},{\"name\":\"光伏组件\",\"unit\":\"\",\"value\":[\"195.360\",\"195.360\",\"195.360\",\"195.360\"]},{\"name\":\"燃气锅炉\",\"unit\":\"\",\"value\":[\"0\",\"0\",\"50000\",\"50000\"]},{\"name\":\"利润总额\",\"unit\":\"万元\",\"value\":[\"61597.75\",\"78508.35\",\"72656.17\",\"159394.34\"]},{\"name\":\"年收入\",\"unit\":\"万元\",\"value\":[\"5815.97\",\"5394.39\",\"5394.39\",\"9566.45\"]},{\"name\":\"总投资\",\"unit\":\"万元\",\"value\":[\"22976.1\",\"18372.34\",\"20459.28\",\"26099.28\"]},{\"name\":\"经营成本\",\"unit\":\"万元\",\"value\":[\"2114.37\",\"1102.91\",\"1276.51\",\"1236.59\"]},{\"name\":\"运维费\",\"unit\":\"万元\",\"value\":[\"132.99\",\"132.99\",\"136.99\",\"172.99\"]},{\"name\":\"燃料动力费\",\"unit\":\"万元\",\"value\":[\"1923.94\",\"923.99\",\"1088.37\",\"998.35\"]},{\"name\":\"保险费\",\"unit\":\"万元\",\"value\":[\"57.44\",\"45.93\",\"51.15\",\"65.25\"]},{\"name\":\"其他费\",\"unit\":\"万元\",\"value\":[\"0\",\"0\",\"0\",\"0\"]}]}]";
	var dataList = JsonSerializer.Deserialize<List<ProjectSimpleContrast>>(a);
	insertProjectContrastTableData(table,dataList);
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

	public static Table CreateTable(this Document doc)
	{
		DocumentBuilder builder = new DocumentBuilder(doc);
		Table table = new Table(doc);
		Row row = new Row(doc);
		row.RowFormat.AllowBreakAcrossPages = true;
		table.AppendChild(row);
		return table;
	}


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


	public static void AppendCellText(this Cell cell, string text)
	{
		if (cell.FirstParagraph == null)
		{
			cell.AppendChild(new Paragraph(cell.Document));
		}
		var rs = cell.FirstParagraph.Runs;

		if (rs.FirstOrDefault() is not Run rf)
		{
			rf = new Run(cell.Document, text);
			rf.Text = text;
			cell.FirstParagraph.Runs.Add(rf);
		}
		else
		{
			rf.Text = text;
		}
	}

	/// <summary>
	/// 水平合并表格
	/// </summary>
	/// <param name="table"></param>
	/// <param name="rowIndex"></param>
	/// <param name="colStartIndex"></param>
	/// <param name="colEndIndex"></param>
	public static void HorizontalMerge(this Table table, int rowIndex, int colStartIndex, int colEndIndex, CellVerticalAlignment? alignment = CellVerticalAlignment.Center)
	{
		var mergeRow = table.Rows[rowIndex];
		mergeRow.Cells[colStartIndex].CellFormat.HorizontalMerge = CellMerge.First;
		if (alignment != null)
		{
			mergeRow.Cells[colStartIndex].CellFormat.VerticalAlignment = (CellVerticalAlignment)alignment;
		}

		for (int i = colStartIndex + 1; i <= colEndIndex; i++)
		{
			mergeRow.Cells[i].CellFormat.HorizontalMerge = CellMerge.Previous;
		}
	}

	/// <summary>
	/// 竖直合并表格
	/// </summary>
	/// <param name="table"></param>
	/// <param name="colIndex"></param>
	/// <param name="rowStartIndex"></param>
	/// <param name="rowEndIndex"></param>
	public static void VerticalMerge(this Table table, int colIndex, int rowStartIndex, int rowEndIndex, CellVerticalAlignment? alignment = CellVerticalAlignment.Center)
	{
		var rows = table.Rows;
		rows[rowStartIndex].Cells[colIndex].CellFormat.VerticalMerge = CellMerge.First;

		if (alignment != null)
		{
			rows[rowStartIndex].Cells[colIndex].CellFormat.VerticalAlignment = (CellVerticalAlignment)alignment;
		}
		for (int i = rowStartIndex + 1; i <= rowEndIndex; i++)
		{
			rows[i].Cells[colIndex].CellFormat.VerticalMerge = CellMerge.Previous;
		}
	}

	/// <summary>
	/// 合并表格区域
	/// </summary>
	/// <param name="table"></param>
	/// <param name="colStartIndex"></param>
	/// <param name="colEndIndex"></param>
	/// <param name="rowStartIndex"></param>
	/// <param name="rowEndIndex"></param>
	public static void RangeMerge(this Table table, int colStartIndex, int colEndIndex, int rowStartIndex, int rowEndIndex, CellVerticalAlignment? alignment = CellVerticalAlignment.Center)
	{
		//先合并行
		for (int i = rowStartIndex; i <= rowEndIndex; i++)
		{
			table.HorizontalMerge(i, colStartIndex, colEndIndex, alignment);
		}

		//再合并上一操作合并后的第一列
		table.VerticalMerge(colStartIndex, rowStartIndex, rowEndIndex, alignment);
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
	public static Node GetAncestorInBody(Node startNode)
	{
		while (startNode.ParentNode.NodeType != NodeType.Body)
		{
			startNode = startNode.ParentNode;
		}
		return startNode;
	}
	#endregion
}

/// <summary>
/// 插入方案基础信息对比表格数据
/// </summary>
/// <param name="table"></param>
/// <param name="datas"></param>
private void insertProjectContrastTableData(Table table, List<ProjectSimpleContrast> datas)
{
	if (datas == null || datas.Count == 0) return;
	var dataList = datas[0].DataList;
	if (dataList == null || dataList.Count == 0) return;

	if (table == null) return;
	var rows = table.Rows;
	if (rows == null || rows.Count == 0) return;
	//构建第一行数据
	var firstRow = rows[0];
	if (dataList.Count > firstRow.Cells.Count - 2)
	{
		var n = dataList.Count - (firstRow.Cells.Count - 2);
		var hasN = firstRow.Cells.Count;

		for (int i = 0; i < n; i++)
		{
			var lastCell = firstRow.LastOrDefault();
			var nc = (Cell)lastCell.Clone(true);
			var num_cn = NumToCNNumberDic[hasN + i - 2];
			nc.AppendCellText("方案" + num_cn);
			firstRow.AppendChild(nc);
		}
	}
	int rowIndex = 0;
	//开始加入数据
	foreach (var data in datas)
	{
		var dl = data.DataList;
		var n = dl.Count;
		for (int i = 0; i < n; i++)
		{
			rowIndex++;
			var rowClone = (Row)firstRow.Clone(true);//Clone(true) true为保留格式
			var sectionName = data.SectionName;
			//var cellClone = (Cell)rowClone.Cells.LastOrDefault();
			var c1 = rowClone.Cells[0];
			c1.AppendCellText(sectionName);

			var d = dl[i];
			var c2 = rowClone.Cells[1];
			c2.AppendCellText(d.Name);
			var values = d.Value;
			var m = values.Count;
			for (int j = 0; j < m; j++)
			{
				var v = values[j];
				var ci = rowClone.Cells[j + 2];
				ci.AppendCellText(v);
			}

			table.AppendChild(rowClone);
			//合并首列
			table.VerticalMerge(0, rowIndex - m + 1, rowIndex);
		}
	}
}

/// <summary>
/// 参数对比
/// </summary>
public class ProjectSimpleContrast
{
	/// <summary>
	/// 
	/// </summary>
	public ProjectSimpleContrast(string name, List<ProjectSimpleContrastValue> dataList)
	{
		SectionName = name;
		DataList = dataList;
	}
	/// <summary>
	/// 段落名称
	/// </summary>
	public string SectionName { get; set; }
	/// <summary>
	/// 内容
	/// </summary>
	public List<ProjectSimpleContrastValue> DataList { get; set; }
}

/// <summary>
/// 项目对比数据
/// </summary>
public class ProjectSimpleContrastValue
{
	/// <summary>
	/// ctor
	/// </summary>
	/// <param name="name"></param>
	/// <param name="value"></param>
	/// <param name="unit"></param>
	public ProjectSimpleContrastValue(string name, List<string> value, string unit = null)
	{
		Name = name;
		Value = value;
		Unit = unit;
	}

	/// <summary>
	/// ctor
	/// </summary>
	/// <param name="name"></param>
	/// <param name="value"></param>
	/// <param name="unit"></param>
	public ProjectSimpleContrastValue(string name, IEnumerable<string> value, string unit = null)
	{
		Name = name;
		Value = value.ToList();
		Unit = unit;
	}

	/// <summary>
	/// 对比项目名称
	/// </summary>
	public string Name { get; set; }
	/// <summary>
	/// 单位
	/// </summary>
	public string Unit { get; set; }
	/// <summary>
	/// 对比数值
	/// </summary>
	public List<string> Value { get; set; }
}


/// <summary>
/// 阿拉伯数字转中文汉字
/// </summary>
public static Dictionary<int, string> NumToCNNumberDic = new Dictionary<int, string>()
		{
			{ 0,"零" },
			{ 1,"一" },
			{ 2,"二" },
			{ 3,"三" },
			{ 4,"四" },
			{ 5,"五" },
			{ 6,"六" },
			{ 7,"七" },
			{ 8,"八" },
			{ 9,"九" }
		};