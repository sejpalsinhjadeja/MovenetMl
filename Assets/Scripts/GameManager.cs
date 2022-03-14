using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int oneRoundTimer = 2;
    [SerializeField] TextMeshProUGUI timerTxt = null;
    public static bool isGameOver = false;
    //public static int gameTotalScore = 0;

    public static List<GameItem> generatedItems;
    
    private void Start()
    {
        generatedItems = new List<GameItem>();
        GameTimer();   
    }
    private async void GameTimer()
        {
            int time = oneRoundTimer * 60;
            while (!isGameOver) // 40
            {
                if(time > 0)
                {
                    time--;
                    var ts = TimeSpan.FromSeconds(time);

                    int minutes = ts.Minutes;
                    int seconds = ts.Seconds;
                    string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
                    timerTxt.text = niceTime;

                    await Task.Delay(1000);
                }
                else
                {
                    isGameOver = true;
                    break;
                }
            }

            timerTxt.text = "00:00";
            isGameOver = true;
            print("Game Is Over !");
        }

    // private void Update()
    // {
    //     print(generatedItemsPosition.Count);
    // }

    private void OnApplicationQuit()
    {
        isGameOver = true;
    }
}
