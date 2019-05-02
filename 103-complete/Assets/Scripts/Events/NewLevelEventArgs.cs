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
    
    public class NewLevelEventArgs : EventArgs {

        public int CurrentLevel { get; private set; }

        public string Clue { get; private set; }

        public Drawing Drawing { get; private set; }


        public static NewLevelEventArgs Create(int currentLevel, string clue, Drawing drawing) {
            NewLevelEventArgs args = new NewLevelEventArgs();
            args.CurrentLevel = currentLevel;
            args.Clue = clue;
            args.Drawing = drawing;
            return args;
        }

        private NewLevelEventArgs() {

        }
    }
}
