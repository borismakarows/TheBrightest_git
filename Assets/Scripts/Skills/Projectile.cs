using Unity.Mathematics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction = Vector2.right;
    public int damage;
    public bool stuneffect;
    public float destroyDelay;
    public bool shouldDamage;
    public bool shouldExplode;
    public string targetTag;
    public GameObject explosionPrefab;
    [SerializeField] float explosionDestroyDelay;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction.normalized);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string skillTag = targetTag + "Skill";
        if (other.CompareTag(targetTag))
        {
            GetComponent<CircleCollider2D>().enabled = false;
            if (shouldDamage)
            {
                if (other.GetComponent<Unit>() != null)
                { other.GetComponent<Unit>().TakeDamage(damage, stuneffect, 0f); }
                if (shouldExplode)
                {
                    explosionPrefab = Instantiate(explosionPrefab, transform.position, quaternion.identity);
                    Destroy(explosionPrefab, explosionDestroyDelay);
                }
            }
            Destroy(gameObject, destroyDelay);
        }
        else if (other.CompareTag(skillTag))
        {
            if (shouldExplode)
            {
                explosionPrefab = Instantiate(explosionPrefab, transform.position, quaternion.identity);
                Destroy(explosionPrefab, explosionDestroyDelay);
            }
            Destroy(gameObject);
            Destroy(other.gameObject, explosionDestroyDelay);
        }
    }
    public void SetProjectileProperties(int projectiledamage, Vector2 projectiledirection, bool WillStunEffect)
    {
        damage = projectiledamage;
        direction = projectiledirection;
        stuneffect = WillStunEffect;
    }
}

