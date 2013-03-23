SciSharp - User Manual
======================

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


Tutorials
---------

Check out the next tutorials to quickly grasp the basic ideas behind
SciSharp.


Examples
--------

The following are example applications developed using the SciSharp
tools. Unlike tutorials, examples don't show the coding process 
step by step, but provide a wider context.

### Language Processing ###

* [Expressions Grammar](./Examples/ExpressionsGrammar.html)
    > Definition of a grammar for parsing arithmetic expressions.

### Simulation ###

* [N Servers Parallel Simulator](./Examples/ParallelServerSimulator.html)
    > Building a simulator for the classical n-servers in parallel
    > simulation problem.


All examples source code is located in the `Examples/` in the root
of the SciSharp distribution folder you should have downloaded. If such
folder is empty, you can build the project, and it will populate the
examples folder. In any case, you can check out inside the `Source/Examples/`
folder for the original source code for all the examples.


Reference
---------

The reference manual for the library and tools contains a brief 
description of most of the modules and classes in the main library.
It is automatically generated in Markdown format in the build process.
It can be automatically converted into HTML using the building tools
for SciSharp. Once compiled, you can browse it 
[here](../Reference/index.html).

Book
----

We are writing a big book about everything you can do with SciSharp, but
it is still in a very initial state. 
Please [check it out](../Book/SciSharp.pdf) if you'd like to
provide some feedback. (You may need to compile the documentation 
with LaTeX).