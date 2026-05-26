using Sdcb.PaddleInference;
using Sdcb.PaddleOCR.Models.Local.Details;
using Sdcb.PaddleOCR.Models.Shared;
using System.Collections.Generic;

namespace Sdcb.PaddleOCR.Models.Local;

/// <summary>
/// Provides a local implementation of PaddleOCR model with the ability to recognize various languages such as Chinese, English, Korean, Japanese, Telugu and Devanagari
/// </summary>
public class LocalRecognizationModel : RecognizationModel
{
    /// <summary>
    /// The name of the model.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// A list of labels for recognition.
    /// </summary>
    public IReadOnlyList<string> Labels { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalRecognizationModel"/> class.
    /// </summary>
    /// <param name="name">The name of the model.</param>
    /// <param name="dictName">The dictionary name of the model.</param>
    /// <param name="version">The version of the model.</param>
    public LocalRecognizationModel(string name, string dictName, ModelVersion version) : base(version)
    {
        Name = name;
        if (version == ModelVersion.V5)
        {
            Labels = Utils.LoadV5Dicts(name);
        }
        else
        {
            Labels = SharedUtils.LoadDicts(dictName);
        }
    }

    /// <summary>
    /// Gets label by index for Labels.
    /// </summary>
    /// <param name="i">The index of the label to get</param>
    /// <returns>The specified label</returns>
    public override string GetLabelByIndex(int i) => GetLabelByIndex(i, Labels);

    /// <summary>
    /// Creates and returns a PaddleConfig instance based on the Name property.
    /// </summary>
    /// <returns>A new instance of PaddleConfig</returns>
    public override PaddleConfig CreateConfig()
    {
        return Utils.LocalModel(Name, Version);
    }

    /// <summary>
    /// Gets the Chinese V5 local recognition model.
    /// </summary>
    public static LocalRecognizationModel ChineseV5 => new("mobile-zh-rec", "", ModelVersion.V5);

    /// <summary>
    /// Gets the English V5 local recognition model.
    /// </summary>
    public static LocalRecognizationModel EnglishV5 => new("en_PP-OCRv5_mobile_rec", "", ModelVersion.V5);

    /// <summary>
    /// Gets the Korean V5 local recognition model.
    /// </summary>
    public static LocalRecognizationModel KoreanV5 => new("korean_PP-OCRv5_mobile_rec", "", ModelVersion.V5);

    /// <summary>
    /// Gets the Latin V5 local recognition model.
    /// </summary>
    public static LocalRecognizationModel LatinV5 => new("latin_PP-OCRv5_mobile_rec", "", ModelVersion.V5);

    /// <summary>
    /// Gets the East Slavic V5 local recognition model.
    /// </summary>
    public static LocalRecognizationModel EastSlavicV5 => new("eslav_PP-OCRv5_mobile_rec", "", ModelVersion.V5);

    /// <summary>
    /// Gets the Thai V5 local recognition model.
    /// </summary>
    public static LocalRecognizationModel ThaiV5 => new("th_PP-OCRv5_mobile_rec", "", ModelVersion.V5);

    /// <summary>
    /// Gets the Greek V5 local recognition model.
    /// </summary>
    public static LocalRecognizationModel GreekV5 => new("el_PP-OCRv5_mobile_rec", "", ModelVersion.V5);

    /// <summary>
    /// Gets the Cyrillic V5 local recognition model.
    /// </summary>
    public static LocalRecognizationModel CyrillicV5 => new("cyrillic_PP-OCRv5_mobile_rec", "", ModelVersion.V5);

    /// <summary>
    /// Gets the Arabic V5 local recognition model.
    /// </summary>
    public static LocalRecognizationModel ArabicV5 => new("arabic_PP-OCRv5_mobile_rec", "", ModelVersion.V5);

    /// <summary>
    /// Gets the Devanagari V5 local recognition model.
    /// </summary>
    public static LocalRecognizationModel DevanagariV5 => new("devanagari_PP-OCRv5_mobile_rec", "", ModelVersion.V5);

    /// <summary>
    /// Gets the Telugu V5 local recognition model.
    /// </summary>
    public static LocalRecognizationModel TeluguV5 => new("te_PP-OCRv5_mobile_rec", "", ModelVersion.V5);

    /// <summary>
    /// Gets the Tamil V5 local recognition model.
    /// </summary>
    public static LocalRecognizationModel TamilV5 => new("ta_PP-OCRv5_mobile_rec", "", ModelVersion.V5);

    /// <summary>
    /// An array containing all instances of the LocalRecognizationModel
    /// </summary>
    public static LocalRecognizationModel[] All => new[]
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
