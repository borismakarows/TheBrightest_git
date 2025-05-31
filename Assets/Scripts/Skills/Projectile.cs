using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction = Vector2.right;
    public int damage;
    public bool stuneffect;
    public float destroyDelay;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction.normalized);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GetComponent<CircleCollider2D>().enabled = false;
            other.GetComponent<Unit>().TakeDamage(damage, stuneffect, 0f);
            Destroy(gameObject, destroyDelay);  
        }
    }
    public void SetProjectileProperties(int projectiledamage, Vector2 projectiledirection, bool WillStunEffect)
    {
        damage = projectiledamage;
        direction = projectiledirection;
        stuneffect = WillStunEffect;
    }
}

