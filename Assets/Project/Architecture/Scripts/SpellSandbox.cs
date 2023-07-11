using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public abstract class SpellSandbox
{
    [SerializeField] protected Tilemap tmap;
    protected abstract void Invoke(Vector2Int pos);
    protected void Poison(Vector2Int pos){
        Poison(HexManager.CheckFor<UnitController>(tmap,pos));
    }
    protected void Poison(UnitController unit){
        unit.hp.SubValue(1);
    }
}
