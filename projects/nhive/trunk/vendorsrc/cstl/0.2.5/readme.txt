CSTL
Version 0.2.0
Copyright (c) 2006 by Harold Howe.
hhjunk@mchsi.com
http://sourceforge.net/projects/cstl


TABLE OF CONTENTS
=================
1- Introduction
2- Version history
3- File list
4- Installation
5- Design Philosophy
6- Notes
7- Limitations and bugs
8- Licensing terms


(1) INTRODUCTION
================

CSTL is an port of the C++ STL to C# 2.0. The library aims to maximize the 
beneficial features of C#, such as generics, anonymous methods, and enumerable 
iterators, while alleviating some of its deficiencies. Namely, the lack of C++
style iterators, templates, and complete operator overloading.

CSTL requires .NET 2.0. The current distribution requires Visual Studio 2005 to
build, although it may be possible to build from the command line using 
MSBuild. I have not tried to test Mono.

When the library is more complete, I will try to improve the build system. 
Requiring Visual Studio is lame.

(2) VERSION HISTORY
===================

0.2.0 (Pre-alpha): Posted on 05/23/2006
0.2.1 (Pre-alpha): Posted on 09/26/2006
0.2.2 (Pre-alpha): Posted on 09/27/2006
0.2.3 (Pre-alpha): Posted on 09/28/2006
0.2.4 (Pre-alpha): Posted on 10/02/2006
0.2.5 (Pre-alpha): Posted on 11/13/2006


(3) FILE LIST
=============

