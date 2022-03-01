using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class EventManager : MonoBehaviour
	{
		public static Action GenerateNewItem;

		public static void NotifyGenerateNewItem()
		{
			GenerateNewItem?.Invoke();
		}
	}
}