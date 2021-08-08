namespace GleyMobileAds
{
    using System.Collections.Generic;
    using UnityEngine.Events;

    /// <summary>
    /// interface implemented by all supported advertisers
    /// </summary>
    public interface ICustomAds
    {
        void InitializeAds(GDPRConsent consent, List<PlatformSettings> platformSettings);
        bool IsRewardVideoAvailable();
        void ShowRewardVideo(UnityAction<bool> CompleteMethod);
        void ShowRewardVideo(UnityAction<bool,string> CompleteMethod);
        bool IsInterstitialAvailable();
        void ShowInterstitial(UnityAction InterstitialClosed);
        void ShowInterstitial(UnityAction<string> InterstitialClosed);
        bool IsBannerAvailable();
        void ShowBanner(BannerPosition position, BannerType bannerType, UnityAction<bool, BannerPosition, BannerType> DisplayResult);
        void HideBanner();
        bool BannerAlreadyUsed();
        void ResetBannerUsage();
        void UpdateConsent(GDPRConsent consent);
    }
}

public enum BannerPosition
{
    TOP,
    BOTTOM
}

public enum BannerType
{
    Banner,
    SmartBanner
}
