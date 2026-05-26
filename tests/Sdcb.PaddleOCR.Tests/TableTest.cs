using OpenCvSharp;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR.Models.Local;
using Sdcb.PaddleOCR.Models.Online;
using Xunit;

namespace Sdcb.PaddleOCR.Tests;

public class TableTest
{
    [Theory]
    [InlineData("en_ppstructure_mobile_v2.0_SLANet")]
    [InlineData("ch_ppstructure_mobile_v2.0_SLANet")]
    public void LocalTableTest(string modelName)
    {
        using PaddleOcrTableRecognizer tableRec = new(LocalTableRecognitionModel.All.Single(x => x.Name == modelName));
        using Mat src = Cv2.ImRead("samples/table.jpg");
        TableDetectionResult result = tableRec.Run(src);
        Assert.NotNull(result);
        Assert.NotEmpty(result.StructureBoxes);
        Assert.NotEmpty(result.HtmlTags);
        Assert.True(result.Score > 0.9f);
        //using Mat visualized = result.Visualize(src, Scalar.LightGreen);
        //Cv2.ImWrite("table-visualized.jpg", visualized);
    }

    [Theory]
    [InlineData("en_ppstructure_mobile_v2.0_SLANet", "<table><thead><tr><td>Methods</td>")]
    [InlineData("ch_ppstructure_mobile_v2.0_SLANet", "<table><tbody><tr><td>Methods</td>")]
    public async Task LocalTableRebuild(string modelName, string expectedHtmlStart)
    {
        using Mat src = Cv2.ImRead("samples/table.jpg");
        TableDetectionResult tableResult;
        using (PaddleOcrTableRecognizer tableRec = new(LocalTableRecognitionModel.All.Single(x => x.Name == modelName)))
        {
            tableResult = tableRec.Run(src);
        }

        PaddleOcrResult ocrResult;
        using (PaddleOcrAll all = new(LocalFullModels.ChineseV5))
        {
            all.Detector.UnclipRatio = 1.2f;
            ocrResult = all.Run(src);
        }

        //tableResult.Visualize(src, Scalar.LightGreen).ImWrite(modelName + ".jpg");

        string html = tableResult.RebuildTable(ocrResult);
        Assert.StartsWith(expectedHtmlStart, html);
        await Task.CompletedTask;
    }

    [Theory]
    [InlineData("en_ppstructure_mobile_v2.0_SLANet", "<table><thead><tr><td>Methods</td>")]
    [InlineData("ch_ppstructure_mobile_v2.0_SLANet", "<table><tbody><tr><td>Methods</td>")]
    public async Task OnlineTableRebuild(string modelName, string expectedHtmlStart)
    {
        using Mat src = Cv2.ImRead("samples/table.jpg");

        PaddleOcrResult ocrResult;
        using (PaddleOcrAll all = new(LocalFullModels.ChineseV5))
        {
            all.Detector.UnclipRatio = 1.2f;
            ocrResult = all.Run(src);
        }

        OnlineTableRecognitionModel tableOnlineModel = OnlineTableRecognitionModel.All.Single(x => x.Name == modelName);
        TableRecognitionModel tableModel = await tableOnlineModel.DownloadAsync();
        TableDetectionResult tableResult;
        using (PaddleOcrTableRecognizer tableRec = new(tableModel))
        {
            tableResult = tableRec.Run(src);
        }

        string html = tableResult.RebuildTable(ocrResult);
        Assert.StartsWith(expectedHtmlStart, html);
    }

    [Theory]
    [InlineData("en_ppstructure_mobile_v2.0_SLANet")]
    [InlineData("ch_ppstructure_mobile_v2.0_SLANet")]
    public async Task OnlineTableTest(string modelName)
    {
        OnlineTableRecognitionModel tableOnlineModel = OnlineTableRecognitionModel.All.Single(x => x.Name == modelName);
        TableRecognitionModel tableModel = await tableOnlineModel.DownloadAsync();
        using PaddleOcrTableRecognizer tableRec = new(tableModel);
        using Mat src = Cv2.ImRead("samples/table.jpg");
        TableDetectionResult result = tableRec.Run(src);
        Assert.NotNull(result);
        Assert.NotEmpty(result.StructureBoxes);
        Assert.NotEmpty(result.HtmlTags);
        Assert.True(result.Score > 0.9f);
        //using Mat visualized = result.Visualize(src, Scalar.LightGreen);
        //Cv2.ImWrite("table-visualized.jpg", visualized);
    }
}
