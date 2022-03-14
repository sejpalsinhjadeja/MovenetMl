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
    [SerializeField] float objectSeconds = 0;
    [SerializeField] Image processImage = null;
    //[SerializeField] ParticleSystem destroyEffect = null;  TODO : Create and Add ParticaleSystem
    // [SerializeField] int positiveScore = 2;                TODO : ADD Score and Level System  
    // [SerializeField] int negativeScore = 2;
    // [SerializeField] TextMeshProUGUI animText=null;        TODO: Add Text Animation at Time Of Increment or Decrement Score
    [SerializeField] GameObject positiveGameObj = null;
    [SerializeField] GameObject negativeGameObj = null;

    private float screenHalfWidthInWold = 0f;
    private float screenHalfHeightInWold = 0f;
    private Camera mainCam = null;


    void Start()
    {
        //rectTransform = GetComponent<RectTransform>();
        mainCam = Camera.main;

        // Make sure that main camera is not null
        if (!mainCam) return;

        screenHalfHeightInWold = (mainCam.orthographicSize - (transform.localScale.x / 2));
        screenHalfWidthInWold = (mainCam.aspect * mainCam.orthographicSize) - (transform.localScale.x / 2);

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

    public void SetPosition()
    {
        float x = Random.Range(-screenHalfWidthInWold, screenHalfWidthInWold);
        float y = Random.Range(-screenHalfHeightInWold, screenHalfHeightInWold);
        Debug.Log("SetPosition ::: X " + x +"  *** Y ::"+ y);

        transform.position = new Vector3(x, y,0);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        processImage.DOKill();
        processImage.fillAmount = 1;
        processImage.DOFillAmount(0f, objectSeconds - 0.2f).OnComplete(() =>
        {
            SetPosition();
        });
    }
}
