// Copyright 2019 Google LLC
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using GoogleMobileAds.Api;

namespace AwesomeDrawingQuiz.Ads {
    public class AdManager {

        public const string APP_ID_ANDROID = "ca-app-pub-3940256099942544~3048611032";
        public const string APP_ID_IOS = "ca-app-pub-3940256099942544~2753522596";

        #if UNITY_EDITOR
        private const string APP_ID = "unused";
        public const string AD_UNIT_ID = "unused";
        #elif UNITY_ANDROID
        private const string APP_ID = APP_ID_ANDROID;
        public const string AD_UNIT_ID = "ca-app-pub-3940256099942544/3399964826";
        #elif UNITY_IOS
        private const string APP_ID = APP_ID_IOS;
        public const string AD_UNIT_ID = "ca-app-pub-3940256099942544/6557036230";
        #else
        private const string APP_ID = "unexpected_platform";
        public const string AD_UNIT_ID = "unexpected_platform";
        #endif

        private static readonly AdManager instance = new AdManager();

        public static AdManager Instance {
            get {
                return instance;
            }
        }

        public bool IsAdAvailable { 
            get {
                return rewardVideo.IsLoaded();
            } 
            private set { } 
        }

        private RewardBasedVideoAd rewardVideo = null;

        private AdManager() {
            MobileAds.Initialize(APP_ID);
        
            rewardVideo = RewardBasedVideoAd.Instance;

            rewardVideo.OnAdLoaded += this.OnAdLoaded;
            rewardVideo.OnAdFailedToLoad += this.OnAdFailedToLoad;
            rewardVideo.OnAdStarted += this.OnRewardedAdStarted;
            rewardVideo.OnAdRewarded += this.OnAdRewarded;
            rewardVideo.OnAdClosed += this.OnAdClosed;
        }

        public void ShowAd() {
            if (!rewardVideo.IsLoaded()) {
                Debug.Log("Rewarded Ad is not loaded");
                return;
            }
            rewardVideo.Show();
        }

        public void LoadAd() {
            rewardVideo.LoadAd(new AdRequest.Builder()
                .AddTestDevice(AdRequest.TestDeviceSimulator).Build(), 
                AD_UNIT_ID);
        }
        
        #region Rewarded Video callbacks

        private void OnAdLoaded(object sender, EventArgs args) {
            Debug.Log("Rewarded Video Ad Loaded");
            if (this.OnAdReady != null) {
                this.OnAdReady(this, EventArgs.Empty);
            }
        }

        private void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
            Debug.Log("Ad not loaded: " + args.Message);
            if (this.OnAdNotAvailable != null) {
                this.OnAdNotAvailable(this, EventArgs.Empty);
            }
        }

        private void OnRewardedAdStarted(object sender, EventArgs args) {
            Debug.Log("Ad started");
            if (this.OnAdStarted != null) {
                this.OnAdStarted(this, EventArgs.Empty);
            }
        }

        private void OnAdRewarded(object sender, Reward reward) {
            Debug.Log("On rewarded: " + reward.Type + " " + reward.Amount);
            if (this.OnRewarded != null) {
                this.OnRewarded(this, EventArgs.Empty);
            }
        }

        private void OnAdClosed(object sender, EventArgs args) {
            // Prelaod the next Ad
            LoadAd();
        }

        #endregion# region 

        #region AdManager event callbacks

        public event EventHandler<EventArgs> OnAdReady = delegate { };

        public event EventHandler<EventArgs> OnAdNotAvailable = delegate { };

        public event EventHandler<EventArgs> OnAdStarted = delegate { };

        public event EventHandler<EventArgs> OnRewarded = delegate { };

        #endregion
    }
}
