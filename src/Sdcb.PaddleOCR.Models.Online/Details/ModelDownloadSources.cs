using SharpCompress.Archives;
using SharpCompress.Archives.GZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Sdcb.PaddleOCR.Models.Online.Details;

/// <summary>
/// Represents a model download source that can prepare a local inference directory.
/// </summary>
public interface IModelDownloadSource
{
    /// <summary>
    /// Gets a human readable description for logging and diagnostics.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Downloads or materializes the model into the specified directory.
    /// </summary>
    Task PrepareModelAsync(string modelName, string rootDir, CancellationToken cancellationToken);
}

/// <summary>
/// Factory helpers for composing model download source fallbacks.
/// </summary>
public static class ModelDownloadSources
{
    /// <summary>
    /// Creates download sources with an archive primary source only.
    /// </summary>
    public static IModelDownloadSource[] Create(string primaryArchiveUri)
    {
        return new IModelDownloadSource[]
        {
            new ArchiveModelDownloadSource(new Uri(primaryArchiveUri)),
        };
    }

    /// <summary>
    /// Creates download sources with an archive primary source and a deterministic Hugging Face fallback source.
    /// </summary>
    public static IModelDownloadSource[] Create(
        string primaryArchiveUri,
        string huggingFaceRepoId,
        params string[] huggingFaceRemotePaths)
    {
        if (huggingFaceRemotePaths.Length == 0)
        {
            throw new ArgumentException("Hugging Face fallback requires explicit remote paths.", nameof(huggingFaceRemotePaths));
        }

        return new IModelDownloadSource[]
        {
            new ArchiveModelDownloadSource(new Uri(primaryArchiveUri)),
            new HuggingFaceRepoModelDownloadSource(huggingFaceRepoId, huggingFaceRemotePaths),
        };
    }
}

internal abstract class ModelDownloadSourceBase : IModelDownloadSource
{
    private static readonly HttpClient _http = new();

    public abstract string Description { get; }

    public abstract Task PrepareModelAsync(string modelName, string rootDir, CancellationToken cancellationToken);

    protected static async Task DownloadFileAsync(Uri uri, string localFile, CancellationToken cancellationToken)
    {
        try
        {
            HttpResponseMessage resp = await _http.GetAsync(uri, cancellationToken);
            if (!resp.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to download: {uri}, status code: {(int)resp.StatusCode}({resp.StatusCode})");
            }

            using FileStream file = File.Create(localFile);
            await resp.Content.CopyToAsync(file);
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Failed to download: {uri}", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new TimeoutException($"Failed to download: {uri}, timeout.", ex);
        }
    }
}

internal sealed class ArchiveModelDownloadSource : ModelDownloadSourceBase
{
    public ArchiveModelDownloadSource(Uri archiveUri)
    {
        ArchiveUri = archiveUri;
    }

    public Uri ArchiveUri { get; }

    public override string Description => ArchiveUri.ToString();

    public override async Task PrepareModelAsync(string modelName, string rootDir, CancellationToken cancellationToken)
    {
        string localArchiveFile = Path.Combine(rootDir, Path.GetFileName(ArchiveUri.LocalPath));

        if (!File.Exists(localArchiveFile) || new FileInfo(localArchiveFile).Length == 0)
        {
            Console.WriteLine($"Downloading {modelName} model from {ArchiveUri}");
            await DownloadFileAsync(ArchiveUri, localArchiveFile, cancellationToken);
        }

        Console.WriteLine($"Extracting {localArchiveFile} to {rootDir}");
        using (IArchive archive = ArchiveFactory.Open(localArchiveFile))
        {
            if (archive is GZipArchive)
            {
                using Stream stream = archive.Entries.Single().OpenEntryStream();
                using MemoryStream ms = new();
                stream.CopyTo(ms);
                ms.Position = 0;
                using IArchive inner = ArchiveFactory.Open(ms);
                inner.WriteToDirectory(rootDir);
            }
            else
            {
                archive.WriteToDirectory(rootDir);
            }
        }

        File.Delete(localArchiveFile);
    }
}

internal sealed class HuggingFaceRepoModelDownloadSource : ModelDownloadSourceBase
{
    public HuggingFaceRepoModelDownloadSource(string repoId, params string[] remotePaths)
    {
        string normalizedRepoId = repoId.Trim('/');
        BaseUri = new Uri($"https://huggingface.co/{normalizedRepoId}/resolve/main/");
        if (remotePaths.Length == 0)
        {
            throw new ArgumentException("At least one Hugging Face remote path is required.", nameof(remotePaths));
        }

        RemotePaths = remotePaths;
    }

    public Uri BaseUri { get; }

    public IReadOnlyList<string> RemotePaths { get; }

    public override string Description => BaseUri.ToString();

    public override async Task PrepareModelAsync(string modelName, string rootDir, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Downloading {modelName} model files from {BaseUri}");

        foreach (string remotePath in RemotePaths)
        {
            Uri uri = new(BaseUri, remotePath + "?download=true");
            string localFileName = Path.GetFileName(uri.LocalPath);
            await DownloadFileAsync(uri, Path.Combine(rootDir, localFileName), cancellationToken);
        }
    }
}