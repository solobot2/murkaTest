using UnityEngine;
using System.Collections;

public class MouseDrawer : MonoBehaviour
{
	private Ray castedRay;
	public GameObject cursor;
	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
		/*if (Input.GetMouseButtonDown(0))
	    {
	        cursor.GetComponent<TrailRenderer>().time = 10;	        
	    }
	    if (Input.GetMouseButtonUp(0))
	    {
            cursor.GetComponent<TrailRenderer>().time = 1f;
        }
        */
		var temp = Input.GetKey("Control");

		if (Input.GetMouseButton(0))
		{
			castedRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			cursor.transform.position = castedRay.GetPoint(0);
		}
	}
}
