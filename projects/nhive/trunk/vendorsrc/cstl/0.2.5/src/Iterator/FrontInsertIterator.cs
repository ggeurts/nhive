using System;
using System.Collections.Generic;

namespace STL.Iterator
{

public class FrontInsertIterator<T> : OutputIterator<T>
{
    private IList<T> m_Collection;
    
    public FrontInsertIterator(IList<T> collection)
    {
        m_Collection = collection;
    }
    
    #region OutputIterator<T> Members
    public void Write(T t)
    {
        m_Collection.Insert(0, t);
    }

    public void Next()
    {
        // no-op
    }

    public void Assign(InputIterator<T> iterator)
    {
        FrontInsertIterator<T> backInserter = iterator as FrontInsertIterator<T>;
        if(backInserter == null)
            throw new ArgumentException("Iterator type mismatch.");
        Assign(backInserter);
    }
    #endregion
    
    public void Assign(FrontInsertIterator<T> iterator)
    {
        m_Collection = iterator.m_Collection;
    }
}

}