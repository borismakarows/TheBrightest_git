using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Gangster/Guard")]
public class Guard : Skill
{
    public override string SkillName { get => "Guard"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Reduces Damage"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.Defense; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.Protection; set => base.SkillFeature = value; }
    public override int Damage { get => 5; set => base.Damage = value; }
    public override int Cost { get => 2; set => base.Cost = value; }
    [SerializeField] private GameObject effectPrefab;
    public override GameObject EffectPrefab { get => effectPrefab; set => base.EffectPrefab = value; }


    public override void SkillActivation(GameObject user, GameObject[] targets)
    {
        user.GetComponent<Unit>().currentActionPoints -= Cost;
        if (!user.GetComponent<Unit>().isGuarded)
        {
            user.GetComponent<Unit>().isGuarded = true;
            user.GetComponent<Animator>().SetTrigger("Defence");
            user.GetComponent<Unit>().guardReduction = Damage;
        }
    }


    public override int GetCost()
    {
        return Cost;
    }
}
