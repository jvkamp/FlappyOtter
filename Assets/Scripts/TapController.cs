using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class TapController : MonoBehaviour
{

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public static TapController Instance;

    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;
    public AudioSource tapSound;
    public AudioSource scoreSound;
    public AudioSource dieSound;
    public AudioSource fartSound;
    public ParticleSystem bubbles;
    public Text urchinScoreBoard;
    public int urchinScore;

    private float i;
    Rigidbody2D rigidBody;
    Quaternion downRotation;
    Quaternion forwardRotation;

    GameManager game;
    Animator otter;
    int zones;
    int spinSpeed=0;
    bool reset;
    bool spin = false;
    
    

    void Start()
    {
        urchinScore = PlayerPrefs.GetInt("Urchins");
        rigidBody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -100);
        forwardRotation = Quaternion.Euler(0, 0, 25);
        game = GameManager.Instance;
        rigidBody.simulated = false;
        otter = rigidBody.GetComponent<Animator>();
        otter.StartPlayback();
        reset = false;
        //trail = GetComponent<TrailRenderer>();
        //trail.sortingOrder = 20; 
        urchinScoreBoard.text = urchinScore.ToString();
    }

    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    public void OnGameStarted()
    {
        rigidBody.gameObject.SetActive(true);
        rigidBody.gravityScale = 3f;
        reset = false;
        rigidBody.velocity = Vector3.zero;
        rigidBody.simulated = true;
        i = Time.time;
        otter.StopPlayback();
    }

    public void OnGameOverConfirmed()
    {
        rigidBody.simulated = false;
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
        reset = true;
        spin = false;
        spinSpeed = 0;
        rigidBody.gameObject.SetActive(false);
    }

    void Update()
    {
       
        if (spin)
        {
            transform.Rotate(Vector3.forward *10* spinSpeed * Time.deltaTime);
            spinSpeed+=10;
        }
    
        if (game.GameOver) return;

        if (Input.GetMouseButtonDown(0))
        {

            rigidBody.velocity = Vector2.zero;
            transform.rotation = Quaternion.Lerp(transform.rotation, forwardRotation, tiltSmooth * 20);
            var exp = bubbles;
            exp.Play();

            if (rigidBody.position.y < 4.85) 
            {
                    rigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
            }
           
            if (tapSound.gameObject.activeInHierarchy)
            {
                tapSound.Play();
            }
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);

    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (game.GameOver) return;
        if (col.gameObject.tag == "ScoreZone" && zones == 0)
        {
            zones = 1;
        }
        if (col.gameObject.tag == "ScoreZone" && zones == 2)
        {
            zones = 1;
        }
        if (col.gameObject.tag == "Scorezone2" && zones == 1)
        {
            zones = 2;
        }

        if (zones == 2)
        {
            OnPlayerScored();
            if (scoreSound.gameObject.activeInHierarchy)
            {
                scoreSound.Play();
            }
            zones = 0;
        }
        if (col.gameObject.tag == "DeadZone")
        {
            otter.StartPlayback();
            OnPlayerDied();
            if (dieSound.gameObject.activeInHierarchy)
            {
                dieSound.Play();
            }
            rigidBody.simulated = false;
            rigidBody.velocity = Vector2.zero;
            rigidBody.gravityScale = 15;
            if (reset == false) { rigidBody.simulated = true; spin = true; rigidBody.AddForce(Vector2.up * 1500, ForceMode2D.Force); }
        }
        if (col.gameObject.tag == "Urchin")
        {
            GameObject urchin = col.gameObject;
            UrchinScore(urchin);

            if (fartSound.gameObject.activeInHierarchy)
            {
                fartSound.Play();
            }

        }
    }

    void UrchinScore(GameObject urchin)
    {
        urchinScore = PlayerPrefs.GetInt("Urchins");
        
        urchin.transform.position = Vector3.one * -1000;
        urchinScore++;
        urchinScoreBoard.text = urchinScore.ToString();
        PlayerPrefs.SetInt("Urchins", urchinScore);
    }

}



    
       
    

