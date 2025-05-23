using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum teamType { Ally, Enemy };
    public teamType team;
    public Skills[] skills;
    [SerializeField] int maxActionPoints;
    [SerializeField] int currentActionPoints;
    [SerializeField] string unitName;
    [SerializeField] int maxHP;
    [SerializeField] int currentHP;
    public int speed;
    [SerializeField] Sprite unitIcon;

    private bool isAlive = true;


    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0)
        {
            isAlive = false;
            currentHP = 0;
            Debug.Log($"{unitName} has died.");
        }
    }
    public bool IsAlive()
    {
        return isAlive;
    }


}
