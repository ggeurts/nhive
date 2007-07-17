namespace NHive.UnitTests.SizeOperations
{
    using System;
    using MbUnit.Framework;
    using NHive.Base.Size;

    [TestFixture]
    public class Int64OperationsTests
    {
        private Int64Operations Size = new Int64Operations();

        #region Predefined constant tests

        [Test]
        public void Zero()
        {
            Assert.AreEqual(0, Size.Zero);
        }

        #endregion

        #region Conversion tests

        [Test]
        public void ConvertToInt64()
        {
            Assert.AreEqual(long.MaxValue, Size.ToInt64(long.MaxValue));
        }

        [Test]
        public void ConvertFromInt64()
        {
            Assert.AreEqual(long.MinValue, Size.From(long.MinValue));
        }

        [Test]
        public void ConvertToInt32()
        {
            Assert.AreEqual(int.MaxValue, Size.ToInt64((long) int.MaxValue));
        }

        [Test]
        public void ConvertFromInt32()
        {
            Assert.AreEqual(int.MinValue, Size.From(int.MinValue));
        }

        #endregion

        #region Increment/Decrement tests

        [Test]
        public void Decrement()
        {
            long x = 1;
            Size.Decrement(ref x);
            Assert.AreEqual(0, x);
        }

        [Test]
        public void Increment()
        {
            long x = 1;
            Size.Increment(ref x);
            Assert.AreEqual(2, x);
        }

        #endregion

        #region Mathematical tests

        [Test]
        public void Add()
        {
            Assert.AreEqual(10, Size.Add(4, 6));
        }

        [Test, ExpectedException(typeof(OverflowException))]
        public void AddThrowsOverflowExceptionIfResultExceedsSizeLimits()
        {
            Size.Add(long.MaxValue, 1);
        }

        [Test]
        public void AddWith()
        {
            long x = 4;
            Assert.AreEqual(10, Size.AddWith(ref x, 6), "AddWith()");
            Assert.AreEqual(10, x, "x");
        }

        [Test, ExpectedException(typeof(OverflowException))]
        public void AddWithThrowsOverflowExceptionIfResultExceedsSizeLimits()
        {
            long x = long.MaxValue;
            Size.AddWith(ref x, 1);
        }

        [Test]
        public void Subtract()
        {
            Assert.AreEqual(4, Size.Subtract(10, 6));
        }

        [Test, ExpectedException(typeof(OverflowException))]
        public void SubtractThrowsOverflowExceptionIfResultExceedsSizeLimits()
        {
            Size.Subtract(long.MinValue, 1);
        }

        [Test]
        public void SubtractWith()
        {
            long x = 10;
            Assert.AreEqual(4, Size.SubtractWith(ref x, 6), "SubtractWith()");
            Assert.AreEqual(4, x, "x");
        }

        [Test, ExpectedException(typeof(OverflowException))]
        public void SubtractWithThrowsOverflowExceptionIfResultExceedsSizeLimits()
        {
            long x = long.MinValue;
            Size.SubtractWith(ref x, 1);
        }

        [Test]
        public void Multiply()
        {
            Assert.AreEqual(24, Size.Multiply(4, 6));
        }

        [Test, ExpectedException(typeof(OverflowException))]
        public void MultiplyThrowsOverflowExceptionIfResultExceedsSizeLimits()
        {
            Size.Multiply(long.MaxValue, 6);
        }

        [Test]
        public void MultiplyWith()
        {
            long x = 4;
            Assert.AreEqual(24, Size.MultiplyWith(ref x, 6), "MultiplyWith()");
            Assert.AreEqual(24, x, "x");
        }

        [Test, ExpectedException(typeof(OverflowException))]
        public void MultiplyWithThrowsOverflowExceptionIfResultExceedsSizeLimits()
        {
            long x = long.MaxValue;
            Size.MultiplyWith(ref x, 6);
        }

        [Test]
        public void Divide()
        {
            Assert.AreEqual(4, Size.Divide(24, 6));
        }

        [Test, ExpectedException(typeof(DivideByZeroException))]
        public void DivideThrowsDivideByZeroExceptionOnDivisionByZero()
        {
            Size.Divide(4, 0);
        }

        [Test]
        public void DivideWith()
        {
            long x = 24;
            Assert.AreEqual(4, Size.DivideWith(ref x, 6), "DivideWith()");
            Assert.AreEqual(4, x, "x");
        }

        [Test, ExpectedException(typeof(DivideByZeroException))]
        public void DivideWithThrowsDivideByZeroExceptionOnDivisionByZero()
        {
            long x = 4;
            Size.DivideWith(ref x, 0);
        }

        #endregion

        #region Array tests

        [Test]
        public void CreateArrayOfSpecifiedLength()
        {
            string[] array = Size.CreateArray<string>(10);
            Assert.AreEqual(10, array.Length);
        }

        [Test]
        public void ClearArrayBegin()
        {
            string[] array = new string[] { "This", "is", "some", "imaginitive", "writing!" };
            Size.ClearArray(array, 0, 2);
            CollectionAssert.AreEqual(
                new string[] { null, null, "some", "imaginitive", "writing!" },
                array);
        }

        [Test]
        public void ClearArrayEnd()
        {
            string[] array = new string[] { "This", "is", "some", "imaginitive", "writing!" };
            Size.ClearArray(array, 3, 5);
            CollectionAssert.AreEqual(
                new string[] { "This", "is", "some", null, null },
                array);
        }

        [Test]
        public void ClearArrayMiddle()
        {
            string[] array = new string[] { "This", "is", "some", "imaginitive", "writing!" };
            Size.ClearArray(array, 2, 3);
            CollectionAssert.AreEqual(
                new string[] { "This", "is", null, "imaginitive", "writing!" },
                array);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ClearArrayThrowsArgumentOutOfRangeExceptionIfBeginIndexIsLessThanZero()
        {
            Size.ClearArray(new string[4], -1, 2);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ClearArrayThrowsArgumentOutOfRangeExceptionIfEndIndexIsLessThanBeginIndex()
        {
            Size.ClearArray(new string[4], 3, 2);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ClearArrayThrowsArgumentOutOfRangeExceptionIfEndIndexIsGreaterThanArrayLength()
        {
            Size.ClearArray(new string[4], 3, 5);
        }

        [Test]
        public void CopyArrayBegin()
        {
            string[] sourceArray = new string[] { "This", "is", "some", "imaginitive", "writing!" };
            string[] targetArray = new string[3];
            Size.CopyArray(sourceArray, 0, 2, targetArray, 0);
            CollectionAssert.AreEqual(
                new string[] { "This", "is", null },
                targetArray);
        }

        [Test]
        public void CopyArrayEnd()
        {
            string[] sourceArray = new string[] { "This", "is", "some", "imaginitive", "writing!" };
            string[] targetArray = new string[3];
            Size.CopyArray(sourceArray, 2, 5, targetArray, 0);
            CollectionAssert.AreEqual(
                new string[] { "some", "imaginitive", "writing!" },
                targetArray);
        }

        [Test]
        public void CopyArrayMiddle()
        {
            string[] sourceArray = new string[] { "This", "is", "some", "imaginitive", "writing!" };
            string[] targetArray = new string[3];
            Size.CopyArray(sourceArray, 2, 4, targetArray, 0);
            CollectionAssert.AreEqual(
                new string[] { "some", "imaginitive", null },
                targetArray);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CopyArrayThrowsArgumentOutOfRangeExceptionIfSourceBeginIndexIsLessThanZero()
        {
            Size.CopyArray(new string[4], -1, 2, new string[3], 0);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CopyArrayThrowsArgumentOutOfRangeExceptionIfSourceEndIndexIsLessThanSourceBeginIndex()
        {
            Size.CopyArray(new string[4], 3, 2, new string[2], 0);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CopyArrayThrowsArgumentOutOfRangeExceptionIfSourceEndIndexIsGreaterThanArrayLength()
        {
            Size.CopyArray(new string[4], 3, 5, new string[2], 0);
        }

        [Test]
        public void GetValueFromArray()
        {
            string[] array = new string[] { "This", "is", "some", "text" };
            Assert.AreEqual("This", Size.GetValueFromArray(array, 0), "First element");
            Assert.AreEqual("text", Size.GetValueFromArray(array, 3), "Last element");
            Assert.AreEqual("is", Size.GetValueFromArray(array, 1), "Element in middle");
        }

        [Test, ExpectedException(typeof(IndexOutOfRangeException))]
        public void GetValueFromArrayThrowsIndexOutOfRangeExceptionIfIndexIsGreaterThanOrEqualToLength()
        {
            Size.GetValueFromArray(new string[4], 4);
        }

        [Test, ExpectedException(typeof(IndexOutOfRangeException))]
        public void GetValueFromArrayThrowsIndexOutOfRangeExceptionIfIndexIsLessThanZero()
        {
            Size.GetValueFromArray(new string[4], -1);
        }

        [Test]
        public void SetValueInArray()
        {
            string[] array = new string[4];
            Size.SetValueInArray(array, "This", 0);
            Size.SetValueInArray(array, "is", 1);
            Size.SetValueInArray(array, "text", 3);
            CollectionAssert.AreEqual(
                new string[] { "This", "is", null, "text" },
                array);
        }

        [Test, ExpectedException(typeof(IndexOutOfRangeException))]
        public void SetValueInArrayThrowsIndexOutOfRangeExceptionIfIndexIsGreaterThanOrEqualToLength()
        {
            Size.SetValueInArray(new string[4], "bla", 4);
        }

        [Test, ExpectedException(typeof(IndexOutOfRangeException))]
        public void SetValueInArrayThrowsIndexOutOfRangeExceptionIfIndexIsLessThanZero()
        {
            Size.SetValueInArray(new string[4], "blurb", -1);
        }

        #endregion
    }
}
