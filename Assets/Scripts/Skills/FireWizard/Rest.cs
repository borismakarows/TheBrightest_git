using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Rest")]
public class Rest : Skill
{
    public override string SkillName { get => "Yolos"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Rest to get action point"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.Rest; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.ActionPoint; set => base.SkillFeature = value; }
    public override int Damage { get => 10; set => base.Damage = value; }
    public override int Cost { get => 2; set => base.Cost = value; }
    [SerializeField] private GameObject effectPrefab;
    public override GameObject EffectPrefab { get => effectPrefab; set => base.EffectPrefab = value; }
    public override void SkillActivation(GameObject user, GameObject[] targets)
    {
        Debug.Log("Action Point Gained");
    }

}
