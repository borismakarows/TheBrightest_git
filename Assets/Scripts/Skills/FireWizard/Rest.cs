using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Shared/Rest")]
public class Rest : Skill
{
    public override string SkillName { get => "Yolos"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Rest to get action point"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.Rest; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.ActionPoint; set => base.SkillFeature = value; }
    public override int Damage { get => 10; set => base.Damage = value; }
    public override int Cost { get => 0; set => base.Cost = value; }

    public override GameObject EffectPrefab { get => base.EffectPrefab; set => base.EffectPrefab = value; }
    public override float skillDuration { get => 0.5f; set => base.skillDuration = value; }
    public override void SkillActivation(GameObject user, GameObject[] targets)
    {
        user.GetComponent<Unit>().currentActionPoints += 1;
    }

    public override int GetCost()
    {
        return Cost;
    }

}
