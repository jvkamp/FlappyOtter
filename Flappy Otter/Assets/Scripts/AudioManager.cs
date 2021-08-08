using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

   public GameObject SFXon;
    public GameObject SFXoff;
    public GameObject MusicOn;
    public GameObject MusicOff;
    public GameObject SFXsource;
    public GameObject MusicSource;
    bool MusicStatus;
    bool SFXStatus;
   


    public void OnEnable()
    {
        SFXStatus = PlayerPrefs.GetInt("sfx", 1) == 1;
        MusicStatus = PlayerPrefs.GetInt("music", 1) == 1;
        
        if (!SFXStatus)
        {
            SFXoff.SetActive(true);
            SFXsource.SetActive(false);
          
        }
        else
        {
            SFXon.SetActive(true);
            SFXsource.SetActive(true);
        }
        if (!MusicStatus)
        {
            MusicOff.SetActive(true);
            MusicSource.SetActive(false);
        }
        else
        {
            MusicOn.SetActive(true);
            MusicSource.SetActive(true);
        }
    }

    public void OnDisable()
    {
        PlayerPrefs.Save();
    }

    public void SetSFXOff()
    {
        PlayerPrefs.SetInt("sfx", 0);
        Debug.Log("Set sfx off");
        PlayerPrefs.Save();
    }

    public void SetSFXOn()
    {
        PlayerPrefs.SetInt("sfx", 1);
        Debug.Log("Set sfx on");
        PlayerPrefs.Save();
    }
    public void SetMusicOff()
    {
        PlayerPrefs.SetInt("music", 0);
        Debug.Log("Set music off");
        PlayerPrefs.Save();
    }

    public void SetMusicOn()
    {
        PlayerPrefs.SetInt("music", 1);
        Debug.Log("Set music on");
        PlayerPrefs.Save();
    }
}