using System;

namespace Sdcb.PaddleInference;

/// <summary>
/// Represents a managed callback that receives Paddle glog messages.
/// </summary>
/// <param name="severity">The raw severity value forwarded from the native glog callback. According to glog documentation, the common values are 0=INFO, 1=WARNING, 2=ERROR, and 3=FATAL, but this API does not strictly limit the value and will pass through any native integer as-is.</param>
/// <param name="file">The native source file name, if available.</param>
/// <param name="line">The native source line number.</param>
/// <param name="message">The log message text.</param>
public delegate void PaddleGLogCallback(int severity, string file, int line, string message);

