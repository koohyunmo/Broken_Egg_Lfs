using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdManager
{
    private RewardedAd _rewardedAd;
    private InterstitialAd _interstitialAd;
    private Action _rewardedCallback;

    string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
    public void Init()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        CreateAndLoadRewardedAd();
    }
    private void CreateAndLoadRewardedAd()
    {
        //ca-app-pub-1382577074205698/2393768043 ID
        //ca-app-pub-3940256099942544/5224354917 TESTID
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        string adUnitId = "unexpected_platform";
#endif

        /*
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
        */

        /*
        _interstitialAd = new InterstitialAd(adUnitId);
        _interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        _interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        _interstitialAd.OnAdOpening += HandleOnAdOpened;
        _interstitialAd.OnAdClosed += HandleOnAdClosed;
        _interstitialAd.LoadAd(new AdRequest.Builder().Build());
        */

        
        //this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        //this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        _rewardedAd = new RewardedAd(adUnitId);
        _rewardedAd.OnAdLoaded += HandleOnAdLoaded;
        _rewardedAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        _rewardedAd.OnAdOpening += HandleOnAdOpened;
        _rewardedAd.OnAdClosed += HandleOnAdClosed;
        _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        _rewardedAd.LoadAd(request);

    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        this.CreateAndLoadRewardedAd();
        Debug.Log("Reward Ad Close");
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLoaded");
    }

    public void HandleOnAdFailedToLoad(object sender, EventArgs args)
    {
        Debug.Log($"HandleFailedToReceiveAd : {args}");
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleAdOpened");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleAdClosed");
    }

    private void HandleUserEarnedReward(object sender, EventArgs args)
    {
        Debug.Log("HandleUserEarnedReward");
        _rewardedCallback?.Invoke();
        _rewardedCallback = null;
    }

    public void ShowRewardAd(Action rewardedCallback)
    {
        _rewardedCallback = null;
        _rewardedCallback = rewardedCallback;

        //CreateAndLoadRewardedAd();
        if (_rewardedAd.IsLoaded())
        {
            _rewardedAd.Show();
        }
        else
            CreateAndLoadRewardedAd();
    }

    public void GetSideButtonReward()
    {
        Action a = () => { Managers.Game.Additem("CH0004"); };

        ShowRewardAd(a);
    }
    public void GetMaketAdResetReward(Action b)
    {
        ShowRewardAd(b);
    }
}
