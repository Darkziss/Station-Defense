using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleRenderer : MonoBehaviour
{
    [SerializeField, Min(minRadius)] private float _radius;
    [SerializeField, Min(minWidth)] private float _width = minWidth;
    [SerializeField, Range(minSegmentCount, maxSegmentCount)] private int _segmentCount;
    
    private LineRenderer _lineRenderer;

    private const float minRadius = 0.1f;

    private const float minWidth = 1f;

    private const int minSegmentCount = 1;
    private const int maxSegmentCount = 64;

    private void OnValidate()
    {
        if (_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _lineRenderer.positionCount = _segmentCount;
        _lineRenderer.widthMultiplier = _width;

        float angle = 20f;

        for (int i = 0; i < _segmentCount; i++)
        {
            Vector3 position = new()
            {
                x = Mathf.Sin(angle * Mathf.Deg2Rad) * _radius,
                y = Mathf.Cos(angle * Mathf.Deg2Rad) * _radius
            };

            _lineRenderer.SetPosition(i, position);

            angle += 380f / _segmentCount;
        }
    }

    public void SetAlpha(float alpha)
    {
        Color color = _lineRenderer.startColor;
        color.a = alpha;

        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }
}