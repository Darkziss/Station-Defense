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

    private const int minSegmentCount = 4;
    private const int maxSegmentCount = 128;

    private void OnValidate()
    {
        if (_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();

        DrawCircle();
    }

    public void SetAlpha(float alpha)
    {
        Color color = _lineRenderer.startColor;
        color.a = alpha;

        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }

    private void DrawCircle()
    {
        _lineRenderer.positionCount = _segmentCount;
        _lineRenderer.widthMultiplier = _width;
        _lineRenderer.loop = true;

        for (int i = 0; i < _segmentCount; i++)
        {
            float progress = (float)i / _segmentCount;

            const float Tau = Mathf.PI * 2f;
            float radian = (float)i / _segmentCount * Tau;

            Vector3 position = new()
            {
                x = Mathf.Cos(radian) * _radius,
                y = Mathf.Sin(radian) * _radius
            };

            _lineRenderer.SetPosition(i, position);
        }
    }
}