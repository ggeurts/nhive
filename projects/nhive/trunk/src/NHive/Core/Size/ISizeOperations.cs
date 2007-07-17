namespace NHive.Base.Size
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Operations that are supported on size arguments of type <typeparamref name="TSize"/>.
    /// </summary>
    /// <typeparam name="TSize">A signed or unsigned ordinal type that is used to measure the number 
    /// of items in a data structure or operation.</typeparam>
    public interface ISizeOperations<TSize> : IComparer<TSize>, IEqualityComparer<TSize>
        where TSize: struct, IConvertible
    {
        #region Predefined constants

        /// <summary>
        /// Returns zero value.
        /// </summary>
        TSize Zero { get; }

        #endregion

        #region Conversion operations

        /// <summary>
        /// Converts integer value to <typeparamref name="TSize"/> instance.
        /// </summary>
        /// <param name="x">The value to be converted.</param>
        /// <returns>The convertsion result.</returns>
        TSize From(int x);

        /// <summary>
        /// Converts long value to <typeparamref name="TSize"/> instance.
        /// </summary>
        /// <param name="x">The value to be converted.</param>
        /// <returns>The convertsion result.</returns>
        TSize FromInt64(long x);

        /// <summary>
        /// Converts instance of integral type <typeparamref name="T"/> to <typeparamref name="TSize"/> instance.
        /// </summary>
        /// <typeparam name="T">The source type</typeparam>
        /// <param name="x">The value to be converted.</param>
        /// <returns>The convertsion result.</returns>
        TSize From<T>(T x) where T : struct, IConvertible;

        /// <summary>
        /// Converts <typeparamref name="TSize"/> value to integer value.
        /// </summary>
        /// <param name="x">The value to be converted.</param>
        /// <returns>The conversion result.</returns>
        int ToInt32(TSize x);

        /// <summary>
        /// Converts <typeparamref name="TSize"/> value to long value.
        /// </summary>
        /// <param name="x">The value to be converted.</param>
        /// <returns>The conversion result.</returns>
        long ToInt64(TSize x);

        #endregion

        #region Increment/Decrement operations

        /// <summary>
        /// Decrements <typeparamref name="TSize"/> variable with 1.
        /// </summary>
        /// <param name="x">Variable to be decremented.</param>
        /// <remarks>post: x = pre(x) - 1</remarks>
        void Decrement(ref TSize x);

        /// <summary>
        /// Increments <typeparamref name="TSize"/> variable with 1.
        /// </summary>
        /// <param name="x">Variable to be incremented.</param>
        /// <remarks>post: x = pre(x) + 1</remarks>
        void Increment(ref TSize x);

        #endregion

        #region Mathematical operations

        /// <summary>
        /// Adds two <typeparamref name="TSize"/> values.
        /// </summary>
        /// <param name="x">First add operand.</param>
        /// <param name="y">Second add operand.</param>
        /// <returns>Sum of <paramref name="x"/> and <paramref name="y"/>.</returns>
        /// <exception cref="OverflowException">Type <typeparamref name="TSize"/> is too small
        /// to represent the outcome of this mathematical operation</exception>
        TSize Add(TSize x, TSize y);

        /// <summary>
        /// Adds an integer value and a <typeparamref name="TSize"/> value.
        /// </summary>
        /// <param name="x">First add operand.</param>
        /// <param name="y">Second add operand.</param>
        /// <returns>Sum of <paramref name="x"/> and <paramref name="y"/>.</returns>
        /// <exception cref="OverflowException">Type <typeparamref name="TSize"/> is too small
        /// to represent the outcome of this mathematical operation</exception>
        TSize Add(TSize x, int y);

        /// <summary>
        /// Adds <typeparamref name="TSize"/> value to value of <typeparamref name="TSize"/> variable.
        /// </summary>
        /// <param name="x">The <typeparamref name="TSize"/> variable. Contains the addition 
        /// result on call completion.</param>
        /// <param name="y">The value to be added to <paramref name="x"/>.</param>
        /// <returns>Sum of <paramref name="x"/> and <paramref name="y"/>.</returns>
        /// <exception cref="OverflowException">Type <typeparamref name="TSize"/> is too small
        /// to represent the outcome of this mathematical operation</exception>
        /// <remarks>post: x = pre(x) + y</remarks>
        TSize AddWith(ref TSize x, TSize y);

        /// <summary>
        /// Adds integer value to value of <typeparamref name="TSize"/> variable.
        /// </summary>
        /// <param name="x">The <typeparamref name="TSize"/> variable. Contains the addition 
        /// result on call completion.</param>
        /// <param name="y">The value to be added to <paramref name="x"/>.</param>
        /// <returns>Sum of <paramref name="x"/> and <paramref name="y"/>.</returns>
        /// <exception cref="OverflowException">Type <typeparamref name="TSize"/> is too small
        /// to represent the outcome of this mathematical operation</exception>
        /// <remarks>post: x = pre(x) + y</remarks>
        TSize AddWith(ref TSize x, int y);

        /// <summary>
        /// Subtracts <typeparamref name="TSize"/> value from another <typeparamref name="TSize"/> value.
        /// </summary>
        /// <param name="x">First subtraction operand.</param>
        /// <param name="y">Second subtraction operand.</param>
        /// <returns>Returns result of subtraction <paramref name="x"/> - <paramref name="y"/>.</returns>
        /// <exception cref="OverflowException">Type <typeparamref name="TSize"/> is too small
        /// to represent the outcome of this mathematical operation</exception>
        TSize Subtract(TSize x, TSize y);

        /// <summary>
        /// Subtracts integer value from another <typeparamref name="TSize"/> value.
        /// </summary>
        /// <param name="x">First subtraction operand.</param>
        /// <param name="y">Second subtraction operand.</param>
        /// <returns>Returns result of subtraction <paramref name="x"/> - <paramref name="y"/>.</returns>
        /// <exception cref="OverflowException">Type <typeparamref name="TSize"/> is too small
        /// to represent the outcome of this mathematical operation</exception>
        TSize Subtract(TSize x, int y);

        /// <summary>
        /// Subtracts <typeparamref name="TSize"/> value from <typeparamref name="TSize"/> variable.
        /// </summary>
        /// <param name="x">A <typeparamref name="TSize"/> variable. Contains the subtraction 
        /// result on call completion.</param>
        /// <param name="y">The value to be subtracted from <paramref name="x"/>.</param>
        /// <returns>Returns result of subtraction <paramref name="x"/> - <paramref name="y"/>.</returns>
        /// <exception cref="OverflowException">Type <typeparamref name="TSize"/> is too small
        /// to represent the outcome of this mathematical operation</exception>
        /// <remarks>post: x = pre(x) - y</remarks>
        TSize SubtractWith(ref TSize x, TSize y);

        /// <summary>
        /// Subtracts integer value from <typeparamref name="TSize"/> variable.
        /// </summary>
        /// <param name="x">A <typeparamref name="TSize"/> variable. Contains the subtraction 
        /// result on call completion.</param>
        /// <param name="y">The value to be subtracted from <paramref name="x"/>.</param>
        /// <returns>Returns result of subtraction <paramref name="x"/> - <paramref name="y"/>.</returns>
        /// <exception cref="OverflowException">Type <typeparamref name="TSize"/> is too small
        /// to represent the outcome of this mathematical operation</exception>
        /// <remarks>post: x = pre(x) - y</remarks>
        TSize SubtractWith(ref TSize x, int y);

        /// <summary>
        /// Multiplies two <typeparamref name="TSize"/> values.
        /// </summary>
        /// <param name="x">First multiplication operand.</param>
        /// <param name="y">Second multiplication operand.</param>
        /// <returns>Product of <paramref name="x"/> and <paramref name="y"/>.</returns>
        /// <exception cref="OverflowException">Type <typeparamref name="TSize"/> is too small
        /// to represent the outcome of this mathematical operation</exception>
        TSize Multiply(TSize x, TSize y);

        /// <summary>
        /// Multiplies a <typeparamref name="TSize"/> value with an integer value.
        /// </summary>
        /// <param name="x">First multiplication operand.</param>
        /// <param name="y">Second multiplication operand.</param>
        /// <returns>Product of <paramref name="x"/> and <paramref name="y"/>.</returns>
        /// <exception cref="OverflowException">Type <typeparamref name="TSize"/> is too small
        /// to represent the outcome of this mathematical operation</exception>
        TSize Multiply(TSize x, int y);

        /// <summary>
        /// Multiplies a <typeparamref name="TSize"/> variable value with a <typeparamref name="TSize"/> value.
        /// </summary>
        /// <param name="x">The <typeparamref name="TSize"/> variable. Contains the multiplication
        /// result on call completion.</param>
        /// <param name="y">The multiplier value.</param>
        /// <returns>Product of <paramref name="x"/> and <paramref name="y"/>.</returns>
        /// <exception cref="OverflowException">Type <typeparamref name="TSize"/> is too small
        /// to represent the outcome of this mathematical operation</exception>
        /// <remarks>post: x = pre(x) * y</remarks>
        TSize MultiplyWith(ref TSize x, TSize y);

        /// <summary>
        /// Multiplies a <typeparamref name="TSize"/> variable value with an integer value.
        /// </summary>
        /// <param name="x">The <typeparamref name="TSize"/> variable. Contains the multiplication
        /// result on call completion.</param>
        /// <param name="y">The multiplier value.</param>
        /// <returns>Product of <paramref name="x"/> and <paramref name="y"/>.</returns>
        /// <exception cref="OverflowException">Type <typeparamref name="TSize"/> is too small
        /// to represent the outcome of this mathematical operation</exception>
        /// <remarks>post: x = pre(x) * y</remarks>
        TSize MultiplyWith(ref TSize x, int y);

        /// <summary>
        /// Divides <typeparamref name="TSize"/> value with another <typeparamref name="TSize"/> value.
        /// </summary>
        /// <param name="x">The operand to divide.</param>
        /// <param name="y">The operand to divide with.</param>
        /// <returns>Returns result of division <paramref name="x"/> / <paramref name="y"/>.</returns>
        /// <exception cref="DivideByZeroException">Cannot divide by zero.</exception>
        TSize Divide(TSize x, TSize y);

        /// <summary>
        /// Divides <typeparamref name="TSize"/> value with an integer value.
        /// </summary>
        /// <param name="x">The operand to divide.</param>
        /// <param name="y">The operand to divide with.</param>
        /// <returns>Result of division <paramref name="x"/> / <paramref name="y"/>.</returns>
        /// <exception cref="DivideByZeroException">Cannot divide by zero.</exception>
        TSize Divide(TSize x, int y);

        /// <summary>
        /// Divides a <typeparamref name="TSize"/> variable value by a <typeparamref name="TSize"/> value.
        /// </summary>
        /// <param name="x">The <typeparamref name="TSize"/> variable. Contains the division
        /// result on call completion.</param>
        /// <param name="y">The value to divide with.</param>
        /// <returns>Result of division <paramref name="x"/> / <paramref name="y"/>.</returns>
        /// <exception cref="DivideByZeroException">Cannot divide by zero.</exception>
        /// <remarks>post: x = pre(x) / y</remarks>
        TSize DivideWith(ref TSize x, TSize y);

        /// <summary>
        /// Divides a <typeparamref name="TSize"/> variable value by an integer value.
        /// </summary>
        /// <param name="x">The <typeparamref name="TSize"/> variable. Contains the division
        /// result on call completion.</param>
        /// <param name="y">The value to divide with.</param>
        /// <returns>Result of division <paramref name="x"/> / <paramref name="y"/>.</returns>
        /// <exception cref="DivideByZeroException">Cannot divide by zero.</exception>
        /// <remarks>post: x = pre(x) / y</remarks>
        TSize DivideWith(ref TSize x, int y);

        #endregion

        #region Array operations

        /// <summary>
        /// Creates array of specified length.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="length">The length of the new array.</param>
        /// <returns>A new array with the specified <paramref name="length"/>.</returns>
        T[] CreateArray<T>(TSize length);

        /// <summary>
        /// Copies range of elements from one array to another array.
        /// </summary>
        /// <typeparam name="T">Array element type of both source and target arrays.</typeparam>
        /// <param name="sourceArray">The array from which elements will be copied. Must not be null.</param>
        /// <param name="sourceBeginIndex">The index into <paramref name="sourceArray"/> of the first 
        /// element to be copied.</param>
        /// <param name="sourceEndIndex">The index into <paramref name="sourceArray"/> of 
        /// the element after the last element to be copied. Must be greater than or equal 
        /// to <paramref name="sourceBeginIndex"/>.
        /// </param>
        /// <param name="targetArray">The array to which elements will be copied. Must not be null.</param>
        /// <param name="targetIndex">The index into <paramref name="targetArray"/> for
        /// the first element to be copied.</param>
        /// <remarks>
        /// pre:  0 &lt;= sourceBeginIndex &lt;= sourceEndIndex &lt;= sourceArray.Length
        /// pre:  0 &lt;= targetIndex &lt;= targetArray.Length - (sourceEndIndex - sourceBeginIndex)
        /// post: forall(i in [0..sourceEndIndex - sourceBeginIndex - 1])
        ///           targetArray[targetIndex + i] = sourceArray[sourceBeginIndex + i]
        /// </remarks>
        void CopyArray<T>(T[] sourceArray, TSize sourceBeginIndex, TSize sourceEndIndex, T[] targetArray, TSize targetIndex);

        /// <summary>
        /// Sets range of array elements to their default values.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="array">The array in which elements will be cleared.</param>
        /// <param name="beginIndex">The index into <paramref name="array"/> of the first 
        /// element to clear.</param>
        /// <param name="endIndex">The index into <paramref name="array"/> of the element after 
        /// the last element to be cleared.</param>
        /// <remarks>
        /// pre:  0 &lt;= beginIndex &lt;= endIndex &lt;= array.Length
        /// post: forall(i in [beginIndex..endIndex])
        ///           array[i] = default(T)
        /// </remarks>
        void ClearArray<T>(T[] array, TSize beginIndex, TSize endIndex);

        /// <summary>
        /// Retrieves array element at specified index.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="array">The array from which an element must be retrieved.</param>
        /// <param name="index">The index into <paramref name="array"/> of the element 
        /// to retrieve.</param>
        /// <returns>The array element at the specified index.</returns>
        /// <remarks>
        /// pre:  0 &lt;= index &lt;= array.Length
        /// </remarks>
        T GetValueFromArray<T>(T[] array, TSize index);

        /// <summary>
        /// Stores value at specified index in an array.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="array">The array in which an element must be stored.</param>
        /// <param name="item">The value to store.</param>
        /// <param name="index">The index into <paramref name="array"/> where the value
        /// must be stored.</param>
        /// <remarks>
        /// pre:  0 &lt;= index &lt;= array.Length
        /// post: array[index] = item
        /// </remarks>
        void SetValueInArray<T>(T[] array, T item, TSize index);

        #endregion
    }
}
