﻿using UnityEngine;

[RequireComponent(typeof(OSVRRaycaster))]
public class OSVRMousePointer : MonoBehaviour {

    [Tooltip("Period of inactivity before mouse disappears")]
    public float inactivityTimeout = 1;

    public enum MouseShowPolicy
    {
        always,
        withGaze,
        withActivity
    };
    
    [Tooltip("Policy regarding when mouse pointer should be shown")]
    public MouseShowPolicy mouseShowPolicy;

    [Tooltip("Should the mouse pointer being active cause the gaze pointer to fade")]
    public bool hideGazePointerWhenActive = false;

    [Tooltip("Move the pointer in response to mouse movement")]
    public bool defaultMouseMovement = true;

    public float mouseMoveSpeed = 5;
    
    private float lastMouseActivityTime;


    public GameObject pointerObject
    {
        get
        {
            return GetComponent<OSVRRaycaster>().pointer;
        }
    }
    
    
    OSVRRaycaster raycaster;
    void Awake()
    {        
        raycaster = GetComponent<OSVRRaycaster>();       
#if UNITY_ANDROID
        pointerObject.SetActive(false);
        this.enabled = false;
#endif
    }

	// Update is called once per frame
	void Update () 
    {
        // Set mouse visibility based on the chosen policy
        if (mouseShowPolicy == MouseShowPolicy.withActivity)
            pointerObject.SetActive(HasMovedRecently() && raycaster.IsFocussed());
        else if (mouseShowPolicy == MouseShowPolicy.withGaze)
            pointerObject.SetActive(raycaster.IsFocussed());

        if (hideGazePointerWhenActive && HasMovedRecently() && raycaster.IsFocussed())
            OSVRGazePointer.instance.RequestHide();

        if (defaultMouseMovement && raycaster.IsFocussed())
        {
            // If we're responsible for moving the pointer and we're the currently active
            // raycaster then update the mouse position, keeping it clamped to inside the canvas.
            Vector2 mousePos = pointerObject.GetComponent<RectTransform>().localPosition;
            mousePos += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseMoveSpeed;
            float currentPanelWidth = GetComponent<RectTransform>().rect.width;
            float currentPanelHeight = GetComponent<RectTransform>().rect.height;
            mousePos.x = Mathf.Clamp(mousePos.x, -currentPanelWidth / 2, currentPanelWidth / 2);
            mousePos.y = Mathf.Clamp(mousePos.y, -currentPanelHeight / 2, currentPanelHeight / 2);

            // Position mouse pointer
            SetLocalPosition(mousePos);
        }
	
	}

    public bool HasMovedRecently()
    {
        return lastMouseActivityTime + inactivityTimeout > Time.time;
    }
    


    public Vector3 GetWorldPosition()
    {
        return pointerObject.transform.position;
    }

    /// <summary>
    /// Set position of pointer. Only makes sense to use if defaultMouseMovement is false
    /// </summary>
    /// <param name="pos"></param>
    public void SetLocalPosition(Vector3 pos)
    {
        if ((pointerObject.GetComponent<RectTransform>().localPosition - pos).magnitude> 0.001f)
        {
            lastMouseActivityTime = Time.time;
        }
        pointerObject.GetComponent<RectTransform>().localPosition = pos;
    }

   
}
