using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class B03_ListArray
{
    public List<float> Array;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SO_Layer")]
public class SO_Layer : ScriptableObject
{
    [SerializeField] public float[,] Layer;

    [SerializeField] private List<B03_ListArray> lists;

    public void Load()
    {
        Layer = new float[lists.Count, lists[0].Array.Count];

        for(int x = 0; x < lists.Count; ++x)
        {
            for(int y = 0; y < lists[x].Array.Count; ++y)
            {
                Layer[x, y] = lists[x].Array[y];
            }
        }
    }

    public void Save()
    {
        int col = Layer.GetLength(0);
        int row = Layer.GetLength(1);

        lists = new List<B03_ListArray>(col);
        for(int x = 0; x < col; ++x)
        {
            lists.Add(new B03_ListArray());
            lists[x].Array = new List<float>(row);
            for(int y = 0; y < row; ++y)
            {
                lists[x].Array.Add(Layer[x, y]);
            }
        }
    }
}
