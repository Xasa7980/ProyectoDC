using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Top Down Engine/Upgrade")]
public class Upgrade : ScriptableObject
{
    public enum Target
    {
        PlayerHealth,                   //Afecta la salud del personaje
        PlayerEnergy,                   //Afecta la energia maxima del personaje
        PlayerEnergyRecover,            //Afecta la velocidad de regeneracion de energia
        Weapon,                         //Afecta al arma
        Skill,                          //Afecta a las habilidades
        Automatic                       //Se activa de forma automatica segun un intervalo de tiempo y una probabilidad
    }

    public enum Mode
    {
        Percent,                        //Valores porcentuales entre 0 y 1 (se multplican al stat)
        Absolute                        //Valores absolutos (se suman o restan al stat)
    }

    [SerializeField] string _displayName;
    [SerializeField] Sprite _mainImage;
    [Multiline(6)]
    [SerializeField] string _description;

    [Space(10)]
    [SerializeField] Target _target;
    [SerializeField] float _value;
    [SerializeField] float _timeInterval = 1;
    [SerializeField, Range(0, 1)] float _rangedValue;
    [SerializeField] Mode _mode;

    [Space(10)]
    [SerializeField] GameObject useEffect;

    public string displayName => _displayName;
    public Sprite mainImage => _mainImage;
    public string description => _description;
    public Target target => _target;
    public float value => (_mode == Mode.Percent || _target == Target.Weapon || _target == Target.Automatic) ? _rangedValue : _value;
    public float timeInterval => _timeInterval;
    public Mode mode => _mode;

    public T Use<T>(Transform origin) where T : Component
    {
        return Instantiate(useEffect,origin.position,origin.rotation).GetComponent<T>();
    }
}