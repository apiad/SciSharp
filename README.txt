SciSharp - Scientific Computing for .NET
========================================

SciSharp is a set of libraries and tools for .NET 4 tailored for
scientific computing. It includes one main library (SciSharp.dll)
with the core API, a few smaller libraries which provide alternative
implementations that might not work on your system (for instance,
GPU acceleration), a few utilities and a lot of examples.

SciSharp is a work in progress, and it is still in a very unstable
state, so use it at your own risk. So said, we think it does
contain some helpful and mostly working tools for scientific 
computing applications that may be of interest both for researchers
in academic areas and for developers who want to spice their software
with a bit of extra magic.

It currently encompasses the following areas:

 * Algorithms on graphs, trees and other collections
 * Language Processing
 * Machine Learning
 * Numerical Optimization (exact and heuristic) 
 * Statistics and Probabilities
 * Simulation


Installation
------------

To install SciSharp, simply run the corresponding installer
(x.y.z is the corresponding version number):

 * On Windows: SciSharp-x86-x.y.z.msi
               SciSharp-x64-x.y.z.msi
			   
 * On Linux:   # dpkg -i SciSharp-x86-x.y.z.deb
               # dpkg -i SciSharp-x64-x.y.z.deb
			   
	NOTE: On Linux you must have Mono to install
	      and use SciSharp.

This procedure will install the project libraries in the GAC
and create shortcuts to the utilities, examples and documentation,
where your operating system demands:

 * On Windows: C:\Program Files\SciSharp          <-- x64 version
               C:\Program Files (x86)\SciSharp    <-- x86 version
 
 * On Linux: /usr/lib/scisharp/                   <-- libraries
             /usr/doc/scisharp/                   <-- documentation
			   
After installation, check the documentation and examples.

			   
Source Code
-----------

Source is available under the MIT License on the following URLs:

Plain download (zip file):
	http://github.com/AlejandroPiad/SciSharp/master.zip

Plain download (tar file)
	http://github.com/AlejandroPiad/SciSharp/master.tar.gz
	
Clone with Git (read-only)
	git clone http://github.com/AlejandroPiad/SciSharp.git
	
To build the project you will need a C# 4.0 compatible
compiler. On Windows you can use Visual Studio 2012,
SharpDevelop, or any other compatible IDE.
On Linux we recommend using MonoDevelop.

	
Colaboration
------------
	
For colaboration, bug reporting, and updates, please visit
http://github.com/AlejandroPiad/SciSharp/

You can contact the project manager on <scisharp@apiad.net>
