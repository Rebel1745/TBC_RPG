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
    [SerializeField] GameObject comboAttack1Prefab;
    [SerializeField] GameObject comboAttack2Prefab;
    [SerializeField] GameObject comboAttack3Prefab;
    [SerializeField] GameObject comboAttack4Prefab;
    [SerializeField] GameObject matchAttackPrefab;
    [SerializeField] GameObject alternateAttackPrefab;

    bool canAttack = true;
    Animator animToUse;
    string animToPlay;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(canAttack)
            CheckForInput();
    }

    public void AttackAgain()
    {
        canAttack = true;
        animToUse.Play(animToPlay);
    }

    public void DoDamage()
    {
        enemyAnim?.GetComponentInParent<Health>().ChangeHealth(-55f);
    }

    void CheckForInput()
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
    }
}
