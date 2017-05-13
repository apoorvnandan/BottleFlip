using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
 
public class ShareScreenshot : MonoBehaviour {
	  
	 private bool isProcessing = false;
	 public float startX;
	 public float startY;
	 public int valueX;
	 public int valueY;
	 
	public void RateOnPlayStore() {
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.bottleflip.bottleflip3d");
	}
	 
	 public void shareScreenshot(){
		 
		if (!isProcessing)
			StartCoroutine(takeScreenshotAndSave());
	 }

	private IEnumerator takeScreenshotAndSave()
	{
		isProcessing = true;
		string ShareSubject = "BottleFlip Screenshot";
		string shareLink = "";
		string textToShare = "Check out my high score!";
		string destination = "" + Application.persistentDataPath;
		//Debug.Log(destination);
		Application.CaptureScreenshot("screenshot.png");

		if (!Application.isEditor)
		{

			AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
			AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
			intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
			AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", destination + "/screenshot.png");

			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), textToShare + shareLink);
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), ShareSubject);
			intentObject.Call<AndroidJavaObject>("setType", "image/png");
			AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
			currentActivity.Call("startActivity", intentObject);
		}
		yield return null;
		isProcessing = false;
	}
}