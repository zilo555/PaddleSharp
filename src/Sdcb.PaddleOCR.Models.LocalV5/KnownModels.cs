using System.Collections.Generic;

namespace Sdcb.PaddleOCR.Models.LocalV5;

/// <summary>
/// Contains known models for local version 5
/// </summary>
public static class KnownModels
{
    /// <summary>
    /// HashSet containing all the model names
    /// </summary>
    public static HashSet<string> All = new(new[]
    {
        "mobile-zh-det",
        "mobile-zh-rec",
        "en_PP-OCRv5_mobile_rec",
        "korean_PP-OCRv5_mobile_rec",
        "latin_PP-OCRv5_mobile_rec",
        "eslav_PP-OCRv5_mobile_rec",
        "th_PP-OCRv5_mobile_rec",
        "el_PP-OCRv5_mobile_rec",
        "cyrillic_PP-OCRv5_mobile_rec",
        "arabic_PP-OCRv5_mobile_rec",
        "devanagari_PP-OCRv5_mobile_rec",
        "te_PP-OCRv5_mobile_rec",
        "ta_PP-OCRv5_mobile_rec",
    });
}
