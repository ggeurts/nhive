# Vision #
The NHive Collection library will be a comprehensive set of generic .NET collections and related algorithms with extension facilities to provide additional servics such as events and views.

# Motivation #
The main reason to start a new collection project is the lack of a .NET collection library that:
  * Provides a comprehensive set of generic .NET collections;
  * Supports the .NET collection interfaces as much as possible;
  * Has a liberal license, which for example allows use with Mono.

# Approach #
The NHive collection library will use the C5 Collections as a starting point. We like the clean design and features of this library, but believe that the concrete realisation of both can be improved and made more "palatable" to main-stream .NET developers.

## C5 collections refactoring ##
A summary of what we consider doing with the C5 Collections:
  * Refactor the current codebase
    * Split the 3 files that are compiled twice using different conditional defines into individual files to benefit from refactoring support (by VS.NET 2005 for example).
    * Concentrate common functionality into base classes
  * Separate essential and non-essential functionality. Create core collection implementations that can be extended/decorated with additional functionality:
    * Views/windows over collections
    * Events
    * Algorithms for searching, sorting, etc.
  * Support relevent .NET collection interface (IList, ICollection, IDictionary, IList

&lt;T&gt;

, ICollection

&lt;T&gt;

, IDictionary

&lt;T&gt;

)
  * Consider support for STL-like iterators and associated algorithms (see [CSTL](http://sourceforge.net/projects/cstl) project on sourceforge)
  * Add new collection types (skip lists for example)

## Design principles ##
We would like to maintain the interface centred design of the C5 collections, but suspect that the existing interfaces can be simplified. The implication is that backward compatibility will not be a design goal. Or to be more truthful, we expect the result to deviate substantially from the current C5 implementation, though the core collection implementations and the overall functionality should still be present in the final result.

## Coding guidelines ##
When it comes to design and coding guidelines we would consider using Microsoft's .NET framework design guidelines and borrowing [Peter Golde's naming conventions for PowerCollections](http://www.wintellect.com/cs/blogs/pgolde/archive/2005/06/08/nomenclature-and-new-methods-in-algorithms.aspx).