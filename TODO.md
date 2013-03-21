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


To-do list
----------

This is a non-comprehensive list of milestones, ideas, and general
changes. This list will **not** include bug fixes and individual
features, since these change very quickly. For those, visit the
bug tracking section at [Github](https://github.com/AlejandroPiad/SciSharp).

This list includes long term ideas that we should strive for, as
a general guide to both users and developers on where is SciSharp
going. Please only add items to this list after a conscious discussion
in the [discussion list](https://lists.apiad.net/scisharp).
Some of the items of this list will eventually become individual
features that will be tracked down on the bugs list, and removed
or rewritten from (in) this list.

* Collections
 * Unified interfaces design
 * Graph representations
 * B-Tree variants
 * Heaps
* Statistics and probabilities
 * Redesign the `RandomEx` class to separate random generation
   from the different distributions
 * Quasi-random generators
 * Other pseudo-random generators (besides `System.Random`)
 * Standard hypothesis tests
 * Classic probability framework
* Time-based simulation framework
* Optimization
 * Heuristics framework
 * Quasi-Newton methods
 * Linear programming methods (simplex, branch-and-x)
* Language processing
 * Natural language framework (preprocessing, feature extraction)
 * Other parser generators (LL, GLR, SLR)
 * Language generation tools (natural and formal)
 * Grammar transformation and validation tools
* Machine learning
 * Classification algorithms
 * Clustering algorithms
 * Regression algorithms
* Alternative algebraic operation implementations
 * GPU implementation (different frameworks: DX, OpenCL, CUDA)
 * TPL implementation
 * Sparse structures
* Documentation
 * User manual
 * Developer documentation
 * Tutorials and examples
* Automation
 * Automated and unified build for the framework and documentation
 * Unit testing for the entire library
 * Profiling (performance) testing for the entire library