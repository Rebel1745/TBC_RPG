using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationReferral : MonoBehaviour
{
    [SerializeField] AttackController ac;

    public void DoDamage()
    {
        print("AnimationReferral::DoDamage");
        ac.DoDamage();
    }
}
