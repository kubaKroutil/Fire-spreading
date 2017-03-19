using UnityEngine;

public class Manager : Singleton<Manager> {

    public Mode clickingMode = Mode.Add;
    [SerializeField]
    private int boxQuantity = 200;
    [SerializeField]
    private int lightBoxesQuantity = 50;
    private BoxGenerator boxGenerator;
    private bool isSimulating = false;
    

    public delegate void GeneralEventHandler();
    public event GeneralEventHandler StartSimulation;
    public event GeneralEventHandler StopSimulation;
    public event GeneralEventHandler Clear;

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
            if (Physics.Raycast(ray,out hit))
            {
                // if hits terrain, create green box
                if (hit.transform.tag == "Ground")
                {
                    boxGenerator.CreateGreenBox(hit.point);
                }
            }
        }
	}

    //called via button
    public void GenerateBoxes()
    {
        boxGenerator.GenerateBoxes(boxQuantity);
    }

    //called via button
    public void LightBoxes()
    {
        boxGenerator.LightBoxes(lightBoxesQuantity);
    }

    //called via button, destroy all boxes in scene
    public void CallClearEvent()
    {
        if (Clear != null) Clear();
        boxGenerator.boxList.Clear();
    }

    //called via button
    public void Simulation()
    {
        isSimulating = !isSimulating;
        if (isSimulating)
        {
            if (StartSimulation != null) StartSimulation();
        }
        else
        {
            if (StopSimulation != null) StopSimulation();
        }
    }

    //called via button, destroy all boxes in scene
    public void Quit()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
             Application.Quit();
    #endif
    }
}
