using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class TB_BattleManager : MonoBehaviour
{
    [SerializeField] Transform[] playerSpawnPoints;
    [SerializeField] Transform[] enemySpawnPoints;
    [SerializeField] GameObject[] playerPrefabs;
    [SerializeField] GameObject[] enemyPrefabs;
    private List<Unit> allUnits = new List<Unit>();
    private Queue<Unit> turnQueue = new Queue<Unit>();
    public bool playerAdvantage = false;
    public bool enemyAdvantage = false;

    void Start()
    {
        spawnAllUnits();
        GenerateTurnOrder();
        StartCoroutine(BattleLoop());
    }

    void spawnAllUnits()
    {
        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            GameObject player = Instantiate(playerPrefabs[i], playerSpawnPoints[i].position, Quaternion.identity);
            allUnits.Add(player.GetComponent<Unit>());
        }
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            GameObject enemy = Instantiate(enemyPrefabs[i], enemySpawnPoints[i].position, quaternion.identity);
            allUnits.Add(enemy.GetComponent<Unit>());
        }
    }
    void GenerateTurnOrder()
    {
        List<Unit> aliveUnits = allUnits.Where(unit => unit.IsAlive()).ToList();

        Debug.Log("Alive Units: " + aliveUnits.Count);
        if (playerAdvantage)
        {
            var playerUnits = aliveUnits
                .Where(unit => unit.team == Unit.teamType.Ally)
                .OrderByDescending(unit => unit.speed);
            var enemyUnits = aliveUnits
            .Where(unit => unit.team == Unit.teamType.Enemy)
            .OrderByDescending(unit => unit.speed);
            foreach (var unit in playerUnits) turnQueue.Enqueue(unit);
            foreach (var unit in enemyUnits) turnQueue.Enqueue(unit);
        }
        else if (enemyAdvantage)
        {
            var enemyUnits = aliveUnits
            .Where(unit => unit.team == Unit.teamType.Enemy)
            .OrderByDescending(u => u.speed);

            var playerUnits = aliveUnits
            .Where(u => u.team == Unit.teamType.Ally)
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
    }

    IEnumerator BattleLoop()
    {
        while (true)
        {
            if (turnQueue.Count == 0)
            {
                playerAdvantage = false;
                enemyAdvantage = false;
                GenerateTurnOrder();
                if (turnQueue.Count == 0)
                { Debug.Log("Battle over! no Units"); yield break; }
            }

            Unit currentUnit = turnQueue.Dequeue();

            if (!currentUnit.IsAlive()) continue;

            Debug.Log(currentUnit.team + "'s Turnn");

            if (currentUnit.team == Unit.teamType.Ally)
            {
                yield return StartCoroutine(AllyTurn(currentUnit));
            }
            else if (currentUnit.team == Unit.teamType.Enemy)
            {
                yield return StartCoroutine(EnemyTurn(currentUnit));
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator AllyTurn(Unit unit)
    {
        Debug.Log("ALLY TURN");

        Unit target = allUnits.FirstOrDefault(u => u.team == Unit.teamType.Enemy && u.IsAlive());
        if (target != null)
        {
            target.TakeDamage(20);
        }
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator EnemyTurn(Unit unit)
    {
        Debug.Log("ENEMY TURN");
        Unit target = allUnits.FirstOrDefault(u => u.team == Unit.teamType.Ally && u.IsAlive());
        if (target != null)
        {
            target.TakeDamage(10);
        }
        yield return new WaitForSeconds(1f);
    }


}
