using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject
{
    [SerializeField] string _displayName;
    [SerializeField] Sprite _mainImage;
    [SerializeField] string _description;
    [SerializeField] Sprite _classIcon;
    [SerializeField, Range(1,4)] int _level = 1;

    public string displayName => _displayName;
    public Sprite mainImage=> _mainImage;
    public string description=> _description;
    public Sprite classIcon=> _classIcon;
    public int level => _level;
}
