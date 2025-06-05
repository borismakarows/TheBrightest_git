using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Slider h_slider;
    public List<GameObject> team;

    void Start()
    {
        team.Add(gameObject);
        h_slider.value = GetComponent<Unit>().currentUnitData.currentHP / 100f;
    }



}
