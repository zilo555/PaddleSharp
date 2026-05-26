using Sdcb.PaddleInference;
using Sdcb.PaddleOCR.Models.Local.Details;

namespace Sdcb.PaddleOCR.Models.Local;

/// <summary>
/// This class represents a local detection model used by PaddleOCR to detect text from an image.
/// </summary>
public class LocalDetectionModel : DetectionModel
{
    /// <summary>
    /// Gets the name of this model.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalDetectionModel"/> class with the specified name and version.
    /// </summary>
    /// <param name="name">The name of the model.</param>
    /// <param name="version">The version of the model.</param>
    public LocalDetectionModel(string name, ModelVersion version) : base(version)
    {
        Name = name;
    }

    /// <inheritdoc/>
    public override PaddleConfig CreateConfig() => Utils.LocalModel(Name, Version);

    /// <summary>
    /// Gets the Chinese language detection model for version 5.
    /// </summary>
    public static LocalDetectionModel ChineseV5 => new("mobile-zh-det", ModelVersion.V5);

    /// <summary>
    /// Gets an array of all the available <see cref="LocalDetectionModel"/> objects.
    /// </summary>
    public static LocalDetectionModel[] All => new[]
    {
        ChineseV5,
    };
}
