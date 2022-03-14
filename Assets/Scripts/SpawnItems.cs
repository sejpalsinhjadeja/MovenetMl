using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
	private float screenHalfWidthInWold = 0f;
	private float screenHalfHeightInWold = 0f;
	[SerializeField] private GameItem item = null;
	[SerializeField] private int numberOfGenerateAtTime = 3;
	[SerializeField] private float separateDistance = 0.5f;

	private Camera mainCam = null;

	private void OnEnable()
	{
		//EventManager.GenerateNewItem += GenerateNextItem;
	}


	private void OnDisable()
	{
		//EventManager.GenerateNewItem -= GenerateNextItem;
	}
	void Start()
	{
		mainCam = Camera.main;

		// Make sure that main camera is not null
		if (!mainCam) return;

		screenHalfHeightInWold = (mainCam.orthographicSize - (item.transform.localScale.x / 2));
		screenHalfWidthInWold = (mainCam.aspect * mainCam.orthographicSize) - (item.transform.localScale.x / 2);

		for (int i = 0; i < 2; i++)
		{
			GameItem obj = Instantiate(item.gameObject, transform).GetComponent<GameItem>();
			obj.SetPosition();
			obj.ItemType = (ObjectType)i;
			GameManager.generatedItems.Add(obj);
		}
		//GenerateItem(true);
	}


	private void GenerateItem(bool generateBothItems = false)
	{
		Debug.Log("GenerateItem : " + generateBothItems);
		int maxAllowed = 1;

		if (generateBothItems)
		{
			maxAllowed = 2;
		}

		// [gi1, gi2] = [0,0]

		for (int i = 0; i < maxAllowed; i++) // ma : 2
		{
			var x = Random.Range(-screenHalfWidthInWold, screenHalfWidthInWold);
			var y = Random.Range(-screenHalfHeightInWold, screenHalfHeightInWold);
			Vector3 location = new Vector3(x, y, 0f);

			// , 100, [gi1,gi2]
			//if (i > 0 && IsTooClose(location, separateDistance, GameManager.generatedItems))
			//{
			//	i--;
			//}
			//else
			{
				//GameManager.isDetecting = true;

				GameManager.generatedItems[i].ItemType = (ObjectType)i;

				var oPos = location; // GameManager.generatedItems[i].GetTransform().localPosition;
				GameManager.generatedItems[i].GetTransform().localPosition = new Vector3(oPos.x, oPos.y, 0f);

				GameManager.generatedItems[i].gameObject.SetActive(true);
			}
		}
	}

	

	//private void GenerateNextItem()
	//{
	//	// for (int a = 0; a < numberOfGenerateAtTime; a++)
	//	// {
	//	// 	GenerateItem(true, "GenerateNextItem");
	//	// }
	//	GenerateItem(true);
	//}


	bool IsTooClose(Vector3 pos, float minimumDistance, List<GameItem> list)
	{
		if (list == null)
		{
			return false;
		}

		foreach (var f in list)
		{
			var dist = Vector3.Distance(pos, f.transform.localPosition);
			
		//	print(dist);
			
			if (dist < minimumDistance)
			{
				return true;
			}
		}
		
		return false;
	}


	private void OnDestroy()
	{
		DOTween.KillAll();
	}
}