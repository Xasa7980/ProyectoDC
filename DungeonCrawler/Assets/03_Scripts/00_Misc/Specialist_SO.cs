using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Specialist", menuName = "Specialist")]
public class Specialist_SO : ScriptableObject
{
    public string displayName;
    public Sprite image;

    [Header("Models")]
    [SerializeField] GameObject presentationModel;
    [SerializeField] GameObject playableModel;

    [Header("Weapons")]
    public RifleController rangeWeapon;
    public MeleeWeaponController meleeWeapon;

    [Header("Stats")]
    public int vitality = 30;
    public int energy = 13;
    public int endurance = 7;
    public int presition = 3;

    [Header("Skills")]
    public SkillSO[] skills;

    public GameObject CreatePresentationModel(Transform parent)
    {
        return Instantiate(presentationModel, parent);
    }

    public class Stat
    {
        [SerializeField] string _name;
        [SerializeField] int _value;

        public string name => _name;
        public int value => _value;
    }
}
