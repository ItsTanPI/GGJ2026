using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public PickItems pickItems;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<10;i++)
        {
            Vector3 spawnPosition=new Vector3(Random.Range(-5f,5f),5f,Random.Range(-5f,5f));
            PickItems Temp=Instantiate(pickItems, spawnPosition, Quaternion.identity);
            Temp.itemIndex=Random.Range(0, Temp.foundMeshes.Count);
            Temp.ApplySelection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
