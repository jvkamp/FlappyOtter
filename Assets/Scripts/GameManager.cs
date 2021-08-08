using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;


public class GameManager : MonoBehaviour
{

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;
    public static event GameDelegate SeeIfAdButtonShouldShow;



    public delegate bool BoolDelegate();

    public static GameManager Instance;


    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public GameObject credits;
    public GameObject scoreBoard;
    public GameObject AdButton;
    public GameObject continueButton;

    public int urchinScore;
    public int score;
    public int timesContinued = 0;

    Animator otter;

    public Rigidbody2D rigidBody;

    public Text scoreText;
    public Text urchinScoreBoard;


    public bool watchedAd;
    public bool gameOver;
    public bool ShowAd;

    public enum PageState
    {
        None,
        Start,
        Countdown,
        GameOver
    }

  

    public bool GameOver { get { return gameOver; } }

    void Awake()
    {
        gameOver = true;
        watchedAd = false;
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

      

        
        otter = rigidBody.GetComponent<Animator>();
        urchinScore = PlayerPrefs.GetInt("Urchins");
        urchinScoreBoard.text = urchinScore.ToString();

       
    }

    void OnEnable()
    {
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerScored += OnPlayerScored;
        CountdownText.OnCountdownFinished += OnCountdownFinished;


        

    }

    void OnDisable()
    {
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScored -= OnPlayerScored;
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
    }

    void Start()
    {
       
    }

    private void Update()
    {
        urchinScore = PlayerPrefs.GetInt("Urchins");
        urchinScoreBoard.text = urchinScore.ToString();
    }

    public void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        if (PlayerPrefs.GetInt("ContinueScore") > 0)
        { score = PlayerPrefs.GetInt("ContinueScore"); }
        else 
        { 
            score = 0;
            scoreText.text = score.ToString();
        }
        gameOver = false;
    }

    void OnPlayerScored()
    {
        score++;
        scoreText.text = score.ToString();

        if (score >= 10)
        {
            GameServices.Instance.SubmitAchievement(AchievementNames.GettingTheHangOfIt);
        }
        if (score >= 50)
        {
            GameServices.Instance.SubmitAchievement(AchievementNames.NotToFifty);
        }
    }

    void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        if (score > 0 && timesContinued < 2)
        { 
            continueButton.SetActive(true);
        }
        else if (timesContinued == 2)
        {
            continueButton.SetActive(false);
            timesContinued = 0;
        }
        else
        {
            continueButton.SetActive(false);
        }

        SetPageState(PageState.GameOver);

        if (Social.localUser.authenticated) 
        {
            urchinScore = PlayerPrefs.GetInt("Urchins");
            GameServices.Instance.SubmitScore(urchinScore, LeaderboardNames.UrchinKing);
            GameServices.Instance.SubmitScore(score, LeaderboardNames.HighScores);

            if(score < 1)
            {
                 GameServices.Instance.SubmitAchievement(AchievementNames.WannaReadTheInstructions);
            }
         
        }

  
       if (Advertisements.Instance.IsRewardVideoAvailable())
        {
            if (watchedAd == false)
            {
                AdButton.SetActive(true);
            }
            else if (watchedAd == true)
            {
                AdButton.SetActive(false);
            }
        }
       else
        {
           AdButton.SetActive(false);
        }
        
    }

    public void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                scoreBoard.SetActive(true);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                scoreBoard.SetActive(false);
                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                credits.SetActive(false);
                break;
        }
    }

    public void ConfirmGameOver()
    {
        SetPageState(PageState.Start);
        scoreText.text = score.ToString();
        OnGameOverConfirmed();
    }

    public void StartGame()
    {
        rigidBody.gameObject.SetActive(true);
        otter.StartPlayback();
        SetPageState(PageState.Countdown);
    }
   
    
    public void GameRestart ()
    {
        OnGameOverConfirmed();
        StartGame();
        scoreText.text = "0";
    }

    public void  PaidHomeage()
    {
        GameServices.Instance.SubmitAchievement(AchievementNames.PaidHomage);
    }
}