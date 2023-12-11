using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    Canvas attackParentCanvas;

    [SerializeField] Animator anim;
    [SerializeField] GameObject target;

    Ability currentAbility;

    Damage[] ProcessedHits;

    private void Start()
    {
        attackParentCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public void SetupAttack(Ability ability)
    {
        currentAbility = ability;
        PerformAttack(ability.AbilitySwingMeterPrefab, ability.AnimationName);
    }

    void PerformAttack(GameObject attackPrefab, string animation)
    {
        if (!attackPrefab)
            Debug.LogError("AnimationTest-PerformAttack: No attack prefab recieved");
        
        //UIManager.instance.ShowHideInfoBar(false);
        UIManager.instance.ShowHideAttackList(false);

        GameObject go = Instantiate(attackPrefab, attackParentCanvas.transform);

        // I HATE THIS CODE. THERE MUST BE A BETTER WAY (Use an interface)
        if (go.GetComponent<SwingMeterCombo>() != null)
            go.GetComponent<SwingMeterCombo>().Swinger = this.gameObject;
        if (go.GetComponent<SwingMeterMatch>() != null)
            go.GetComponent<SwingMeterMatch>().Swinger = this.gameObject;
        if (go.GetComponent<SwingMeterMash>() != null)
            go.GetComponent<SwingMeterMash>().Swinger = this.gameObject;
    }

    public void PlayAbilityAnimation()
    {
        anim.Play(currentAbility.AnimationName);
    }

    public void SetupDamage(Damage[] hits)
    {
        float damageMultiplier = 1f;
        for (int d = 0; d < hits.Length; d++)
        {
            if (hits[d] == null)
            {
                hits[d] = new Damage
                {
                    DamageType = DAMAGE_TYPE.Miss
                };
            }

            switch (hits[d].DamageType)
            {
                case DAMAGE_TYPE.Miss:
                    damageMultiplier = 0f;
                    break;
                case DAMAGE_TYPE.Weak:
                    damageMultiplier = currentAbility.WeakDamageMultiplier;
                    break;
                case DAMAGE_TYPE.Critical:
                    damageMultiplier = currentAbility.CritDamageMultiplier;
                    break;
            }

            hits[d].AmountOfDamage = currentAbility.DamagePerHit * damageMultiplier;
        }

        // save this in a variable to utilise when the EventAction of the animation is ready
        ProcessedHits = hits;
    }

    public void DoDamage()
    {
        StartCoroutine(target.GetComponent<Health>().ApplyHits(ProcessedHits));
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}
