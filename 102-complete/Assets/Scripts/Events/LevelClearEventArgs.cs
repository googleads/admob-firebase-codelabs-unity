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

using AwesomeDrawingQuiz.Data;

namespace AwesomeDrawingQuiz.Events {
    
    public class LevelClearEventArgs : EventArgs {

        public int NumAttempts { get; private set; }

        public int ElapsedTimeInSeconds { get; private set; }

        public bool IsFinalLevel { get; private set; }

        public bool IsHintUsed { get; private set; }

        public Drawing Drawing { get; private set; }

        public static LevelClearEventArgs Create(
            int attempts, int elapsedTime, bool isFinalLevel, bool isHintUsed, Drawing drawing) {
            LevelClearEventArgs args = new LevelClearEventArgs();
            args.NumAttempts = attempts;
            args.ElapsedTimeInSeconds = elapsedTime;
            args.IsFinalLevel = isFinalLevel;
            args.IsHintUsed = isHintUsed;
            args.Drawing = drawing;
            return args;
        }

        private LevelClearEventArgs() {

        }
    }
}
