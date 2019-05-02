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

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using AwesomeDrawingQuiz.Ads;
using AwesomeDrawingQuiz.Analytics;
using AwesomeDrawingQuiz.Game;

using Firebase.RemoteConfig;

namespace AwesomeDrawingQuiz.Scene {
    public class Main : MonoBehaviour {

        public Button buttonStartGame;

        void Start () {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;

            #if UNITY_ANDROID
            // Disable 'Start a game' button until
            // Firebase dependencies are ready to use on the Android
            buttonStartGame.interactable = false;

            // Check Google Play Services on Android device is up to date
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available) {
                    Debug.Log("All Firebase services are available");
                    
                    QuizAnalytics.SetScreenName(QuizAnalytics.SCREEN_MAIN);
                } else {
                    throw new System.InvalidOperationException(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                }
            }).ContinueWith(task => { 
                InitAndFetchRemoteConfig();
            }).ContinueWith(task => {
                ActivateRemoteConfigValues();

                // Enable 'Start a game' button
                UnityMainThreadDispatcher.Instance()
                    .Enqueue(() => buttonStartGame.interactable = true);
            });
            #else
            QuizAnalytics.SetScreenName(QuizAnalytics.SCREEN_MAIN);

            InitAndFetchRemoteConfig().ContinueWith(task => {
                ActivateRemoteConfigValues();
            });
            #endif
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();
            }
        }
        
        public void StartGame() {
            SceneManager.LoadScene("Game");
        }

        private Task InitAndFetchRemoteConfig() {
            Dictionary<string, object> defaults = new Dictionary<string, object>();
            // TODO: Set a default value for 'difficulty' Remote Config parameter (103)
            
            defaults.Add(GameSettings.KEY_REWARD_AMOUNT, 1);
            FirebaseRemoteConfig.SetDefaults(defaults);

            if (Debug.isDebugBuild) {
                ConfigSettings config = new ConfigSettings();
                config.IsDeveloperMode = true;
                FirebaseRemoteConfig.Settings = config;
                return FirebaseRemoteConfig.FetchAsync(System.TimeSpan.Zero);
            } else {
                return FirebaseRemoteConfig.FetchAsync();
            }
        }

        private void ActivateRemoteConfigValues() {
            FirebaseRemoteConfig.ActivateFetched();
        }
    }
}
