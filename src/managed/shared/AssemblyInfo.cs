/*
* The MIT License (MIT)
* 
* Copyright (c) 2012-2014 Fredrik Holmstrom (fredrik.johan.holmstrom@gmail.com)
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System.Reflection;
using System.Runtime.InteropServices;

#if UDPKIT_DLL_UDPKIT
[assembly: AssemblyTitle("udpkit")]
[assembly: AssemblyProduct("udpkit")]
#elif UDPKIT_DLL_PLATFORM_ANDROID
[assembly: AssemblyTitle("udpkit.platform.android")]
[assembly: AssemblyProduct("udpkit.platform.android")]
#elif UDPKIT_DLL_PLATFORM_IOS
[assembly: AssemblyTitle("udpkit.platform.ios")]
[assembly: AssemblyProduct("udpkit.platform.ios")]
#elif UDPKIT_DLL_PLATFORM_WIN32
[assembly: AssemblyTitle("udpkit.platform.win32")]
[assembly: AssemblyProduct("udpkit.platform.win32")]
#elif UDPKIT_DLL_PLATFORM_MANAGED
[assembly: AssemblyTitle("udpkit.platform.managed")]
[assembly: AssemblyProduct("udpkit.platform.managed")]
#endif

[assembly: AssemblyDescription(".Net/Mono/Unity networking library for games")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyCopyright("Copyright © 2012-2014 Fredrik Holmstrom")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("0.1.7.2")]
[assembly: AssemblyFileVersion("0.1.7.2")]
