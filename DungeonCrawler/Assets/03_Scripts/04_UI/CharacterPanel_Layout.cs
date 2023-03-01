using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterPanel_Layout : MonoBehaviour
{
    [SerializeField] Specialist_SO[] specialists;
    int currentSpecialistIndex = 0;
    public Specialist_SO currentSpecialist => specialists[currentSpecialistIndex];

    [SerializeField] Image currentSpecialistImage;

    [SerializeField] Transform specialistModelContainer;
    GameObject currentSpecialistModel;

    [Header("Weapons")]
    [SerializeField] Image specialistRangeWeapon;
    [SerializeField] Image specialistMeleeWeapon;

    [Header("Stats")]
    public TextMeshProUGUI vitality;
    public TextMeshProUGUI energy;
    public TextMeshProUGUI endurance;
    public TextMeshProUGUI presition;

    [Header("Skills")]
    [SerializeField] SpecialistSkill[] skills;

    private void Start()
    {
        ConfigureUI();
    }

    void ConfigureUI()
    {
        if (currentSpecialistModel != null)
            Destroy(currentSpecialistModel);

        currentSpecialistModel = currentSpecialist.CreatePresentationModel(specialistModelContainer);

        currentSpecialistImage.sprite = currentSpecialist.image;
        specialistRangeWeapon.sprite = currentSpecialist.rangeWeapon.image;
        specialistMeleeWeapon.sprite = currentSpecialist.meleeWeapon.image;

        vitality.text = currentSpecialist.vitality.ToString();
        energy.text = currentSpecialist.energy.ToString();
        endurance.text = currentSpecialist.endurance.ToString();
        presition.text = currentSpecialist.presition.ToString();

        for(int i = 0;i<currentSpecialist.skills.Length; i++)
        {
            skills[i].Configure(currentSpecialist.skills[i]);
        }
    }

    public void NextSpecialits()
    {
        if (currentSpecialistIndex < specialists.Length - 1)
            currentSpecialistIndex++;
        else
            currentSpecialistIndex = 0;

        ConfigureUI();
    }

    public void PrevSpecialits()
    {
        if (currentSpecialistIndex > 0)
            currentSpecialistIndex--;
        else
            currentSpecialistIndex = specialists.Length - 1;

        ConfigureUI();
    }

    [System.Serializable]
    class SpecialistStat
    {
        [SerializeField] TextMeshProUGUI name;
        [SerializeField] TextMeshProUGUI value;

        public void Configure(Specialist_SO.Stat stat)
        {
            name.text = stat.name;
            value.text = stat.value.ToString();
        }
    }

    [System.Serializable]
    class SpecialistSkill
    {
        [SerializeField] TextMeshProUGUI name;
        [SerializeField] TextMeshProUGUI description;
        [SerializeField] Image image;

        public void Configure(SkillSO skill)
        {
            name.text = skill.displayName;
            description.text = skill.description;
            image.sprite = skill.image;
        }
    }
}
