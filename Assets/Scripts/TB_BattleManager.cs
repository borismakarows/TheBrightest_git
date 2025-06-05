using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TB_BattleManager : MonoBehaviour
{
    //Spawn Points
    [SerializeField] Transform[] playerSpawnPoints;
    [SerializeField] Transform[] enemySpawnPoints;
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
    public GameObject TurnBanner;
    public TextMeshProUGUI t_ActionPoints;
    public Slider h_slider;

    // GameDataToStore
    [HideInInspector] public List<Unit.UnitData> StoredUnitData;

    void Start()
    {
        PlayerActions = GetComponent<RT_PlayerActions>();
        TurnBanner.SetActive(false);
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
                GameObject[] enemyTeamArray = enemyActors.OrderByDescending(u => u.GetComponent<Unit>().currentUnitData.positionIndex).ToArray();
                GameObject enemy = Instantiate(enemyTeamArray[i], enemySpawnPoints[i].position, quaternion.identity);
                enemy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
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
        StartCoroutine(NextTurn(0));
    }

    public void ChangeGameState(GameStates state)
    {
        currentGameState = state;
    }

    public void StoreData()
    {
        StoredUnitData = (List<Unit.UnitData>)allUnits.Where(u => u.IsAlive() && u.actorTeamType == Unit.TeamType.Ally);
    }

    public IEnumerator NextTurn(float delaybeforeTurn)
    {
        yield return new WaitForSecondsRealtime(delaybeforeTurn);
        if (turnQueue.Count == 0)
        {
            Debug.Log("No Unit Left");
            currentGameState = GameStates.RealTime;
            ReloadTheScene();
        }
        List<Unit> aliveEnemies = turnQueue.Where(u => u.actorTeamType == Unit.TeamType.Enemy && u.IsAlive()).ToList();
        List<Unit> aliveAllies = turnQueue.Where(u => u.actorTeamType == Unit.TeamType.Ally && u.IsAlive()).ToList();
        if (aliveEnemies.Count == 0)
        {
            Debug.Log("No Enemies Left");
            currentGameState = GameStates.RealTime;
            ReloadTheScene();
        }
        Unit current = turnQueue.Dequeue();
        if (current.actorTeamType == Unit.TeamType.Ally)
        {
            current.currentActionPoints += 1;
            if (current.IsAlive())
            {
                h_slider.gameObject.SetActive(true);
                h_slider.value = current.currentUnitData.currentHP / 100;
                t_ActionPoints.gameObject.SetActive(true);
                t_ActionPoints.SetText("Action Points: " + current.currentActionPoints.ToString());
                if (current.currentUnitData.id == 2) { current.isGuarded = false; Debug.Log("GuardOff"); }
                foreach (Button skillButton in UI_Manager.Instance.B_Skills)
                {
                    skillButton.onClick.RemoveAllListeners();
                    skillButton.gameObject.SetActive(true);
                    skillButton.onClick.AddListener(() => AllySkillUse(skillButton, current, aliveEnemies));
                }
            }
            else
            {
                StartCoroutine(NextTurn(0));
            }
        }

        else if (current.actorTeamType == Unit.TeamType.Enemy)
        {
            h_slider.gameObject.SetActive(false);
            t_ActionPoints.gameObject.SetActive(false);
            foreach (Button skillButton in UI_Manager.Instance.B_Skills)
            {
                skillButton.gameObject.SetActive(false);
            }
            EnemyAttacks(current, aliveAllies);
        }
        StartCoroutine(SetTurnIndicatorOn(current));
        Debug.Log(aliveAllies.Count);
        turnQueue.Enqueue(current);
    }

    public void AllySkillUse(Button skillButton, Unit current, List<Unit> targetUnits)
    {
        skillButton.onClick.RemoveAllListeners();
        List<GameObject> targets = new();
        foreach (Unit targetUnit in targetUnits)
        {
            targets.Add(targetUnit.gameObject);
        }
        List<Unit> alivetargets = targetUnits.Where(u => u.IsAlive()).ToList();
        if (alivetargets.Count == 0) { ReloadTheScene(); }
        switch (skillButton.name)
        {
            case "B_Attack":
                if (CheckCost(current.LightAttack, current))
                {
                    skillButton.gameObject.SetActive(true);
                    current.LightAttack.SkillActivation(current.gameObject, targets.ToArray());
                    StartCoroutine(NextTurn(current.LightAttack.skillDuration));
                }
                else { Debug.Log("not enough action points"); }
                break;
            case "B_Special":
                if (CheckCost(current.Special, current))
                {
                    skillButton.gameObject.SetActive(true);
                    StartCoroutine(NextTurn(current.Special.skillDuration));
                    current.Special.SkillActivation(current.gameObject, targets.ToArray());
                }
                else { Debug.Log("not enough action points"); }
                break;
            case "B_Defence":
                if (CheckCost(current.Defence, current))
                {
                    skillButton.gameObject.SetActive(true);
                    current.Defence.SkillActivation(current.gameObject, targets.ToArray());
                    StartCoroutine(NextTurn(current.Defence.skillDuration));
                }
                else { Debug.Log("not enough action points"); }
                break;
            case "B_Rest":
                if (CheckCost(current.Rest, current))
                {
                    skillButton.gameObject.SetActive(true);
                    current.Rest.SkillActivation(current.gameObject, targets.ToArray());
                    StartCoroutine(NextTurn(current.Rest.skillDuration));
                }
                else { Debug.Log("not enough action points"); }
                break;
        }
    }

    private bool CheckCost(Skill skill, Unit current)
    {
        bool canUseSkill = false;
        if (current.currentActionPoints >= skill.GetCost())
        {
            canUseSkill = true;
        }
        return canUseSkill;
    }

    private void EnemyAttacks(Unit current, List<Unit> targetUnits)
    {
        List<Unit> aliveTargets = targetUnits.Where(u => u.IsAlive()).ToList();
        if (aliveTargets.Count == 0)
        {
            ReloadTheScene();
        }
        else
        {
            if (current.actorTeamType == Unit.TeamType.Enemy && current.IsAlive())
            {
                List<GameObject> targets = new();
                foreach (Unit targetUnit in targetUnits)
                {
                    targets.Add(targetUnit.gameObject);
                }
                current.LightAttack.SkillActivation(current.gameObject, targets.ToArray());
                StartCoroutine(NextTurn(current.LightAttack.skillDuration));
        }
            else { StartCoroutine(NextTurn(0f)); }
        }
    }

    private IEnumerator SetTurnIndicatorOn(Unit current)
    {
        TurnBanner.GetComponent<TurnBanner>().SetTurnIndicator(current.unitName);
        TurnBanner.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        TurnBanner.SetActive(false);
    }

    private void ReloadTheScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

