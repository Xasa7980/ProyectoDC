using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [SerializeField] string _displayName;
    [SerializeField] Sprite _image;

    public string displayName => _displayName;
    public Sprite image => _image;

    [SerializeField] protected float damage = 7;
}
