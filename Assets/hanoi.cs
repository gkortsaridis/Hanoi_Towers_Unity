using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class hanoi : MonoBehaviour {

	private GameObject[] cells;
	public GameObject leftTop,middleTop,rightTop;

	private bool inCoroutine = false;
	private List<int> cylinders;
	private List<int> ways;
	private int[] spots;
	private float waiting;

	public Text algorithmText;
	public InputField TowersNum;
	public Button playGame;
	public Button cancelGame;

	private bool isPlaying;
	private bool finished;
	private GUIStyle myStyle;
	private int steps;

	public Texture2D exitLogo;


	// Use this for initialization
	void Start () {

		steps = 0;

		myStyle = new GUIStyle ();
		myStyle.fontSize = Screen.height / 40;

		playGame.onClick.AddListener(StartGame);
		cancelGame.onClick.AddListener(CancelGame);
		waiting = 1f;
		isPlaying = false;
	}

	void CancelGame(){
		Application.LoadLevel (0);
	}

	void StartGame(){
		if (!isPlaying) {
			float height = 2.0f;

			int numOfTowers;
			if (TowersNum.text == "2")
				numOfTowers = 2;
			else if (TowersNum.text == "3")
				numOfTowers = 3;
			else if (TowersNum.text == "4")
				numOfTowers = 4;
			else if (TowersNum.text == "5")
				numOfTowers = 5;
			else if (TowersNum.text == "6")
				numOfTowers = 6;
			else if (TowersNum.text == "7")
				numOfTowers = 7;
			else
				numOfTowers = 3;

			algorithmText.text = "";

			spots = new int[numOfTowers];
			cells = new GameObject[numOfTowers];

			GameObject[] oldCylinders = GameObject.FindGameObjectsWithTag ("cylinder");
			for (int i=0; i<oldCylinders.Length; i++)
				Destroy (oldCylinders [i]);
			StopCoroutine (customHanoi ());
		
			for (int i=0; i<numOfTowers; i++) {
				spots [i] = -1;
				GameObject instance = Instantiate (Resources.Load ("Cylinder" + (i + 1), typeof(GameObject))) as GameObject;
				instance.transform.position = new Vector3 (-3f, height, 0.001f);
				instance.tag = "cylinder";

				cells [i] = instance;
				height -= 0.3f;
			}
		
			cylinders = new List<int> ();
			ways = new List<int> ();
		
			hanoiTower (numOfTowers, 1, 0);
		
			StartCoroutine (customHanoi ());
		}
	}

	void OnGUI(){

		GUI.DrawTexture (new Rect(Screen.width/40 , Screen.height-Screen.height/6, Screen.width/15 , Screen.width/15),exitLogo);
		if (GUI.Button (new Rect (Screen.width / 40, Screen.height - Screen.height / 6, Screen.width / 15, Screen.width / 15), "",myStyle)) Application.Quit ();

		GUI.Label (new Rect(Screen.width/8,Screen.height-Screen.height/8,Screen.width,50),"Left Arrow : Slow Down | Right Arrow : Speed Up | waiting = "+waiting+" seconds",myStyle);

		if(finished) GUI.Label (new Rect(Screen.width/2 - Screen.width/10,Screen.height/2,Screen.width,50),"Finished in "+steps+" steps.",myStyle);

	}

	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyUp(KeyCode.LeftArrow))
		{
			if(waiting < 9.9f)waiting+=0.1f;
		}
		else if(Input.GetKeyUp(KeyCode.RightArrow))
		{
			if(waiting > 0.5f)waiting-=0.1f;
		}
	}

	void hanoiTower(int N, int d , int cnt) {
		if (N==0) return;

		for(int i=0; i<=cnt; i++){ algorithmText.text += "   "; }
		algorithmText.text += "hanoi("+N+","+(-d)+");\n";

		hanoiTower(N-1,-d,cnt+1);

		for(int i=0; i<=cnt; i++){ algorithmText.text += "   "; }
		algorithmText.text += "shiftCell ("+N+","+d+");\n";

		cylinders.Add (N);
		ways.Add (d);

		for(int i=0; i<=cnt; i++){ algorithmText.text += "   "; }
		algorithmText.text += "hanoi("+N+","+(-d)+");\n";

		hanoiTower(N-1,-d,cnt+1);
	} 

	IEnumerator customHanoi(){
		isPlaying = true;
		finished = false;
		for(int i=0; i<cylinders.Count; i++)
		{
			shiftCell (cylinders[i], ways[i]);
			steps++;
			yield return new WaitForSeconds (waiting);
		}

		finished = true;
		yield return new WaitForSeconds (5);

		steps = 0;
		isPlaying = false;
		Application.LoadLevel (0);
	}

	void shiftCell(int N , int d){

		Transform targetPos = null;
		if (d == 1) {
			if (spots[N-1] == -1){
				targetPos = middleTop.transform;
				//Debug.Log(N+" from A to B");
			}
			else if(spots[N-1] == 0){
				targetPos = rightTop.transform;
				//Debug.Log(N+" from C to A");
			}
			else {
				targetPos = leftTop.transform;
				//Debug.Log(N+" from B to C");
			}

			if(spots[N-1] == 1)spots[N-1] = -1;
			else spots[N-1]++;
		}
		else
		{
			if (spots[N-1] == -1){
				targetPos = rightTop.transform;
				//Debug.Log(N+" from A to C");
			}
			else if(spots[N-1] == 0){
				targetPos = leftTop.transform;
				//Debug.Log(N+" from C to B");
			}
			else {
				targetPos = middleTop.transform;
				//Debug.Log(N+" from B to A");
			}

			if(spots[N-1] == -1)spots[N-1] = 1;
			else spots[N-1]--;
		}

		cells [N - 1].transform.position = targetPos.transform.position;


	}
	
}

