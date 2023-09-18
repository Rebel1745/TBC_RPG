using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationTest : MonoBehaviour
{
    [SerializeField] Animator anim;

    private void Update()
    {
        CheckForInput();
    }

    void CheckForInput()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            anim.Play("Idle");
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            anim.Play("Slash");
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            anim.Play("Crash");
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            anim.Play("Kick");
        }
    }
}
