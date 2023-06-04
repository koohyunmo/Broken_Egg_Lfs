using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class FrontAd : MonoBehaviour
{
    private InterstitialAd interstitial;

    private void RequestInterstitial()
    {
#if     UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif   UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad is shown.
        this.interstitial.OnAdClosed += HandleOnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void AdShow()
    {
        RequestInterstitial();
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("Front Ad Closed");
    }
}
