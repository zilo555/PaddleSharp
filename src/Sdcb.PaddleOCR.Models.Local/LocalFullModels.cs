namespace Sdcb.PaddleOCR.Models.Local;

/// <summary>
/// Provides a collection of the recommended local OCR models.
/// </summary>
public static class LocalFullModels
{
    /// <summary>
    /// Chinise v5 version
    /// </summary>
    public static FullOcrModel ChineseV5 => new(LocalDetectionModel.ChineseV5, LocalClassificationModel.ChineseMobileV2, LocalRecognizationModel.ChineseV5);

    /// <summary>
    /// English v5 version.
    /// </summary>
    public static FullOcrModel EnglishV5 => new(LocalDetectionModel.ChineseV5, LocalClassificationModel.ChineseMobileV2, LocalRecognizationModel.EnglishV5);

    /// <summary>
    /// Korean v5 version.
    /// </summary>
    public static FullOcrModel KoreanV5 => new(LocalDetectionModel.ChineseV5, LocalClassificationModel.ChineseMobileV2, LocalRecognizationModel.KoreanV5);

    /// <summary>
    /// Latin v5 version.
    /// </summary>
    public static FullOcrModel LatinV5 => new(LocalDetectionModel.ChineseV5, LocalClassificationModel.ChineseMobileV2, LocalRecognizationModel.LatinV5);

    /// <summary>
    /// East Slavic v5 version.
    /// </summary>
    public static FullOcrModel EastSlavicV5 => new(LocalDetectionModel.ChineseV5, LocalClassificationModel.ChineseMobileV2, LocalRecognizationModel.EastSlavicV5);

    /// <summary>
    /// Thai v5 version.
    /// </summary>
    public static FullOcrModel ThaiV5 => new(LocalDetectionModel.ChineseV5, LocalClassificationModel.ChineseMobileV2, LocalRecognizationModel.ThaiV5);

    /// <summary>
    /// Greek v5 version.
    /// </summary>
    public static FullOcrModel GreekV5 => new(LocalDetectionModel.ChineseV5, LocalClassificationModel.ChineseMobileV2, LocalRecognizationModel.GreekV5);

    /// <summary>
    /// Cyrillic v5 version.
    /// </summary>
    public static FullOcrModel CyrillicV5 => new(LocalDetectionModel.ChineseV5, LocalClassificationModel.ChineseMobileV2, LocalRecognizationModel.CyrillicV5);

    /// <summary>
    /// Arabic v5 version.
    /// </summary>
    public static FullOcrModel ArabicV5 => new(LocalDetectionModel.ChineseV5, LocalClassificationModel.ChineseMobileV2, LocalRecognizationModel.ArabicV5);

    /// <summary>
    /// Devanagari v5 version.
    /// </summary>
    public static FullOcrModel DevanagariV5 => new(LocalDetectionModel.ChineseV5, LocalClassificationModel.ChineseMobileV2, LocalRecognizationModel.DevanagariV5);

    /// <summary>
    /// Telugu v5 version.
    /// </summary>
    public static FullOcrModel TeluguV5 => new(LocalDetectionModel.ChineseV5, LocalClassificationModel.ChineseMobileV2, LocalRecognizationModel.TeluguV5);

    /// <summary>
    /// Tamil v5 version.
    /// </summary>
    public static FullOcrModel TamilV5 => new(LocalDetectionModel.ChineseV5, LocalClassificationModel.ChineseMobileV2, LocalRecognizationModel.TamilV5);

    /// <summary>
    /// Provides an array of all available OCR models for PaddleOCR
    /// </summary>
    public static FullOcrModel[] All => new[]
    {
        ChineseV5,
        EnglishV5,
        KoreanV5,
        LatinV5,
        EastSlavicV5,
        ThaiV5,
        GreekV5,
        CyrillicV5,
        ArabicV5,
        DevanagariV5,
        TeluguV5,
        TamilV5,
    };
}
