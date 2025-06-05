using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Gangster/Gun Stroke")]
public class GunStroke : Skill
{
    public override string SkillName { get => "Gun Stroke"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Basic Gun Stroke"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.LightAttack; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.Damage; set => base.SkillFeature = value; }
    public override int Damage { get => 18; set => base.Damage = value; }
    public override int Cost { get => 1; set => base.Cost = value; }
    [SerializeField] private GameObject effectPrefab;
    public override GameObject EffectPrefab { get => effectPrefab; set => base.EffectPrefab = value; }
    public override float skillDuration { get => 3f; set => base.skillDuration = value; }


    public override void SkillActivation(GameObject user, GameObject[] targets)
    {
        user.GetComponent<Unit>().currentActionPoints -= Cost;
        GameObject closestTarget = user.GetComponent<Unit>().FindClosestTarget(user, targets);
        user.GetComponent<Unit>().MoveToTarget(user, closestTarget.transform.position, 5f, "LightAttack", 0.7f,2f);
        closestTarget.GetComponent<Unit>().TakeDamage(Damage, false, 0.65f);
    }



    public override int GetCost()
    {
        return Cost;
    }
}
