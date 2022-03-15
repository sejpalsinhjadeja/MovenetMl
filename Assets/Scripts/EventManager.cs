using System;
using UnityEngine;


public class EventManager : MonoBehaviour
{
    public static Action<ObjectType> UpdateScore;

    public static void NotifyUpdateScore(ObjectType itemType)
    {
        UpdateScore?.Invoke(itemType);
    }
    
    public static Action GameOver;

    public static void NotifyGameOver()
    {
        GameOver?.Invoke();
    }
    
    public static Action StartGame;

    public static void NotifyStartGame()
    {
        StartGame?.Invoke();
    }
}