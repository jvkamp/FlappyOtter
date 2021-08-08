namespace GleyMobileAds
{
    using UnityEngine.Events;
    using UnityEngine;
#if USE_UNITYADS
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.Advertisements;
    using UnityEngine.Monetization;
#endif

    public class CustomUnityAds : MonoBehaviour, ICustomAds
    {
#if USE_UNITYADS
        private UnityAction<bool> OnCompleteMethod;
        private UnityAction<bool, string> OnCompleteMethodWithAdvertiser;
        private UnityAction OnInterstitialClosed;
        private UnityAction<string> OnInterstitialClosedWithAdvertiser;

        private string unityAdsId;
        private string bannerPlacement;
        private string videoAdPlacement;
        private string rewardedVideoAdPlacement;
        private bool debug;
        private bool bannerUsed;
        private global::BannerPosition position;
        private BannerType bannerType;
        private UnityAction<bool, global::BannerPosition, BannerType> DisplayResult;

        /// <summary>
        /// Initializing Unity Ads
        /// </summary>
        /// <param name="consent">user consent -> if true show personalized ads</param>
        /// <param name="platformSettings">contains all required settings for this publisher</param>
        public void InitializeAds(GDPRConsent consent, List<PlatformSettings> platformSettings)
        {
            debug = Advertisements.Instance.debug;

            //get settings
#if UNITY_ANDROID
            PlatformSettings settings = platformSettings.First(cond => cond.platform == SupportedPlatforms.Android);
#endif
#if UNITY_IOS
            PlatformSettings settings = platformSettings.First(cond => cond.platform == SupportedPlatforms.iOS);
#endif
            //apply settings
            unityAdsId = settings.appId.id;
            bannerPlacement = settings.idBanner.id;
            videoAdPlacement = settings.idInterstitial.id;
            rewardedVideoAdPlacement = settings.idRewarded.id;

            //verify settings
            if (debug)
            {
                Debug.Log(this + " Initialization Started");
                ScreenWriter.Write(this + " Initialization Started");
                Debug.Log(this + " App ID: " + unityAdsId);
                ScreenWriter.Write(this + " App ID: " + unityAdsId);
                Debug.Log(this + " Banner placement ID: " + bannerPlacement);
                ScreenWriter.Write(this + " Banner Placement ID: " + bannerPlacement);
                Debug.Log(this + " Interstitial Placement ID: " + videoAdPlacement);
                ScreenWriter.Write(this + " Interstitial Placement ID: " + videoAdPlacement);
                Debug.Log(this + " Rewarded Video Placement ID: " + rewardedVideoAdPlacement);
                ScreenWriter.Write(this + " Rewarded Video Placement ID: " + rewardedVideoAdPlacement);
            }

            //preparing Unity Ads SDK for initialization
            if (consent != GDPRConsent.Unset)
            {
                UnityEngine.Monetization.MetaData gdprMetaData = new UnityEngine.Monetization.MetaData("gdpr");
                if (consent == GDPRConsent.Accept)
                {
                    gdprMetaData.Set("consent", "true");
                }
                else
                {
                    gdprMetaData.Set("consent", "false");
                }
                Monetization.SetMetaData(gdprMetaData);
            }
            Monetization.Initialize(unityAdsId, false);
            //Advertisement.Initialize(unityAdsId, false);
        }




        /// <summary>
        /// Updates consent at runtime
        /// </summary>
        /// <param name="consent">the new consent</param>
        public void UpdateConsent(GDPRConsent consent)
        {
            if (consent != GDPRConsent.Unset)
            {
                UnityEngine.Monetization.MetaData gdprMetaData = new UnityEngine.Monetization.MetaData("gdpr");
                if (consent == GDPRConsent.Accept)
                {
                    gdprMetaData.Set("consent", "true");
                }
                else
                {
                    gdprMetaData.Set("consent", "false");
                }
                Monetization.SetMetaData(gdprMetaData);
            }

            Debug.Log(this + " Update consent to " + consent);
            ScreenWriter.Write(this + " Update consent to " + consent);
        }


        /// <summary>
        /// Check if Unity Ads interstitial is available
        /// </summary>
        /// <returns>true if an interstitial is available</returns>
        public bool IsInterstitialAvailable()
        {
            return Monetization.IsReady(videoAdPlacement);
        }


        /// <summary>
        /// Show Unity Ads interstitial
        /// </summary>
        /// <param name="InterstitialClosed">callback called when user closes interstitial</param>
        public void ShowInterstitial(UnityAction InterstitialClosed)
        {
            if (IsInterstitialAvailable())
            {
                OnInterstitialClosed = InterstitialClosed;
                ShowAdPlacementContent ad = null;
                ad = Monetization.GetPlacementContent(videoAdPlacement) as ShowAdPlacementContent;

                if (ad != null)
                {
                    ad.Show(InterstitialCallback);
                }
            }
        }


        /// <summary>
        /// Show Unity Ads interstitial
        /// </summary>
        /// <param name="InterstitialClosed">callback called when user closes interstitial</param>
        public void ShowInterstitial(UnityAction<string> InterstitialClosed)
        {
            if (IsInterstitialAvailable())
            {
                OnInterstitialClosedWithAdvertiser = InterstitialClosed;
                ShowAdPlacementContent ad = null;
                ad = Monetization.GetPlacementContent(videoAdPlacement) as ShowAdPlacementContent;

                if (ad != null)
                {
                    ad.Show(InterstitialCallback);
                }
            }
        }


        /// <summary>
        /// Callback method called by interstitial closed event
        /// </summary>
        /// <param name="result"></param>
        private void InterstitialCallback(UnityEngine.Monetization.ShowResult result)
        {
            if (debug)
            {
                Debug.Log(this + "Interstitial result: " + result.ToString());
                ScreenWriter.Write(this + "Interstitial result: " + result.ToString());
            }

            if (OnInterstitialClosed != null)
            {
                OnInterstitialClosed();
                OnInterstitialClosed = null;
            }

            if (OnInterstitialClosedWithAdvertiser != null)
            {
                OnInterstitialClosedWithAdvertiser(SupportedAdvertisers.Unity.ToString());
                OnInterstitialClosedWithAdvertiser = null;
            }
        }


        /// <summary>
        /// Check if Unity Ads rewarded video is available
        /// </summary>
        /// <returns>true if a rewarded video is available</returns>
        public bool IsRewardVideoAvailable()
        {
            return Monetization.IsReady(rewardedVideoAdPlacement);
        }


        /// <summary>
        /// Show Unity Ads rewarded video
        /// </summary>
        /// <param name="CompleteMethod">callback called when user closes the rewarded video -> if true, video was not skipped</param>
        public void ShowRewardVideo(UnityAction<bool> CompleteMethod)
        {
            if (IsRewardVideoAvailable())
            {
                OnCompleteMethod = CompleteMethod;
                ShowAdPlacementContent ad = null;
                ad = Monetization.GetPlacementContent(rewardedVideoAdPlacement) as ShowAdPlacementContent;

                if (ad != null)
                {
                    ad.Show(RewardedCallback);
                }
            }
        }

        /// <summary>
        /// Show Unity Ads rewarded video
        /// </summary>
        /// <param name="CompleteMethod">callback called when user closes the rewarded video -> if true, video was not skipped</param>
        public void ShowRewardVideo(UnityAction<bool, string> CompleteMethod)
        {
            if (IsRewardVideoAvailable())
            {
                OnCompleteMethodWithAdvertiser = CompleteMethod;
                ShowAdPlacementContent ad = null;
                ad = Monetization.GetPlacementContent(rewardedVideoAdPlacement) as ShowAdPlacementContent;

                if (ad != null)
                {
                    ad.Show(RewardedCallback);
                }
            }
        }


        /// <summary>
        /// Callback method called by rewarded video closed event
        /// </summary>
        /// <param name="result"></param>
        private void RewardedCallback(UnityEngine.Monetization.ShowResult result)
        {
            if (debug)
            {
                Debug.Log(this + "Complete method result: " + result.ToString());
                ScreenWriter.Write(this + "Complete method result: " + result.ToString());
            }
            if (result == UnityEngine.Monetization.ShowResult.Finished)
            {
                if (OnCompleteMethod != null)
                {
                    OnCompleteMethod(true);
                    OnCompleteMethod = null;
                }
                if (OnCompleteMethodWithAdvertiser != null)
                {
                    OnCompleteMethodWithAdvertiser(true, SupportedAdvertisers.Unity.ToString());
                    OnCompleteMethodWithAdvertiser = null;
                }
            }
            else
            {
                if (OnCompleteMethod != null)
                {
                    OnCompleteMethod(false);
                    OnCompleteMethod = null;
                }
                if (OnCompleteMethodWithAdvertiser != null)
                {
                    OnCompleteMethodWithAdvertiser(false, SupportedAdvertisers.Unity.ToString());
                    OnCompleteMethodWithAdvertiser = null;
                }
            }
        }


        //Unity Ads does not support banner ads
        public bool IsBannerAvailable()
        {
            return Advertisement.IsReady(bannerPlacement);
        }


        /// <summary>
        /// Used for mediation purpose
        /// </summary>
        public void ResetBannerUsage()
        {
            bannerUsed = false;
        }


        /// <summary>
        /// Used for mediation purpose
        /// </summary>
        /// <returns>true if current banner failed to load</returns>
        public bool BannerAlreadyUsed()
        {
            return bannerUsed;
        }
        public void ShowBanner(global::BannerPosition position, BannerType bannerType, UnityAction<bool, global::BannerPosition, BannerType> DisplayResult)
        {
            if (IsBannerAvailable())
            {
                bannerUsed = true;

                this.position = position;
                this.bannerType = bannerType;
                this.DisplayResult = DisplayResult;

                BannerLoadOptions options = new BannerLoadOptions
                {
                    errorCallback = BannerLoadFailed,
                    loadCallback = BannerLoadSuccess
                };
                if (debug)
                {
                    Debug.Log(this + "Start Loading Placement:" + bannerPlacement + " is ready " + Advertisement.IsReady(bannerPlacement));
                    ScreenWriter.Write(this + "Start Loading Placement:" + bannerPlacement + " is ready " + Advertisement.IsReady(bannerPlacement));
                }
                if (position == global::BannerPosition.BOTTOM)
                {
                    Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
                }
                else
                {
                    Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
                }
                Advertisement.Banner.Load(bannerPlacement, options);
            }
        }

        private void BannerLoadSuccess()
        {
            if (debug)
            {
                Debug.Log(this + "Banner Load Success");
                ScreenWriter.Write(this + " Banner Load Success");
            }

            BannerOptions options = new BannerOptions
            {
                showCallback = BanerDisplayed,
                hideCallback = BannerHidded
            };
            Advertisement.Banner.Show(bannerPlacement, options);
        }

        private void BannerLoadFailed(string message)
        {
            if (debug)
            {
                Debug.Log(this + "Banner Load Failed " + message);
                ScreenWriter.Write(this + " Banner Load Failed " + message);
            }
            if (DisplayResult != null)
            {
                DisplayResult(false, position, bannerType);
                DisplayResult = null;
            }
        }
        private void BanerDisplayed()
        {
            if (debug)
            {
                Debug.Log(this + "Banner Displayed");
                ScreenWriter.Write(this + "Banner Displayed");
            }
            if (DisplayResult != null)
            {
                DisplayResult(true, position, bannerType);
                DisplayResult = null;
            }
        }


        private void BannerHidded()
        {
            if (debug)
            {
                Debug.Log(this + "Banner Hidden");
                ScreenWriter.Write(this + "Banner Hidden");
            }
        }

        public void HideBanner()
        {
            Advertisement.Banner.Hide(true);
        }



#else
        //dummy interface implementation, used when Heyzap is not enabled
        public void HideBanner()
        {

        }

        public void InitializeAds(GDPRConsent consent, System.Collections.Generic.List<PlatformSettings> platformSettings)
        {

        }

        public bool IsBannerAvailable()
        {
            return false;
        }

        public bool IsInterstitialAvailable()
        {
            return false;
        }

        public bool IsRewardVideoAvailable()
        {
            return false;
        }

        public void ShowBanner(BannerPosition position, BannerType type, UnityAction<bool, BannerPosition, BannerType> DisplayResult)
        {

        }

        public void ResetBannerUsage()
        {

        }

        public bool BannerAlreadyUsed()
        {
            return false;
        }

        public void ShowInterstitial(UnityAction InterstitialClosed = null)
        {

        }

        public void ShowInterstitial(UnityAction<string> InterstitialClosed)
        {

        }

        public void ShowRewardVideo(UnityAction<bool> CompleteMethod)
        {

        }

        public void ShowRewardVideo(UnityAction<bool, string> CompleteMethod)
        {

        }

        public void UpdateConsent(GDPRConsent consent)
        {

        }
#endif
    }
}

