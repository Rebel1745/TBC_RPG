﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwingMeter : MonoBehaviour {

    public Slider swingMeter;
    public Texture2D meterImage;

    protected bool isSwinging = false;

    public bool canPress = true;
    public float pressAgainThreshold;
    public float nextPressThreshold;

    public float meterSpeed = 20f;

    public int noOfSwings;
    public int swingNo = 0;
    public bool keysToPressRandom = false;
    public string[] keysToPress;
    public string[] possibleKeys;

    // Define a color for the keysToPress panels so they can be reverted after they change to the color corresponding to the stopping color
    public Color panelColor;

    public string keyPressed = "";

    // Define an array of text boxes for the keysToPress
    public Text[] keysToPressText;
    // Define an array of transforms for the keysToPress (allows their position to follow the swingMeter.value)
    public Transform[] keysToPressPanels;
    // Define spawn points for the keysToPressPanels
    public Transform[] keysToPressPanelSpawns;
    // Keep a reference to the current moving panel
    public Transform currentPanel;
    public Transform currentPanelSpawn;

    void Start ()
    {
        InitialiseKeys();
    }

    public void StartSwinging()
    {
        isSwinging = true;
    }

    // virtual functions
    public virtual void InitialiseKeys()
    {
        canPress = true;
        // check to make sure possibleKeys is not null - if it is add 1,2,3 and 4 as options
        if (possibleKeys == null || possibleKeys.Length == 0)
        {
            possibleKeys = new string[4];
            possibleKeys[0] = "1";
            possibleKeys[1] = "2";
            possibleKeys[2] = "3";
            possibleKeys[3] = "4";
        }

        // if the keysToPress is going to be random, pick from the possibleKeys as many times as needed
        if (keysToPressRandom)
        {
            RandomiseKeys();
        }

        // Place the keysToPressPanels in the positions dictated by keysToPressPanelSpawns
        for (int i = 0; i < keysToPressPanels.Length; i++)
        {
            keysToPressPanels[i].position = keysToPressPanelSpawns[i].position;
            keysToPressPanels[i].gameObject.GetComponent<Image>().color = panelColor;
        }
    }

    public virtual void RandomiseKeys() { }

    public virtual void StopSwinging(bool missed) { }

    public virtual void UpdateSlider() {
        // Move the swing meter according to the meterSpeed variable
        swingMeter.value += meterSpeed * Time.deltaTime;
    }

    //public virtual void CheckHitType(Color stoppingColor, Transform panel) { }
    // end virtual functions

    public float GetXPixelFromSwingMeterValue(float meterValue)
    {
        // Returns the pixel corresponding to the position of the swing meter click
        return (meterValue / 100f) * meterImage.width;
    }

    public Color GetColorAtStoppingPoint(float xPos)
    {
        float xPixel = GetXPixelFromSwingMeterValue(xPos);

        Color pixelColor = meterImage.GetPixel(Mathf.RoundToInt(xPixel), 0);

        return pixelColor;
    }

    public void Close()
    {
        //MoveListController mlc = FindObjectOfType<MoveListController>();
        //mlc.Cancel();

        // TESTING ONLY
        AnimationTest.instance.AttackAgain();

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Move");
        foreach (GameObject go in gos)
        {
            Destroy(go);
        }
    }
}
