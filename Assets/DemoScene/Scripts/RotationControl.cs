using UnityEngine;

public class RotationControl : MonoBehaviour {
    float targetSpeed = 1;
    float currentSpeed = 1;
    public float fastSpeed = 5;
    public float slowSpeed = 1;
    public float accel = 3;
    float mult = -1;
	// Use this for initialization
	void Start () {
        currentSpeed = targetSpeed;
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(Vector3.up, Time.deltaTime * currentSpeed * 60);

        if (targetSpeed * mult > currentSpeed)
            currentSpeed += Time.deltaTime * accel;

        else if (targetSpeed * mult < currentSpeed)
            currentSpeed -= Time.deltaTime * accel;
	}

    public void SetFast()
    {
        targetSpeed = fastSpeed;
    }
    public void SetSlow()
    {
        targetSpeed = slowSpeed;
    }
    public void SetForward()
    {
        mult = 1;
    }
    public void SetBackward()
    {
        mult = -1;
    }

    public void LeverEvent(bool on)
    {
        if (on)
            SetForward();
        else
            SetBackward();
    }
    public Transform[] transforms;
    public void SetRotation0(float f)
    {
        transforms[0].localRotation = Quaternion.Euler(0, 0, f);
    }
    public void SetRotation1(float f)
    {
        transforms[1].localRotation = Quaternion.Euler(0, 0, f);
    }
    public void SetRotation2(float f)
    {
        transforms[2].localRotation = Quaternion.Euler(0, 0, -f);
    }
    public void SetRotation3(float f)
    {
        transforms[3].localRotation = Quaternion.Euler(0, 0, f);
    }
}
