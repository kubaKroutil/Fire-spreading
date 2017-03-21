using UnityEngine;
using UnityEngine.EventSystems;

public class Manager : Singleton<Manager> {

    //WIND
    public float WindSpeed { get; set; } // modified by slider. values 0 - 10    
    public float WindRotation { get; private set; } // modified by slider. values 0 - 359
    [SerializeField]
    private GameObject windArrow;         // visual indicator for wind rotation
   
    //BOXES
    [SerializeField]
    private int boxQuantity = 200;          // for generating boxes
    [SerializeField]
    private int lightBoxesQuantity = 50;    // for random light boxes
    private BoxGenerator boxGenerator;

    //MANAGER
    private bool isSimulating = false;
    private Mode clickingMode = Mode.Add;   // this is modified in dropdown

    public delegate void GeneralEventHandler();
    public event GeneralEventHandler StartSimulation;
    public event GeneralEventHandler StopSimulation;
    public event GeneralEventHandler Clear;

    private void Start () 
	{
        boxGenerator = GetComponent<BoxGenerator>();
        WindRotation = 0;
    }

	private void Update () 
	{   //if you click and mouse is not over UI
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
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

    public void ChangeWindDirection(float rotation)
    {
        WindRotation = rotation;
        windArrow.transform.localEulerAngles = new Vector3(0, 0, rotation);
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

    //EXIT
    public void Quit()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
             Application.Quit();
    #endif
    }
}
