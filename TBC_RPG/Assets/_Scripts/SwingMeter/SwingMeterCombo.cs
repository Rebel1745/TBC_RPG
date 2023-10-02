using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwingMeterCombo : SwingMeter {

    // Update is called once per frame
    void Update()
    {
        // TODO: Get this and the other scripts using the new input system
        // Press SPACE to start the swing meter
        if (!isSwinging && Input.GetButtonDown("Jump"))
        {
            StartSwinging();
            return;
        }

        // if we are not swinging then bail
        if (!isSwinging)
            return;

        // The meter is swinging; a button was pressed; check to see whether it was the correct button
        if (canPress && Input.anyKeyDown)
        {
            StopSwinging(false);
            return;
        }

        UpdateSlider();
    }

    public override void UpdateSlider()
    {
        base.UpdateSlider();

        // If the meter gets to 100 without the final required key being pressed, return a miss
        if (swingMeter.value >= swingMeter.maxValue || (canPress && swingMeter.value >= nextPressThreshold))
        {
            swingNo = 0;
            StopSwinging(true);
        }

        // Stop multiple clicking being allowed (double-tap)
        if (!canPress && swingMeter.value >= nextPressThreshold)
        {
            canPress = true;
            nextPressThreshold += pressAgainThreshold;
            currentPanel = keysToPressPanels[swingNo];
            currentPanelSpawn = keysToPressPanelSpawns[0];
        }

        // Move the current keyToPressPanel along with the slider value
        if (currentPanel != null)
        {
            Vector3 newPos = new Vector3(GetXPixelFromSwingMeterValue(swingMeter.value), 0, 0);
            currentPanel.position = currentPanelSpawn.position + newPos;
        }
    }

    public override void InitialiseKeys()
    {
        base.InitialiseKeys();

        // calculate threshold for next swing (i.e corresponding swingMeter.value)
        pressAgainThreshold = (1f / (float)noOfSwings) * 100f;
        nextPressThreshold = pressAgainThreshold;

        // Display the keysToPress in the correct text fields
        for (int i = 0; i < keysToPressText.Length; i++)
        {
            string keyText = keysToPress[i].ToUpper();
            keysToPressText[i].text = keyText;
        }

        // set up the panel to start following the swingMeter.value
        currentPanel = keysToPressPanels[0];
        currentPanelSpawn = keysToPressPanelSpawns[0];
    }

    public override void RandomiseKeys()
    {
        keysToPress = new string[noOfSwings];
        for (int i = 0; i < noOfSwings; i++)
        {
            int x = Random.Range(0, possibleKeys.Length);
            keysToPress[i] = possibleKeys[x];
        }
    }

    public override void StopSwinging(bool missed)
    {
        canPress = false;

        // If a button was pressed (i.e not a miss) increment the current swing number
        if (!missed)
        {
            swingNo++;
            keyPressed = Input.inputString;
        }

        // Check the color of the image corresponding with the position on the image the swing meter stopped on
        Color stoppingColor = GetColorAtStoppingPoint(swingMeter.value);
        DAMAGE_TYPE hitType = CheckHitType(stoppingColor, currentPanel);

        HitsDamage[swingNo-1] = new Damage { DamageType = hitType };

        currentPanel = null;
        currentPanelSpawn = null;

        // If it was the last key pressed (or a miss) reset the swing meter
        if (swingNo == noOfSwings || missed)
        {
            isSwinging = false;
            swingMeter.value = swingMeter.minValue;
            swingNo = 0;
            Invoke("Close",1f);
        }
        
    }

    DAMAGE_TYPE CheckHitType(Color stoppingColor, Transform panel)
    {
        DAMAGE_TYPE dt = DAMAGE_TYPE.Miss;

        // textToAdd not used since DAMAGE_TYPE has been implemented
        //string textToAdd = stoppingColor.ToString();
        bool wrongButton = false;
        if (swingNo > 0)
        {
            if (stoppingColor.g == 1f)
            {
                // CRIT BABY!
                if (keyPressed == keysToPress[swingNo - 1])
                {
                    dt = DAMAGE_TYPE.Critical;
                    //textToAdd = "crit";
                }
                else
                {
                    dt = DAMAGE_TYPE.Miss;
                    //textToAdd = "miss";
                    wrongButton = true;
                    swingNo = noOfSwings;
                }
            }
            else if (stoppingColor.b == 1f)
            {
                if (keyPressed == keysToPress[swingNo - 1])
                {
                    dt = DAMAGE_TYPE.Normal;
                    //textToAdd = "normal";
                }
                else
                {
                    dt = DAMAGE_TYPE.Miss;
                    //textToAdd = "miss";
                    wrongButton = true;
                    swingNo = noOfSwings;
                }
            }
            else if (stoppingColor.r == 1f)
            {
                if (keyPressed == keysToPress[swingNo - 1])
                {
                    dt = DAMAGE_TYPE.Weak;
                    //textToAdd = "weak";
                }
                else
                {
                    dt = DAMAGE_TYPE.Miss;
                    //textToAdd = "miss";
                    wrongButton = true;
                    swingNo = noOfSwings;
                }
            }

            if (wrongButton)
            {
                panel.gameObject.GetComponent<Image>().color = Color.black;
            }
            else
            {
                panel.gameObject.GetComponent<Image>().color = stoppingColor;
            }
            
        }
        else
        {
            panel.gameObject.GetComponent<Image>().color = Color.black;
            dt = DAMAGE_TYPE.Miss;
            //textToAdd = "miss";
        }

        return dt;
    }
}
