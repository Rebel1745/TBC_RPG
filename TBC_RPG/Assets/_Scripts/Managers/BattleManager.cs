using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemyPrefab;

    [SerializeField] Transform startBattlePanel;
    [SerializeField] Transform attackPanel;

    GameObject[] characters;
    GameObject currentCharacter;

    public BATTLE_STATUS battleStatus { get; protected set; }

    //
    // TODO: connect the buttons on the attack panel to the select attack function so when an attack is selected it is shown on screen
    //

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        startBattlePanel.gameObject.SetActive(true);
    }

    public void StartBattle()
    {
        // first, hide the start battle button
        startBattlePanel.gameObject.SetActive(false);

        // spawn the characters and set the nodes they start on as unwalkable
        Node playerSpawnNode = NodeGrid.instance.NodeFromWorldPoint( new Vector3(-2.5f, 0f, -1.5f));
        GameObject player = Instantiate(playerPrefab, playerSpawnNode.worldPosition, Quaternion.identity, transform);
        playerSpawnNode.SetNodeCharacter(player, false);

        Node enemySpawnNode = NodeGrid.instance.NodeFromWorldPoint(new Vector3(-1.5f, 0f, -1.5f));
        GameObject enemy = Instantiate(enemyPrefab, enemySpawnNode.worldPosition, Quaternion.identity, transform);
        enemySpawnNode.SetNodeCharacter(enemy, false);

        // add the two characters to the character array
        characters = new GameObject[2];
        characters[0] = player;
        characters[1] = enemy;

        // set the player as the current character
        currentCharacter = characters[0];

        ChangeBattleStatus(BATTLE_STATUS.WaitingForMove);
    }

    public void ChangeBattleStatus(BATTLE_STATUS newStatus)
    {
        battleStatus = newStatus;

        switch (newStatus)
        {
            case BATTLE_STATUS.WaitingForMove:
                currentCharacter.GetComponent<PlayerMovement>().ShowAvailableMoves();
                break;
            case BATTLE_STATUS.WaitingForAttackSelection:
                attackPanel.gameObject.SetActive(true);
                break;
            case BATTLE_STATUS.WaitingForTarget:
                break;
        }
    }

    public void SelectAttack()
    {

    }

    public void SelectTarget()
    {

    }
}

public enum BATTLE_STATUS
{
    WaitingForMove,
    WaitingForAttackSelection,
    WaitingForTarget
}
