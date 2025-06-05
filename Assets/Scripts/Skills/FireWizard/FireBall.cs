using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Skills/Wizard/Fireball")]
public class FireBall : Skill
{
    public override string SkillName { get => "Kuyizaat"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Fire Ball/Projectile"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.LightAttack; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.Damage; set => base.SkillFeature = value; }
    public override int Damage { get => 15; set => base.Damage = value; }
    public override int Cost { get => 1; set => base.Cost = value; }
    [SerializeField] private GameObject effectPrefab;
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
            user.GetComponent<Unit>().ProjectileSpawn(user, VFXSpawnPoint, effectPrefab, Damage, Vector2.right, false, 0.9f, skillOffset);
            user.GetComponent<Unit>().currentActionPoints -= Cost;
        }
    }

    public override int GetCost()
    {
        return Cost;
    }

}
