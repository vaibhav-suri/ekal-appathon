// Copyright 2017 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using UnityEngine.UI;

using PolyToolkit;

/// <summary>
/// Simple example that loads and displays one asset.
/// 
/// This example requests a specific asset and displays it.
/// </summary>
public class HelloPoly : MonoBehaviour {

    // ATTENTION: Before running this example, you must set your API key in Poly Toolkit settings.
    //   1. Click "Poly | Poly Toolkit Settings..."
    //      (or select PolyToolkit/Resources/PtSettings.asset in the editor).
    //   2. Click the "Runtime" tab.
    //   3. Enter your API key in the "Api key" box.
    //
    public string finalstring = "brother";
    public bool isDebug = false;
    // Here's where we store the translated text!
    public string translatedText = "";
    // This example does not use authentication, so there is no need to fill in a Client ID or Client Secret.

    // Text where we display the current status.
    public Text statusText;
    public string currentmodel="cow";
    public InputField textfield;
    public IEnumerator Process(string targetLang, string sourceText)
    {
        // We use Auto by default to determine if google can figure it out.. sometimes it can't.
        string sourceLang = "auto";
        // Construct the url using our variables and googles api.
        string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl="
            + sourceLang + "&tl=" + targetLang + "&dt=t&q=" + UnityWebRequest.EscapeURL(sourceText);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Check to see if we don't have any errors.
            if (string.IsNullOrEmpty(webRequest.error))
            {
                var N = JSONNode.Parse(webRequest.downloadHandler.text);
                // Dig through and take apart the text to get to the good stuff.
                translatedText = N[0][0][0];
                // This is purely for debugging in the Editor to see if it's the word you wanted.
                finalstring = translatedText;
                PolyListAssetsRequest req = new PolyListAssetsRequest();
                // Search by keyword:
                req.keywords = finalstring;
                //   print(translatedText);
                //  StartCoroutine(translations.Process)
                //print(v1.translatedText);
                // Only curated assets:
                req.curated = true;

                // Limit complexity to medium.
                req.maxComplexity = PolyMaxComplexityFilter.SIMPLE;
                // Only Blocks objects.
                //  req.formatFilter = PolyFormatFilter.OBJ;
                // Order from best to worst.
                req.orderBy = PolyOrderBy.BEST;
                // Up to 20 results per page.
                //   req.pageSize = 20;
                // Send the request.
                PolyApi.ListAssets(req, MyCallback);

                //   yield return finalstring;

                //if (isDebug)
                print(finalstring);
            }
        }
    }

    // Exactly the same as above but allow the user to change from Auto, for when google get's all Jerk Butt-y
    public IEnumerator Process(string sourceLang, string targetLang, string sourceText)
    {
        string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl="
            + sourceLang + "&tl=" + targetLang + "&dt=t&q=" + UnityWebRequest.EscapeURL(sourceText);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Check to see if we don't have any errors.
            if (string.IsNullOrEmpty(webRequest.error))
            {
                // Parse the response using JSON.
                var N = JSONNode.Parse(webRequest.downloadHandler.text);
                // Dig through and take apart the text to get to the good stuff.
                translatedText = N[0][0][0];
                // This is purely for debugging in the Editor to see if it's the word you wanted.
                finalstring = translatedText;
                PolyListAssetsRequest req = new PolyListAssetsRequest();
                // Search by keyword:
                req.keywords = finalstring;
                //   print(translatedText);
                //  StartCoroutine(translations.Process)
                //print(v1.translatedText);
                // Only curated assets:
                req.curated = true;

                // Limit complexity to medium.
                req.maxComplexity = PolyMaxComplexityFilter.SIMPLE;
                // Only Blocks objects.
                //  req.formatFilter = PolyFormatFilter.OBJ;
                // Order from best to worst.
                req.orderBy = PolyOrderBy.BEST;
                // Up to 20 results per page.
                //   req.pageSize = 20;
                // Send the request.
                PolyApi.ListAssets(req, MyCallback);

                //   yield return finalstring;

                //if (isDebug)
                print(finalstring);
            }
        }
    }
    private void Start() {
    // Request the asset.
    //Debug.Log("Requesting asset...");
   // PolyApi.GetAsset("assets/5vbJ5vildOq", GetAssetCallback);
   // statusText.text = "Requesting...";
        //modelCall();
        textfield.text = "rocket";
  }
    public void ButtonPressSpawn()
    {
        Destroy(GameObject.Find("PolyImport"));

        modelCall();
    }
    void modelCall()
    {
       StartCoroutine(Process("en", textfield.text));
   
    }
    //  PolyApi.ListMyAssets(req, GetAssetCallback);
    void MyCallback(PolyStatusOr<PolyListAssetsResult> result)
    {
        if (!result.Ok)
        {
            // Handle error.
            return;
        }
        // Success. result.Value is a PolyListAssetsResult and
        // result.Value.assets is a list of PolyAssets.
        for(int i=0;i<result.Value.assets.Count;i++)
        {
            int randomnumber = Random.Range(i, result.Value.assets.Count);
            PolyApi.GetAsset(result.Value.assets[randomnumber].name, GetAssetCallback);
 
            break;
        }
        //foreach (PolyAsset asset in result.Value.assets)
        //{
        //    PolyApi.GetAsset(asset.name, GetAssetCallback);
        //   break;
        //    // print();
        //    // Do something with the asset here.
        //}
    }
    // Callback invoked when the featured assets results are returned.
    private void GetAssetCallback(PolyStatusOr<PolyAsset> result) {
    if (!result.Ok) {
      Debug.LogError("Failed to get assets. Reason: " + result.Status);
      statusText.text = "ERROR: " + result.Status;
      return;
    }
    Debug.Log("Successfully got asset!");

    // Set the import options.
    PolyImportOptions options = PolyImportOptions.Default();
    // We want to rescale the imported mesh to a specific size.
    options.rescalingMode = PolyImportOptions.RescalingMode.FIT;
    // The specific size we want assets rescaled to (fit in a 5x5x5 box):
    options.desiredSize = 5.0f;
    // We want the imported assets to be recentered such that their centroid coincides with the origin:
    options.recenter = true;

    statusText.text = "Importing...";
    PolyApi.Import(result.Value, options, ImportAssetCallback);
  }

  // Callback invoked when an asset has just been imported.
  private void ImportAssetCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result) {
    if (!result.Ok) {
      Debug.LogError("Failed to import asset. :( Reason: " + result.Status);
      statusText.text = "ERROR: Import failed: " + result.Status;
      return;
    }
    Debug.Log("Successfully imported asset!");

    // Show attribution (asset title and author).
    statusText.text = asset.displayName + "\nby " + asset.authorName;

        // Here, you would place your object where you want it in your scene, and add any
        // behaviors to it as needed by your app. As an example, let's just make it
        // slowly rotate:

        //result.Value.gameObject.AddComponent<Rotate>();
        //  var thiss = GameObject.Find("PolyImport");
        result.Value.gameObject.AddComponent<Lean.Touch.LeanTouch>();
        result.Value.gameObject.AddComponent<Lean.Touch.LeanDragTranslate>();
        result.Value.gameObject.AddComponent<Lean.Touch.LeanPinchScale>();
        result.Value.gameObject.AddComponent<Lean.Touch.LeanTwistRotate>();
        result.Value.gameObject.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + 5);
        result.Value.gameObject.transform.position = GameObject.Find("Player").gameObject.transform.localScale;


    }
}