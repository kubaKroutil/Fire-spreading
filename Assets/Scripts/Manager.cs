using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
 
    private float rayRange= 1000f;
    private BoxGenerator boxGenerator;

    private void Start () 
	{
        boxGenerator = GetComponent<BoxGenerator>();
	}

	private void Update () 
	{
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit,rayRange))
            {
                // if hits terrain, create green box
                if (hit.transform.tag == "Ground")
                {
                    boxGenerator.CreateGreenBox(hit.point);
                }
            }
        }
	}

    public void StartSimulation()
    {
        Time.timeScale = 1;
    }
}
