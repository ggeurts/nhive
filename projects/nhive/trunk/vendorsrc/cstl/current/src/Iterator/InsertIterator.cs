//-----------------------------------------------------------------------------
// CSTL
// Copyright (C) 2006 by Harold Howe hhjunk@mchsi.com
// http://sourceforge.net/projects/cstl

using System;
using System.Collections.Generic;

namespace CSTL.Iterator
{

public class InsertIterator<T> : OutputIterator<T>
{
    private IList<T> m_List;
    private int m_Position;
    
    public InsertIterator(IList<T> collection, int position)
    {
        m_List = collection;
        m_Position = position;
    }
    
    #region OutputIterator<T> Members
    public void Write(T t)
    {
        m_List.Insert(m_Position, t);
    }

    public void MoveNext()
    {
        // no-op
    }

    public void Assign(InputIterator<T> iterator)
    {
        InsertIterator<T> backInserter = iterator as InsertIterator<T>;
        if(backInserter == null)
            throw new ArgumentException("Iterator type mismatch.");
        Assign(backInserter);
    }
    #endregion
    
    public void Assign(InsertIterator<T> iterator)
    {
        m_List = iterator.m_List;
    }

    #region ICloneable Members
    public object Clone()
    {
        return new InsertIterator<T>(m_List, m_Position);
    }
    #endregion
}

}