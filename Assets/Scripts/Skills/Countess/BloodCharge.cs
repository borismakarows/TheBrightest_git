using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Countess/Blood Charge")]
public class BloodCharge : Skill
{

    public override string SkillName { get => "Blood Charge"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Blood Charge Through Enemies"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.LightAttack; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.Damage; set => base.SkillFeature = value; }
    [SerializeField] public override int Damage { get => 10; set => base.Damage = value; }
    [SerializeField] public override int Cost { get => 1; set => base.Cost = value; }
    [SerializeField] private float lifeStealRatio;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private Vector3 skillOffset;
    public override GameObject EffectPrefab { get => effectPrefab; set => base.EffectPrefab = value; }

    public override void SkillActivation(GameObject user, GameObject[] targets)
    {
        if (effectPrefab != null)
        {
            Debug.Log("Blooodddd!!!");
            Transform VFXSpawnPoint = user.transform.Find("VFX").transform;
            user.GetComponent<Unit>().ProjectileSpawn(user, VFXSpawnPoint, effectPrefab, Damage, Vector2.right, false, 0.7f,skillOffset);
            user.GetComponent<Unit>().currentActionPoints -= Cost;
        }
        user.GetComponent<Unit>().LifeSteal(user, targets, lifeStealRatio,false);
        user.GetComponent<Animator>().SetTrigger("LightAttack");
    }

    public override int GetCost()
    {
        return Cost;
    }
}
