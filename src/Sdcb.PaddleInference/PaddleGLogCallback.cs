using System;

namespace Sdcb.PaddleInference;

/// <summary>
/// Represents a managed callback that receives Paddle glog messages.
/// </summary>
/// <param name="severity">The message severity.</param>
/// <param name="file">The native source file name, if available.</param>
/// <param name="line">The native source line number.</param>
/// <param name="message">The log message text.</param>
public delegate void PaddleGLogCallback(PaddleGLogSeverity severity, string file, int line, string message);

/// <summary>
/// Represents the severity level of a Paddle glog message.
/// </summary>
public enum PaddleGLogSeverity
{
    /// <summary>
    /// The native severity value is not recognized.
    /// </summary>
    Unknown = -1,

    /// <summary>
    /// Informational message.
    /// </summary>
    Info = 0,

    /// <summary>
    /// Warning message.
    /// </summary>
    Warning = 1,

    /// <summary>
    /// Error message.
    /// </summary>
    Error = 2,

    /// <summary>
    /// Fatal message.
    /// </summary>
    Fatal = 3,
}

