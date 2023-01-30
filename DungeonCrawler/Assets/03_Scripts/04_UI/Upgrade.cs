using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Top Down Engine/Skill")]
public class Upgrade : ScriptableObject
{
    [SerializeField] string _displayName;
    [SerializeField] Sprite _mainImage;
    [Multiline(6)]
    [SerializeField] string _description;

    public string displayName => _displayName;
    public Sprite mainImage=> _mainImage;
    public string description=> _description;
}
