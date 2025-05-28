using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Fireball")]
public class FireBall : Skill
{
    public override string SkillName { get => "Kuyizaat"; set => base.SkillName = value; }
    public override string SkillDescription { get => "Fire Ball/Projectile"; set => base.SkillDescription = value; }
    public override SkillTypes SkillType { get => SkillTypes.LightAttack; set => base.SkillType = value; }
    public override int Damage { get => 10; set => base.Damage = value; }
    public override int Cost { get => 1; set => base.Cost = value; }
    [SerializeField] private GameObject effectPrefab;
    public override GameObject EffectPrefab { get => effectPrefab; set => base.EffectPrefab = value; }
    public override void SkillActivation(GameObject user, GameObject target)
    {
        Debug.Log("FireBall");
    }
}
