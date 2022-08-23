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

// TODO: Import Firebase.RemoteConfig (102)


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
                // TODO: Call InitAndFetchRemoteConfig() (102)
                // TODO: Call ActivateRemoteConfigValues() (102)
            }).ContinueWith(task => {
                // Enable 'Start a game' button
                UnityMainThreadDispatcher.Instance()
                    .Enqueue(() => buttonStartGame.interactable = true);
            });
            #else
            QuizAnalytics.SetScreenName(QuizAnalytics.SCREEN_MAIN);

            // TODO: Call InitAndFetchRemoteConfig() (102)
            // TODO: Call ActivateRemoteConfigValues() (102)
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
            // TODO: Initialize and Fetch values from the Remote Config (102)

            return null;
        }

        private void ActivateRemoteConfigValues() {
            // TODO: Activate fetched Remote Config values (102)

        }
    }
}
