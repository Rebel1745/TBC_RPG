using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public DAMAGE_TYPE DamageType;
}

public enum DAMAGE_TYPE
{
    Miss,
    Weak,
    Normal,
    Critical
}
