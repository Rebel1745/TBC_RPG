using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] PopupNumbers pn;
    [SerializeField] Slider healthBar;

    [SerializeField] float startingHealth = 100f;
    float currentHealth;

    Animator anim;
    [SerializeField] string damageAnimation = "Damaged";
    [SerializeField] string deathAnimation = "Death";

    // Damage colours
    [SerializeField] Color normalDamageColor = Color.red;
    [SerializeField] Color weakDamageColor = Color.blue;
    [SerializeField] Color criticalDamageColor = Color.green;
    [SerializeField] Color missDamageColor = Color.black;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        currentHealth = startingHealth;
        healthBar.maxValue = startingHealth;
        healthBar.value = currentHealth;
    }

    public IEnumerator ApplyHits(Damage[] hits)
    {
        bool playAnimation = false;
        Color damageCol = Color.black;

        for (int i = 0; i < hits.Length; i++)
        {
            yield return new WaitForSeconds(0.2f);
            // only play the animation on the first hit
            playAnimation = i == 0 ? true : false;

            switch (hits[i].DamageType)
            {
                case DAMAGE_TYPE.Weak: damageCol = weakDamageColor; break;
                case DAMAGE_TYPE.Normal: damageCol = normalDamageColor; break;
                case DAMAGE_TYPE.Critical: damageCol = criticalDamageColor; break;
            }

            ChangeHealth(-hits[i].AmountOfDamage, playAnimation, damageCol, false);
        }
    }

    public void ChangeHealth(float amount, bool playAnimation, Color damageCol, bool isPercentageChange = false)
    {
        if (playAnimation)
            anim.Play(damageAnimation);

        float change = amount;
        pn.CreatePopup(Mathf.Abs(amount).ToString(), damageCol);

        if (isPercentageChange)
        {
            // TODO: check if this maths is correct
            change = (currentHealth * amount) / 100f;
        }

        currentHealth += change;

        healthBar.value = currentHealth;

        if(currentHealth <= 0f)
        {
            anim.Play(deathAnimation);
        }
    }
}
