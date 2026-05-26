using System;
using System.Runtime.InteropServices;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

namespace Sdcb.PaddleInference.Native;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void PD_GlogRedirectCallback(
    int severity,
    IntPtr file,
    int line,
    IntPtr message,
    nuint messageLen,
    IntPtr userData);