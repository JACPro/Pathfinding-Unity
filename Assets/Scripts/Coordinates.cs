using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates
{
    public int _x, _y;
    public static Coordinates _empty = new Coordinates(-1,-1);

    public Coordinates()
    {
        _x = -1;
        _y = -1;
    }

    public Coordinates(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public bool IsNull()
    {
        return (_x == -1 || _y == -1);
    }
    
    public bool Equals(Coordinates coordinates) {
        return (_x == coordinates._x && _y == coordinates._y);
    }
}
