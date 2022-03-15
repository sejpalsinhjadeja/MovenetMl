using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
	[SerializeField] private GameItem item = null;

	void Start()
	{
		for (int i = 0; i < 2; i++)
		{
			GameItem obj = Instantiate(item.gameObject, transform).GetComponent<GameItem>();
			obj.SetPosition();
			obj.ItemType = (ObjectType)i;
			GameManager.generatedItems.Add(obj);
		}
	}
}