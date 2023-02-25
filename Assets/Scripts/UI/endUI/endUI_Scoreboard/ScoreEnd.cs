using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ScoreEnd : IComparable<ScoreEnd>, IEquatable<ScoreEnd>
{


    public string PlayerName;
    public float score;

    public ScoreEnd(string PlayerName, float score)
    {
        this.PlayerName = PlayerName;
        this.score = score;

    }

    public int CompareTo(ScoreEnd other)
    {
        if (other.score < score)
        
            return -1;
        
        else if (other.score > score)
        
            return 1;
       
        else 

            return 0;
    }

    public bool Equals(ScoreEnd other)
    {
        if (other.PlayerName == PlayerName)
            return true;
        else
            return false;
    }
}
