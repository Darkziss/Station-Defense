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
    private const int maxSegmentCount = 128;

    private const bool Loop = true;

    private void OnValidate()
    {
        if (_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();

        DrawCircle();
    }

    private void DrawCircle()
    {
        _lineRenderer.loop = Loop;

        _lineRenderer.positionCount = _segmentCount;
        _lineRenderer.widthMultiplier = _width;

        for (int i = 0; i < _segmentCount; i++)
        {
            const float Tau = Mathf.PI * 2f;

            float progress = (float)i / _segmentCount;
            float radian = progress * Tau;

            Vector3 position = new()
            {
                x = Mathf.Sin(radian) * _radius,
                y = Mathf.Cos(radian) * _radius
            };

            _lineRenderer.SetPosition(i, position);
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