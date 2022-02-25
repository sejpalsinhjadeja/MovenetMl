using System.Collections;
using System.Collections.Generic;
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

    
    
    public ObjectType ItemType
    {
       get{return this.itemType;}
       set{this.itemType =value;}
    }



    public Transform GetTransform()
    {return this.transform;}
    
    private void Destroy() {
            
                Destroy(this.gameObject);
         
         }



}
