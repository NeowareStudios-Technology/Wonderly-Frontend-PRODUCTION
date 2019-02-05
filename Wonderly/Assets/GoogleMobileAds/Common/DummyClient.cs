// Copyright (C) 2015 Google, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Reflection;

using GoogleMobileAds.Api;
using UnityEngine;

namespace GoogleMobileAds.Common
{
    public class DummyClient : IBannerClient, IInterstitialClient, IRewardBasedVideoAdClient,
            IAdLoaderClient, IMobileAdsClient
    {
        public DummyClient()
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        // Disable warnings for unused dummy ad events.
#pragma warning disable 67

        public event EventHandler<EventArgs> OnAdLoaded;

        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        public event EventHandler<EventArgs> OnAdOpening;

        public event EventHandler<EventArgs> OnAdStarted;

        public event EventHandler<EventArgs> OnAdClosed;

        public event EventHandler<Reward> OnAdRewarded;

        public event EventHandler<EventArgs> OnAdLeavingApplication;

        public event EventHandler<EventArgs> OnAdCompleted;

        public event EventHandler<CustomNativeEventArgs> OnCustomNativeTemplateAdLoaded;

#pragma warning restore 67

        public string UserId
        {
            get
            {
                //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
                return "UserId";
            }

            set
            {
                //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
            }
        }

        public void Initialize(string appId)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void SetApplicationMuted(bool muted)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void SetApplicationVolume(float volume)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void SetiOSAppPauseOnBackground(bool pause)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateBannerView(string adUnitId, AdSize adSize, AdPosition position)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateBannerView(string adUnitId, AdSize adSize, int positionX, int positionY)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void LoadAd(AdRequest request)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void ShowBannerView()
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void HideBannerView()
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void DestroyBannerView()
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public float GetHeightInPixels()
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
            return 0;
        }

        public float GetWidthInPixels()
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
            return 0;
        }

        public void SetPosition(AdPosition adPosition)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void SetPosition(int x, int y)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateInterstitialAd(string adUnitId)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public bool IsLoaded()
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
            return true;
        }

        public void ShowInterstitial()
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void DestroyInterstitial()
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateRewardBasedVideoAd()
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void SetUserId(string userId)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void LoadAd(AdRequest request, string adUnitId)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void DestroyRewardBasedVideoAd()
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void ShowRewardBasedVideoAd()
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateAdLoader(AdLoader.Builder builder)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void Load(AdRequest request)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void SetAdSize(AdSize adSize)
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public string MediationAdapterClassName()
        {
            //debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
            return null;
        }

    }
}
