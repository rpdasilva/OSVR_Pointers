using UnityEngine;

public class LightControl : MonoBehaviour {
    private float counterFlash = 0;
    private float counterMove = 0;
    private Light sceneLight;
    public float r, g, b;

    public bool flashEnabled;
    public bool moveEnabled;

    public void SetRed(float x)
    {
        r = x;
    }
    public void SetGreen(float x)
    {
        g = x;
    }
    public void SetBlue(float x)
    {
        b = x;
    }
    public void SetFlashEnabled(bool x)
    {
        flashEnabled = x;
    }
    public void SetMoveEnabled(bool x)
    {
        moveEnabled = x;
    }


	// Use this for initialization
	void Start () {
        sceneLight = GetComponent<Light>();

        r = g = b = 1;
	}
	
	// Update is called once per frame
	void Update () {
        sceneLight.color = new Color(r, g, b);

        if (flashEnabled)
        {
            counterFlash += Time.deltaTime;
            sceneLight.intensity = (Mathf.Sin(counterFlash * 20f) + 1f) * 3;
        }
        if (moveEnabled)
        {
            counterMove += Time.deltaTime;
            sceneLight.transform.localRotation =Quaternion.AngleAxis(counterMove * 360, Vector3.forward)  * Quaternion.AngleAxis(15, Vector3.left);
                
        }
	}
}
