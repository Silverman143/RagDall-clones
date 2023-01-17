using UnityEngine;

public class RopeTest : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public int segments = 10;
    public float width = 0.1f;

    private LineRenderer lineRenderer;

    void Start()
    {
        // Create the LineRenderer component and add it to the GameObject
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Set the LineRenderer's properties
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.positionCount = segments + 1;
        lineRenderer.material = new Material(Shader.Find("Standard"));
        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {
        // Generate the rope's vertex positions
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 position = Vector3.Lerp(startPoint.position, endPoint.position, t);
            lineRenderer.SetPosition(i, position);
        }
    }
}



