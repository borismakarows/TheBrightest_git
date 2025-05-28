using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

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
    public Skill ChangePosition;
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

    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    [System.Serializable]
    public class UnitData
    {
        public int id;
        public int currentHP;
        public int positionIndex;
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
 
    
}
