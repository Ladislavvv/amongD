using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTask : MonoBehaviour
{

    public List<SwipePoint> _swipePoints = new List<SwipePoint>();
    public float _countdownMax = 0.5f;

    public GameObject _greenOn;
    public GameObject _redOn;

    //public Transform startPosCard;
 

    private int _currentSwipePointIndex = 0;
    private float _countdown = 0;

    private void OnEnable()
    {
        _currentSwipePointIndex = 0;
        _countdown = 0;
        //startPosCard = transform.GetChild(3);

        _greenOn.SetActive(false);
        _redOn.SetActive(false);
    }

    private void Update()
    {
        _countdown -= Time.deltaTime;

        if (_currentSwipePointIndex != 0 && _countdown <= 0)
        {
            _currentSwipePointIndex = 0;
            //Debug.Log("Error");
            StartCoroutine(FinishTask(false));
        }
    }

    private IEnumerator FinishTask(bool wasSuccessful)
    {
        if (wasSuccessful)
        {
            _greenOn.SetActive(true);
        }
        else
        {
            _redOn.SetActive(true);
        }

        yield return new WaitForSeconds(0.8f);

        _greenOn.SetActive(false);
        _redOn.SetActive(false);

        if (wasSuccessful)
        {
            gameObject.SetActive(false);
        }
    }

    public void SwipePointTrigger(SwipePoint swipePoint)
    {
        if (swipePoint == _swipePoints[_currentSwipePointIndex])
        {
            _currentSwipePointIndex++;
            _countdown = _countdownMax;
        }

        if (_currentSwipePointIndex >= _swipePoints.Count)
        {
            _currentSwipePointIndex = 0;
            //Debug.Log("Finish");
            StartCoroutine(FinishTask(true));
        }
    }

    public void wireOffTask()
    {
        gameObject.SetActive(false);
    }

}