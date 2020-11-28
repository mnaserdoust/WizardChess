using System.Collections;

using UnityEngine;
/// <summary>
/// Based of every chessman
/// </summary>
public abstract class Chessman : MonoBehaviour
{
    public int CurrentX { set; get; }
    public int CurrentZ { set; get; }

    public bool isWhite;
    public int value;
    public void setposition(int x, int z)
    {
        CurrentX = x;
        CurrentZ = z;
    }

    public virtual bool[,] possibleMove()
    {

        return new bool[8,8];
    }


}
