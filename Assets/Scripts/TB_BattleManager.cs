using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class TB_BattleManager : MonoBehaviour
{
    //Prefabs
    [SerializeField] Transform[] playerSpawnPoints;
    [SerializeField] Transform[] enemySpawnPoints;
    //Lists
    private List<Unit> allUnits = new List<Unit>();
    private Queue<Unit> turnQueue = new Queue<Unit>();
    //Game States
    [HideInInspector] public enum GameStates { RealTime, TurnBased };
    [HideInInspector] public GameStates currentGameState = GameStates.RealTime;
    [HideInInspector] public bool playerTurn;
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
        for (int i = 0; i < PlayerActors.Count; i++)
        {
            GameObject[] playerTeamArray = PlayerActors.OrderByDescending(u => u.GetComponent<Unit>().speed).ToArray();
            GameObject player = Instantiate(playerTeamArray[i], playerSpawnPoints[i].position, Quaternion.identity);
            if (StoredUnitData != null)
            {
                foreach (Unit.UnitData unitdata in StoredUnitData)
                {
                    if (player.GetComponent<Unit>().currentUnitData.id == unitdata.id)
                    {
                        player.GetComponent<Unit>().currentUnitData = unitdata;
                    }
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

        Debug.Log("Alive Units: " + aliveUnits.Count);
        if (playerAdvantege)
        {
            Debug.Log("playeradvantage");
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
            foreach (var unit in aliveUnits.OrderByDescending(u => u.speed))
            {
                turnQueue.Enqueue(unit);
            }
        }
        foreach (var unit in turnQueue)
        {
            Debug.Log("Name: " + unit.name + "UnitTeam:" + unit.actorTeamType);
        }
    }

    public void StartBattle(bool isPlayerAdvantage, List<GameObject> playerActors, List<GameObject> enemyActors)
    {
        PlayerActions.enabled = false;
        ChangeGameState(GameStates.TurnBased);
        SpawnAllUnits(playerActors, enemyActors);
        GenerateTurnOrder(isPlayerAdvantage);
        Turn();
    }
    public void IndexingSelectedObject(List<GameObject> selectedList, GameObject selected, int index)
    {
        selectedList.Remove(selected);
        selectedList.Insert(index, selected);
    }

    public void ChangeGameState(GameStates state)
    {
        currentGameState = state;
    }

    public void StoreData()
    {
        StoredUnitData = (List<Unit.UnitData>)allUnits.Where(u => u.IsAlive() && u.actorTeamType == Unit.TeamType.Ally);
    }

    private void Turn()
    {
        if (turnQueue.Count == 0)
        {
            Debug.Log("No UnitLeft");
            currentGameState = GameStates.RealTime;
        }
        List<Unit> aliveEnemies = turnQueue.Where(u => u.actorTeamType == Unit.TeamType.Enemy && u.IsAlive()).ToList();
        if (aliveEnemies.Count == 0)
        {
            Debug.Log("NoEnemiesLeft");
            currentGameState = GameStates.RealTime;
        }
        Unit current = turnQueue.Dequeue();
        Debug.Log(current.name + "'s Turn!");


        turnQueue.Enqueue(current);
    }

}

