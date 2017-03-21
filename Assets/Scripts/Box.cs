using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    [SerializeField]
    private string flammableMask;
    [SerializeField]
    private string defaultMask;
    private bool isBurning = false;
    private bool startSimulate = false;
    private float fireSpreadDelay = 2f;
    private float burningDefaultTime = 4f;
    private float burningTime;
    private MeshRenderer meshRender;

    private void OnEnable()
    {
        meshRender = GetComponent<MeshRenderer>();
        ResetSettings();
        Manager.Instance.StartSimulation += StartSimulation;
        Manager.Instance.StopSimulation += ResetSettings;
        Manager.Instance.Clear += DestroyThis;
    }

    private void OnDisable()
    {
        Manager.Instance.StartSimulation -= StartSimulation;
        Manager.Instance.StopSimulation -= ResetSettings;
        Manager.Instance.Clear -= DestroyThis;
    }

    private void Update()
    {
        if (isBurning && startSimulate)
        {
            burningTime -= Time.deltaTime;
            if (burningTime < 0)
            {   // box burnt
                isBurning = false;
                meshRender.material.color = Color.black;
            }
        }
    }

    public void LightThis()
    {
        isBurning = true;
        meshRender.material.color = Color.red;
        this.gameObject.layer = LayerMask.NameToLayer(defaultMask);
        //check if this happen before or during simulation
        if (startSimulate) StartCoroutine(SpreadFire());
    }

    public void StartSimulation()
    {
        startSimulate = true;
        if(isBurning) StartCoroutine(SpreadFire());
    }
    //box spread fire only once
    private IEnumerator SpreadFire()
    {
        yield return new WaitForSeconds(fireSpreadDelay);
        Collider[] colls = Physics.OverlapSphere(Manager.Instance.GetSpreadingPosition(this.transform), Manager.Instance.SpreadRange, 1 << LayerMask.NameToLayer(flammableMask));
        foreach (Collider coll in colls)
        {
            coll.gameObject.GetComponent<Box>().LightThis();
        }
    }
    // also "stopSimulation"
    public void ResetSettings()
    {
        StopAllCoroutines();
        isBurning = false;
        startSimulate = false;
        meshRender.material.color = Color.green;
        burningTime = burningDefaultTime;
        this.gameObject.layer = LayerMask.NameToLayer(flammableMask); 
    }

    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }

    //debug for overlap sphere in spred fire coroutine
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(Manager.Instance.GetSpreadingPosition(this.transform), Manager.Instance.SpreadRange);
    //}
}
