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
        Rest,
        ChangePosition
    };
    public virtual SkillTypes SkillType {get;set;}
    public virtual string SkillName { get; set; }
    public virtual string SkillDescription { get; set; } 
    public virtual int Cost { get; set; }
    public virtual int Damage{ get; set; }
    public virtual GameObject EffectPrefab { get; set; }

    public abstract void SkillActivation(GameObject user, GameObject target);
}
