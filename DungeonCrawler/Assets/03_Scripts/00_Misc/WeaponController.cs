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

    protected List<Upgrade> upgrades = new List<Upgrade>();
    List<float> probabilities = new List<float>() { 1 };
    float maxProbability = 1;
    float probability;

    public void AddUpgrade(Upgrade upgrade)
    {
        upgrades.Add(upgrade);
        probabilities.Add(upgrade.value);
        maxProbability += upgrade.value;
    }

    public int CalculateUpgrade()
    {
        probability = Random.Range(0, maxProbability);
        float currentCounter = 0;
        for(int i = 0; i < probabilities.Count; i++)
        {
            currentCounter += probabilities[i];

            if (probability < currentCounter)
                return i - 1;
        }

        return -1;
    }
}
