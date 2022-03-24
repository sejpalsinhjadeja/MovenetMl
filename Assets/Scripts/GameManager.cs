using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int oneRoundTimer = 2;
    [SerializeField] TextMeshProUGUI timerTxt = null;
    [SerializeField] TextMeshProUGUI scoreTxt = null;

    public Button restartBtn;
    
    public static bool isGameOver = false;
    //public static int gameTotalScore = 0;

    public int positiveScore = 0;
    public int negativeScore = 0;
    
    public static List<GameItem> generatedItems;
    private int score = 0;
    
    private void Start()
    {
        restartBtn.gameObject.SetActive(false);
        generatedItems = new List<GameItem>();

        EventManager.UpdateScore += UpdateScoreValue;
        
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
                    EventManager.NotifyGameOver();
                    restartBtn.gameObject.SetActive(true);
                    break;
                }
            }

            timerTxt.text = "00:00";
            isGameOver = true;
            print("Game Is Over !");
        }

    private void OnApplicationQuit()
    {
        isGameOver = true;
        Application.Quit();
    }

    void UpdateScoreValue(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Touchable:
                score += positiveScore;
                break;
            case ObjectType.Nontouchable:
                score -= negativeScore;
                break;
        }
        
        scoreTxt.text = score.ToString();
    }

    public void Restart()
    {
        isGameOver = false;
        score = 0;
        EventManager.NotifyRestartGame();
        SceneManager.LoadScene("SampleScene");
        
    }
}
