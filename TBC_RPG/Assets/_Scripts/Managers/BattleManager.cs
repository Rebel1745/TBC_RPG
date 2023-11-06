using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemyPrefab;

    GameObject[] characters;
    GameObject currentCharacter;
    public int currentCharacterId { get; protected set; }
    int totalCharacters = 2;

    public BATTLE_STATUS battleStatus { get; protected set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UIManager.instance.ShowHideBattlePanel(true);
    }

    public void StartBattle()
    {
        // first, hide the start battle button
        UIManager.instance.ShowHideBattlePanel(false);

        // show the info bar
        UIManager.instance.ShowHideInfoBar(true);

        // spawn the characters and set the nodes they start on as unwalkable
        Node playerSpawnNode = NodeGrid.instance.NodeFromWorldPoint( new Vector3(-3.5f, 0f, -1.5f));
        GameObject player = Instantiate(playerPrefab, playerSpawnNode.worldPosition, Quaternion.identity, transform);
        playerSpawnNode.SetNodeCharacter(player, false);

        Node enemySpawnNode = NodeGrid.instance.NodeFromWorldPoint(new Vector3(-1.5f, 0f, -1.5f));
        GameObject enemy = Instantiate(enemyPrefab, enemySpawnNode.worldPosition, Quaternion.identity, transform);
        enemySpawnNode.SetNodeCharacter(enemy, false);

        // make them look at each other
        player.transform.LookAt(enemy.transform);
        enemy.transform.LookAt(player.transform);

        // add the two characters to the character array
        characters = new GameObject[2];
        characters[0] = player;
        characters[1] = enemy;

        // set the player as the current character
        currentCharacter = characters[currentCharacterId];

        ChangeBattleStatus(BATTLE_STATUS.WaitingForAction);
    }

    IEnumerator ChangeBattleStatus(BATTLE_STATUS newStatus, float delay)
    {
        yield return new WaitForSeconds(delay);

        ChangeBattleStatus(newStatus);
    }

    public void ChangeBattleStatus(BATTLE_STATUS newStatus)
    {
        battleStatus = newStatus;

        switch (newStatus)
        {
            case BATTLE_STATUS.WaitingForAction:
                UIManager.instance.SetInfoBarText("Select an action to perform");
                UIManager.instance.ShowHideActionList(true);
                break;
            case BATTLE_STATUS.WaitingForMove:
                UIManager.instance.SetInfoBarText("Select a location to move to");
                currentCharacter.GetComponent<PlayerMovement>().ShowAvailableMoves();
                break;
            case BATTLE_STATUS.WaitingForTarget:
                UIManager.instance.SetInfoBarText("Select a target to attack");
                break;
            case BATTLE_STATUS.WaitingForAttackSelection:
                UIManager.instance.SetInfoBarText("Select an attack to perform");
                UIManager.instance.ShowHideAttackList(true);
                break;
            case BATTLE_STATUS.WaitingForAttack:
                break;
            case BATTLE_STATUS.NextTurn:
                NextTurn();
                break;
        }
    }

    void NextTurn()
    {
        print("BattleManger::NextTurn");
        currentCharacterId = (currentCharacterId + 1) % totalCharacters;
        currentCharacter = characters[currentCharacterId];
        //print(currentCharacter.name + " (" + currentCharacterId + ")");
        ChangeBattleStatus(BATTLE_STATUS.WaitingForAction);
    }

    public void SelectAttack(string attackName)
    {
        currentCharacter.GetComponent<AttackController>().SetupAttack(attackName);
        ChangeBattleStatus(BATTLE_STATUS.WaitingForAttack);
    }

    public void SelectTarget(GameObject target)
    {
        currentCharacter.transform.LookAt(target.transform);
        currentCharacter.GetComponent<AttackController>().SetTarget(target);
        ChangeBattleStatus(BATTLE_STATUS.WaitingForAttackSelection);
    }

    public void SelectAction(string action)
    {
        switch (action)
        {
            case "move":
                StartCoroutine( ChangeBattleStatus(BATTLE_STATUS.WaitingForMove, 1f));
                break;
            case "attack":
                ChangeBattleStatus(BATTLE_STATUS.WaitingForTarget);
                break;
        }

        UIManager.instance.ShowHideActionList(false);
    }
}

public enum BATTLE_STATUS
{
    WaitingForAction,
    WaitingForMove,
    WaitingForTarget,
    WaitingForAttackSelection,
    WaitingForAttack,
    NextTurn
}
