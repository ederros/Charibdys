using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InstanceDeathAtk : AttackBehaviour
{
    public override void Attack(Vector2Int position, Tilemap tMap)
    {
        HexManager.CheckFor<EntityBehaviour>(tMap,position)?.Die();
    }
}
