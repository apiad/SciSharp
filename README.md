SciSharp - Scientific Computing for .NET
========================================

SciSharp is a set of libraries and tools for .NET 4 tailored for
scientific computing. It includes one main library (`SciSharp.dll`)
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

> <http://apiad.net/~apiad/git/SciSharp/Bin/Libraries> 

Third party libraries and software can be obtained in compiled form from:

> <http://apiad.net/~apiad/git/SciSharp/Contrib/>

All third party software is subject to its own license.
We only include third party contributions with an OSI approved
license.

			   
Source Code
-----------

Source is available under the MIT License on the following URLs:

* [Plain download (zip file)](https://github.com/AlejandroPiad/SciSharp/zipball/master.zip)
* [Plain download (tar file)](https://github.com/AlejandroPiad/SciSharp/tarball/master.tar.gz)
* [Clone address with Git (read-only)](https://github.com/AlejandroPiad/SciSharp.git)
* [Alternative clone location](http://apiad.net/~apiad/git/SciSharp/)

To build the project you will need a C# 4.0 compatible
compiler. On Windows you can use Visual Studio 2012,
SharpDevelop, or any other compatible IDE.
On Linux we recommend using MonoDevelop.

The branch `master` is always in a mostly stable status.
Although it may of course contain bugs, features are only
committed to `master` after a thorough testing. On the
other hand, to try the latest features, checkout the
`develop` branch, where features are committed as they are
finished, before enough testing has been done. This is a 
mostly unstable branch, so beware. Features in a development
status (not completed) generally have their own branch,
named `feature-xyz` with a sensible description of
the feature being implemented. If you want to try
these features, or collaborate in them, checkout the
corresponding branch.

	
Collaboration
-------------
	
For collaboration, bug reporting, and updates, please visit 
[the Github repository](https://github.com/AlejandroPiad/SciSharp/).

You can contact the project manager on <scisharp@apiad.net>

You can subscribe the following mailing lists to experience
a wide range of collaboration:

* Big announcements (releases, news, etc.):
[scisharp@lists.apiad.net](mailto:scisharp-subscribe@lists.apiad.net)

    This list is read-only and we'll only send very sparse mails
about major developments news, milestone hits and releases.

* Developers mailing list:
[scisharp-develop@lists.apiad.net](mailto:scisharp-develop-subscribe@lists.apiad.net)

    This list is where the development discussions take place.
If you want to stay up-to-date with the newest features,
and add your voice to the quorum, this is the place. This 
list requires no subscription to send a comment, but
the first mail is always moderated.

* Patches list:
[scisharp-patches@lists.apiad.net](mailto:scisharp-patches@lists.apiad.net)

    Send your patches to this list with a brief explanation
to get them merged into `develop`, if you prefer to bypass
the standard Github `pull-request` method.