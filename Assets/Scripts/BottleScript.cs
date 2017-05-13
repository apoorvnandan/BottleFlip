using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;	
public class BottleScript : MonoBehaviour {
	float MAX_ERROR = 0.001f;
	float FORCE_MULTIPLIER = 0.1f;
	float MIN_LENGTH = 5.0f;
	bool gameover = false;
	bool gamestarted = false;

	Rigidbody rb;
	public Text ScoreText;
	public int score = 0;
	float powertimer = 0.0f;
	public bool IsJumping = false;
	public float IsMoving = 0.0f;

	float lastRot, lastPos;

	public GameObject EndPanel;
	public GameObject StartPanel;
	public Text HighScoreText;


	private Vector3 startpos;
	private Vector3 endpos;
	private float length = 0;
	private bool SW = false;
	private Vector3 final;
	float startime;
	float endtime;

	public Text HintText;
	float powerused;
	public GameObject AudioObject;

	//Color[] BottleColors = new Color[10];
	//int NUM_COLORS = 9;
	//int CurrentColorIndex = 0;
	//public GameObject Bottle;

	public void StartGame() {
		StartPanel.SetActive (false);
		ScoreText.text = "0";
		gamestarted = true;
		HintText.text = "Swipe hard to flip the bottle!";
		transform.position = new Vector3 (0, 1, 0);
		transform.rotation = new Quaternion (0, 0, 0, 0);
		//CurrentColorIndex = Random.Range (0, NUM_COLORS);
		//Bottle.GetComponent<Renderer> ().material.color = BottleColors [CurrentColorIndex];
	}

	public void EndGame() {
		gamestarted = false;
		if (PlayerPrefs.GetInt ("score") < score) {
			PlayerPrefs.SetInt ("score", score);
		}
		ScoreText.f
		ScoreText.text = "Current: " + score + "\nHigh Score: " + PlayerPrefs.GetInt ("score");
		gameover = true;
		EndPanel.SetActive (true);
	}

	public void RestartGame() {
		transform.position = new Vector3 (0, 1, 0);
		transform.rotation = new Quaternion (0, 0, 0, 0);
		powerused = 0.0f;
		gameover = false;
		//Time.timeScale = 2;
		StartPanel.SetActive (true);
		EndPanel.SetActive (false);
		HighScoreText.text = PlayerPrefs.GetInt ("score").ToString ();
		rb = GetComponent<Rigidbody>();
		ScoreText.text = "";
		score = 0;
		powertimer = 0.0f;
		IsJumping = false;
		lastPos = transform.position.y;
		lastRot = transform.rotation.z;
		MIN_LENGTH = (float) Screen.height / 6;
		StartGame ();
	}


	public void JumpWithForce(float Power) {
		
		if (Power < 5000) {
			powerused = Power;
			return;
		}
		AudioObject.GetComponent<AudioSource> ().Play ();
		if (Power > 7000 && Power < 8000) {
			Power = 7800;
		} else if (Power >= 8000 && Power < 9000) {
			Power = 8100;
		} else if (Power > 10500 && Power < 12000) {
			Power = 11350;
		}
		else if (Power >= 13000) {
			Power = 12000;
		}
		powerused = Power;
		Vector3 temp = new Vector3 (0, 1, 0) * Power * FORCE_MULTIPLIER;
		Debug.Log (temp);
		float y = transform.position.y;
		transform.position = new Vector3 (0, y + 2F, 0);
		rb.AddForce(temp);
		//rb.velocity = new Vector3(0, Power / 1000, 0);
		IsJumping = true;
		IsMoving = 0.0f;
		rb.AddTorque (0, 0, Power * FORCE_MULTIPLIER);
	}

	bool HasLanded() {
		if (Mathf.Abs (rb.angularVelocity.z) < MAX_ERROR) {
			return true;
		}
		Debug.Log (rb.velocity.y + "   " + rb.position.y + "   " + rb.angularVelocity.z);
		return false;
	}

	bool Standing() {
		if (Mathf.Abs (transform.localRotation.z) < MAX_ERROR) {
			return true;
		}
		return false;
	}

