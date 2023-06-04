using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAd : MonoBehaviour
{
    private RewardedAd rewardedAd;

    public void CreateAndLoadRewardedAd()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            string adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        //this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        //this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        this.CreateAndLoadRewardedAd();
        Debug.Log("Reward Ad Close");
    }

    private void HandleUserEarnedReward(object sender, EventArgs args)
    {
        Debug.Log("HandleUserEarnedReward");
    }

    public void ShowAd()
    {
        CreateAndLoadRewardedAd();
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }


}
