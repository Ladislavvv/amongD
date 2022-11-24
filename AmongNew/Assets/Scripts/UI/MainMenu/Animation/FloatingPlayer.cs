using UnityEngine;
using UnityEngine.UI;

public class FloatingPlayer : MonoBehaviour {

    public Vector2 SpeedRandom = new Vector2();

    [SerializeField] private GameObject _body;
    [SerializeField] private RectTransform _container;
    [SerializeField] private float _speed = 0;

    private Vector3 _velocity;
    private RectTransform _floatingBound;

    private void Awake() {
        _speed = Random.Range(SpeedRandom.x, SpeedRandom.y);
    }

    private void Update() {
        transform.Translate(_velocity * Time.deltaTime * _speed);

        bool arrivedAtTarget = CheckRectOverlap(GetComponent<RectTransform>(), _floatingBound);
        if (arrivedAtTarget) {
            gameObject.SetActive(false);
        }
    }

    public void Initialize(Vector3 velocity, FloatingBound floatingBound, Color color) {
        _velocity = velocity;
        _floatingBound = floatingBound.GetComponent<RectTransform>();
        _body.GetComponent<SpriteRenderer>().color = color;

        float scale = Random.Range(1.0f, 2.1f);
        _container.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        _container.localScale = new Vector3(scale, scale, 0);
    }

    /// <summary>
    /// Checks if two UI rectangles overlap
    /// </summary>
    private bool CheckRectOverlap(RectTransform rectTrans1, RectTransform rectTrans2) {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }
}