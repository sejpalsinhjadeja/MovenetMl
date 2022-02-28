using DG.Tweening;
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

        if(separateDistance>=screenHalfWidthInWold)
        {
            separateDistance = 0;
            print("Reduce Separate Distance!");
        }

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
        
        if(!isTooClose(x,separateDistance,spawnObjects) || loopCounter > 3)
        {
            Vector3 location = new Vector3(x,y,0f);
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
        for (int a = 0;a<obj;a++)
        {
            var index =  a%2;
            ObjectType[] type = new ObjectType[]{ObjectType.Nontouchable,ObjectType.Touchable};
            GenerateItem(type[index]);
        }
    }


bool isTooClose(float x, float minimumDistance, List<GameItem> list)
 {
         if (list == null)
         {
            return false;
         }
 
            bool tooClose = true;
 
            foreach (var f in list)
            {
                if (Mathf.Abs(x) > Mathf.Abs(f.transform.position.x) + minimumDistance)
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
