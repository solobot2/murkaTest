using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cursor : MonoBehaviour
{
	private ArrayList moveLog = new ArrayList();
	private TrailRenderer trail;
	// Use this for initialization
	private bool _writeLog = false;

	void Awake()
	{
		trail = GetComponent<TrailRenderer>();
	}
	
	// Update is called once per frame
	void Update()
	{				
		transform.position = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(0);
		if (_writeLog)
		{
			moveLog.Add((Vector2)transform.position);
		}
	}

	public ArrayList GetLog()
	{
		return moveLog;
	}

	public void ClearLog()
	{
		moveLog.Clear();
	}

	public void StartWriteLog()
	{
		trail.time = 0.5f;
		trail.enabled = true;
		_writeLog = true;
	}

	public void StopWriteLog()
	{
		trail.time = 0;
		trail.enabled = false;
		_writeLog = false;
	}
}
