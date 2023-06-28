using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AttackCommand : EntityCommand
{
    AttackBehaviour Attack;
    Vector2Int position;
    Tilemap tMap;
    public AttackCommand(EntityBehaviour subject, Vector2Int position, Tilemap tMap):base(subject)
    {
        this.Attack = subject.Attack;
        this.position = position;
        this.tMap = tMap;
    }

    public override uint GetCost()
    {
        return Attack.TurnsRequired;
    }

    protected override void Invoke()
    {
        Attack.Attack(position, tMap);
    }
    
}
