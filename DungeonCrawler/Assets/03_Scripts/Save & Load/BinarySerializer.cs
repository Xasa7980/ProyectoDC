using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class BinarySerializer
{
    public object instance;
    public List<string> fileLibrary = new List<string>();
    public void Serializing(string fileName, object t)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if (!File.Exists(fileName))
        {
            fileLibrary.Add(fileName);
        }
        FileStream file = new FileStream(fileName,FileMode.OpenOrCreate,FileAccess.Write);
        formatter.Serialize(file,t);
        file.Close();
    }
    public void Deserialize(string fileName)
    {
        if (!File.Exists(fileName))
        {
            Debug.LogWarning("Any file has that name");
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        instance = formatter.Deserialize(file);
        file.Close();
    }
}
