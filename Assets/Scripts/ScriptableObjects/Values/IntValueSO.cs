using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu(fileName = "NewIntValue", menuName = "Values/Int Value")]
public class IntValueSO : ValuesBaseSO
{
    [SerializeField] private int value;

    public int Value
    { 
        get => value; 
    }

    public void Set(int val)
    {
        value = val;
    }

    public void Increment(int val)
    {
        value += val;
    }

    private void OnEnable()
    {
        LoadValueFromSave();
    }
    
    [ContextMenu("Load Value From Save")]
    public void LoadValueFromSave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/data", FileMode.OpenOrCreate);
        int val = (int) bf.Deserialize(fileStream);
        this.Set(val);

        fileStream.Close();
    }

    [ContextMenu("Save Value")]
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/data", FileMode.OpenOrCreate);
        bf.Serialize(fileStream, this.Value);

        fileStream.Close();
    }
}
