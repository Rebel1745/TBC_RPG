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
    [SerializeField] Transform actionList;
    [SerializeField] Transform startBattlePanel;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowHideInfoBar(false);
        ShowHideAttackList(false);
        ShowHideActionList(false);
        //SetInfoBarText("Player One, select your attack");
    }

    public void ShowHideBattlePanel(bool show)
    {
        startBattlePanel.gameObject.SetActive(show);
    }

    public void ShowHideActionList(bool show)
    {
        actionList.gameObject.SetActive(show);
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
