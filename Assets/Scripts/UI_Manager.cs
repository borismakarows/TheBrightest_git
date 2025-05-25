using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;
    public Button LightAttack;
    public Button Special;
    public Button Defence;
    public Button Rest;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSkillButtonsActive(bool active)
    {
        LightAttack.gameObject.SetActive(active);
        Special.gameObject.SetActive(active);
        Defence.gameObject.SetActive(active);
        Rest.gameObject.SetActive(active);
        
    }

}
