using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Game/Skill")]
public class Skills : ScriptableObject
{
    public enum SkillTypes
    {
        Light,
        Special,
        Defense,
        Rest
    };
    [SerializeField] SkillTypes skillType;
    [SerializeField] string skillName;
    [SerializeField] string skillDescription; 
    [SerializeField] int cost;
    [SerializeField] int damage;
    [SerializeField] GameObject effectPrefab;
}
