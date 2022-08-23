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

    public class ClueUpdateEventArgs : EventArgs {

        public string NewClue { get; private set; }

        public Drawing Drawing { get; private set; }

        public static ClueUpdateEventArgs Create(string newClue, Drawing drawing) {
            ClueUpdateEventArgs args = new ClueUpdateEventArgs();
            args.NewClue = newClue;
            args.Drawing = drawing;
            return args;
        }

        private ClueUpdateEventArgs() {

        }
    }
}
