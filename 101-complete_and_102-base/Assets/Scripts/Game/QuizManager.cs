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
using UnityEngine;

using AwesomeDrawingQuiz.Data;
using AwesomeDrawingQuiz.Events;

namespace AwesomeDrawingQuiz.Game {

    public class QuizManager {

        private static readonly QuizManager instance = new QuizManager();

        public static QuizManager Instance {
            get {
                return instance;
            }
        }

        // Level-scoped information

        public bool IsHintAvailable {
            get {
                return !isHintUsed;
            }
        }

        private int currentLevel = 1;

        private int numAttempts = 0;

        private int disclosedLettersByDefault;

        private int disclosedLetters;

        private DateTime levelStartTime;

        private bool isHintUsed = false;

        private Drawing drawing;

        // Game-scoped information

        private int numCorrectAnswers;

        public void StartLevel() {
            numCorrectAnswers = 0;
            StartLevel(1);
        }

        public void CheckAnswer(string answer) {
            numAttempts++;

            bool correct = drawing.word.Equals(
                answer, StringComparison.InvariantCultureIgnoreCase);
            
            if (correct) {
                numCorrectAnswers++;
    
                if (this.OnLevelCleared != null) {
                    LevelClearEventArgs args = LevelClearEventArgs.Create(
                        this.numAttempts, GetElapsedTimeInSeconds(),
                        this.currentLevel == GameSettings.MAX_GAME_LEVEL, 
                        this.isHintUsed, this.drawing);

                    this.OnLevelCleared(this, args);
                }
            } else {
                if (this.OnWrongAnwser != null) {
                    WrongAnswerEventArgs args = WrongAnswerEventArgs.Create(this.drawing);

                    this.OnWrongAnwser(this, args);
                }
            }
        }

        public void SkipLevel() {
            if (this.OnLevelSkipped != null) {
                LevelSkipEventArgs args = LevelSkipEventArgs.Create(
                    this.numAttempts, GetElapsedTimeInSeconds(), this.isHintUsed, this.drawing);
                
                this.OnLevelSkipped(this, args);
            }
            MoveToNextLevel();
        }

        public void MoveToNextLevel() {
            if (currentLevel < GameSettings.MAX_GAME_LEVEL) {
                StartLevel(currentLevel + 1);
            } else {
                FinishGame();
            }
        }

        public void UseHint() {
            if (isHintUsed) {
                Debug.Log("Hint already used");
                return;
            }

            isHintUsed = true;
            disclosedLetters += GameSettings.Instance.RewardAmount;

            if (this.OnClueUpdated != null) {
                ClueUpdateEventArgs args = ClueUpdateEventArgs.Create(GenerateClue(), drawing);

                this.OnClueUpdated(this, args);
            }
        }

        private void ApplyDifficulty() {
            switch(GameSettings.Instance.Difficulty) {
                case GameSettings.DIFFICULTY_EASY:
                    disclosedLettersByDefault = 2;
                    break;
                case GameSettings.DIFFICULTY_NORMAL:
                    disclosedLettersByDefault = 1;
                    break;
                default:
                    disclosedLettersByDefault = 1;
                    break;
            }
            disclosedLetters = disclosedLettersByDefault;
        }

        public void StartLevel(int newLevel) {
            numAttempts = 0;
            isHintUsed = false;
            currentLevel = newLevel;
            levelStartTime = System.DateTime.Now;
            
            ApplyDifficulty();
            RequestNewDrawing();
        }

        private void RequestNewDrawing() {
            drawing = Drawing.Parse(Drawings.SAMPLES[currentLevel - 1]);
            
            if (this.OnNewLevel != null) {
                NewLevelEventArgs args = NewLevelEventArgs.Create(
                    this.currentLevel, GenerateClue(), this.drawing);

                this.OnNewLevel(this, args);
            }
        }

        private void FinishGame() {
            if (this.OnGameOver != null) {
                GameOverEventArgs args = GameOverEventArgs.Create(this.numCorrectAnswers);

                this.OnGameOver(this, args);
            }
        }

        private int GetElapsedTimeInSeconds() {
            return (int) DateTime.Now.Subtract(levelStartTime).TotalSeconds;
        }

        private string GenerateClue() {
            return ClueGenerator.Generate(drawing.word, this.disclosedLetters);
        }

        #region QuizManager event callbacks

        public event EventHandler<ClueUpdateEventArgs> OnClueUpdated = delegate { };

        public event EventHandler<GameOverEventArgs> OnGameOver = delegate { };

        public event EventHandler<LevelClearEventArgs> OnLevelCleared = delegate { };

        public event EventHandler<LevelSkipEventArgs> OnLevelSkipped = delegate { };
        
        public event EventHandler<NewLevelEventArgs> OnNewLevel = delegate { };

        public event EventHandler<WrongAnswerEventArgs> OnWrongAnwser = delegate { };

        #endregion
    }
}
