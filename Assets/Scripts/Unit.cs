using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum TeamType { Ally, Enemy };
    public TeamType actorTeamType;
    public Skills[] skills;
    [SerializeField] int maxActionPoints;
    [SerializeField] string unitName;
    [SerializeField] int maxHP;
    public int speed;
    [SerializeField] Sprite unitIcon;
    private bool isAlive = true;
    [HideInInspector] public UnitData currentUnitData;

    public class UnitData
    {
        public int id;
        public int currentHP;
        public int currentActionPoints;

    }

    public void TakeDamage(int damage)
    {
        currentUnitData.currentHP -= damage;
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
