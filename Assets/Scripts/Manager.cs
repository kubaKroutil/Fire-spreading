using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {


    [SerializeField]
    private int boxQuantity = 10;    // how many boxes will be generated
    private float rayRange= 1000f;
    private BoxGenerator boxGenerator;

    private void Start () 
	{
        Time.timeScale = 0;
        boxGenerator = GetComponent<BoxGenerator>();
        boxGenerator.GenerateBoxes(boxQuantity);
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
}
