using System;
using GoogleMobileAds.Common;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class InterstitalGoogleAd : MonoBehaviour
{
    public static InterstitalGoogleAd Instance;
    GameManager game;
  
    int numUrchins;
    public Text urchinScoreBoard;
    int urchinScore;

    private void OnEnable()
    {
        game = GameManager.Instance;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        SeeIfAdButtonShouldShow();
       
    }

    private void CompleteMethod(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            numUrchins = PlayerPrefs.GetInt("Urchins");
            PlayerPrefs.SetInt("Urchins", (numUrchins + 50));
            urchinScore = PlayerPrefs.GetInt("Urchins");
            urchinScoreBoard.text = urchinScore.ToString();
            game.watchedAd = true;
            game.AdButton.SetActive(false);
        }
        else
        {
            //no reward
        }
    }


    public void UserChoseToWatchAd()
    {
        Debug.Log("Chose to watch ad");
        if (Advertisements.Instance.IsRewardVideoAvailable())
        {
            Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
           
        }
    }
  
  
    public void SeeIfAdButtonShouldShow()
    {
        if (Advertisements.Instance.IsRewardVideoAvailable())
        {
            if (game.watchedAd == false)
            {
                game.AdButton.SetActive(true);
            }
            else if (game.watchedAd == true)
            {
                game.AdButton.SetActive(false);
            }
        }
    }

 
}
