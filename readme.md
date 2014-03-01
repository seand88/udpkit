# License

The MIT License (MIT)

Copyright (c) 2012-2013 Fredrik Holmstrom (fredrik.johan.holmstrom@gmail.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

# Compiling

Building the managed assemblies:
1. Make sure you have either Xamarin Studio/Mono Develop/Visual Studio installed
2. Open the udpkit.sln file that is located in src/managed
3. Choose to build the entire solution
4. The .dll and .pdb/.mdb files can now be found bin/managed

Building native shared library for Android:
1. Make sure you have the Android NDK installed, which can be found here: https://developer.android.com/tools/sdk/ndk/index.html
2. Open a terminal and go into the src/native/android directory in the udpkit directory.
3. Run the NDK build command like this: ndk_path/ndk-build udpkit/path/src/native/android
4. The .so file can now be found in the "obj" directory inside src/native/android

Building native static library for iOS:
1. Make sure you have the latest Xcode installed
2. Open the .xcodeproj file located in src/native/ios
3. Build the project from the build menu
4. The static library (.a file) is now located in your Xcode derived data directory,
which can be found by going to XCode/Preferences/Locations and checking “Derived Data”.

