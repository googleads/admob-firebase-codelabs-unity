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
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using SimpleJSON;

namespace AwesomeDrawingQuiz.Data {

    public class Drawing {
        
        public string word;

        public List<Vector3[]> drawing;

        public static Drawing Parse(string jsonString) {
            JSONNode json = JSON.Parse(jsonString);
            Drawing d = new Drawing();

            // Parse drawing name
            d.word = json["word"];

            // Prase strokes
            d.drawing = new List<Vector3[]>();
            
            JSONArray strokes = json["drawing"].AsArray;
            Vector3 toScale = new Vector3(0.015f, -0.015f, 1f);

            // [[1,2,3],[4,5,6]], ...
            foreach (JSONNode stroke in strokes) {
                JSONArray coords = stroke.AsArray;
                // [1,2,3]
                JSONArray xCoords = coords[0].AsArray;
                // [4,5,6]
                JSONArray yCoords = coords[1].AsArray;

                int numPoints = xCoords.Count;
                Vector3[] points = new Vector3[numPoints];
                for (int i=0; i<numPoints; i++) {
                    Vector3 v = new Vector3(xCoords[i].AsFloat-128, yCoords[i].AsFloat-148, 0f);
                    v.Scale(toScale);
                    points[i] = v;
                }
                d.drawing.Add(points);
            }
            
            return d;
        }
    }
}
