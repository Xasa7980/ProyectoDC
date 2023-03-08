using UnityEngine;

[CreateAssetMenu(fileName = "new pasive skill item", menuName = "Create Item/Pasive Skill Item")]
public class PasiveSkillSO : ItemObject
{
    //public Attributes stat;
    public float percentUp;

    public void Upgrade(ref float v)
    {
        v += GetPercentatge(v);
    }
    public float GetPercentatge(float v)
    {
        return v += (v * (percentUp / 100));
    }
}
