using System;
using DefaultNamespace;
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
   //[SerializeField] ParticleSystem destroyEffect = null;  TODO : Create and Add ParticaleSystem
  // [SerializeField] int positiveScore = 2;                TODO : ADD Score and Level System  
  // [SerializeField] int negativeScore = 2;
  // [SerializeField] TextMeshProUGUI animText=null;        TODO: Add Text Animation at Time Of Increment or Decrement Score
   [SerializeField] GameObject positiveGameObj=null;
   [SerializeField] GameObject negativeGameObj =null;
   
    void Start()
    {
	    if (processImage != null)
	    {
		    // processImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
		    processImage.fillAmount = 1;
		    processImage.DOFillAmount(0f, objectSeconds - 0.2f).OnComplete(() =>
		    {
			    if (ObjectType.Touchable == itemType && !GameManager.isGameOver)
			    {
				    GameManager.generatedItemsPosition.Clear();
				    EventManager.NotifyGenerateNewItem();
			    }

			    Destroy(gameObject);

		    });
	    }
    }

    
    // Based on ItemType assigned, it'll change the sprites
    public ObjectType ItemType
    {
       get => itemType;
       set
       {
	       itemType = value;

	       if (itemType == ObjectType.Touchable)
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
    {
	    return transform;
    }
    
}
