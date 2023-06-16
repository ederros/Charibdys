using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexPathFinder : PathFinder
{
    public override Vector2Int[] GetNeighbors(Vector2Int tileCord)
    {
        Vector2Int[] result = new Vector2Int[HexManager.primaryDirs.Count];
        if(tileCord.y%2 == 0)
            HexManager.secondDirs.CopyTo(result);
        else 
            HexManager.primaryDirs.CopyTo(result);

        for(int i = 0; i < HexManager.secondDirs.Count; i++){
            result[i] += tileCord;
        }
        return result;
    }
}
