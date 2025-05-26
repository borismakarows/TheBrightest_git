using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public enum TeamType { Ally, Enemy };
    public TeamType actorTeamType;
    public Skill LightAttack;
    public Skill Special;
    public Skill Defence;
    public Skill Rest;
    [SerializeField] int maxActionPoints;
    [SerializeField] string unitName;
    [SerializeField] int maxHP;
    public int speed;
    [SerializeField] Sprite unitIcon;
    private bool isAlive = true;
    [HideInInspector] bool isStunned;
    public UnitData currentUnitData;
    public int positionIndex;
    [System.Serializable]
    public class UnitData
    {
        public int id;
        public int currentHP;
        public int currentActionPoints;
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
