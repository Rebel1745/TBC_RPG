using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] Transform infoBar;
    [SerializeField] TMP_Text infoBarText;
    [SerializeField] Transform attackList;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowHideInfoBar(false);
        ShowHideAttackList(false);
        SetInfoBarText("Player One, select your attack");
    }

    public void ShowHideAttackList(bool show)
    {
        attackList.gameObject.SetActive(show);
    }

    public void ShowHideInfoBar(bool show)
    {
        infoBar.gameObject.SetActive(show);
    }

    public void SetInfoBarText(string newText)
    {
        infoBarText.text = newText;
    }

    public string GetInfoBarText()
    {
        return infoBarText.text;
    }
}
