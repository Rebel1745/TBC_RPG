using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/New Ability")]
public class Ability : ScriptableObject {

    public string AbilityName = "New Ability";
    public GameObject AbilitySwingMeterPrefab; // the swing meter to use for this ability
    public string AnimationName; // the name of the animation corresponding to the ability
    public bool isSelfCast = false; // is the ability applicable to the caster
    public bool isTargetOpponent = true; // is this an attack against an opponent or an effect applied to teammates
    public bool isAOE = false; // is the effect of this ability an area of effect
    public int AOEArea; // this effect will be applied to all units within this number of nodes
    public int DamagePerHit; // how much damage should be applied for each possible hit of the combo
    public float CritDamageMultiplier; // how much more damage does a crit do than a normal attack
    public float WeakDamageMultiplier; // how much should the damage be decreased if the hit is 'weak'
    public int MinEffectiveDistance = 0; // how many nodes away from the target can this ability be cast
    public int MaxEffectiveDistance = 1; // maximum number of nodes away from target
}
