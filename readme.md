# License

The MIT License (MIT)

Copyright (c) 2012-2014 Fredrik Holmstrom (fredrik.johan.holmstrom@gmail.com)

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

### Building the managed assemblies

* Make sure you have either Xamarin Studio/Mono Develop/Visual Studio installed
* Open the udpkit.sln file that is located in src/managed
* Choose to build the entire solution
* The .dll and .pdb/.mdb files can now be found bin/managed

### Building native shared library for Android

* Make sure you have the Android NDK installed, which can be found here: https://developer.android.com/tools/sdk/ndk/index.html
* Open a terminal and go into the `src/native/android` directory in the udpkit directory.
* Run the NDK build command like this: `ndk-build`
* The .so file can now be found in the "obj" directory inside `src/native/android`

### Building native static library for iOS

* Make sure you have the latest Xcode installed
* Open the .xcodeproj file located in src/native/ios
* Build the project from the build menu
* The static library (.a file) is now located in your Xcode derived data directory,
which can be found by going to XCode/Preferences/Locations and checking “Derived Data”.

