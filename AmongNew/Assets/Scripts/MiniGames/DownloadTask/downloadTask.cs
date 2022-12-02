using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class downloadTask : MonoBehaviour
{

    public Animation myAnim;
    //public UIControl _uiControl;
    GameObject parts;

    public void Start()
    {
        myAnim = GetComponent<Animation>();
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
        StartCoroutine(FinishTask());
    }

    private IEnumerator FinishTask()
    {
        Debug.Log("Before Anim");
        myAnim.Play("loading");

        yield return new WaitForSeconds(6.0f);
        Debug.Log("After Anim");
        dowloadTask();
    }

    public void dowloadTask()
    {
        gameObject.SetActive(false);
    }
}
