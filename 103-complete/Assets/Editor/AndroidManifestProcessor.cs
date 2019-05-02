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

using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;

using AwesomeDrawingQuiz.Ads;

public class AndroidManifestProcessor : IPreprocessBuild
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.Android) 
        {
            string templatePath = Path.Combine(
                        Application.dataPath, "Editor/template-AndroidManifest.txt");
            string outPath = Path.Combine(
                Application.dataPath, "Plugins/Android/GoogleMobileAdsManifest.plugin/AndroidManifest.xml");

            XDocument manifest = XDocument.Load(templatePath);
            XNamespace ns = "http://schemas.android.com/apk/res/android";

            XElement elemManifest = manifest.Element("manifest");
            if (elemManifest == null)
            {
                throw new InvalidOperationException(
                    "[GoogleMobileAds] template-AndroidManifest.txt looks not valid. Try re-importing the plugin.");
            }

            XElement elemApplication = elemManifest.Element("application");
            if (elemApplication == null)
            {
                throw new InvalidOperationException(
                    "[GoogleMobileAds] template-AndroidManifest.txt looks not valid. Try re-importing the plugin.");
            }

            string appId = AdManager.APP_ID_ANDROID;
            if (appId.Length == 0) {
                Debug.Assert(appId.Length != 0,
                    "Android AdMob app id is empty. Please enter a valid app id to run ads properly.");
            }

            XElement meta = new XElement("meta-data",
                new XAttribute(ns + "name", "com.google.android.gms.ads.APPLICATION_ID"),
                new XAttribute(ns + "value", appId));

            elemApplication.Add(meta);

            manifest.Save(outPath);
        }
    }
}