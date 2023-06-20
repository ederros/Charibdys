using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class AttackBehaviour
{
    public float damage;
    public uint turnsRequired;
    public virtual void Attack(Vector2Int position, Tilemap tMap){
        UnitController target = HexManager.CheckFor<UnitController>(tMap,position);
        target?.hp.SubValue(damage);
    }
}
