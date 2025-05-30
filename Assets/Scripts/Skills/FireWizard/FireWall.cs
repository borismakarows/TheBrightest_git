using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Wizard/Fire Wall")]
public class FireWall : Skill
{
    public override string SkillName { get => "Vund"; set => base.SkillName = value; }
    public override string SkillDescription
    { get => "Fire wall to protect the team from projectiles"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.Defense; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.Protection; set => base.SkillFeature = value; }
    public override int Damage { get => 10; set => base.Damage = value; }
    public override int Cost { get => 2; set => base.Cost = value; }
    [SerializeField] private GameObject _effectPrefab;
    public override GameObject EffectPrefab { get => _effectPrefab; set => base.EffectPrefab = value; }
    private Transform spawnPos;
    private GameObject Vfx;
    public Vector3 skillOffset;

    public override void SkillActivation(GameObject user, GameObject[] targets)
    {
        user.GetComponent<Unit>().currentActionPoints -= Cost;
        if (EffectPrefab != null)
        {
            spawnPos = user.GetComponent<Unit>().staticSkillSpawnPoint;
            Vfx = user.GetComponent<Unit>().SpawnStaticSkill(spawnPos, EffectPrefab, skillOffset);
        }
    }

    public override int GetCost()
    {
        return Cost;
    }
}
