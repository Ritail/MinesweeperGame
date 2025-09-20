using TMPro;
using UnityEngine;

public class Cell 
{
    public int Value;
    public Vector2Int Position;
    public bool IsRevealed = false;

    public bool IsCheckPos(Vector2Int pos, int lenght , int width)
    {
        return pos.x >= 0  && pos.x < lenght && pos.y >= 0 && pos.y < width;
    }
}
