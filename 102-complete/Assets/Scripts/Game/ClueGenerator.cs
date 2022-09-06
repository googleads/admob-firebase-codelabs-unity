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

using System.Text;

using AwesomeDrawingQuiz.Events;

namespace AwesomeDrawingQuiz.Game {

    public class ClueGenerator {

        public static string Generate(string answer, int charCntToBeDisclosed) {
            StringBuilder b = new StringBuilder();
            int answerLength = answer.Length;
            int disclosedCharCnt = 0;
            for (int i=0; i < answerLength; i++) {
                char c = answer[i];
                if (' ' == c) {
                    b.Append(c);
                } else if (disclosedCharCnt < charCntToBeDisclosed
                        && disclosedCharCnt < answerLength -1) {
                    b.Append(c);
                    disclosedCharCnt++;
                } else {
                    b.Append('*');
                }
            }
            return b.ToString();
        }
    }

}