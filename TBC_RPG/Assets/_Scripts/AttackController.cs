using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    Canvas attackParentCanvas;

    // Swing meters... TODO: make these an array of abilities (scriptable objects)
    [SerializeField] GameObject basicAttackPrefab;
    [SerializeField] GameObject comboAttack2Prefab;
    [SerializeField] GameObject comboAttack3Prefab;
    [SerializeField] GameObject comboAttack4Prefab;
    [SerializeField] GameObject comboAttack5Prefab;
    [SerializeField] GameObject matchAttackPrefab;
    [SerializeField] GameObject mashAttackPrefab;

    [SerializeField] Animator anim;
    [SerializeField] GameObject target;

    bool canAttack = true;
    Animator animToUse;
    string animToPlay;

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
                animToUse = anim;
                animToPlay = "Slash";
                break;
            case "combo2":
                prefabToUse = comboAttack2Prefab;
                animToUse = anim;
                animToPlay = "Crash";
                break;
            case "combo3":
                prefabToUse = comboAttack3Prefab;
                animToUse = anim;
                animToPlay = "Kick";
                break;
            case "combo4":
                prefabToUse = comboAttack4Prefab;
                animToUse = anim;
                animToPlay = "Slash";
                break;
            case "combo5":
                prefabToUse = comboAttack5Prefab;
                animToUse = anim;
                animToPlay = "Crash";
                break;
            case "match":
                prefabToUse = matchAttackPrefab;
                animToUse = anim;
                animToPlay = "Kick";
                break;
            case "mash":
                prefabToUse = mashAttackPrefab;
                animToUse = anim;
                animToPlay = "Slash";
                break;
        }

        PerformAttack(prefabToUse, animToUse, animToPlay);
    }

    void PerformAttack(GameObject attackPrefab, Animator animator, string animation)
    {
        if (!attackPrefab)
            Debug.LogError("AnimationTest-PerformAttck: No attack prefab recieved");

        canAttack = false;
        animToUse = animator;
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
        animToUse.Play(animToPlay);
        UIManager.instance.ShowHideInfoBar(true);
        UIManager.instance.ShowHideAttackList(true);
        UIManager.instance.SetInfoBarText("Time to attack again");
    }

    public void DoDamage(Damage[] hits)
    {
        foreach(Damage d in hits)
        {
            print(d.DamageType);
        }
    }
}
