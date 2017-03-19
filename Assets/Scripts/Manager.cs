using UnityEngine;

public class Manager : Singleton<Manager> {
 
    [SerializeField]
    private int boxQuantity = 200;
    [SerializeField]
    private int lightBoxesQuantity = 50;
    private BoxGenerator boxGenerator;
    private bool isSimulating = false;
    private Mode clickingMode = Mode.Add;   // this is modified in dropdown

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
                switch(clickingMode)
                {
                    case Mode.Add: CreateBox(hit);
                        break;
                    case Mode.Remove: RemoveBox(hit);
                        break;
                    case Mode.ToggleFire: LightBox(hit);
                        break;
                }
            }
        }
	}

    public void ChangeClickMode(Mode mode)
    {
        clickingMode = mode;
    }

    //CLICKS
    public void CreateBox(RaycastHit hit)
    {
        // if hits terrain, create green box
        if (hit.transform.tag == "Ground")
        {
            boxGenerator.CreateGreenBox(hit.point);
        }
    }

    public void RemoveBox(RaycastHit hit)
    {
        // if hits box, destroy it
        if (hit.transform.tag == "Box")
        {
            Destroy(hit.transform.gameObject);
        }
    }

    public void LightBox(RaycastHit hit)
    {
        // if hits box, light that box
        if (hit.transform.tag == "Box")
        {
            hit.transform.gameObject.GetComponent<Box>().LightThis();
        }
    }


    //BUTTONS
    // create boxes at random position on terrain
    public void GenerateBoxes()
    {
        boxGenerator.GenerateBoxes(boxQuantity);
    }
    // light few random boxes
    public void LightBoxes()
    {
        boxGenerator.LightBoxes(lightBoxesQuantity);
    }

    // destroy all boxes in scene
    public void CallClearEvent()
    {
        if (Clear != null) Clear();
        boxGenerator.boxList.Clear();
    }

    // start/ stop simulate fire spreading
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

    public void Quit()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
             Application.Quit();
    #endif
    }
}
