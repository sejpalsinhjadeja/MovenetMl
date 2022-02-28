using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{

    private  float screenHalfWidthInWold = 0f;
    private float screenHalfHeightInWold = 0f;
   [SerializeField] private GameItem item = null;
   [SerializeField] private int numberOfGenerateAtTime = 3;
   List<GameItem> spawnObjects = null;
   [SerializeField] private float separateDistance = 0.5f;



    private void OnEnable() {
        
        GameItem.generateNewItems += generateNextItem;
    }

  

    private void OnDisable() {
            GameItem.generateNewItems -= generateNextItem;
    }

   
    void Start()
    {
        spawnObjects =  new List<GameItem>();

        screenHalfHeightInWold = (Camera.main.orthographicSize - (item.transform.localScale.x / 2));
        screenHalfWidthInWold = (Camera.main.aspect * Camera.main.orthographicSize) - (item.transform.localScale.x / 2);

     

        for(int i=1;i<2;i++)
        {
            GenerateItem(ObjectType.Touchable);
        }
    }


    public void GenerateItem(ObjectType type)
    {
        int loopCounter = 1;
        GA:
        var x = Random.Range(-screenHalfWidthInWold,screenHalfWidthInWold);
        var y = Random.Range(-screenHalfHeightInWold,screenHalfHeightInWold);
         Vector3 location = new Vector3(x,y,0f);
           
        if(!isTooClose(location,separateDistance,spawnObjects) || loopCounter > 3)
        {
            var obj = Instantiate(item.gameObject, location, Quaternion.identity, this.transform).GetComponent<GameItem>();
            obj.ItemType = type; 
            spawnObjects.Add(obj);
            var oPos =  obj.transform.localPosition;
            obj.transform.localPosition = new Vector3(oPos.x,oPos.y,0f); 
            
       }
       else{
            
           loopCounter++;
           goto GA;

       }

    }



     private void generateNextItem(int obj)
    {
        spawnObjects.Clear();
        for (int a = 0;a<numberOfGenerateAtTime;a++)
        {
            var index =  a%2;
            ObjectType[] type = new ObjectType[]{ObjectType.Nontouchable,ObjectType.Touchable};
            GenerateItem(type[index]);
        }
    }


bool isTooClose(Vector3 pos, float minimumDistance, List<GameItem> list)
 {
         if (list == null)
         {
            return false;
         }
 
            bool tooClose = true;
 
            foreach (var f in list)
            {
                var dist = f.transform.position - pos;
                print(dist.magnitude);
                if (dist.magnitude >  minimumDistance)
                {
                    tooClose = false;
                    break;
                }
            }
        
        return tooClose;
  }


    private void OnDestroy()
    {
        DOTween.KillAll();
    }

}
