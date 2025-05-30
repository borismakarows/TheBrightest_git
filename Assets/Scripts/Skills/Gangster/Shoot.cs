using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Gangster/Shoot")]
public class Shoot : Skill
{
    public override string SkillName { get => "Shoot"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Damage Enemey with bullets"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.Special; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.Damage; set => base.SkillFeature = value; }
    public override int Damage { get => 20; set => base.Damage = value; }
    public override int Cost { get => 2; set => base.Cost = value; }
    [SerializeField] private GameObject effectPrefab;
    public override GameObject EffectPrefab { get => effectPrefab; set => base.EffectPrefab = value; }


    public override void SkillActivation(GameObject user, GameObject[] targets)
    {
        GameObject closestTarget = user.GetComponent<Unit>().FindClosestTarget(user, targets);
        user.GetComponent<Animator>().SetTrigger("Special");
    }



    public override int GetCost()
    {
        return Cost;
    }
}
