using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Skills/Fireball")]
public class FireBall : Skill
{
    public override string SkillName { get => "Kuyizaat"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Fire Ball/Projectile"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.LightAttack; set => base.SkillType = value; }
    public override SkillFeatures SkillFeature { get => SkillFeatures.Damage; set => base.SkillFeature = value; }
    public override int Damage { get => 10; set => base.Damage = value; }
    public override int Cost { get => 1; set => base.Cost = value; }
    [SerializeField] private GameObject effectPrefab;
    public override GameObject EffectPrefab { get => effectPrefab; set => base.EffectPrefab = value; }

    public override void SkillActivation(GameObject user, GameObject[] targets)
    {
        user.GetComponent<Unit>().animator.SetTrigger("LightAttack");
        if (effectPrefab != null)
        {
            Transform VFXSpawnPoint = user.transform.Find("VFX").transform;
            GameObject fireball = Instantiate(effectPrefab,VFXSpawnPoint.position , quaternion.identity);
            fireball.GetComponent<Projectile>().SetProjectileProperties(Damage, Vector2.right, false);
        }
    }
}
