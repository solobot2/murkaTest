using UnityEngine;
using System.Collections;
using System.Security.AccessControl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	public static GameController Instance{ get; private set; }

	public float initialTime;
	public float timeToSubstractByRound;
	public int currentRound;
	public int currentRoundFigureNum;


	private float timeLeft;

	public GameObject UICanvas;

	private GameObject playerCursor;

	private GameObject panelStart;

	private GameObject panelGameover;

	private GameObject panelGameplay;

	private IEnumerator CDRoutine;

	public Cursor cursor;

	[HideInInspector]
	public bool isRoundStarted = false;
	

	// Use this for initialization
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{		
		panelStart = UICanvas.transform.Find("PanelStart").gameObject;
		panelGameplay = UICanvas.transform.Find("PanelGameplay").gameObject;
		panelGameover = UICanvas.transform.Find("PanelGameOver").gameObject;
		panelStart.SetActive(true);
		panelGameplay.SetActive(false);
		panelGameover.SetActive(false);
	}
	
	// Update is called once per frame
	void Update()
	{
		if (isRoundStarted)
		{
			if (Input.GetMouseButtonDown(0))
			{
				cursor.ClearLog();
				cursor.StartWriteLog();
			}
				
			if (Input.GetMouseButtonUp(0))
			{
				cursor.StopWriteLog();
				EvaluateFigure(cursor.GetLog());
			}
		}
	}

	public void StartGame()
	{
		panelStart.SetActive(false);
		panelGameplay.SetActive(true);

		StartNextRound();
	}

	void StartNextRound()
	{
		
		isRoundStarted = true;

		currentRound++;
		panelGameplay.transform.Find("RoundValue").GetComponent<Text>().text = currentRound.ToString();

		CDRoutine = StartCountDown();

		StartCoroutine(CDRoutine);

		cursor.gameObject.SetActive(true);

		

		currentRoundFigureNum = Random.Range(0, GestureTemplates.Instance.Templates.Count);

		DrawRoundMission();
	    //lr.SetPositions(GestureTemplates.Templates[0]);

	}

    private void DrawRoundMission()
    {
        var lr = GameObject.Find("RoundMission").GetComponent<LineRenderer>();
        var figure = GestureTemplates.Instance.Templates[currentRoundFigureNum] as ArrayList;
        lr.SetVertexCount(figure.Count);
        var gr = GestureRecognizer.Instance;
        Vector2 center = gr.calcCenterOfGesture(figure);

        float radians = Mathf.Deg2Rad* (float)GestureTemplates.Instance.TemplateAngles[currentRoundFigureNum];
        figure = gr.RotateGesture(figure, radians, center);
        for (int i = 0; i < figure.Count; i++)
        {
            lr.SetPosition(i, Camera.main.ScreenToWorldPoint((Vector2) figure[i]));
        }
    }

    IEnumerator StartCountDown()
	{
		var timerText = panelGameplay.transform.Find("TimeLeftValue").GetComponent<Text>();
		timeLeft = initialTime - ((currentRound - 1) * timeToSubstractByRound);

		while (timeLeft > 0)
		{
			timeLeft -= Time.deltaTime;
			timerText.text = timeLeft.ToString("0.###");					
			yield return 0;	
		}
		GameOver();

	}

	void GameOver()
	{
		StopRound();
		panelGameover.transform.Find("ScoreValue").GetComponent<Text>().text = currentRound.ToString();
		panelGameplay.SetActive(false); 
		panelGameover.SetActive(true);

	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void EvaluateFigure(ArrayList moveLog)
	{
		if (moveLog.Count < 1)
			return;
	    var result = GestureRecognizer.Instance.startRecognizer(moveLog);
        Debug.Log(currentRoundFigureNum == GestureRecognizer.Instance.lastMatchedNum);
        if ( result && currentRoundFigureNum == GestureRecognizer.Instance.lastMatchedNum)
		{
			StopRound();
			StartNextRound();
		}
			
	}

	void StopRound()
	{		
		StopCoroutine(CDRoutine);
		isRoundStarted = false;
		cursor.gameObject.SetActive(false);
	}

    public void LoadLearnMode()
    {
        SceneManager.LoadScene("Learn");
    }
}
