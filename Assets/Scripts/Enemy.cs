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

}
