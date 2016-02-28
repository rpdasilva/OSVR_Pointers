using UnityEngine;
using UnityEngine.EventSystems;

public class PointersControl : MonoBehaviour {
    
    bool vrMode = false;
    
    private OSVRInputModule _inputModule;
    private OSVRInputModule inputModule
    {
        get 
        {
            if (_inputModule == null)
            {
                _inputModule = EventSystem.current.currentInputModule as OSVRInputModule;
            }
            return _inputModule;
        }
    }

    void Awake()
    {
        if (GameObject.Find("VRViewer0"))
        {
            vrMode = true;
            // In VR we should lock the cursor to the this window.
            Cursor.lockState = UnityEngine.CursorLockMode.Locked;
        }
    }

    void Update()
    {
        // lock cursor when user clicks on window
        if (vrMode && Input.GetMouseButtonDown(0))
            Cursor.lockState = UnityEngine.CursorLockMode.Locked;
    }

    public void SetUseSphereTest(bool on)
    {
        inputModule.performSphereCastForGazepointer = on;
    }

    public void SetMatchNormal(bool on)
    {
        FindObjectOfType<OSVRInputModule>().matchNormalOnPhysicsColliders = on;
    }
    public void SetHideGazepointerByDefault(bool hide)
    {
        OSVRGazePointer.instance.hideByDefault = hide;
    }
    public void SetOnlyDimCursorWhenMouseActive(bool dim)
    {
        OSVRGazePointer.instance.dimOnHideRequest = dim;
    }

	// Use this for initialization
	void Start () {
	}

}
