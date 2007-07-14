using System;

namespace CSTL.Test
{

/// <summary>
/// Constants used for testing
/// </summary>
/// <remarks>
/// The array properties intentionally return new copies on every read of the property. This means that
/// [IteratorUtil.Begin(TEST_INT_ARRAY), IteratorUtil.End(TEST_INT_ARRAY)) is not a valid iterator range. Each call of
/// the property returns a different array.
/// 
/// The reason for this is that some tests modify the resulting array. Returning a new array each time helps isolate
/// the tests. 
/// </remarks>
public static class Constants
{
    public static int[] TEST_INT_ARRAY
    {
        get { return new int[]{5,2,29,34,-17,33, 29 ,29,-1,29,100,12};}
    }
    
    public static string[] TEST_STRING_ARRAY
    {
        get { return new string []{ "hello", "world", "Frank", null, "Frank", "Mary", "frank", "Sue", "Frank", "Bob"};}
    }
    
    public static int[] TEST_BIG_INT_ARRAY
    {
        get 
        { 
            return new int[]    {
                1,2,3,2,3,1,1,1,2,4,5,5,3,4,3,5,2,2,2,1,2,1,2,3,4,5,4,2,3,4,5,3,3,4,5,5,3,2,1,
                1,2,3,2,3,1,1,1,2,4,5,5,3,4,3,5,2,2,2,1,2,1,2,3,4,5,4,2,3,4,5,3,3,4,5,5,3,2,1,
                1,2,3,2,3,1,1,1,2,4,5,5,3,4,3,5,2,2,2,1,2,1,2,3,4,5,4,2,3,4,5,3,3,4,5,5,3,2,1,
                1,2,3,2,3,1,1,1,2,4,5,5,3,4,3,5,2,2,2,1,2,1,2,3,4,5,4,2,3,4,5,3,3,4,5,5,3,2,1,
            };
        }
    }

    public static string[] TEST_BIG_STRING_ARRAY
    {
        get 
        { 
            return new string[] {
                "1","2","3","2","3","1","1","1","2","4","5","5","3","4","3","5","2","2","2","1","2","1","2","3","4","5","4","2","3","4","5","3","3","4","5","5","3","2","1",
                "1","2","3","2","3","1","1","1","2","4","5","5","3","4","3","5","2","2","2","1","2","1","2","3","4","5","4","2","3","4","5","3","3","4","5","5","3","2","1",
                "1","2","3","2","3","1","1","1","2","4","5","5","3","4","3","5","2","2","2","1","2","1","2","3","4","5","4","2","3","4","5","3","3","4","5","5","3","2","1",
                "1","2","3","2","3","1","1","1","2","4","5","5","3","4","3","5","2","2","2","1","2","1","2","3","4","5","4","2","3","4","5","3","3","4","5","5","3","2","1",
            };
        }
    }    
}

}