using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;
    public List<Button> B_Skills;

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
        foreach (Button skillButton in B_Skills)
        {
            skillButton.gameObject.SetActive(active);
        }
    }


}
