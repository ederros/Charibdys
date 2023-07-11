using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
public class AttackBehaviour: MonoBehaviour
{
    [SerializeField]
    protected float damage;
    [SerializeField]
    private uint turnsRequired;

    [SerializeField]
    private uint attackCount;

    public uint TurnsRequired { get => turnsRequired;}

    public virtual void Attack(Vector2Int position, Tilemap tMap){
        UnitController target = HexManager.CheckFor<UnitController>(tMap,position);
        for(int i = 0; i < attackCount;i++){
             target?.hp.SubValue(damage - target.armor);
        }
    }
}
