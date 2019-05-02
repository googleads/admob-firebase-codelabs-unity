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

// TODO: Import Firebase.RemoteConfig (102)


namespace AwesomeDrawingQuiz.Game {

    public class GameSettings {

        private static readonly GameSettings instance = new GameSettings();

        public const int MAX_GAME_LEVEL = 6;

        public const string DIFFICULTY_EASY = "easy";

        public const string DIFFICULTY_NORMAL = "normal";

        // TODO: Add a key for 'difficulty' Remote Config parameter (103)
        
        // TODO: Apply reward amount from the Remote Config (102)
        

        public static GameSettings Instance {
            get {
                return instance;
            }
        }

        public string Difficulty {
            get {
                // TODO: Apply difficulty from the Remote Config (103)
                return DIFFICULTY_NORMAL;
            }
            private set { }
        }

        public int RewardAmount {
            get {
                // TODO: Apply reward amount from the Remote Config (102)
                return 1;
            }
            private set { }
        }
    }

}