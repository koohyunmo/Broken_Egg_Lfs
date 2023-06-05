using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AdManager
{
    private RewardedAd _rewardedAd;
    private InterstitialAd _interstitialAd;
    private Action _rewardedCallback;

    List<string> testDeviceIds = new List<string>();

    public void Init()
    {
        testDeviceIds.Add("d98259fd-80b9-429c-ad8d-5af514ebed3c");
        testDeviceIds.Add("a8f37508-30d4-4e32-88f7-6a46079a32c8");
        testDeviceIds.Add("a4c5f293-463f-47a3-afb4-540626da7039");
        testDeviceIds.Add("0921bb97-a781-4bf2-9c06-d6bf190c6fad");
        testDeviceIds.Add("e26ca221-7478-4a0f-a9af-6a67dc563a49");
        testDeviceIds.Add("7a7678d6-dbfa-41d4-8d32-e8ab246163a5");
        testDeviceIds.Add("e2408ab4-f6d9-4c17-88ed-0be55b85b2ac");
        testDeviceIds.Add("c80ca528-95a4-4907-8c4e-90105ef01c75");

        testDeviceIds.Add(AdRequest.TestDeviceSimulator);
        RequestConfiguration config = new RequestConfiguration.Builder().SetTestDeviceIds(testDeviceIds).build();

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        CreateAndLoadRewardedAd();
    }
    private void CreateAndLoadRewardedAd()
    {
        //ca-app-pub-1382577074205698/8914656664 ID
        //ca-app-pub-3940256099942544/5224354917 TESTID
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-1382577074205698/8914656664";
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
