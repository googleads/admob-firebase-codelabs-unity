// Copyright 2018 Google LLC
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

using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

using AwesomeDrawingQuiz.Ads;

public static class IosPlistProcessor 
{

    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path) 
    {
        if (buildTarget == BuildTarget.iOS) 
        {
            string appId = AdManager.APP_ID_IOS;

            if (appId.Length == 0) {
                Debug.Assert(appId.Length != 0,
                    "iOS AdMob app id is empty. Please enter a valid app id to run ads properly.");
            }

            string plistPath = Path.Combine(path, "Info.plist");
            PlistDocument plist = new PlistDocument();

            plist.ReadFromFile(plistPath);
            plist.root.SetString("GADApplicationIdentifier", appId);
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}