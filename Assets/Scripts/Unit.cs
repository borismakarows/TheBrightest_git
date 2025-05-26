using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public enum TeamType { Ally, Enemy };
    public TeamType actorTeamType;
    public Skill LightAttack;
    public Skill Special;
    public Skill Defence;
    public Skill Rest;
    [SerializeField] int maxActionPoints;
    [SerializeField] string unitName;
    [SerializeField] int maxHP;
    public int speed;
    [SerializeField] Sprite unitIcon;
    private bool isAlive = true;
    [HideInInspector] bool isStunned;
    public UnitData currentUnitData;
    public int positionIndex;
    [System.Serializable]
    public class UnitData
    {
        public int id;
        public int currentHP;
        public int currentActionPoints;
    }
    void Start()
    {
        SetUpListeners();
    }

    public void TakeDamage(int damage, bool stun)
    {
        currentUnitData.currentHP -= damage;
        isStunned = stun;
        if (currentUnitData.currentHP < 0)
        {
            isAlive = false;
            currentUnitData.currentHP = 0;
            Debug.Log($"{unitName} has died.");
        }
    }

    public bool IsAlive()
    {
        return isAlive;
    }
    public void RestoreSavedData(UnitData Data)
    {
        currentUnitData = Data;
    }

    public void SetUpListeners()
    {
        foreach (Button skillButton in UI_Manager.Instance.B_Skills)
        {
            switch (skillButton.name)
            {
                case "B_Attack":
                    skillButton.onClick.AddListener(() => SkillUse(Skill.SkillTypes.LightAttack));
                    break;
                case "B_Special":
                    skillButton.onClick.AddListener(() => SkillUse(Skill.SkillTypes.Special));
                    break;
                case "B_Defence":
                    skillButton.onClick.AddListener(() => SkillUse(Skill.SkillTypes.Defense));
                    break;
                case "B_Rest":
                    skillButton.onClick.AddListener(() => SkillUse(Skill.SkillTypes.Rest));
                    break;
            }
        }
    }

    public void SkillUse(Skill.SkillTypes skillType)
    {
        LightAttack.SkillActivation("asdads");
    }
}
