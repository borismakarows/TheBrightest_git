using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Countess/Blood Wall")]
public class BloodWall : Skill
{
        public override string SkillName { get => "Blood Sphere"; set => base.SkillName = value; }
    public override string SkillDescription
    { get => "Blood sphere to protect the team from projectiles"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.Defense; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.Protection; set => base.SkillFeature = value; }
    public override int Damage { get => 10; set => base.Damage = value; }
    public override int Cost { get => 2; set => base.Cost = value; }
    [SerializeField] private GameObject _effectPrefab;
    public override GameObject EffectPrefab { get => _effectPrefab; set => base.EffectPrefab = value; }
    private Transform spawnPos;
    private GameObject[] Bloods = new GameObject[3];
    public Vector3 skillOffset;

    public override void SkillActivation(GameObject user, GameObject[] targets)
    {
        user.GetComponent<Unit>().currentActionPoints -= Cost;
        if (EffectPrefab != null)
        {
            spawnPos = user.transform.Find("DefencePoint1");
            Bloods[0] = user.GetComponent<Unit>().SpawnStaticSkill(spawnPos, EffectPrefab, skillOffset);
            spawnPos = user.transform.Find("DefencePoint2");
            Bloods[1] = user.GetComponent<Unit>().SpawnStaticSkill(spawnPos, EffectPrefab, skillOffset);
            spawnPos = user.transform.Find("DefencePoint3");
            Bloods[2] = user.GetComponent<Unit>().SpawnStaticSkill(spawnPos, EffectPrefab, skillOffset);
            user.GetComponent<Animator>().SetTrigger("Defence");
            user.GetComponent<Unit>().isGuarded = true;
            user.GetComponent<Unit>().guardReduction = 100;
        }
    }

    public override int GetCost()
    {
        return Cost;
    }
}
