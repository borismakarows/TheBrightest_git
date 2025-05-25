using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Game/Skill")]
public class Skill : ScriptableObject
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

    public void SkillActivation(GameObject player)
    {
        
    }
}
