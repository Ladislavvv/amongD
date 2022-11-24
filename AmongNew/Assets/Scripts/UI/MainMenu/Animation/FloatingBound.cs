using UnityEngine;

public class FloatingBound : MonoBehaviour {
    public enum Direction {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT
    }

    public Direction CurrentDirection;

    private RectTransform _rectTransform;

    private void Awake() {
        _rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Generates a random position contained in this bound
    /// </summary>
    public Vector3 GetRandomPosition() {
        return new Vector3(Random.Range(_rectTransform.rect.xMin, _rectTransform.rect.xMax),
                   Random.Range(_rectTransform.rect.yMin, _rectTransform.rect.yMax), 0) + _rectTransform.transform.position;
    }
}