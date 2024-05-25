using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;
    [SerializeField]
    private TileBase floorTile, wallTop, wallSideRight, wallSiderLeft, wallBottom, wallFull, 
        wallInnerCornerDownLeft, wallInnerCornerDownRight, 
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft,
        mazeTile, dotTile;

    #region Paint Imediate

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    public void PaintMazeTiles(IEnumerable<Vector2Int> mazePosition)
    {
        PaintTiles(mazePosition, floorTilemap, mazeTile);
    }

    public void PaintDotTiles(IEnumerable<Vector2Int> dotPosition)
    {
        PaintTiles(dotPosition, floorTilemap, dotTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tileMap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tileMap, tile, position);
        }
    }

    
    

    #endregion
    #region Paint Async

    public async Task PaintFloorTilesAsync(IEnumerable<Vector2Int> floorPositions)
    {
        await PaintTilesAsync(floorPositions, floorTilemap, floorTile);
    }
    
    public async Task PaintMazeTilesAsync(IEnumerable<Vector2Int> mapPositions)
    {
        await PaintTilesAsync(mapPositions, floorTilemap, floorTile);
    }
    
    public async Task PaintDotTilesAsync(IEnumerable<Vector2Int> dotPositions)
    {
        await PaintTilesAsync(dotPositions, floorTilemap, dotTile);
    }

    private async Task PaintTilesAsync(IEnumerable<Vector2Int> positions, Tilemap tileMap, TileBase tile)
    {
        var pos = positions.GetEnumerator();
        while (pos.MoveNext())
        {
            await Task.Delay(10); 
            PaintSingleTile(tileMap, tile, pos.Current);
            if (pos.MoveNext())
            {
                PaintSingleTile(tileMap, tile, pos.Current);
            }
            
            if (pos.MoveNext())
            {
                PaintSingleTile(tileMap, tile, pos.Current);
            }
            
            if (pos.MoveNext())
            {
                PaintSingleTile(tileMap, tile, pos.Current);
            }
            
            if (pos.MoveNext())
            {
                PaintSingleTile(tileMap, tile, pos.Current);
            }
            
            if (pos.MoveNext())
            {
                PaintSingleTile(tileMap, tile, pos.Current);
            }
        }
    }    

    #endregion
    
    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSiderLeft;
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }

        if (tile!=null)
            PaintSingleTile(wallTilemap, tile, position);
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeASInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeASInt))
        {
            tile = wallInnerCornerDownLeft;
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeASInt))
        {
            tile = wallInnerCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeASInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeASInt))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeASInt))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeASInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeASInt))
        {
            tile = wallFull;
        }
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeASInt))
        {
            tile = wallBottom;
        }

        if (tile != null)
            PaintSingleTile(wallTilemap, tile, position);
    }
}
