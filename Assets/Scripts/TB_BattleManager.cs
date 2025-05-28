using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TB_BattleManager : MonoBehaviour
{
    //Spawn Points
    [SerializeField] Transform[] playerSpawnPoints;
    [SerializeField] Transform[] enemySpawnPoints;
    private Transform playerFrontSpawnPoint;
    private Transform playerMiddleSpawnPoint;
    private Transform playerBackSpawnPoint;
    //Lists
    private List<Unit> allUnits = new List<Unit>();
    private Queue<Unit> turnQueue = new Queue<Unit>();
    //Game States
    [HideInInspector] public enum GameStates { RealTime, TurnBased };
    [HideInInspector] public GameStates currentGameState = GameStates.RealTime;
    [HideInInspector] public bool playerTurn;
    [HideInInspector] public bool actionTaken;
    //Components
    RT_PlayerActions PlayerActions;
    // GameDataToStore
    [HideInInspector] public List<Unit.UnitData> StoredUnitData;

    void Start()
    {
        PlayerActions = GetComponent<RT_PlayerActions>();
    }

    public void SpawnAllUnits(List<GameObject> PlayerActors, List<GameObject> enemyActors)
    {
        // store unit data
        if (StoredUnitData == null)
        {
            foreach (var playerActor in PlayerActors)
            {
                StoredUnitData.Add(playerActor.GetComponent<Unit>().currentUnitData);
            }
        }
        for (int i = 0; i < PlayerActors.Count; i++)
            {
                GameObject[] playerTeamArray = PlayerActors.OrderByDescending(u => u.GetComponent<Unit>().currentUnitData.positionIndex).ToArray();
                GameObject player = Instantiate(playerTeamArray[i], playerSpawnPoints[i].position, Quaternion.identity);
                // check data and load for ally
            foreach (Unit.UnitData unitdata in StoredUnitData)
            {
                if (player.GetComponent<Unit>().currentUnitData.id == unitdata.id)
                {
                    player.GetComponent<Unit>().currentUnitData = unitdata;
                }
            }
                allUnits.Add(player.GetComponent<Unit>());
            }
        for (int i = 0; i < enemyActors.Count; i++)
        {
            {
                GameObject[] enemyTeamArray = enemyActors.OrderByDescending(u => u.GetComponent<Unit>().speed).ToArray();
                GameObject enemy = Instantiate(enemyTeamArray[i], enemySpawnPoints[i].position, quaternion.identity);
                allUnits.Add(enemy.GetComponent<Unit>());
            }
        }
    }

    private void GenerateTurnOrder(bool playerAdvantege)
    {
        turnQueue.Clear();
        List<Unit> aliveUnits = allUnits.Where(unit => unit.IsAlive()).ToList();

        if (playerAdvantege)
        {
           // Debug.Log("Suprise Attack");
            var playerUnits = aliveUnits
                .Where(unit => unit.actorTeamType == Unit.TeamType.Ally)
                .OrderByDescending(unit => unit.speed);
            var enemyUnits = aliveUnits
            .Where(unit => unit.actorTeamType == Unit.TeamType.Enemy)
            .OrderByDescending(unit => unit.speed);
            foreach (var unit in playerUnits) { turnQueue.Enqueue(unit); }
            foreach (var unit in enemyUnits) { turnQueue.Enqueue(unit); }
        }
        else if (!playerAdvantege)
        {
            Debug.Log("enemyadvantage");
            var enemyUnits = aliveUnits
            .Where(unit => unit.actorTeamType == Unit.TeamType.Enemy)
            .OrderByDescending(u => u.speed);

            var playerUnits = aliveUnits
            .Where(u => u.actorTeamType == Unit.TeamType.Ally)
            .OrderByDescending(u => u.speed);
            foreach (var u in enemyUnits) turnQueue.Enqueue(u);
            foreach (var u in playerUnits) turnQueue.Enqueue(u);
        }
        else
        {
            foreach (var unit in aliveUnits.OrderByDescending(u => u.currentUnitData.positionIndex))
            {
                turnQueue.Enqueue(unit);
            }
        }
        
        //foreach (var unit in turnQueue)
        //{
          //  Debug.Log(unit.name);
        //}
    }

    public void StartBattle(bool isPlayerAdvantage, List<GameObject> playerActors, List<GameObject> enemyActors)
    {
        PlayerActions.enabled = false;
        ChangeGameState(GameStates.TurnBased);
        SpawnAllUnits(playerActors, enemyActors);
        GenerateTurnOrder(isPlayerAdvantage);
        NextTurn();
    }

    public void ChangeGameState(GameStates state)
    {
        currentGameState = state;
    }

    public void StoreData()
    {
        StoredUnitData = (List<Unit.UnitData>)allUnits.Where(u => u.IsAlive() && u.actorTeamType == Unit.TeamType.Ally);
    }

    private void NextTurn()
    {
        if (turnQueue.Count == 0)
        {
            Debug.Log("No Unit Left");
            currentGameState = GameStates.RealTime;
        }
        List<Unit> aliveEnemies = turnQueue.Where(u => u.actorTeamType == Unit.TeamType.Enemy && u.IsAlive()).ToList();
        if (aliveEnemies.Count == 0)
        {
            Debug.Log("No Enemies Left");
            currentGameState = GameStates.RealTime;
        }
        Unit current = turnQueue.Dequeue();
        Debug.Log(current.name+ "'s Turn");
        
        if (current.actorTeamType == Unit.TeamType.Ally)
        {
            foreach (Button skillButton in UI_Manager.Instance.B_Skills)
            {
                skillButton.gameObject.SetActive(true);
                skillButton.onClick.AddListener(() => AllySkillUse(skillButton, current));
            }
        }
        else
        {
            foreach (Button skillButton in UI_Manager.Instance.B_Skills)
            {
                skillButton.gameObject.SetActive(false);
            }
        }
        turnQueue.Enqueue(current);
    }
 
    public void AllySkillUse(Button skillButton, Unit current)
    {
        switch (skillButton.name)
        {
            case "B_Attack":
                current.LightAttack.SkillActivation(current.gameObject, current.gameObject);
                current.animator.SetTrigger("LightAttack");
                NextTurn();
                break;
            case "B_Special":
                current.Special.SkillActivation(current.gameObject, current.gameObject);
                NextTurn();
                break;
            case "B_Defence":
                current.Defence.SkillActivation(current.gameObject, current.gameObject);
                NextTurn();
                break;
            case "B_Rest":
                current.Rest.SkillActivation(current.gameObject, current.gameObject);
                NextTurn();
                break;
            case "B_ChangePos":
                break;
        }
    }

}

