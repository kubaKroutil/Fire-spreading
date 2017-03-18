using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    private bool isBurning = false;
    private bool isBurnt = false;
    private float fireSpreadDelay = 2f;
    private float burningDefaultTime = 4f;
    private float burningTime;
    private MeshRenderer meshRender;

    private void Start()
    {
        meshRender = GetComponent<MeshRenderer>();
        meshRender.material.color = Color.green;
        burningTime = burningDefaultTime;
    }

    private void Update()
    {
        if (isBurnt) return;
        if (isBurning) burningTime -= Time.deltaTime;
        if (burningTime < 0) Burnt();
    }

    public void StartBurning()
    {
        isBurning = true;
        meshRender.material.color = Color.red;
        this.tag = "Untagged";
        StartCoroutine(SpreadFire());
    }

    private void Burnt()
    {
        isBurnt = true;
        meshRender.material.color = Color.black;
    }

    private IEnumerator SpreadFire()
    {
        yield return new WaitForSeconds(fireSpreadDelay);
        Collider[] colls = Physics.OverlapSphere(this.transform.position, 20);
        foreach (Collider coll in colls)
        {
            if (coll.tag == "Flammable") coll.gameObject.GetComponent<Box>().StartBurning();
        }

    }

    public void ResetSettings()
    {
        StopAllCoroutines();
        isBurning = false;
        isBurnt = false;
        meshRender.material.color = Color.green;
        burningTime = burningDefaultTime;
        this.tag = "Flammable";
    }
}
