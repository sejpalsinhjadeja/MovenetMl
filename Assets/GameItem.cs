using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ObjectType
{
    Touchable = 0,
    Nontouchable = 1
}

public class GameItem : MonoBehaviour
{
   
   [SerializeField] ObjectType itemType;
   [SerializeField] float  objectSeconds=0;
   [SerializeField] Image processImage = null;
   [SerializeField] ParticleSystem destroyEffect = null; 
   [SerializeField] int positiveScore = 0;
   [SerializeField] int negativeScore = 0;
   [SerializeField] TextMeshProUGUI animText;
   [SerializeField] GameObject positiveGameObj;
   [SerializeField] GameObject negativeGameObj;

   public  static Action<int> generateNewItems = delegate{};




    void Start()
    {
      if(processImage!=null)
      {
    //     processImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
         processImage.fillAmount=1;
         processImage.DOFillAmount(0f,objectSeconds-0.2f).OnComplete(()=>{
             if (ObjectType.Touchable==itemType)
             {
                 generateNewItems?.Invoke(2);
             }
             Destroy(this.gameObject);

         });   
      }
    }

    
    public ObjectType ItemType
    {
       get{return this.itemType;}
       set{
      
               this.itemType =value;
               if(this.itemType == ObjectType.Touchable)
               {
                  positiveGameObj.gameObject.SetActive(true);
                  negativeGameObj.gameObject.SetActive(false);
               }  
               else
               {
                  negativeGameObj.gameObject.SetActive(true);
                  positiveGameObj.gameObject.SetActive(false);
               }     
          }
    }



    public Transform GetTransform()
    {return this.transform;}
    



}
