using Sdcb.PaddleInference.Native;
using System;

namespace Sdcb.PaddleInference;

internal static class PaddleAdvancedNativeApi
{
    private static readonly Version V2CapiVersion = new(3, 3, 0);
    private static readonly object GlogSync = new();
    private static readonly PD_GlogRedirectCallback NativeGlogRedirectCallback = OnNativeGlogRedirect;
    private static PaddleGLogCallback? _glogRedirectCallback;

    public static bool IsV2CapiAvailable => PaddleConfig.GetVersion() >= V2CapiVersion;

    public static void EnsureV2CapiAvailable(string apiName)
    {
        if (!IsV2CapiAvailable)
        {
            throw new NotSupportedException($"{apiName} requires Paddle Inference {V2CapiVersion} or later.");
        }
    }

    public static string GetLastErrorMessage()
    {
        return PaddleNative.PD_GetLastError().UTF8PtrToString() ?? "Unknown Paddle native error.";
    }

    public static void ThrowIfLastError(string operation)
    {
        int errorCode = PaddleNative.PD_GetLastErrorCode();
        if (errorCode != 0)
        {
            throw new InvalidOperationException($"{operation} failed with Paddle status code {errorCode}: {GetLastErrorMessage()}");
        }
    }

    public static void ThrowIfFailed(string operation, int statusCode)
    {
        if (statusCode != 0)
        {
            throw new InvalidOperationException($"{operation} failed with Paddle status code {statusCode}: {GetLastErrorMessage()}");
        }
    }

    public static unsafe IntPtr CreatePredictor(IntPtr config)
    {
        if (!IsV2CapiAvailable)
        {
            return PaddleNative.PD_PredictorCreate(config);
        }

        IntPtr predictor = IntPtr.Zero;
        int statusCode = PaddleNative.PD_PredictorCreate2(config, (IntPtr)(&predictor));
        ThrowIfFailed("Creating predictor", statusCode);
        return predictor;
    }

    public static unsafe bool RunPredictor(IntPtr predictor)
    {
        if (!IsV2CapiAvailable)
        {
            return PaddleNative.PD_PredictorRun(predictor) != 0;
        }

        sbyte result = 0;
        int statusCode = PaddleNative.PD_PredictorRun2(predictor, (IntPtr)(&result));
        ThrowIfFailed("Running predictor", statusCode);
        return result != 0;
    }

    public static int GetGlogMinLogLevel()
    {
        EnsureV2CapiAvailable("GLog minimum log level");
        int level = PaddleNative.PD_GetGlogMinLogLevel();
        ThrowIfLastError("Getting GLog minimum log level");
        return level;
    }

    public static void SetGlogMinLogLevel(int level)
    {
        EnsureV2CapiAvailable("GLog minimum log level");
        PaddleNative.PD_SetGlogMinLogLevel(level);
        ThrowIfLastError("Setting GLog minimum log level");
    }

    public static void SetGlogRedirectCallback(PaddleGLogCallback? callback)
    {
        EnsureV2CapiAvailable("GLog redirect callback");

        lock (GlogSync)
        {
            PaddleNative.PD_SetGlogRedirectCallback(callback == null ? null! : NativeGlogRedirectCallback, IntPtr.Zero);
            ThrowIfLastError("Setting GLog redirect callback");
            _glogRedirectCallback = callback;
        }
    }

    private static void OnNativeGlogRedirect(
        int severity,
        IntPtr file,
        int line,
        IntPtr message,
        nuint messageLen,
        IntPtr userData)
    {
        PaddleGLogCallback? callback;

        lock (GlogSync)
        {
            callback = _glogRedirectCallback;
        }

        if (callback == null)
        {
            return;
        }

        callback(
            ConvertSeverity(severity),
            file.ANSIToString() ?? string.Empty,
            line,
            message.ANSIToString(checked((int)messageLen)) ?? string.Empty);
    }

    private static PaddleGLogSeverity ConvertSeverity(int severity)
    {
        return severity switch
        {
            0 => PaddleGLogSeverity.Info,
            1 => PaddleGLogSeverity.Warning,
            2 => PaddleGLogSeverity.Error,
            3 => PaddleGLogSeverity.Fatal,
            _ => PaddleGLogSeverity.Unknown,
        };
    }
}