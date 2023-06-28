using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 
public class MoveCommand : EntityCommand
{
    
    private readonly Vector2Int movePos;

    public MoveCommand(EntityBehaviour subject, Vector2Int movePos):base(subject)
    {
        
        this.movePos = movePos;
    }

    public override uint GetCost()
    {
        return subject.myTileMap.GetTile<FloorTile>((Vector3Int)movePos).turnsRequired;
    }

    protected override void Invoke()
    {
        subject.TileCoord = movePos;
        if(!subject.CheckAffiliation()) return;
        MonoBehaviour.Destroy(CursorController.Instance.pathPointsContainer.GetChild(0).gameObject);
        
   }
}
