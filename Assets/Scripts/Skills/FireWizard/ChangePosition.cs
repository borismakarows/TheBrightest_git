using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Change Position")]
public class ChangePosition : Skill
{
     public override string SkillName { get => "Change Position"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Change your position"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.ChangePosition; set => base.SkillType = value; }
    public override int Damage { get => 0; set => base.Damage = value; }
    public override int Cost { get => 1; set => base.Cost = value; }
    [SerializeField] private GameObject effectPrefab;
    public override GameObject EffectPrefab { get => effectPrefab; set => base.EffectPrefab = value; }
    public override void SkillActivation(GameObject user, GameObject target)
    {
        Debug.Log("Position Change");
    }
}
