using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationTest : MonoBehaviour
{
    [SerializeField] Animator playerAnim;
    [SerializeField] Animator enemyAnim;

    private void Update()
    {
        CheckForInput();
    }

    public void DoDamage()
    {
        Health enemyHealth = enemyAnim?.GetComponentInParent<Health>();
        enemyHealth.ChangeHealth(-55f);
    }

    void CheckForInput()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            playerAnim.Play("Idle");
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            playerAnim.Play("Slash");
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            playerAnim.Play("Crash");
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            playerAnim.Play("Kick");
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            playerAnim.Play("Damaged");
        }


        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            enemyAnim.Play("Death");
        }
        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            enemyAnim.Play("Punch");
        }
        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            enemyAnim.Play("Scream");
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            enemyAnim.Play("Idle");
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            enemyAnim.Play("Damaged");
        }
    }
}
