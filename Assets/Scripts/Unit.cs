using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
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
    public int currentActionPoints;
    [SerializeField] string unitName;
    [SerializeField] int maxHP;
    public int speed;
    [SerializeField] Sprite unitIcon;
    private bool isAlive = true;
    [HideInInspector] bool isStunned;
    public UnitData currentUnitData;
    private Coroutine moveCoroutine;
    
    public Animator animator;
    [System.Serializable]
    public class UnitData
    {
        public int id;
        public int currentHP;
        public int positionIndex;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage, bool stun)
    {
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

    public void MoveToTarget(GameObject user, Vector2 target, float speed)
    {
        Transform OriginalPosition = user.transform;
        if (moveCoroutine != null)
            user.GetComponent<MonoBehaviour>().StopCoroutine(moveCoroutine);
        moveCoroutine = user.GetComponent<MonoBehaviour>().StartCoroutine(MoveToPosition(user, target, speed)); 
    }

    private IEnumerator MoveToPosition(GameObject user, Vector2 target, float speed)
    {
        Vector2 OriginalPos = user.transform.position;
        Transform t = user.transform;
        while (Vector2.Distance(t.position, target) > 1f)
        {
            Vector2 newPos = Vector2.MoveTowards(t.position, target, speed * Time.deltaTime);
            t.position = new Vector3(newPos.x, t.position.y, t.position.z);
            yield return null;
        }
        user.GetComponent<Unit>().animator.SetTrigger("Special");
        yield return new WaitForSeconds(1.8f);

        while (Vector2.Distance(t.position, OriginalPos) > 0.1f)
        {
            t.position = Vector2.MoveTowards(t.position, OriginalPos, speed * Time.deltaTime);
            yield return null;
        }

    }
}
