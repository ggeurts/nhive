/*
 Copyright (c) 2003-2006 Niels Kokholm and Peter Sestoft
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
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
*/


using System;
using System.Diagnostics;
using System.Text;
using SCG = System.Collections.Generic;

namespace C5
{
    [Serializable]
    public struct Multiplicity<T> : IEquatable<Multiplicity<T>>, IShowable
    {
        private T _value;
        private int _count;

        public Multiplicity(T value, int count)
        {
            this._value = value;
            this._count = count;
        }

        public Multiplicity(T value)
        {
            _value = value;
            _count = 0;
        }

        public T Value
        {
            get { return _value; }
            internal set { _value = value; }
        }

        public int Count
        {
            get { return _count; }
            internal set { _count = value; }
        }

        /// <summary>
        /// Pretty print an entry
        /// </summary>
        /// <returns>(key, value)</returns>
        [Tested]
        public override string ToString()
        {
            return "(" + this._value + ", " + this._count + ")";
        }

        /// <summary>
        /// Check equality of entries. 
        /// </summary>
        /// <param name="obj">The other object</param>
        /// <returns>True if obj is an entry of the same type and has the same key and value</returns>
        [Tested]
        public override bool Equals(object obj)
        {
            if (!(obj is Multiplicity<T>))
            {
                return false;
            }
            Multiplicity<T> other = (Multiplicity<T>) obj;
            return Equals(other);
        }

        /// <summary>
        /// Get the hash code of the pair.
        /// </summary>
        /// <returns>The hash code</returns>
        [Tested]
        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(this._value)
                + 13984681 * this._count;
        }

        public bool Equals(Multiplicity<T> other)
        {
            return EqualityComparer<T>.Default.Equals(this._value, other._value)
                && this._count == other._count;
        }

        public static bool operator ==(Multiplicity<T> pair1, Multiplicity<T> pair2)
        {
            return pair1.Equals(pair2);
        }

        public static bool operator !=(Multiplicity<T> pair1, Multiplicity<T> pair2)
        {
            return !pair1.Equals(pair2);
        }

        #region IShowable Members

        public bool Show(StringBuilder sb, ref int rest, IFormatProvider formatProvider)
        {
            if (rest < 0)
                return false;
            if (!Showing.Show(_value, sb, ref rest, formatProvider))
                return false;
            sb.Append(" => ");
            rest -= 4;
            if (!Showing.Show(_count, sb, ref rest, formatProvider))
                return false;
            return rest >= 0;
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Showing.ShowString(this, format, formatProvider);
        }

        #endregion
    }

    /// <summary>
    /// Default equalityComparer for multiplicities.
    /// Operations only look at values and uses an externaly defined equalityComparer for that.
    /// </summary>
    [Serializable]
    internal sealed class MultiplicityEqualityComparer<T> : SCG.IEqualityComparer<Multiplicity<T>>
    {
        SCG.IEqualityComparer<T> _valueEqualityComparer;

        #region Constructors

        /// <summary>
        /// Create an entry equalityComparer using the default equalityComparer for values
        /// </summary>
        public MultiplicityEqualityComparer()
        {
            _valueEqualityComparer = EqualityComparer<T>.Default;
        }

        /// <summary>
        /// Create an entry equalityComparer from a specified item equalityComparer for the values
        /// </summary>
        public MultiplicityEqualityComparer(SCG.IEqualityComparer<T> valueEqualityComparer)
        {
            if (valueEqualityComparer == null)
            {
                throw new ArgumentNullException("valueEqualityComparer");
            }
            _valueEqualityComparer = valueEqualityComparer;
        }

        #endregion

        #region IEqualityComparer<SCG.KeyValuePair<K,V>> Members

        /// <summary>
        /// Test two entries for equality
        /// </summary>
        /// <param name="x">First entry</param>
        /// <param name="y">Second entry</param>
        /// <returns>True if entry values are equal</returns>
        public bool Equals(Multiplicity<T> x, Multiplicity<T> y)
        {
            return _valueEqualityComparer.Equals(x.Value, y.Value);
        }

        /// <summary>
        /// Get the hash code of the entry
        /// </summary>
        /// <param name="obj">The entry</param>
        /// <returns>The hash code of the entry value</returns>
        public int GetHashCode(Multiplicity<T> obj)
        {
            return _valueEqualityComparer.GetHashCode(obj.Value);
        }

        #endregion
    }
}
