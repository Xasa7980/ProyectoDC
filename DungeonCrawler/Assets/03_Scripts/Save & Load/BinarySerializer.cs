using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BinarySerializer
{
    public static void SerializingPlayerData<T>(string s, T stats)
    {
        T pd = stats;
        BinaryFormatter bf = new();
        Stream file = new FileStream(s, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

        bf.Serialize(file, pd);
        file.Close();
    }

    public static T Deserialize<T>(string fileName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = new(fileName, FileMode.OpenOrCreate);
        T instance = (T)formatter.Deserialize(file);
        file.Close();
        return instance;
    }

    public static void ClearData(string fileName)
    {
        if(File.Exists(fileName)) File.Delete(fileName);
    }
}
