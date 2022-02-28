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


    private void Start() {

          GameTimer();   
    }

   
       private async void GameTimer()
        {
            int time = oneRoundTimer * 60;
            while (time > 0) // 40
            {
                time--;
                var ts = TimeSpan.FromSeconds(time);    

                int minutes = ts.Minutes;
                int seconds = ts.Seconds;
                string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
                timerTxt.text = niceTime;

                await Task.Delay(1000);
            }

            timerTxt.text = "00:00";
            isGameOver = true;
            print("Game Is Over !");
        }
}
