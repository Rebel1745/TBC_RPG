using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] PopupNumbers pn;

    [SerializeField] float startingHealth = 100f;
    float currentHealth;

    Animator anim;
    [SerializeField] string damageAnimation = "Damaged";
    [SerializeField] string deathAnimation = "Death";

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        currentHealth = startingHealth;
    }

    public void ChangeHealth(float amount, bool isPercentageChange = false)
    {
        anim.Play(damageAnimation);

        float change = amount;
        pn.CreatePopup(Mathf.Abs(amount).ToString(), Color.red);

        if (isPercentageChange)
        {
            // TODO: check if this maths is correct
            change = (currentHealth * amount) / 100f;
        }

        currentHealth += change;

        if(currentHealth <= 0f)
        {
            anim.Play(deathAnimation);
        }
    }
}
