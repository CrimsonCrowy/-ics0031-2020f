using System;
using System.Collections.Generic;

class GFD {
    static int __gcd(int a, int b)   
    {
        if (a == 0)   
            return b;   
        if (b == 0)   
            return a;
        if (a == b)   
            return a;
        if (a > b)   
            return __gcd(a-b, b);   
        return __gcd(a, b-a);   
    }
    public static int[] PrimitiveRoots(int p)  
    {  
        var result = new List<int>();
        int count = 0;
        for (int i = 2; i < p; i++)  
            if (__gcd(i, p) == 1)  
                result.Add(i);
        var resultArray = result.ToArray();
        return resultArray;  
    }
}