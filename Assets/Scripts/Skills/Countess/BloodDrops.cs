using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Countess/Blood Drops")]
public class BloodDrop : Skill
{

    public override string SkillName { get => "Blood Drops"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Blood Drops from Special"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.LightAttack; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.Damage; set => base.SkillFeature = value; }
    [SerializeField] public override int Damage { get => 8; set => base.Damage = value; }
    [SerializeField] public override int Cost { get => 2; set => base.Cost = value; }
    [SerializeField] private float lifeStealRatio;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private Vector3 skillOffset;
    public override GameObject EffectPrefab { get => effectPrefab; set => base.EffectPrefab = value; }
    public override float skillDuration { get => 2f; set => base.skillDuration = value; }

    public override void SkillActivation(GameObject user, GameObject[] targets)
    {
        if (effectPrefab != null)
        {
            Debug.Log("BloooddddDropps!!!");
            Transform VFXSpawnPoint;
            foreach (GameObject target in targets)
            {
                Debug.Log(target.name);
                VFXSpawnPoint = target.transform;
                user.GetComponent<Unit>().ProjectileSpawn(user, VFXSpawnPoint, effectPrefab, Damage, Vector2.up, false, 0.35f, skillOffset);
            }
            user.GetComponent<Unit>().currentActionPoints -= Cost;
        }
        user.GetComponent<Unit>().LifeSteal(user, targets, lifeStealRatio, true);
        user.GetComponent<Animator>().SetTrigger("Special");
    }

    public override int GetCost()
    {
        return Cost;
    }
}
