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
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using AwesomeDrawingQuiz.Ads;
using AwesomeDrawingQuiz.Analytics;
using AwesomeDrawingQuiz.Data;
using AwesomeDrawingQuiz.Events;
using AwesomeDrawingQuiz.Game;

namespace AwesomeDrawingQuiz.Scene {

    public class Game : MonoBehaviour {

        private const int DIALOG_AD_PROMPT = 0;

        private const int DIALOG_CORRECT_ANSWER = 1;

        private const int DIALOG_WRONG_ANSWER = 2;

        private const int DIALOG_GAME_END = 3;

        public Canvas dialogAdPrompt;

        public Canvas dialogCorrectAnswer;

        public Canvas dialogWrongAnswer;

        public Canvas dialogGameEnd;

        public Text textScore;

        public Text textLevel;

        public Text textClue;

        public InputField inputAnswer;

        public Button buttonHint;

        public Material lineMaterial;

        public float lineWidth;

        private bool IsHintAvailable {
            get {
                return QuizManager.Instance.IsHintAvailable && AdManager.Instance.IsAdAvailable;
            }
        }

        private List<LineRenderer> lines;

        private bool uiUpdatedRequested = false;

        private int targetDialogId = -1;

        private bool showDialog = false;

        void Start () {
            QuizAnalytics.SetScreenName(QuizAnalytics.SCREEN_GAME);

            lines = new List<LineRenderer>();

            // Setup AdManager callbacks
            AdManager.Instance.OnAdReady += OnAdReady;
            AdManager.Instance.OnAdNotAvailable += OnAdNotAvailable;
            AdManager.Instance.OnAdStarted += OnAdStarted;
            AdManager.Instance.OnRewarded += OnRewarded;

            AdManager.Instance.LoadAd();

            // Setup QuizManager callbacks
            QuizManager.Instance.OnClueUpdated += OnClueUpdated;
            QuizManager.Instance.OnGameOver += OnGameOver;
            QuizManager.Instance.OnLevelCleared += OnLevelCleared;
            QuizManager.Instance.OnLevelSkipped += OnLevelSkipped;
            QuizManager.Instance.OnNewLevel += OnNewLevel;
            QuizManager.Instance.OnWrongAnwser += OnWrongAnwser;

            // Hide dialogs at startup
            dialogCorrectAnswer.gameObject.SetActive(false);
            dialogWrongAnswer.gameObject.SetActive(false);
            dialogGameEnd.gameObject.SetActive(false);

            SetHintButtonEnabled(false);

            QuizAnalytics.LogGameStart();
            QuizManager.Instance.StartLevel();
        
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        
        // Update is called once per frame
        void Update () {
            if (uiUpdatedRequested) {
                SetDrawingVisibility(!showDialog);

                switch (targetDialogId) {
                    case DIALOG_AD_PROMPT:
                        dialogAdPrompt.gameObject.SetActive(showDialog);
                        break;
                    case DIALOG_CORRECT_ANSWER:
                        dialogCorrectAnswer.gameObject.SetActive(showDialog);
                        break;
                    case DIALOG_WRONG_ANSWER:
                        dialogWrongAnswer.gameObject.SetActive(showDialog);
                        break;
                    case DIALOG_GAME_END:
                        dialogGameEnd.gameObject.SetActive(showDialog);
                        break;
                }
                targetDialogId = -1;
                uiUpdatedRequested = false;
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                SceneManager.LoadScene("Main");
            }
        }

        void OnDestroy() {
            // Remove AdManager event handlers
            AdManager.Instance.OnAdReady -= OnAdReady;
            AdManager.Instance.OnAdNotAvailable -= OnAdNotAvailable;
            AdManager.Instance.OnAdStarted -= OnAdStarted;
            AdManager.Instance.OnRewarded -= OnRewarded;

            // Remove QuizManager event handlers
            QuizManager.Instance.OnClueUpdated -= OnClueUpdated;
            QuizManager.Instance.OnGameOver -= OnGameOver;
            QuizManager.Instance.OnLevelCleared -= OnLevelCleared;
            QuizManager.Instance.OnLevelSkipped -= OnLevelSkipped;
            QuizManager.Instance.OnNewLevel -= OnNewLevel;
            QuizManager.Instance.OnWrongAnwser -= OnWrongAnwser;
        }

        #region UI callbacks

        public void OnClickHint() {
            QuizAnalytics.LogAdRewardPrompt(AdManager.AD_UNIT_ID);
            ShowDialog(DIALOG_AD_PROMPT);
        }

        public void OnAcceptHint() {
            AdManager.Instance.ShowAd();
            DismissDialog(DIALOG_AD_PROMPT);
            SetHintButtonEnabled(false);
        }

        public void OnRejectHint() {
            DismissDialog(DIALOG_AD_PROMPT);
        }

        public void OnClickSkip() {
            QuizManager.Instance.SkipLevel();
        }

        public void OnSubmitAnswer() {
            if (inputAnswer.text.Equals("")) {
                return;
            }
            QuizManager.Instance.CheckAnswer(inputAnswer.text);
            inputAnswer.text = "";
        }

        public void OnClickNextLevel() {
            QuizManager.Instance.MoveToNextLevel();

            DismissDialog(DIALOG_CORRECT_ANSWER);
        }

        public void OnClickTryAgain() {
            DismissDialog(DIALOG_WRONG_ANSWER);
        }

        public void OnClickFinishGame() {
            SceneManager.LoadScene("Main");
        }

        #endregion

        #region AdManager callbacks

        public void OnAdReady(object sender, EventArgs args) {
            if (IsHintAvailable) {
                UnityMainThreadDispatcher.Instance().Enqueue(() => SetHintButtonEnabled(true));
            }
        }

        public void OnAdNotAvailable(object sender, EventArgs args) {
            
        }

        public void OnAdStarted(object sender, EventArgs args) {
            QuizAnalytics.LogAdRewardImpression(AdManager.AD_UNIT_ID);
        }

        public void OnRewarded(object sender, EventArgs args) {
            QuizManager.Instance.UseHint();
        }

        #endregion

        #region QuizManager callbacks

        public void OnClueUpdated(object sender, ClueUpdateEventArgs args) {
            Debug.Log("Clue updated: " + args.NewClue);
            UnityMainThreadDispatcher.Instance().Enqueue((() => RenderClue(args.NewClue)));
        }

        public void OnGameOver(object sender, GameOverEventArgs args) {
            QuizAnalytics.LogGameComplete(args.NumCorrectAnswers);

            textScore.text = String.Format("Score: {0}", args.NumCorrectAnswers);
            ShowDialog(DIALOG_GAME_END);
        }

        public void OnLevelCleared(object sender, LevelClearEventArgs args) {
            QuizAnalytics.LogLevelSuccess(
                args.Drawing.word, args.NumAttempts, args.ElapsedTimeInSeconds, args.IsHintUsed);

            if (args.IsFinalLevel) {
                QuizManager.Instance.MoveToNextLevel();
            } else {
                ShowDialog(DIALOG_CORRECT_ANSWER);
            }
        }

        public void OnLevelSkipped(object sender, LevelSkipEventArgs args) {
            QuizAnalytics.LogLevelFail(
                args.Drawing.word, args.NumAttempts, args.ElapsedTimeInSeconds, args.IsHintUsed);
        }

        public void OnNewLevel(object sender, NewLevelEventArgs args) {
            QuizAnalytics.LogLevelStart(args.Drawing.word);

            SetHintButtonEnabled(AdManager.Instance.IsAdAvailable);
            // If Rewarded Video Ad is not available at this moment, try it again
            if (!AdManager.Instance.IsAdAvailable) {
                AdManager.Instance.LoadAd();
            }

            RenderLevel(args.CurrentLevel);
            RenderClue(args.Clue);
            ClearCurrentDrawing();
            RenderDrawing(args.Drawing);
        }

        public void OnWrongAnwser(object sender, WrongAnswerEventArgs args) {
            QuizAnalytics.LogLevelWrongAnswer(args.Drawing.word);

            ShowDialog(DIALOG_WRONG_ANSWER);
        }

        #endregion

        private void SetHintButtonEnabled(bool enabled) {
            buttonHint.interactable = enabled;
        }

        private void RenderClue(string clue) {
            textClue.text = clue;
            textClue.SetAllDirty();
        }

        private void RenderDrawing(Drawing d) {
            // Remove previous lines
            ClearCurrentDrawing();

            List<Vector3[]> strokes = d.drawing;

            foreach (Vector3[] points in strokes) {
                LineRenderer lineRenderer = new GameObject().AddComponent<LineRenderer>();
                lineRenderer.numCornerVertices = 5;
                lineRenderer.material = lineMaterial;
                lineRenderer.positionCount = points.Length;
                lineRenderer.SetPositions(points);
                lineRenderer.startWidth = lineWidth;
                lineRenderer.endWidth = lineWidth;
                lines.Add(lineRenderer);
            }
        }

        private void SetDrawingVisibility(bool show) {
            foreach (LineRenderer line in lines) {
                line.gameObject.SetActive(show);
            }
        }

        private void ClearCurrentDrawing() {
            foreach (LineRenderer line in lines) {
                if (line.gameObject != null) {
                    Destroy(line.gameObject);
                }
            }
            lines.Clear();
        }

        private void RenderLevel(int level) {
            textLevel.text = String.Format("Level {0}/{1}", level, GameSettings.MAX_GAME_LEVEL);
        }

        private void ShowDialog(int dialogId) {
            targetDialogId = dialogId;
            showDialog = true;
            uiUpdatedRequested = true;
        }

        private void DismissDialog(int dialogId) {
            targetDialogId = dialogId;
            showDialog = false;
            uiUpdatedRequested = true;
        }
    }
}
