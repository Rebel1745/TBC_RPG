using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    Canvas attackParentCanvas;

    // Swing meters... TODO: make these an array of abilities (scriptable objects)
    [SerializeField] GameObject basicAttackPrefab;
    [SerializeField] string basicAttackAnimation;
    [SerializeField] GameObject comboAttack2Prefab;
    [SerializeField] string comboAttack2Animation;
    [SerializeField] GameObject comboAttack3Prefab;
    [SerializeField] string comboAttack3Animation;
    [SerializeField] GameObject comboAttack4Prefab;
    [SerializeField] string comboAttack4Animation;
    [SerializeField] GameObject comboAttack5Prefab;
    [SerializeField] string comboAttack5Animation;
    [SerializeField] GameObject matchAttackPrefab;
    [SerializeField] string matchAttackAnimation;
    [SerializeField] GameObject mashAttackPrefab;
    [SerializeField] string mashAttackAnimation;

    [SerializeField] Animator anim;
    [SerializeField] GameObject target;

    bool canAttack = true;
    string animToPlay;

    Damage[] ProcessedHits;

    private void Start()
    {
        attackParentCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public void SetupAttack(string attackType)
    {
        if (!canAttack)
            return;

        GameObject prefabToUse = null;

        switch (attackType)
        {
            case "basic":
                prefabToUse = basicAttackPrefab;
                animToPlay = basicAttackAnimation;
                break;
            case "combo2":
                prefabToUse = comboAttack2Prefab;
                animToPlay = comboAttack2Animation;
                break;
            case "combo3":
                prefabToUse = comboAttack3Prefab;
                animToPlay = comboAttack3Animation;
                break;
            case "combo4":
                prefabToUse = comboAttack4Prefab;
                animToPlay = comboAttack4Animation;
                break;
            case "combo5":
                prefabToUse = comboAttack5Prefab;
                animToPlay = comboAttack5Animation;
                break;
            case "match":
                prefabToUse = matchAttackPrefab;
                animToPlay = matchAttackAnimation;
                break;
            case "mash":
                prefabToUse = mashAttackPrefab;
                animToPlay = mashAttackAnimation;
                break;
        }

        PerformAttack(prefabToUse, animToPlay);
    }

    void PerformAttack(GameObject attackPrefab, string animation)
    {
        print("AttackController::PerformAttack");
        if (!attackPrefab)
            Debug.LogError("AnimationTest-PerformAttck: No attack prefab recieved");

        canAttack = false;
        animToPlay = animation;
        UIManager.instance.ShowHideInfoBar(false);
        UIManager.instance.ShowHideAttackList(false);

        GameObject go = Instantiate(attackPrefab, attackParentCanvas.transform);

        // I HATE THIS CODE. THERE MUST BE A BETTER WAY
        if (go.GetComponent<SwingMeterCombo>() != null)
            go.GetComponent<SwingMeterCombo>().Swinger = this.gameObject;
        if (go.GetComponent<SwingMeterMatch>() != null)
            go.GetComponent<SwingMeterMatch>().Swinger = this.gameObject;
        if (go.GetComponent<SwingMeterMash>() != null)
            go.GetComponent<SwingMeterMash>().Swinger = this.gameObject;
    }

    public void AttackAgain()
    {
        canAttack = true;
        anim.Play(animToPlay);
        //UIManager.instance.ShowHideInfoBar(true);
        //UIManager.instance.ShowHideAttackList(true);
        //UIManager.instance.SetInfoBarText("Time to attack again");
    }

    public void SetupDamage(Damage[] hits)
    {
        print("AttackController::SetupDamage");
        // TODO: maybe remove this hardcoded damage amount with something variable on the character
        float totalAttackDamage = 50f;
        float damagePerHit = totalAttackDamage / (float)hits.Length;
        float damageMultiplier = 1f;
        foreach (Damage d in hits)
        {
            if (d == null)
                d.DamageType = DAMAGE_TYPE.Miss;

            switch (d.DamageType)
            {
                case DAMAGE_TYPE.Miss:
                    damageMultiplier = 0f;
                    break;
                case DAMAGE_TYPE.Weak:
                    damageMultiplier = 0.5f;
                    break;
                case DAMAGE_TYPE.Critical:
                    damageMultiplier = 1.5f;
                    break;
            }

            d.AmountOfDamage = damagePerHit * damageMultiplier;
        }

        // save this in a variable to utilise when the EventAction of the animation is ready
        ProcessedHits = hits;
    }

    public void DoDamage()
    {
        print("AttackController::DoDamage");
        StartCoroutine(target.GetComponent<Health>().ApplyHits(ProcessedHits));
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}
