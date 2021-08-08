namespace GleyMobileAds
{
    using System.Collections.Generic;

    //used to generate the settings file
    [System.Serializable]
    public enum SupportedAdvertisers
    {
        Admob,
        Vungle,
        AdColony,
        Chartboost,
        Unity,
        Heyzap,
        AppLovin,
        Facebook,
        MoPub,
        IronSource
    }

    [System.Serializable]
    public class AdOrder
    {
        public SupportedMediation bannerMediation;
        public SupportedMediation interstitialMediation;
        public SupportedMediation rewardedMediation;
        public List<MediationSettings> advertisers = new List<MediationSettings>();
    }
}
