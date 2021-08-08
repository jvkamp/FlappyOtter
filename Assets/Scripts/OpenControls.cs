using UnityEngine;
using UnityEngine.UI;



public class OpenControls : MonoBehaviour
{
  
    public Text authStatus;
    public GameObject ldrButton;
    

    // Start is called before the first frame update
    public void Start()
    {
       
        GameServices.Instance.LogIn(SignInCallback);

    }
    public void SignInCallback(bool success)
    {
        if (success)
        {
            Debug.Log("(Flappy Otter) Signed in!");

            // Change sign-in button text
         

            // Show the user's name
            authStatus.text = "Signed in as: " + Social.localUser.userName;


        }
        else
        {
            Debug.Log("(Flappy Otter) Sign-in failed...");

            // Show failure message

        }
    }


    public void SignIn()
    {
        Debug.Log("signInButton clicked!");

        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(SignInCallback);

            GameServices.Instance.ShowLeaderboadsUI();
        }
        else
        {
            // Sign out of play games
            GameServices.Instance.ShowLeaderboadsUI();
        }

    }


}
