using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGenerator : MonoBehaviour {

    [SerializeField]
    private GameObject greenBoxPrefab;
    [SerializeField]
    private Transform boxParent;
    private float boxOffset = 2.5f;     // so box will instatiate on the top of terrain
     
    //Create box with mouse click
    public void CreateGreenBox(Vector3 point)
    {
        if (IsPositionValid(point))
        {
            Vector3 boxPos = new Vector3(point.x, point.y + boxOffset, point.z);
            Instantiate(greenBoxPrefab, boxPos, Quaternion.identity, boxParent);
        }
    }
    // generate boxes at random positions on terrain
    public void GenerateBoxes(int quantity)
    {
        //DestroyAllBoxes();
        Manager.Instance.CallClearEvent();
        float terrainWidth = Terrain.activeTerrain.terrainData.size.x;
        float terrainLength = Terrain.activeTerrain.terrainData.size.z;
        float terrainPositionX = Terrain.activeTerrain.transform.position.x;
        float terrainPositionZ = Terrain.activeTerrain.transform.position.z;

        while (quantity != 0)
        {
            float posX = Random.Range(terrainPositionX, terrainPositionX + terrainWidth);
            float posZ = Random.Range(terrainPositionZ, terrainPositionZ + terrainLength);
            float posY = Terrain.activeTerrain.SampleHeight(new Vector3(posX, 0, posZ));
            Vector3 newPos = new Vector3(posX, posY + boxOffset, posZ);     // new random position for box

            if (IsPositionValid(newPos))
            {
                Instantiate(greenBoxPrefab, newPos, Quaternion.identity, boxParent);
                quantity--;
            }
        }
    }
    //check if theres enought space for box
    private bool IsPositionValid (Vector3 position) 
	{
        Collider[] colls = Physics.OverlapBox(position, new Vector3(boxOffset, boxOffset, boxOffset));
        //check if you hit only terrain collider
        if (colls.Length == 1) return true;
        else return false;
	}

    public void LightBoxes(int fireTheseBoxes)
    {   //check if there are boxes
        if (boxParent.childCount == 0) return;
        List<Transform> usedBoxes = new List<Transform>();  //collect boxes, you already light, so you dont light it twice
        while (fireTheseBoxes != 0)
        {   //take random child from box parent and check if you already used it
            int boxIndex = Random.Range(0, boxParent.childCount);
            if (!usedBoxes.Contains(boxParent.GetChild(boxIndex)))
            {
                boxParent.GetChild(boxIndex).GetComponent<Box>().LightThis();
                usedBoxes.Add(boxParent.GetChild(boxIndex));
                fireTheseBoxes--;
            }
        }
    }
}