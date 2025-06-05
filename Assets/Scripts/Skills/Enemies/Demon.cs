using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Enemies/Demon/Hell Dash")]
public class Demon : Skill
{
    public override string SkillName { get => "Hell Dash"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Dash through Enemy"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.LightAttack; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.Damage; set => base.SkillFeature = value; }
    public override int Damage { get => 8; set => base.Damage = value; }
    public override int Cost { get => 1; set => base.Cost = value; }
    [SerializeField] private GameObject effectPrefab;
    public override GameObject EffectPrefab { get => effectPrefab; set => base.EffectPrefab = value; }
    public override float skillDuration { get => 4f; set => base.skillDuration = value; }

    public override void SkillActivation(GameObject user, GameObject[] targets)
    {
        GameObject closestTarget = user.GetComponent<Unit>().FindClosestTarget(user, targets);
        user.GetComponent<Unit>().MoveToTarget(user, closestTarget.transform.position, 10f, "LightAttack",0.5f, 1.2f);
        closestTarget.GetComponent<Unit>().TakeDamage(Damage, false, 1.1f);
        user.GetComponent<Unit>().currentActionPoints -= Cost;
    }

    public override int GetCost()
    {
        return Cost;
    }
}
