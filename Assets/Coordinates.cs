using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates
{
    public int x, y;
    static public Coordinates EMPTY = new Coordinates(-1,-1);

    public Coordinates()
    {
        x = -1;
        y = -1;
    }

    public Coordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool IsNull()
    {
        return (x == -1 || y == -1);
    }
    
    public bool Equals(Coordinates coordinates) {
        return (x == coordinates.x && y == coordinates.y);
    }
}
