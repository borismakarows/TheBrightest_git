using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Unit : MonoBehaviour
{
    //Team Type
    public enum TeamType { Ally, Enemy };
    public TeamType actorTeamType;
    //Skill
    public Skill LightAttack;
    public Skill Special;
    public Skill Defence;
    public Skill Rest;
    //Stats
    [SerializeField] int maxActionPoints;
    public int currentActionPoints { get; set; }
    [SerializeField] string unitName;
    [SerializeField] int maxHP;
    public int speed;
    [SerializeField] Sprite unitIcon;
    private bool isAlive = true;
    [HideInInspector] bool isStunned;
    [HideInInspector] public bool isGuarded;
    public Transform staticSkillSpawnPoint;
    public UnitData currentUnitData;
    [HideInInspector] public int guardReduction;
    // Coroutines
    private Coroutine moveCoroutine;
    private Coroutine ProjectileSpawnCoroutine;
    [HideInInspector] public Animator animator;

    [System.Serializable]
    public class UnitData
    {
        public int id;
        public float currentHP;
        public int positionIndex;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        staticSkillSpawnPoint = GameObject.Find("Middle Spawn Point").transform;
    }

    public void TakeDamage(int damage, bool stun)
    {
        if (isGuarded)
        {
            isGuarded = false;
            damage -= guardReduction;
            damage = Mathf.Clamp(damage, 0, 100);
        }
        currentUnitData.currentHP -= damage;
        isStunned = stun;
        if (currentUnitData.currentHP < 0)
        {
            isAlive = false;
            currentUnitData.currentHP = 0;
            Debug.Log($"{unitName} has died.");
        }
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void RestoreSavedData(UnitData Data)
    {
        currentUnitData = Data;
    }

    public void MoveToTarget(GameObject user, Vector2 target, float speed, string animationTrigger, float AcceptedRadius)
    {
        Transform OriginalPosition = user.transform;
        if (moveCoroutine != null)
            user.GetComponent<MonoBehaviour>().StopCoroutine(moveCoroutine);
        moveCoroutine = user.GetComponent<MonoBehaviour>().StartCoroutine(MoveToPosition(user, target, speed,animationTrigger,AcceptedRadius));
    }

    private IEnumerator MoveToPosition(GameObject user, Vector2 target, float speed, string animationTrigger, float AcceptedRadius)
    {
        Vector2 OriginalPos = user.transform.position;
        Transform t = user.transform;
        while (Vector2.Distance(t.position, target) > AcceptedRadius)
        {
            Vector2 newPos = Vector2.MoveTowards(t.position, target, speed * Time.deltaTime);
            t.position = new Vector3(newPos.x, t.position.y, t.position.z);
            yield return null;
        }
        if (animationTrigger != null){user.GetComponent<Unit>().animator.SetTrigger(animationTrigger);}
        yield return new WaitForSeconds(1.5f);

        while (Vector2.Distance(t.position, OriginalPos) > 0.1f)
        {
            t.position = Vector2.MoveTowards(t.position, OriginalPos, speed * Time.deltaTime);
            yield return null;
        }

    }

    public void ProjectileSpawn
    (GameObject user, Transform spawnPos, GameObject vfxPrefab, int Damage, Vector2 Direction, bool canStun, float delay, Vector3 skillOffset)
    {
        //if (ProjectileSpawnCoroutine != null)
          //  user.GetComponent<MonoBehaviour>().StopCoroutine(ProjectileSpawnCoroutine);
        ProjectileSpawnCoroutine = user.GetComponent<MonoBehaviour>().StartCoroutine(SpawnProjectileWithDelay(spawnPos, vfxPrefab, Damage, Direction, canStun, delay,skillOffset));
    }

    private IEnumerator SpawnProjectileWithDelay
    (Transform spawnPos, GameObject vfxPrefab, int Damage, Vector2 Direction, bool canStun, float delay, Vector3 skillOffset)
    {
        yield return new WaitForSeconds(delay);
        GameObject Projectile = Instantiate(vfxPrefab, spawnPos.position + skillOffset, quaternion.identity);
        Projectile.GetComponent<Projectile>().SetProjectileProperties(Damage, Direction, canStun);
    }

    public GameObject FindClosestTarget(GameObject user, GameObject[] targets)
    {
        GameObject closestTarget = targets[0];
        float distance = 5000000;
        foreach (GameObject target in targets)
        {
            if (Vector2.Distance(target.transform.position, user.transform.position) < distance)
            {
                distance = Vector2.Distance(target.transform.position, user.transform.position);
                closestTarget = target;
            }
        }
        Debug.Log("Closest Target: " + closestTarget.name);
        return closestTarget;
    }

    public GameObject SpawnStaticSkill(Transform spawnPos, GameObject vfxPrefab, Vector3 SkillOffset)
    {
        spawnPos.position += SkillOffset;
        GameObject staticSkill = Instantiate(vfxPrefab, spawnPos.position, quaternion.identity);
        return staticSkill;
    }

    public void Heal(float healAmount)
    {
        if (currentUnitData.currentHP > maxHP)
        {
            currentUnitData.currentHP = maxHP;
        }
        else { currentUnitData.currentHP += healAmount; }
    }
    public void LifeSteal(GameObject user, GameObject[] targets, float lifeStealRatio, bool AOE)
    {
        if (!AOE)
        {
            GameObject target = FindClosestTarget(user, targets);
            if (target != null)
            {
                Unit targetUnit = target.GetComponent<Unit>();
                float LifestealAmount = targetUnit.currentUnitData.currentHP / lifeStealRatio;
                targetUnit.currentUnitData.currentHP -= LifestealAmount;
                Heal(LifestealAmount);
            }
        }
        else
        {
            foreach (GameObject target in targets)
            {
                Unit targetUnit = target.GetComponent<Unit>();
                float LifestealAmount = targetUnit.currentUnitData.currentHP / lifeStealRatio;
                targetUnit.currentUnitData.currentHP -= LifestealAmount;
                Heal(LifestealAmount);
            }
        }
        
    }

}
