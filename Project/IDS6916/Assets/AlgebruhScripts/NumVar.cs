using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NumVar
{
    // Start is called before the first frame update
    public int Value { get; set; }
    public NumVar()
    {
        Value = 0;    
    }
    public void GenerateNumber(int numRangeStart, int numRangeEnd)
    {
        var rand = new System.Random();
        Value = rand.Next(numRangeStart, numRangeStart);
    }


}
