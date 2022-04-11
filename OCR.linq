<Query Kind="Program">
  <Namespace>Sdcb.PaddleOCR</Namespace>
  <Namespace>Sdcb.PaddleOCR.KnownModels</Namespace>
</Query>

void Main()
{
	//需要引用的包如下
	/*
<ItemGroup>
  <PackageReference Include="OpenCvSharp4" Version="4.5.5.20211231" />
  <PackageReference Include="OpenCvSharp4.runtime.win" Version="4.5.5.20211231" />
  <PackageReference Include="Sdcb.PaddleInference" Version="2.2.1.16" />
  <PackageReference Include="Sdcb.PaddleInference.runtime.win64.mkl" Version="2.2.1.9" />
  <PackageReference Include="Sdcb.PaddleOCR" Version="2.2.1.16" />
  <PackageReference Include="Sdcb.PaddleOCR.KnownModels" Version="2.2.1.16" />
</ItemGroup>
	*/
	OCRModel model = KnownOCRModel.PPOcrV2;
	await model.EnsureAll();

	//byte[] sampleImageData;
	//string sampleImageUrl = @"https://www.tp-link.com.cn/content/images/detail/2164/TL-XDR5450易展Turbo版-3840px_03.jpg";
	//using (HttpClient http = new HttpClient())
	//{
	//    Console.WriteLine("Download sample image from: " + sampleImageUrl);
	//    sampleImageData = await http.GetByteArrayAsync(sampleImageUrl);
	//}

	using (PaddleOcrAll all = new PaddleOcrAll(model.RootDirectory, model.KeyPath)
	{
		AllowRotateDetection = true, /* 允许识别有角度的文字 */
		Enable180Classification = false, /* 允许识别旋转角度大于90度的文字 */
	})
	{
		//using Mat src = Cv2.ImDecode(sampleImageData, ImreadModes.Color);
		// Load local file by following code:
		using Mat src = Cv2.ImRead(@"D:\2.webp");
		PaddleOcrResult result = all.Run(src);
		Console.WriteLine("Detected all texts: \n" + result.Text);
		foreach (PaddleOcrResultRegion region in result.Regions)
		{
			Console.WriteLine($"Text: {region.Text}, Score: {region.Score}, RectCenter: {region.Rect.Center}, RectSize: {region.Rect.Size}, Angle: {region.Rect.Angle}");
		}

	}
	Console.ReadKey();
}

