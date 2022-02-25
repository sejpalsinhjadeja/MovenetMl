using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{

   private  float screenHalfWidthInWold;
    private float screenHalfHeightInWold;
   [SerializeField] private GameItem item;
   [SerializeField] private int generateTimeing = 3;
   [SerializeField] private int numberOfGenerateAtTime = 3;
   
    void Start()
    {
        screenHalfHeightInWold = (Camera.main.orthographicSize - (item.transform.localScale.x / 2));
        screenHalfWidthInWold = (Camera.main.aspect * Camera.main.orthographicSize) - (item.transform.localScale.x / 2);

        for(int i=1;i<10;i++)
        {
            GenerateItem(ObjectType.Touchable);
        }
    }


    public void GenerateItem(ObjectType type)
    {
        var x = Random.RandomRange(-screenHalfWidthInWold,screenHalfWidthInWold);
        var y = Random.RandomRange(-screenHalfHeightInWold,screenHalfHeightInWold);
        Vector3 location = new Vector3(x,y,0f);
        item.ItemType = type;
        
       var obj =  Instantiate(item.gameObject,location,Quaternion.identity,this.transform);
       var oPos =  obj.transform.localPosition;
      obj.transform.localPosition = new Vector3(oPos.x,oPos.y,0f); 

    }

   
}
