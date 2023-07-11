using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSpell : SpellSandbox
{
    protected override void Invoke(Vector2Int pos)
    {
        Poison(pos);
    }
}
