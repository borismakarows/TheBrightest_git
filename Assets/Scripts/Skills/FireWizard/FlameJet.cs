using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Flame Jet")]
public class FireJet : Skill
{
    public override string SkillName { get => "Lingrah"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Flame thrower effect, AOE"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.Special; set => base.SkillType = value; }
    public override int Damage { get => 10; set => base.Damage = value; }
    public override int Cost { get => 2; set => base.Cost = value; }
    [SerializeField] private GameObject effectPrefab;
    public override GameObject EffectPrefab { get => effectPrefab; set => base.EffectPrefab = value; }
    public override void SkillActivation(GameObject user, GameObject target)
    {
        Debug.Log("Flame Jet");
    }
}
