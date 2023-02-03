using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Score : IComparable<Score>, IEquatable<Score>
{


    public string PlayerName;
    public int score;

    public Score(string PlayerName, int score)
    {
        this.PlayerName = PlayerName;
        this.score = score;

    }

    public int CompareTo(Score other)
    {
        if (other.score < score)
        
            return -1;
        
        else if (other.score > score)
        
            return 1;
       
        else 

            return 0;
    }

    public bool Equals(Score other)
    {
        if (other.PlayerName == PlayerName)
            return true;
        else
            return false;
    }
}
