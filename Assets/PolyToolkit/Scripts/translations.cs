using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;

public class translations : MonoBehaviour
{
	// Should we debug?
	public string finalstring="brother";
	public bool isDebug = false;
	// Here's where we store the translated text!
	public string translatedText = "";

	// This is only called when the scene loads.
	void Start()
	{
		// Strictly for debugging to test a few words!
		if (isDebug)
			StartCoroutine(Process("en", "गाड़ी"));
	}

	// We have use googles own api built into google Translator.
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
				// Parse the response using JSON.
				var N = JSONNode.Parse(webRequest.downloadHandler.text);
				// Dig through and take apart the text to get to the good stuff.
				translatedText = N[0][0][0];
				finalstring = translatedText;

				// This is purely for debugging in the Editor to see if it's the word you wanted.
				//	if (isDebug)
				yield return finalstring;
				//print(translatedText);
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
				yield return finalstring;

				//if (isDebug)
				//	print(translatedText);
			}
		}
	}
}