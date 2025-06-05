using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
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
    public int currentActionPoints;
    public string unitName;
    [SerializeField] int maxHP;
    public int speed;
    [SerializeField] Sprite unitIcon;
    private bool isAlive = true;
    [HideInInspector] bool isStunned;
    [HideInInspector] public bool isGuarded;
    public float deathDelayOffset;
    public Transform staticSkillSpawnPoint;
    public UnitData currentUnitData;
    [HideInInspector] public int guardReduction;
    // Coroutines
    private Coroutine moveCoroutine;
    private Coroutine ProjectileSpawnCoroutine;
    //Components
    [HideInInspector] public Animator animator;
    public GameObject damageTextPrefab;
    
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

    public void TakeDamage(int damage, bool stun, float hurtDelay)
    {
        isStunned = stun;
        if (isGuarded)
        {
            isGuarded = false;
            damage -= guardReduction;
            damage = Mathf.Clamp(damage, 0, 100);
        }
        if (currentUnitData.currentHP - damage < 0)
        {
            isAlive = false;
            currentUnitData.currentHP = 0;
            StartCoroutine(DeadEvents("Death", hurtDelay, hurtDelay + deathDelayOffset));
        }
        else if (damage > 0)
        {
            StartCoroutine(TriggerHurtAnimationwithDelay("Hurt", hurtDelay, damage));
        }
        currentUnitData.currentHP -= damage;
        if (actorTeamType == TeamType.Enemy)
        { gameObject.GetComponent<Enemy>().h_slider.value = currentUnitData.currentHP / 100f; }
    }

    private void ShowDamage(int damage, Color color)
    {
        float randomX = UnityEngine.Random.Range(-0.2f, 0.2f);
        float randomY = UnityEngine.Random.Range(0.1f, 0.2f);
        Vector3 spawnPos = transform.position + new Vector3(randomX, randomY, 0);
        GameObject popup = Instantiate(damageTextPrefab, spawnPos, quaternion.identity);
        popup.GetComponent<PopupDamage>().SetText(damage.ToString());
        popup.GetComponent<PopupDamage>().ChangeColor(color);
    }
    private IEnumerator DeadEvents(string triggername, float hurtdelay, float delayfordisableUnit)
    {
        yield return new WaitForSeconds(hurtdelay);
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(delayfordisableUnit);
        gameObject.SetActive(false);
    }

    private IEnumerator TriggerHurtAnimationwithDelay(string triggerName, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger(triggerName);
        ShowDamage(damage, Color.red);
    }
    public bool IsAlive()
    {
        return isAlive;
    }

    public void RestoreSavedData(UnitData Data)
    {
        currentUnitData = Data;
    }

    public void MoveToTarget(GameObject user, Vector2 target, float speed, string animationTrigger, float AcceptedRadius, float delayOfReturn)
    {
        Transform OriginalPosition = user.transform;
        if (moveCoroutine != null)
            user.GetComponent<MonoBehaviour>().StopCoroutine(moveCoroutine);
        moveCoroutine = user.GetComponent<MonoBehaviour>().StartCoroutine(MoveToPosition(user, target, speed, animationTrigger, AcceptedRadius, delayOfReturn));
    }

    private IEnumerator MoveToPosition(GameObject user, Vector2 target, float speed, string animationTrigger, float AcceptedRadius, float delayOfReturn)
    {
        Vector2 OriginalPos = user.transform.position;
        Transform t = user.transform;
        while (Vector2.Distance(t.position, target) > AcceptedRadius)
        {
            Vector2 newPos = Vector2.MoveTowards(t.position, target, speed * Time.deltaTime);
            t.position = new Vector3(newPos.x, t.position.y, t.position.z);
            yield return null;
        }
        if (animationTrigger != null) { user.GetComponent<Unit>().animator.SetTrigger(animationTrigger); }
        yield return new WaitForSeconds(delayOfReturn);

        while (Vector2.Distance(t.position, OriginalPos) > 0.01f)
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
        if (user.GetComponent<Unit>().isAlive)
        {
            ProjectileSpawnCoroutine = user.GetComponent<MonoBehaviour>().StartCoroutine(SpawnProjectileWithDelay(spawnPos, vfxPrefab, Damage, Direction, canStun, delay, skillOffset));
        }
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
        GameObject closestTarget = null;
        float shortestDistance = float.MaxValue;

        foreach (GameObject target in targets)
        {
            float currentDistance = Mathf.Abs(target.transform.position.x - user.transform.position.x);
            if (currentDistance < shortestDistance)
            {
                shortestDistance = currentDistance;
                closestTarget = target;
            }
        }
        if (closestTarget != null)
        { Debug.Log("Closest Target: " + closestTarget.name); }
        else { Debug.Log("No Target found"); }
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
        else
        {
            currentUnitData.currentHP += Mathf.CeilToInt(healAmount);
        }
    }
    public void LifeSteal(GameObject user, GameObject[] targets, float lifeStealRatio, bool AOE)
    {
        if (!AOE)
        {
            GameObject target = FindClosestTarget(user, targets);
            if (target != null)
            {
                Unit targetUnit = target.GetComponent<Unit>();
                float LifestealAmount = targetUnit.currentUnitData.currentHP / lifeStealRatio / 100f;
                targetUnit.currentUnitData.currentHP -= LifestealAmount;
                ShowDamage(Mathf.CeilToInt(LifestealAmount), Color.green);
                Heal(Mathf.CeilToInt(LifestealAmount));
            }
        }
        else
        {
            foreach (GameObject target in targets)
            {
                Unit targetUnit = target.GetComponent<Unit>();
                float LifestealAmount = targetUnit.currentUnitData.currentHP / lifeStealRatio / 100f;
                targetUnit.currentUnitData.currentHP -= LifestealAmount;
                ShowDamage(Mathf.CeilToInt(LifestealAmount), Color.green);
                Heal(LifestealAmount);
            }
        }

    }

    public void DamageAllEnemies(int damage, GameObject[] targets, float hurtDelay)
    {
        foreach (GameObject target in targets)
        {
            target.GetComponent<Unit>().TakeDamage(damage, false, hurtDelay);
        }
    }
}
