using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTest : MonoBehaviour
{
    public static AnimationTest instance;

    [SerializeField] Animator playerAnim;
    [SerializeField] Animator enemyAnim;

    [SerializeField] Canvas attackParentCanvas;
    [SerializeField] GameObject basicAttackPrefab;
    [SerializeField] GameObject comboAttack2Prefab;
    [SerializeField] GameObject comboAttack3Prefab;
    [SerializeField] GameObject comboAttack4Prefab;
    [SerializeField] GameObject comboAttack5Prefab;
    [SerializeField] GameObject matchAttackPrefab;
    [SerializeField] GameObject mashAttackPrefab;

    bool canAttack = true;
    Animator animToUse;
    string animToPlay;

    private void Awake()
    {
        instance = this;
    }

    /*private void Update()
    {
        if(canAttack)
            CheckForInput();
    }*/

    public void SetupAttack(string attackType)
    {
        if (!canAttack)
            return;

        GameObject prefabToUse = null;

        switch (attackType)
        {
            case "basic":
                prefabToUse = basicAttackPrefab;
                animToUse = playerAnim;
                animToPlay = "Slash";
                break;
            case "combo2":
                prefabToUse = comboAttack2Prefab;
                animToUse = playerAnim;
                animToPlay = "Crash";
                break;
            case "combo3":
                prefabToUse = comboAttack3Prefab;
                animToUse = playerAnim;
                animToPlay = "Kick";
                break;
            case "combo4":
                prefabToUse = comboAttack4Prefab;
                animToUse = playerAnim;
                animToPlay = "Slash";
                break;
            case "combo5":
                prefabToUse = comboAttack5Prefab;
                animToUse = playerAnim;
                animToPlay = "Crash";
                break;
            case "match":
                prefabToUse = matchAttackPrefab;
                animToUse = playerAnim;
                animToPlay = "Kick";
                break;
            case "mash":
                prefabToUse = mashAttackPrefab;
                animToUse = playerAnim;
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
        Instantiate(attackPrefab, attackParentCanvas.transform);
        animToUse = animator;
        animToPlay = animation;
        UIManager.instance.ShowHideInfoBar(false);
        UIManager.instance.ShowHideAttackList(false);
    }

    public void AttackAgain()
    {
        canAttack = true;
        animToUse.Play(animToPlay);
        UIManager.instance.ShowHideInfoBar(true);
        UIManager.instance.ShowHideAttackList(true);
        UIManager.instance.SetInfoBarText("Time to attack again");
    }

    public void DoDamage()
    {
        enemyAnim?.GetComponentInParent<Health>().ChangeHealth(-55f);
    }

    /*void CheckForInput()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            canAttack = false;
            Instantiate(basicAttackPrefab, attackParentCanvas.transform);
            animToUse = playerAnim;
            animToPlay = "Slash";
            //playerAnim.Play("Idle");
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            canAttack = false;
            Instantiate(comboAttack1Prefab, attackParentCanvas.transform);
            animToUse = playerAnim;
            animToPlay = "Slash";
            //playerAnim.Play("Slash");
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            canAttack = false;
            Instantiate(comboAttack2Prefab, attackParentCanvas.transform);
            animToUse = playerAnim;
            animToPlay = "Crash";
            //playerAnim.Play("Crash");
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            canAttack = false;
            Instantiate(comboAttack3Prefab, attackParentCanvas.transform);
            animToUse = playerAnim;
            animToPlay = "Kick";
            //playerAnim.Play("Kick");
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            canAttack = false;
            Instantiate(comboAttack4Prefab, attackParentCanvas.transform);
            animToUse = playerAnim;
            animToPlay = "Damaged";
            //playerAnim.Play("Damaged");
        }


        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            //enemyAnim.Play("Death");
        }
        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            //enemyAnim.Play("Punch");
        }
        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            //enemyAnim.Play("Scream");
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            canAttack = false;
            Instantiate(alternateAttackPrefab, attackParentCanvas.transform);
            animToUse = enemyAnim;
            animToPlay = "Punch";
            //enemyAnim.Play("Idle");
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            canAttack = false;
            Instantiate(matchAttackPrefab, attackParentCanvas.transform);
            animToUse = enemyAnim;
            animToPlay = "Death";
            //enemyAnim.Play("Damaged");
        }
    }*/
}
