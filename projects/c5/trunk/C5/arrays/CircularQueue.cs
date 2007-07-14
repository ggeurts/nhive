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
using SCG = System.Collections.Generic;
namespace C5
{
  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class CircularQueue<T> : SequencedBase<T>, IQueue<T>, IStack<T>
  {
    #region Fields
    /*
        Invariant: the itemes in the queue ar the elements from front upwards, 
        possibly wrapping around at the end of array, to back.

        if front<=back then size = back - front + 1;
        else size = array.Length + back - front + 1;

        */
    int front, back;
    /// <summary>
    /// The internal container array is doubled when necessary, but never shrinked.
    /// </summary>
    T[] array;
    bool forwards = true, original = true;
    #endregion

    #region Events

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public override EventTypeEnum ListenableEvents { get { return EventTypeEnum.Basic; } }

    #endregion

    #region Util
    void expand()
    {
      int newlength = 2 * array.Length;
      T[] newarray = new T[newlength];

      if (front <= back)
        Array.Copy(array, front, newarray, 0, Size);
      else
      {
        int half = array.Length - front;
        Array.Copy(array, front, newarray, 0, half);
        Array.Copy(array, 0, newarray, half, Size - half);
      }

      front = 0;
      back = Size;
      array = newarray;
    }

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    public CircularQueue() : this(8) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="capacity"></param>
    public CircularQueue(int capacity)
      : base(EqualityComparer<T>.Default)
    {
      int newlength = 8;
      while (newlength < capacity) newlength *= 2;
      array = new T[newlength];
    }

    #endregion

    #region IQueue<T> Members
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public virtual bool AllowsDuplicates { get { return true; } }

    /// <summary>
    /// Get the i'th item in the queue. The front of the queue is at index 0.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public virtual T this[int i]
    {
      get
      {
        if (i < 0 || i >= Size)
          throw new IndexOutOfRangeException();
        i = i + front;
        //Bug fix by Steve Wallace 2006/02/10
        return array[i >= array.Length ? i - array.Length : i];
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    [Tested]
    public virtual void Enqueue(T item)
    {
      if (!original)
        throw new ReadOnlyCollectionException();
      Stamp++;
      if (Size == array.Length - 1) expand();
      Size++;
      int oldback = back++;
      if (back == array.Length) back = 0;
      array[oldback] = item;
      if (ActiveEvents != 0)
        raiseForAdd(item);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Tested]
    public virtual T Dequeue()
    {
      if (!original)
        throw new ReadOnlyCollectionException("Object is a non-updatable clone");
      Stamp++;
      if (Size == 0)
        throw new NoSuchItemException();
      Size--;
      int oldfront = front++;
      if (front == array.Length) front = 0;
      T retval = array[oldfront];
      array[oldfront] = default(T);
      if (ActiveEvents != 0)
        raiseForRemove(retval);
      return retval;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    public void Push(T item) //== Enqueue
    {
      if (!original)
        throw new ReadOnlyCollectionException();
      Stamp++;
      if (Size == array.Length - 1) expand();
      Size++;
      int oldback = back++;
      if (back == array.Length) back = 0;
      array[oldback] = item;
      if (ActiveEvents != 0)
        raiseForAdd(item);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public T Pop()
    {
      if (!original)
        throw new ReadOnlyCollectionException("Object is a non-updatable clone");
      Stamp++;
      if (Size == 0)
        throw new NoSuchItemException();
      Size--;
      back--;
      if (back == -1) back = array.Length - 1;
      T retval = array[back];
      array[back] = default(T);
      if (ActiveEvents != 0)
        raiseForRemove(retval);
      return retval;
    }
    #endregion

    #region ICollectionValue<T> Members

    //TODO: implement these with Array.Copy instead of relying on XxxBase:
    /*
        public void CopyTo(T[] a, int i)
        {
        }

        public T[] ToArray()
        {
        }*/

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Tested]
    public override T Choose()
    {
      if (Size == 0)
        throw new NoSuchItemException();
      return array[front];
    }

    #endregion

    #region IEnumerable<T> Members

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override SCG.IEnumerator<T> GetEnumerator()
    {
      int stamp = this.Stamp;
      if (forwards)
      {
        int position = front;
        int end = front <= back ? back : array.Length;
        while (position < end)
        {
          if (stamp != this.Stamp)
            throw new CollectionModifiedException();
          yield return array[position++];
        }
        if (front > back)
        {
          position = 0;
          while (position < back)
          {
            if (stamp != this.Stamp)
              throw new CollectionModifiedException();
            yield return array[position++];
          }
        }
      }
      else
      {
        int position = back - 1;
        int end = front <= back ? front : 0;
        while (position >= end)
        {
          if (stamp != this.Stamp)
            throw new CollectionModifiedException();
          yield return array[position--];
        }
        if (front > back)
        {
          position = array.Length - 1;
          while (position >= front)
          {
            if (stamp != this.Stamp)
              throw new CollectionModifiedException();
            yield return array[position--];
          }
        }
      }
    }

    #endregion

    #region IDirectedCollectionValue<T> Members

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override IDirectedCollectionValue<T> Backwards()
    {
      CircularQueue<T> retval = (CircularQueue<T>)MemberwiseClone();
      retval.original = false;
      retval.forwards = !forwards;
      return retval;
    }

    #endregion

    #region IDirectedEnumerable<T> Members

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IDirectedEnumerable<T> IDirectedEnumerable<T>.Backwards()
    {
      return Backwards();
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual bool Check()
    {
      if (front < 0 || front >= array.Length || back < 0 || back >= array.Length ||
          (front <= back && Size != back - front) || (front > back && Size != array.Length + back - front))
      {
        Console.WriteLine("Bad combination of (front,back,size,array.Length): ({0},{1},{2},{3})",
            front, back, Size, array.Length);
        return false;
      }
      return true;
    }
  }
}