./readme.txt  : this file
./src/*       : CSTL source code
./lib/*       : Binaries that CSTL needs to build. Namely, NUnit.
./test/*      : NUnit unit tests.


(4) INSTALLATION
================

(1) Extract the contents of this archive to a directory (c:\lib\CSTL)

(2) Open CSTL.sln in Visual Studio 2005 and perform a build.

(3) Currently, I am only doing debug builds. Copy bin\debug\cstl.dll to a 
    suitable location, and link to it from your apps. 


(5) Design Philosophy
=====================

CSTL is very young, so its overall design is still evolving. In general, the 
library follows these core ideas:

1- That the C++ STL is simply awesome. The STL's belief that algorithms and 
collections should be separate entities is the best way to design a flexible,
generic library, and that iterators are the key to making that separation work.

2- That any .NET version of the STL should strive to be as powerful as the
original, while remaining easy to use by developers that are not STL pros.

3- That a C# STL should leverage the good features of C#, such as delegates,
generics, anonymous methods, etc.

4- That a C# STL should minimize the areas where C# is inferior to C++: such 
as the lack of C++ style iterators, templates, and complete operator 
overloading.

5- That a C# STL should be usable from any .NET language.

6- That C++ developers should take a back seat if necessary, since they already
have something better available to them.

Using these ideas, I have started to piece together a library that, while not
quite as powerful as the C++ STL, still manages to add significant value to 
the .NET framework. Here is how things map from the C++ to CSTL.

1- Function objects are implemented using delegates. The need to write a class
that implements operator () simply goes away completely. Which is good, because 
C# doesn't allow you to do that anyway. Furthermore, I envision that many 
small function delegates will be implemented with anonymous methods.

2- C# and .NET don't have a solid replacement for the C++ iterator. The closest
thing would be IEnumerator<T> and enumerable collections. However, enumerators
are not as full featured as C++ iterators. They essentially can only do what an 
input iterator in C++ can do. That is, you can read an enumerator, and you can 
advance it to the next item, but that is pretty much it. Well, I guess you can 
reset it, but that is rarely useful.

Enumerators do not allow you to write to them. The features of bidirectional
iterators and random access iterators are also missing. And you can't compare
to enumerators to see if they point to the same element.

This is a real bummer becuase one of the neat new features of C# 2.0 is the idea
of iterators. These are methods that can use a new "yield return" statement to 
build an enumerable collection. It is unfortunate for us that Microsoft chose to
name these things iterators. For the sake of clarity, I will call them
enumerable iterators.

So we need an iterator concept that is more powerful than C# enumerators.
Furthermore, we have to implement this concept using C# language features. No 
templates and no operator overloading of pointer operands. 

My solution is to map the C++ iterator categories into C# using interfaces.
Interfaces are the best C# answer to how iterators are handled inside the C++
STL. Interfaces enforce a type contract, while giving us relatively loose 
coupling.

The iterator hierarchy looks like this:

  InputIterator      OutputIterator
       |__________________|
                |
          ForwardIterator
                |
       BidirectionalIterator 
                |
        RandomAccessIterator
        
Instead of operators *, ->, ++ and so on, we have to use fully qualified names,
like MoveNext, Read, and so on.

3- Collections are less of a problem than iterators. The only hangup is that we
would like our collections to have methods for creating an iterator into the 
collection. The collections in CSTL have Begin and End methods. The trickier
problem is creating iterators for built in .NET collections.

To deal with this issue, CSTL contains iterator classes for wrapping .NET 
collections. Well...the current version has one such class anyway: 
ListIterator<T>. This class can iterate over any IList<T> collection, which 
includes arrays and List<T>. The utility class IteratorUtil has methods for 
creating iterators for a given IList.

int[] x = new int[100];
ListIterator<int> begin = IteratorUtil.Begin(x);

4- With a concept of iterators in place, the stage is set for writing
algorithms. Most C++ algorithms map over directly. There are some issues to
resolve, but for the most part, implementing a C# version of a C++ algorithm is
straightforward.

One of our design goals is to keep CSTL simple to use by developers who are 
unfamiliar with C++. To achieve this goal, CSTL offers more overloaded versions
of each algorithm. There is generally a version that works with iterators, and
a version that works with IEnumerable<T>. For example, here are the prototypes
for the Copy algorithm:

// Iterator version
void Copy<T>(InputIterator<T> begin, InputIterator<T> end, OutputIterator<T> target);

// IEnumerable<T> as the source, iterator as the target
void Copy<T>(IEnumerable<T> enumerable, OutputIterator<T> target);

// Iterator as the source, IList<T> as the target
void Copy<T>(InputIterator<T> begin, InputIterator<T> end, IList<T> target);

// IEnumerable<T> as the source, IList<T> as the target
// This will probably be the most frequently called method.
void Copy<T>(IEnumerable<T> enumerable, IList<T> target);

Note that IEnumerable<T> and IEnumerator<T> do not allow us to write elements. 
Neither does ICollection<T>. It can add, but not overwrite. Copy needs to 
overwrite. As such, the target must be an IList<T>.

These four sets of overloads give the user quite a bit of flexibility. You can 
pass Copy a set of iterators if you have them. If you don't, that is fine, just
pass an enumerable collection. And remember, the C# 2.0 feature of enumerable
iterators allows the user to build that collection on the fly.

public IEnumerable<int> MySpecialValues()
{
   yield return 1;
   yield return 2;
   for(int i=0; i<100; ++i)
    yield return i;
}

...
List<int> output = new List<int>();
Algorithm.Copy(MySpecialValues(), IteratorUtil.BackInserter(output));


(6) NOTES
=========

CSTL is currently in a pre-alpha stage. It is not ready for use in a production
environment. 

Currently, CSTL is about 30% complete. The most complete section is the
algorithm portion of the library. About 80% of the C++ STL algorithms have
been incorporated into CSTL. The collections portion of the library is 
the least complete. Only one collection, VectorList is currently available.
The functional and iterator sections are mostly complete. 

Here is a list of what is present:

--------------------------------------------------------------------------------
Algorithms:
  Modifying               NonModifying       Removing          Mutating
    Copy                    AdjacentFind       Remove            Reverse
    CopyBackward            Count              RemoveCopy        ReverseCopy
    CopyN (extension)       CountIf            RemoveCopyIf      Rotate
    Fill                    Equal              RemoveIf          RotateCopy
    FillN                   Find               Unique            NextPermutation
    Generate                FindEnd            UniqueCopy        PrevPermutation
    GenerateN               FindIf                             
    Merge                   ForEach                            Numeric
    Replace                 MaxElement                           Accumulate
    ReplaceCopy             MinElement                          
    ReplaceCopyIf           Mismatch                           Sorting
    ReplaceIf               Search                               Sort 
    SwapRanges              SearchN                              StableSort *** 
    Transform               LexCompare                           Partition
                                                                 NthElement

Iterators
  BackInsertIterator
  InsertIterator (FrontInserter implemented as a special case)
  ListIterator (Iterator wrapper for any IList<T>
  Utility methods
    Advance
    Distance
    Swap
    CreateEnumerator (creates an IEnumerable from an iterator range)
    IteratorUtil (misc utils)

Collections 
  VectorList

Functional
  Mostly complete.
  
*** StableSort is currently a non-stable stub. The implementation still needs 
to be worked on.
------------------------------------------------------------------------------

The following algorithms need to be implemented:
    StablePartition, some of the sorting algorithms, and all of those heap 
    algorithms that I never really understood. There may be some others that I
    missed.

At a minimum, the following collections need to be implemented:
    Vector, List (may be renamed), Deque, Map, Set.

STL Functional is fully implemented. Note that some of the C++ functions and 
classes are not necessary in CSTL (mem_fun and prt_fun). I have not implemented
any form of compose. I don't know if it is necessary.

Eventually, the CSTL assembly will be strongly named and GAC-able. But for now,
that would just get in the way. Beware that until we reach a stable version,
0.9.X or so, breaking changes may be required.


(7) LIMITATIONS AND BUGS
========================

(7.1) Some of the algorithms that should return an iterator are currently 
returning void

(7.2) The unit tests lag the development of the the core library somewhat. As
such, not everything has been tested adequately.


(8) LICENSING TERMS
===================

BSD license (the one without the advertising clause).


