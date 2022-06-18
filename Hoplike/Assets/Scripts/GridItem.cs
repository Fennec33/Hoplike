using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{
    private HexCoordinate _location;
    private Vector2 _worldPosition;

    public void MoveTo(HexCoordinate hex)
    {
        if (_location != null)
        {
            GridManager.Instance.GetHexTileAtCoordinate(_location).Occupant = null;
        }

        _location = hex;
        _worldPosition = GridManager.Instance.HexGridCoordinateToWorldPosition(_location);
        transform.position = _worldPosition;
        GridManager.Instance.GetHexTileAtCoordinate(_location).Occupant = this;
    }

    public HexCoordinate GetLocation()
    {
        return _location;
    }
}
