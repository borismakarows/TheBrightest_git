using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Enemies/Toaster/Kamahame")]
public class Toaster : Skill
{
    public override string SkillName { get => "Kamahame"; set => base.SkillName = value; }
    public override string SkillDescription { get => "a ray which damages enemies"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.LightAttack; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.Damage; set => base.SkillFeature = value; }
    public override int Damage { get => 5; set => base.Damage = value; }
    public override int Cost { get => 1; set => base.Cost = value; }
    [SerializeField] private GameObject effectPrefab;
    public override GameObject EffectPrefab { get => effectPrefab; set => base.EffectPrefab = value; }
    public override float skillDuration { get => 4f; set => base.skillDuration = value; }

    public override void SkillActivation(GameObject user, GameObject[] targets)
    {
        GameObject closestTarget = user.GetComponent<Unit>().FindClosestTarget(user, targets);
        user.GetComponent<Unit>().MoveToTarget(user, closestTarget.transform.position,5, "LightAttack",0.5f,2.6f);
        closestTarget.GetComponent<Unit>().DamageAllEnemies(Damage, targets, 1.1f);
        user.GetComponent<Unit>().currentActionPoints -= Cost;
    }

    public override int GetCost()
    {
        return Cost;
    }
}
