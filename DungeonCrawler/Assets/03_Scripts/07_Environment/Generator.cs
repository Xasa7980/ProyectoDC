using System.Collections;
using UnityEngine;

public abstract class Generator : MonoBehaviour
{
    public bool randomSeed = false;
    public int seed = 1234567;

    public abstract void Generate();

    public abstract void Generate(int seed);

    public abstract void SpawnProps();

    public abstract void ClearDungeon();
}