	// Use this for initialization
	void Start () {
		powerused = 0.0f;
		gameover = false;
		Time.timeScale = 2;
		StartPanel.SetActive (true);
		EndPanel.SetActive (false);
		HighScoreText.text = PlayerPrefs.GetInt ("score").ToString ();
		rb = GetComponent<Rigidbody>();
		ScoreText.text = "";
		score = 0;
		powertimer = 0.0f;
		IsJumping = false;
		lastPos = transform.position.y;
		lastRot = transform.rotation.z;
		MIN_LENGTH = (float) Screen.height / 6;


//		BottleColors [0] = new Color ((float) 150/255, (float) 71/255, (float)184/255);
//		BottleColors [1] = new Color ((float) 51/255, (float) 71/255, (float)143/255);
//		BottleColors [2] = new Color ((float) 51/255, (float) 240/255, (float)143/255);
//		BottleColors [3] = new Color ((float) 255/255, (float) 33/255, (float)60/255);
//		BottleColors [4] = new Color ((float) 49/255, (float) 242/255, (float)238/255);
//		BottleColors [5] = new Color ((float) 255/255, (float) 254/255, (float)41/255);
//		BottleColors [6] = new Color ((float) 0/255, (float) 129/255, (float)0/255);
//		BottleColors [7] = new Color ((float) 255/255, (float) 11/255, (float)146/255);
//		BottleColors [7] = new Color ((float) 62/255, (float) 157/255, (float)246/255);
	}
	
	// Update is called once per frame
	void Update () {
		//if (!IsJumping && !gameover) {
		//	float temp = transform.position.y;
		//	transform.position = new Vector3 (0, temp, 0);
		//}
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) 
		{
			final = Vector3.zero;
			length = 0;
			SW = false;
			Vector2 touchDeltaPosition = Input.GetTouch (0).position;
			startpos = new Vector3 (touchDeltaPosition.x, 0, touchDeltaPosition.y);
			startime = 0.0f;
			endtime = 0.0f;
		}      
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) 
		{
			SW = true;
			endtime += Time.deltaTime;
		}

		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Canceled) 
		{
			SW = false;
		}

		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Stationary) 
		{
			SW = false;
		}
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) 
		{
			if (SW) 
			{
				Vector2 touchPosition = Input.GetTouch (0).position;
				endpos = new Vector3 (touchPosition.x, 0, touchPosition.y);
				final = endpos - startpos;
				length = final.magnitude;
				if (length > MIN_LENGTH && !IsJumping && gamestarted) {
					
					float swipe = length / endtime;
					//ScoreText.text = "" + swipe;
					powerused = swipe * 2.2F;
					JumpWithForce (swipe * 2.2F);


				}
			}
		} 
		/* if (Input.GetMouseButton(0)) 
		{
			powertimer += Time.deltaTime;
		}
		if (Input.GetMouseButtonUp (0)) {
			IsJumping = true;
			IsMoving = 0.0f;
			JumpWithForce (powertimer);
			powertimer = 0.0f;
		} */
		if (IsJumping) {
			
			if(Mathf.Abs(transform.position.y - lastPos) < MAX_ERROR && Mathf.Abs(transform.rotation.z - lastRot) < MAX_ERROR) {
				IsMoving = IsMoving + Time.deltaTime;
			}
			else {
				IsMoving = 0.0f;
				lastPos = transform.position.y;
				lastRot = transform.rotation.z;
			} 

			if (IsMoving > 0.75f) {
				IsJumping = false;
				if (Standing ()) {
					if (powerused > 9000) {
						score += 2;
					} else {
						score++;
					}
					ScoreText.text = "" + score;
					HintText.text = "Awesome! Now do it again!";
					float temp = transform.position.y;
					transform.position = new Vector3 (0, temp, 0);
					transform.rotation = new Quaternion (0, 0, 0, 0);
						
				} else {
					//ScoreText.text = "GAME OVER";
					if (powerused < 7000) {
						HintText.text = "Too slow! Swipe harder!";
					} else if (powerused >= 8500) {
						HintText.text = "Too much power!";
					}
					EndGame();
				}
			} else {
				//ScoreText.text = "In Air";
			}
		}
	}
}
