using UnityEngine;
using UnityEngine.EventSystems;

public class Manager : Singleton<Manager> {

    //WIND
    public float WindSpeed { get; set; } // modified by slider. values 0 - 10    
    [SerializeField]
    private GameObject windArrow;         // visual indicator for wind rotation
    private int windMultiplier = 2;     //coz map is big

    //BOXES
    [SerializeField]
    private int boxQuantity = 200;          // for generating boxes
    [SerializeField]
    private int lightBoxesQuantity = 50;    // for random light boxes
    private BoxGenerator boxGenerator;
    public float SpreadRange
    {
        get { return WindSpeed * windMultiplier; }
    }

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
        this.transform.localEulerAngles = new Vector3(0, 90, 0); // == arrow start rotation
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

    public Vector3 GetSpreadingPosition(Transform boxTransform)
    {  
        return boxTransform.position + this.transform.forward * WindSpeed * windMultiplier;
    }

    public void ChangeWindDirection(float rotation)
    {       
        windArrow.transform.localEulerAngles = new Vector3(0, 0, rotation);
        this.transform.localEulerAngles = new Vector3(0, 90 - rotation, 0);
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
        isSimulating = false;
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
