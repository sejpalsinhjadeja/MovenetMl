using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
	private float screenHalfWidthInWold = 0f;
	private float screenHalfHeightInWold = 0f;
	[SerializeField] private GameItem item = null;
	[SerializeField] private int numberOfGenerateAtTime = 3;
	List<GameItem> spawnObjects = null;
	[SerializeField] private float separateDistance = 0.5f;

	private Camera mainCam = null;

	private void OnEnable()
	{
		EventManager.GenerateNewItem += GenerateNextItem;
	}


	private void OnDisable()
	{
		EventManager.GenerateNewItem -= GenerateNextItem;
	}


	void Start()
	{
		spawnObjects = new List<GameItem>();

		mainCam = Camera.main;

		// Make sure that main camera is not null
		if (!mainCam) return;
		
		screenHalfHeightInWold = (mainCam.orthographicSize - (item.transform.localScale.x / 2));
		screenHalfWidthInWold = (mainCam.aspect * mainCam.orthographicSize) - (item.transform.localScale.x / 2);

		GenerateItem(true);
	}


	private void GenerateItem(bool generateBothItems = false)
	{
		int maxAllowed = 1;
		
		if (generateBothItems)
		{
			maxAllowed = 2;
		}
		
		for (int i = 0; i < maxAllowed; ++i)
		{
			var x = Random.Range(-screenHalfWidthInWold, screenHalfWidthInWold);
			var y = Random.Range(-screenHalfHeightInWold, screenHalfHeightInWold);
			Vector3 location = new Vector3(x, y, 0f);

			if (IsTooClose(location, separateDistance, spawnObjects))
			{
				i--;
			}
			else
			{
				var obj = Instantiate(item.gameObject, location, Quaternion.identity, this.transform)
					.GetComponent<GameItem>();

				obj.ItemType = (ObjectType)i;
				spawnObjects.Add(obj);
				var oPos = obj.transform.localPosition;
				obj.transform.localPosition = new Vector3(oPos.x, oPos.y, 0f);

				GameManager.generatedItemsPosition.Add(obj.GetTransform());
			}
		}
	}


	private void GenerateNextItem()
	{
		spawnObjects.Clear();
		// for (int a = 0; a < numberOfGenerateAtTime; a++)
		// {
		// 	GenerateItem(true, "GenerateNextItem");
		// }
		GenerateItem(true);
	}


	bool IsTooClose(Vector3 pos, float minimumDistance, List<GameItem> list)
	{
		if (list == null)
		{
			return false;
		}

		foreach (var f in list)
		{
			var dist = Vector3.Distance(pos, f.transform.localPosition);
			
			print(dist);
			
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