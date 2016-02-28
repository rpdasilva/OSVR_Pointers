using UnityEngine;
using UnityEngine.Events;
using System;

public class leverSystem : MonoBehaviour {

    bool previousState;
    bool currentState;
    bool moving;
    float movement = -40;
    public float speed;

    [Serializable]
    public class LeverClickedEvent : UnityEvent<bool> { }

    [SerializeField]
    private LeverClickedEvent onClick = new LeverClickedEvent();
	// Use this for initialization
	void Start () {
        transform.localRotation = Quaternion.Euler(movement, 0, 0);
	
	}
	
	// Update is called once per frame
	void Update () {
        if (moving)
        {
            float initialSign = Mathf.Sign(movement);
            movement += Time.deltaTime * (previousState ? -1 : 1) * speed;

            transform.rotation = Quaternion.Euler(new Vector3(movement, 0, 0));
            if ((movement > 40f && !previousState) || (movement < -40 && previousState))
            {
                currentState = !previousState;
                moving = false;
            }
            if (Mathf.Sign(movement) != initialSign)
            {
                onClick.Invoke(!previousState);
            }

            transform.localRotation = Quaternion.Euler(movement, 0, 0);
        }
	
	}

    public void OnPointerClick()
    {
        if (!moving)
        {
            previousState = currentState;
            moving = true;
        }
    }
}
