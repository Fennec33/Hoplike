using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class HexCoordinate
{
    private int _q;
    private int _r;
    private int _s;
    
    public int Q => _q;
    public int R => _r;
    public int S => _s;

    public HexCoordinate(int q, int r)
    {
        _q = q;
        _r = r;
        _s = -(q + r);
    }

    public void SetCoordinate(int q, int r)
    {
        _q = q;
        _r = r;
        _s = -(q + r);
    }

    public List<HexCoordinate> GetNeighbors()
    {
        return new List<HexCoordinate>()
        {
            new HexCoordinate(_q, _r + 1),
            new HexCoordinate(_q + 1, _r),
            new HexCoordinate(_q + 1, _r - 1),
            new HexCoordinate(_q, _r - 1),
            new HexCoordinate(_q - 1, _r),
            new HexCoordinate(_q - 1, _r + 1)
        };
    }

    public List<HexCoordinate> GetDiagonals()
    {
        return new List<HexCoordinate>()
        {
            new HexCoordinate(_q + 1, _r + 1),
            new HexCoordinate(_q + 2, _r - 1),
            new HexCoordinate(_q + 1, _r - 2),
            new HexCoordinate(_q - 1, _r - 1),
            new HexCoordinate(_q - 2, _r + 1),
            new HexCoordinate(_q - 1, _r + 2)
        };
    }

    public List<HexCoordinate> GetHexesWithinRange(int range)
    {
        List<HexCoordinate> result = new List<HexCoordinate>();
        
        for (int q = -range; q <= range; q++)
        {
            for (int r = Math.Max(-range, -q - range); r <= Math.Min(range, -q + range); r++)
            {
                result.Add(new HexCoordinate(_q + q, _r + r));
            }
        }

        return result;
    }

    public int DistanceFrom(HexCoordinate target)
    {
        var vec = Subtract(target);
        return (Math.Abs(vec.Q) + Math.Abs(vec.R) + Math.Abs(vec.S)) / 2;
    }

    public override bool Equals(Object obj)
    {
        if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            HexCoordinate hex = (HexCoordinate)obj;
            return (_q == hex.Q && _r == hex.R);
        }
    }

    public HexCoordinate North() => new HexCoordinate(_q, _r + 1);
    public HexCoordinate NorthEast() => new HexCoordinate(_q + 1, _r);
    public HexCoordinate SouthEast() => new HexCoordinate(_q + 1, _r - 1);
    public HexCoordinate South() => new HexCoordinate(_q, _r - 1);
    public HexCoordinate SouthWest() => new HexCoordinate(_q - 1, _r);
    public HexCoordinate NorthWest() => new HexCoordinate(_q - 1, _r + 1);

    public override int GetHashCode()
    {
        Vector3 vec = new Vector3(_q, _r, _s);
        return vec.GetHashCode();
    }
    
    public static bool operator ==(HexCoordinate left, HexCoordinate right)
    {
        return (left?.Q == right?.Q && left?.R == right?.R);
    }
    
    public static bool operator !=(HexCoordinate left, HexCoordinate right)
    {
        return (left?.Q != right?.Q || left?.R != right?.R);
    }

    private HexCoordinate Subtract(HexCoordinate sub)
    {
        return new HexCoordinate(_q - sub.Q, _r - sub.R);
    }
    
    private HexCoordinate Add(HexCoordinate sub)
    {
        return new HexCoordinate(_q + sub.Q, _r + sub.R);
    }
}
