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

using Firebase.Analytics;

namespace AwesomeDrawingQuiz.Analytics {

    public class QuizAnalytics {

        private const string EVENT_AD_REWARD_PROMPT = "ad_reward_prompt";

        private const string EVENT_AD_REWARD_IMPRESSION = "ad_reward_impression";

        private const string EVENT_LEVEL_FAIL = "level_fail";

        private const string EVENT_LEVEL_SUCCESS = "level_success";

        private const string EVENT_LEVEL_WRONG_ANSWER = "level_wrong_answer";

        private const string EVENT_GAME_START = "game_start";

        private const string EVENT_GAME_COMPLETE = "game_complete";

        private const string PARAM_AD_UNIT_ID = "ad_unit_id";

        private const string PARAM_ELAPSED_TIME_SEC = "elapsed_time_sec";

        private const string PARAM_HINT_USED = "hint_used";

        private const string PARAM_NUMBER_OF_ATTEMPTS = "number_of_attempts";

        private const string PARAM_NUMBER_OF_CORRECT_ANSWERS = "number_of_correct_answers";

        public const string SCREEN_MAIN = "main";

        public const string SCREEN_GAME = "game";

        public static void LogGameStart() {
            FirebaseAnalytics.LogEvent(EVENT_GAME_START);
        }

        public static void LogLevelStart(string levelName) {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, 
                FirebaseAnalytics.ParameterLevelName, levelName);
        }

        public static void LogLevelWrongAnswer(string levelName) {
            FirebaseAnalytics.LogEvent(EVENT_LEVEL_WRONG_ANSWER, 
                FirebaseAnalytics.ParameterLevelName, levelName);
        }

        public static void LogAdRewardPrompt(string adUnitId) {
            FirebaseAnalytics.LogEvent(EVENT_AD_REWARD_PROMPT, PARAM_AD_UNIT_ID, adUnitId);
        }

        public static void LogAdRewardImpression(string adUnitId) {
            FirebaseAnalytics.LogEvent(EVENT_AD_REWARD_IMPRESSION, PARAM_AD_UNIT_ID, adUnitId);
        }

        public static void LogLevelSuccess(
            string levelName, int numberOfAttemps, int elapsedTimeInSec, bool hintUsed
        ) {
            FirebaseAnalytics.LogEvent(EVENT_LEVEL_SUCCESS, new Parameter[] {
                new Parameter(FirebaseAnalytics.ParameterLevelName, levelName),
                new Parameter(PARAM_NUMBER_OF_ATTEMPTS, numberOfAttemps),
                new Parameter(PARAM_ELAPSED_TIME_SEC, elapsedTimeInSec),
                new Parameter(PARAM_HINT_USED, hintUsed ? 1 : 0)
            });
        }

        public static void LogLevelFail(
            string levelName, int numberOfAttempts, int elapsedTimeInSec, bool hintUsed
        ) {
            FirebaseAnalytics.LogEvent(EVENT_LEVEL_FAIL, new Parameter[] {
                new Parameter(FirebaseAnalytics.ParameterLevelName, levelName),
                new Parameter(PARAM_NUMBER_OF_ATTEMPTS, numberOfAttempts),
                new Parameter(PARAM_ELAPSED_TIME_SEC, elapsedTimeInSec),
                new Parameter(PARAM_HINT_USED, hintUsed ? 1 : 0)
            });
        }

        public static void LogGameComplete(int numberOfCorrectAnswers) {
            FirebaseAnalytics.LogEvent(EVENT_GAME_COMPLETE, 
                PARAM_NUMBER_OF_CORRECT_ANSWERS, numberOfCorrectAnswers);
        }

        public static void SetScreenName(string screenName) {
            FirebaseAnalytics.SetCurrentScreen(screenName, null);
        }
    }
}
