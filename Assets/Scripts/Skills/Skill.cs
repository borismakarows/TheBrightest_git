using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skills")]
public abstract class Skill : ScriptableObject
{
    public enum SkillTypes
    {
        LightAttack,
        Special,
        Defense,
        Rest
    };
    public enum SkillFeatures
    {
        Damage,
        AOE,
        Heal,
        Protection,
        ActionPoint,
    }
    public virtual SkillFeatures SkillFeature { get; set; }
    public virtual SkillTypes SkillType { get; set; }
    public virtual string SkillName { get; set; }
    public virtual string SkillDescription { get; set; }
    public virtual int Cost { get; set; }
    public virtual int Damage { get; set; }
    public virtual GameObject EffectPrefab { get; set; }
    public virtual float skillDuration { get; set; }

    public abstract void SkillActivation(GameObject user, GameObject[] targets);

    public abstract int GetCost();
    
}
