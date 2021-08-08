using UnityEngine;


public class GoogleAdBanner : MonoBehaviour
{
  

    public void OnEnable()
    {
        Advertisements.Instance.Initialize();
        Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM);
    }

}


