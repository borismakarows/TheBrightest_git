using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Game/Skill")]
public class Skill : ScriptableObject
{
    public enum SkillTypes
    {
        LightAttack,
        Special,
        Defense,
        Rest
    };
    public SkillTypes skillType;
    public string skillName;
    public string skillDescription;
    public int cost;
    public int damage;
    [SerializeField] GameObject effectPrefab;

    public void SkillActivation(string text)
    {
        Debug.Log("Skill Used By: " + text + "Skill Type: " + skillType.ToString());
    }
}
