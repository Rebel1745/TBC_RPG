using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class SwingMeter : MonoBehaviour
{

    public Slider swingMeter;

    public GameObject Swinger;

    public Damage[] HitsDamage;

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
    public TMP_Text[] keysToPressText;
    // Define an array of transforms for the keysToPress (allows their position to follow the swingMeter.value)
    public Transform[] keysToPressPanels;
    // Define spawn points for the keysToPressPanels
    public Transform[] keysToPressPanelSpawns;
    // Keep a reference to the current moving panel
    public Transform currentPanel;
    public Transform currentPanelSpawn;

    public Image MeterImage;
    float meterImageHeight;
    float meterImageWidth;  // width of the original texture
    float meterImageWidthOnscreen; // width of the image onscreen
    float meterImageWidthRatio;  // get the ratio of the two sizes

    Dictionary<HIT_TYPE, Color> HitTypeColours = new Dictionary<HIT_TYPE, Color>();
    [SerializeField] Color NormalHitColour = Color.blue;
    [SerializeField] Color CriticalHitColour = Color.green;
    [SerializeField] Color WeakHitColour = Color.red;
    [SerializeField] Color MissColour = Color.black;

    [SerializeField] GameObject KeypressPanelSpawnPrefab;
    [SerializeField] GameObject KeypressPanelPrefab;
    [SerializeField] float KeypressPanelYOffset;

    [SerializeField] HitTypePercentage[] hitTypesAndPercentages;

    void Start()
    {
        meterImageHeight = MeterImage.sprite.texture.height;  // height of the original texture
        meterImageWidth = MeterImage.sprite.texture.width;  // width of the original texture
        meterImageWidthOnscreen = MeterImage.GetPixelAdjustedRect().width; // width of the image onscreen
        meterImageWidthRatio = meterImageWidth / meterImageWidthOnscreen;  // get the ratio of the two sizes

        HitTypeColours[HIT_TYPE.Normal] = NormalHitColour;
        HitTypeColours[HIT_TYPE.Critical] = CriticalHitColour;
        HitTypeColours[HIT_TYPE.Weak] = WeakHitColour;
        HitTypeColours[HIT_TYPE.Miss] = MissColour;

        CreateTexture(noOfSwings, hitTypesAndPercentages);

        InitialiseKeys();
        HitsDamage = new Damage[noOfSwings];
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

    public virtual void UpdateSlider()
    {
        // Move the swing meter according to the meterSpeed variable
        swingMeter.value += meterSpeed * Time.deltaTime;
    }
    // end virtual functions

    public float GetXPixelFromSwingMeterValue(float meterValue)
    {
        return (meterValue / 100f) * MeterImage.GetPixelAdjustedRect().width;
    }

    public Color GetColorAtStoppingPoint(float xPos)
    {
        float adjustedXPixel = GetXPixelFromSwingMeterValue(xPos * meterImageWidthRatio); // adjust the stopping pixel by the ratio

        Color pixelColor = MeterImage.sprite.texture.GetPixel(Mathf.RoundToInt(adjustedXPixel), 0);

        return pixelColor;
    }

    public void Close()
    {
        AttackController ac = Swinger.GetComponentInChildren<AttackController>();
        ac.PlayAbilityAnimation();
        ac.SetupDamage(HitsDamage);

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Move");
        foreach (GameObject go in gos)
        {
            Destroy(go);
        }
    }

    public void CreateTexture(int numberOfSwings, HitTypePercentage[] hitTypePercentages)
    {
        Texture2D currentTexture = MeterImage.sprite.texture;

        // get the total number from the percentages (ideally should be 100)
        float percentageTotal = 0;
        foreach (HitTypePercentage i in hitTypePercentages)
        {
            percentageTotal += i.Percentage;
        }

        // if the total is not 100, make it 100
        if (percentageTotal != 100f)
        {
            print("CreateTexture:: Percentage total is not 100. Recalculating...");
            for (int i = 0; i < hitTypePercentages.Length; i++)
            {
                hitTypePercentages[i].Percentage = (hitTypePercentages[i].Percentage / percentageTotal) * 100;
            }
        }

        HitTypePercentage[] hitTypePercentageMultiSwing = new HitTypePercentage[numberOfSwings * hitTypePercentages.Length];

        for (int i = 0; i < hitTypePercentageMultiSwing.Length; i++)
        {
            hitTypePercentageMultiSwing[i] = new HitTypePercentage(
                hitTypePercentages[i % hitTypePercentages.Length].HitType, 
                hitTypePercentages[i % hitTypePercentages.Length].Percentage / numberOfSwings
            );
        }

        // first, set all the pixels to black
        for (int y = 0; y < meterImageHeight; y++)
        {
            for (int x = 0; x < meterImageWidth; x++)
            {
                currentTexture.SetPixel(x, y, Color.black);
            }
        }
        currentTexture.Apply();

        // remove any currently created keypress panels
        foreach (Transform t in keysToPressPanels)
        {
            Destroy(t.gameObject);
        }
        // reset the array
        keysToPressPanels = new Transform[numberOfSwings];

        foreach (TMP_Text txt in keysToPressText)
        {
            Destroy(txt.gameObject);
        }
        // reset the array
        keysToPressText = new TMP_Text[numberOfSwings];

        // TODO: Remove references to the spawn pos
        foreach (Transform t in keysToPressPanelSpawns)
        {
            Destroy(t.gameObject);
        }
        keysToPressPanelSpawns = new Transform[numberOfSwings];

        int currentPos = 0, currentIndex = 0;
        GameObject newKeypressPanel;
        Vector3 panelSpawnPos = Vector3.zero;

        for (int i = 0; i < hitTypePercentageMultiSwing.Length; i++)
        {
            // is this the starting colour for a new swing?
            if (i % hitTypePercentages.Length == 0)
            {
                panelSpawnPos = new Vector3((currentPos / meterImageWidthRatio) - (meterImageWidthOnscreen / 2f), KeypressPanelYOffset, 0f);
                newKeypressPanel = Instantiate(KeypressPanelPrefab, MeterImage.transform);
                newKeypressPanel.GetComponent<RectTransform>().localPosition = panelSpawnPos;
                keysToPressPanels[currentIndex] = newKeypressPanel.transform;
                keysToPressText[currentIndex] = newKeypressPanel.GetComponentInChildren<TMP_Text>();
                // TODO Remove
                keysToPressPanelSpawns[currentIndex] = Instantiate(KeypressPanelSpawnPrefab, MeterImage.transform).transform;
                keysToPressPanelSpawns[currentIndex].GetComponent<RectTransform>().localPosition = panelSpawnPos;

                currentIndex++;
            }

            float percentageWidthInPixels = (hitTypePercentageMultiSwing[i].Percentage / 100f) * meterImageWidth;

            for (int y = 0; y < meterImageHeight; y++)
            {
                for (int x = currentPos; x < currentPos + percentageWidthInPixels; x++)
                {
                    currentTexture.SetPixel(x, y, HitTypeColours[hitTypePercentageMultiSwing[i].HitType]);
                }
            }
            currentPos += Mathf.RoundToInt(percentageWidthInPixels);
        }

        currentTexture.Apply();
    }
}

public enum HIT_TYPE
{
    Normal,
    Critical,
    Weak,
    Miss
}
