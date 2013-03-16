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

Currently there is no installation package for for SciSharp,
instead it is distributed in two forms: binaries and source.
The binary packing can be used to directly reference the
libraries on your project. 

Compiled binaries can be obtained for both Windows and Linux,
and different platforms from:

	http://apiad.net/~apiad/SciSharp/Bin/Libraries

			   
Source Code
-----------

Source is available under the MIT License on the following URLs:

Plain download (zip file):
	https://github.com/AlejandroPiad/SciSharp/zipball/master.zip

Plain download (tar file):
	https://github.com/AlejandroPiad/SciSharp/tarball/master.tar.gz
	
Clone with Git (read-only):
	git clone https://github.com/AlejandroPiad/SciSharp.git

Alternative clone locations:
	http://apiad.net/~apiad/SciSharp/
	
To build the project you will need a C# 4.0 compatible
compiler. On Windows you can use Visual Studio 2012,
SharpDevelop, or any other compatible IDE.
On Linux we recommend using MonoDevelop.

The branch <master> is always in a mostly stable status.
Altough it may of course contain bugs, features are only
commited to <master> after a thorought testing. On the
other hand, to try the latest features, checkout the
<develop> branch, where features are commited as they are
finished, before enough testing has been done. This is a 
mostly unstable branch, so beware. Features in a development
status (not completed) generally have their own branch,
named <feature-#issue> where #issue correspond to an 
issue number on the bug tracking list. If you want to try
these features, or colaborate in them, checkout the
corresponding branch.

	
Colaboration
------------
	
For colaboration, bug reporting, and updates, please visit
https://github.com/AlejandroPiad/SciSharp/

You can contact the project manager on <scisharp@apiad.net>
