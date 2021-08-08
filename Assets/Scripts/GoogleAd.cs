using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;


public class GoogleAd : MonoBehaviour
{
    private BannerView bannerView;

    public void Start()
    {
  /*    #if UNITY_ANDROID
            string appId = "";
#elif UNITY_IOS
            string appId = "";
#else
            string appId = "unexpected_platform";
#endif*/

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize("ca-app-pub-9272444020139042~4363836941");

        this.RequestBanner();
    }

    private void RequestBanner()
    {
    /*   #if UNITY_ANDROID
            string adUnitId = "";
       #elif UNITY_IPHONE
            string adUnitId = "";
        #else
            string adUnitId = "unexpected_platform";
        #endif*/

        // Create a 320x50 banner at the bottom of the screen.
        this.bannerView = new BannerView("ca-app-pub-3940256099942544/6300978111", AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);
    }
}


