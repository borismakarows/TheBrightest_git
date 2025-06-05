using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Wizard/Flame Jet")]
public class FireJet : Skill
{
    public override string SkillName { get => "Lingrah"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Flame thrower effect, AOE"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.Special; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.AOE; set => base.SkillFeature = value; }
    public override int Damage { get => 18; set => base.Damage = value; }
    public override int Cost { get => 2; set => base.Cost = value; }
    [SerializeField] private GameObject effectPrefab;
    public override GameObject EffectPrefab { get => effectPrefab; set => base.EffectPrefab = value; }
    public override float skillDuration { get => 3.5f; set => base.skillDuration = value; }


    public override void SkillActivation(GameObject user, GameObject[] targets)
    {
        GameObject closestTarget = user.GetComponent<Unit>().FindClosestTarget(user, targets);
        user.GetComponent<Unit>().MoveToTarget(user, closestTarget.transform.position, 5f, "Special", 0.3f, 1.8f);
        closestTarget.GetComponent<Unit>().TakeDamage(Damage, false, 0.4f);
        user.GetComponent<Unit>().currentActionPoints -= Cost;
    }


    public override int GetCost()
    {
        return Cost;
    }
    
}
