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
// TODO: Import AwesomeDrawingQuiz.Analytics (101)

using AwesomeDrawingQuiz.Game;

// TODO: Import Firebase.RemoteConfig (102)


namespace AwesomeDrawingQuiz.Scene {
    public class Main : MonoBehaviour {

        public Button buttonStartGame;

        void Start () {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;

            // TODO: Check Google Play Services on Android (101)
            
            // TODO: Set screen name (101)
            
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

            // TODO: Set a default value for 'difficulty' Remote Config parameter (103)

            return null;
        }

        private void ActivateRemoteConfigValues() {
            // TODO: Activate fetched Remote Config values (102)

        }
    }
}
