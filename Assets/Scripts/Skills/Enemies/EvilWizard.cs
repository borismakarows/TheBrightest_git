using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Enemies/EvilWizard/Void Shot")]
public class EvilWizard : Skill
{
    public override string SkillName { get => "Void Shoot"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Void Shoot"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.LightAttack; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.Damage; set => base.SkillFeature = value; }
    public override int Damage { get => 10; set => base.Damage = value; }
    public override int Cost { get => 1; set => base.Cost = value; }
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Vector3 skillOffset;
    public override GameObject EffectPrefab { get => effectPrefab; set => base.EffectPrefab = value; }
    public override float skillDuration { get => 4f; set => base.skillDuration = value; }

    public override void SkillActivation(GameObject user, GameObject[] targets)
    {
        user.GetComponent<Unit>().animator.SetTrigger("LightAttack");
        user.GetComponent<Unit>().currentActionPoints -= Cost;
        if (effectPrefab != null)
        {
            Transform VFXSpawnPoint = user.transform.Find("VFX").transform;
            Unit userUnit = user.GetComponent<Unit>();
            userUnit.ProjectileSpawn(user, VFXSpawnPoint, effectPrefab, Damage, Vector2.left, false, 1f, skillOffset);
            userUnit.currentActionPoints -= Cost;
        }
    }

    public override int GetCost()
    {
        return Cost;
    }
}
