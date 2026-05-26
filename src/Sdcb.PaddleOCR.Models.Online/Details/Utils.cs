using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Sdcb.PaddleOCR.Models.Online.Details;

internal static class Utils
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> _modelLocks = new();

    public static async Task DownloadAndExtractAsync(
        string name, IModelDownloadSource[] sources, string rootDir, CancellationToken cancellationToken)
    {
        string paramsFile = Path.Combine(rootDir, "inference.pdiparams");
        if (File.Exists(paramsFile))
            return;

        string key = name;
        SemaphoreSlim gate = _modelLocks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

        await gate.WaitAsync(cancellationToken);
        try
        {
            if (File.Exists(paramsFile))
                return;

            Directory.CreateDirectory(rootDir);

            Exception? lastError = null;
            foreach (IModelDownloadSource source in sources)
            {
                try
                {
                    CleanupPartialModelFiles(rootDir);

                    await source.PrepareModelAsync(name, rootDir, cancellationToken);
                    CheckLocalOCRModel(rootDir);
                    return;
                }
                catch (Exception ex) when (!(ex is OperationCanceledException))
                {
                    lastError = ex;
                    Console.WriteLine($"Failed to prepare model from {source.Description}: {ex.Message}");
                    CleanupPartialModelFiles(rootDir);
                }
            }

            throw new Exception($"Failed to prepare model {name} from all sources.", lastError);
        }
        finally
        {
            gate.Release();

            if (gate.CurrentCount == 1)
                _modelLocks.TryRemove(key, out _);
        }
    }

    private static void CleanupPartialModelFiles(string rootDir)
    {
        string[] filesToDelete = new[]
        {
            "inference.json",
            "inference.pdmodel",
            "model.json",
            "model.pdmodel",
            "inference.pdiparams",
            "model.pdiparams",
            "inference.yml",
        };

        foreach (string fileName in filesToDelete)
        {
            string path = Path.Combine(rootDir, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        foreach (string archivePath in Directory.EnumerateFiles(rootDir, "*.tar"))
        {
            File.Delete(archivePath);
        }
    }

    public static void CheckLocalOCRModel(string rootDir)
    {
        string[] filesToCheck = new[]
        {
            Path.Combine(rootDir, "inference.pdiparams"),
        };

        foreach (string path in filesToCheck)
        {
            string fileName = Path.GetFileName(path);

            if (!File.Exists(path))
            {
                throw new Exception($"{fileName} not found in {rootDir}, model error?");
            }

            if (new FileInfo(path).Length == 0)
            {
                throw new Exception($"{fileName} invalid(length = 0), model error?");
            }
        }
    }

    public readonly static Type RootType = typeof(Settings);
    public readonly static Assembly RootAssembly = typeof(Settings).Assembly;
}
