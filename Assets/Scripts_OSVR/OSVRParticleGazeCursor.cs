using UnityEngine;

[RequireComponent(typeof(OSVRGazePointer))]
public class OSVRParticleGazeCursor : MonoBehaviour
{
    public float emissionScale;
    public float maxSpeed;
    [Header("Particle emission curves")]
    // The scale on the x axis of the curves runs from 0 to maxSpeed
    [Tooltip("Curve for trailing edge of pointer")]
    public AnimationCurve halfEmission;
    [Tooltip("Curve for full perimeter of pointer")]
    public AnimationCurve fullEmission;

    [Tooltip("Curve for full perimeter of pointer")]
    public bool particleTrail;
    public float particleScale = 0.68f;

    Vector3 lastPos;
    ParticleSystem psHalf;
    ParticleSystem psFull;
    MeshRenderer quadRenderer;
    Color particleStartColor;
    

    OSVRGazePointer gazePointer;

    // Use this for initialization
    void Start()
    {
        gazePointer = GetComponent<OSVRGazePointer>();
        foreach (Transform child in transform)
        {
            if (child.name.Equals("Half"))
                psHalf = child.GetComponent<ParticleSystem>();
            if (child.name.Equals("Full"))
                psFull = child.GetComponent<ParticleSystem>();
            if (child.name.Equals("Quad"))
                quadRenderer = child.GetComponent<MeshRenderer>();
        }
        float scale =  transform.lossyScale.x;
        psHalf.startSize *= scale;
        psHalf.startSpeed *= scale;
        psFull.startSize *= scale;
        psFull.startSpeed *= scale;

        particleStartColor = psFull.startColor;

        if (!particleTrail)
        {
            GameObject.Destroy(psHalf);
            GameObject.Destroy(psFull);
        }

    }

    // Update is called once per frame
    void Update()
    {
        var delta = GetComponent<OSVRGazePointer>().positionDelta;
        var psHalfEm = psHalf.emission;
        var psFullEm = psFull.emission;

        if (particleTrail)
        {
            // Evaluate these curves to decide the emission rate of the two sources of particles.
            psHalfEm.rate = new ParticleSystem.MinMaxCurve(halfEmission.Evaluate((delta.magnitude / Time.deltaTime) / maxSpeed) * emissionScale);
            psFullEm.rate = new ParticleSystem.MinMaxCurve(fullEmission.Evaluate((delta.magnitude / Time.deltaTime) / maxSpeed) * emissionScale);

            // Make the particles fade out with visibitly the same way the main ring does
            Color color = particleStartColor;
            color.a = gazePointer.visibilityStrength;
            psHalf.startColor = color;
            psFull.startColor = color;

            // Particles also scale when the gaze pointer scales
            psFull.startSize = particleScale * transform.lossyScale.x;
            psHalf.startSize = particleScale * transform.lossyScale.x;
        }

        // Set the main pointers alpha value to the correct level to achieve the desired level of fade
        quadRenderer.material.SetColor("_TintColor",new Color(1, 1, 1, gazePointer.visibilityStrength));
        
    }
}
