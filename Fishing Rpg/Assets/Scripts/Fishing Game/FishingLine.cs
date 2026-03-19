using UnityEngine;

public class FishingLine : MonoBehaviour
{
    [SerializeField] private Transform rodTip;
    private LineRenderer lineRenderer;
    private bool isEnabled = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (isEnabled && lineRenderer != null && rodTip != null)
        {
            lineRenderer.SetPosition(0, rodTip.position);
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    public void EnableLine(bool enable)
    {
        isEnabled = enable;
        if (lineRenderer != null)
        {
            lineRenderer.enabled = enable;
        }
    }
}
