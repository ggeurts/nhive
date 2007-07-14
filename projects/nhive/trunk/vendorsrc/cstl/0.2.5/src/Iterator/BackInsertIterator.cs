//-----------------------------------------------------------------------------
// CSTL
// Copyright (C) 2006 by Harold Howe hhjunk@mchsi.com
// http://sourceforge.net/projects/cstl

using System;
using System.Collections.Generic;

namespace CSTL.Iterator
{

public class BackInsertIterator<T> : OutputIterator<T>
{
    private ICollection<T> m_Collection;
    
    public BackInsertIterator(ICollection<T> collection)
    {
        m_Collection = collection;
    }
    
    #region OutputIterator<T> Members
    public void Write(T t)
    {
        m_Collection.Add(t);
    }

    public void MoveNext()
    {
        // no-op
    }

    public void Assign(InputIterator<T> iterator)
    {
        BackInsertIterator<T> backInserter = iterator as BackInsertIterator<T>;
        if(backInserter == null)
            throw new ArgumentException("Iterator type mismatch.");
        Assign(backInserter);
    }
    #endregion
    
    public void Assign(BackInsertIterator<T> iterator)
    {
        m_Collection = iterator.m_Collection;
    }

    #region ICloneable Members
    public object Clone()
    {
        return new BackInsertIterator<T>(m_Collection);
    }
    #endregion
}

}