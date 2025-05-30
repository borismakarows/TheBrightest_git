using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<GameObject> team;

    void Start()
    {
        team.Add(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ally"))
        {
            Destroy(gameObject, 0.2f);
        }
    }
}
