using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UrchinPoints : MonoBehaviour
{
    int numUrchins;
    public delegate void PlayerDelegate();

    public Text urchinScoreBoard;
     int urchinScore=0;

    GameManager game;
  
    int freeUrchinClick;

  
    // Start is called before the first frame update
    void Start()
    {
        game = GameManager.Instance;

        freeUrchinClick = 0;
        Debug.Log("UrchinPoints Started");
    }

    // Update is called once per frame
    void Update()
    {
        numUrchins = PlayerPrefs.GetInt("Urchins");

        if (numUrchins >= 500)
        {
                GameServices.Instance.SubmitAchievement(AchievementNames.OtterlyRich);
        }
        if (numUrchins >= 1000)
        {
            GameServices.Instance.SubmitAchievement(AchievementNames.OnePercent);
        }
    }

    public void ContinueButton()
    {
        if (PlayerPrefs.GetInt("Urchins")>99)
        {
            PlayerPrefs.SetInt("ContinueScore", game.score);
            PlayerPrefs.SetInt("Urchins", (numUrchins - 100));
            urchinScore = PlayerPrefs.GetInt("Urchins");
            urchinScoreBoard.text = urchinScore.ToString();
            game.ConfirmGameOver();
            
            game.StartGame();
            game.timesContinued++;

            if (game.timesContinued == 1)
            {
                GameServices.Instance.SubmitAchievement(AchievementNames.TryAgain, SubmitComplete);
            }
            else if (game.timesContinued == 2)
            {
                GameServices.Instance.SubmitAchievement(AchievementNames.TryTryAgain);
            }
        }
   
    }
    private void SubmitComplete(bool success, GameServicesError message)
    {
        if (success)
        {
            //achievement was submitted
        }
        else
        {
            //an error occurred
            Debug.LogError("Achievement failed to submit: " + message);
        }
    }

    public void NoContinue()
    {
        PlayerPrefs.SetInt("ContinueScore", 0);
      
        //game.watchedAd = false;
    }

    public void FreeUrchins()
    {
        
        freeUrchinClick++;
        Debug.Log(freeUrchinClick);
        if (PlayerPrefs.GetString("GotFreeUrchins") != "Yes")
        {
            
            if (freeUrchinClick == 10)
            {
                int addUrchins = PlayerPrefs.GetInt("Urchins");
                PlayerPrefs.SetInt("Urchins", addUrchins + 300);
                PlayerPrefs.SetString("GotFreeUrchins", "Yes");
                urchinScore = PlayerPrefs.GetInt("Urchins");
                GameServices.Instance.SubmitAchievement(AchievementNames.EasterEgg);
            }
        }
    }
}
