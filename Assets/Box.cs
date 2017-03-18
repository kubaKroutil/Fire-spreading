using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    private bool isBurning = false;
    private bool isBurnt = false;
    private float burningTime;

	private void Update () 
	{
        if (Time.timeScale == 0 || isBurnt) return;
	}

    public void StartBurning()
    {
        isBurning = true;
        //2 korutiny: 1 zhori, 2: snazi se rozhoret kolem
    }
}
