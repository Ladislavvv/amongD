using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadTask : MonoBehaviour
{
    public Text _cardCode;
    public Text _inputCode;
    public int _codeLength = 5;
    public float _codeResetTimeInSeconds = 0.5f;

    public bool _isReseting = false;

    private void OnEnable()
    {
        string code = string.Empty;

        for (int i = 0; i < _codeLength; i++)
        {
            code += Random.Range(1, 10);
        }

        _cardCode.text = code;
        _inputCode.text = string.Empty;
    }

    public void ButtonClick(int number)
    {
        if (_isReseting) { return; }

        _inputCode.text += number;

        if (_inputCode.text == _cardCode.text)
        {
            _inputCode.text = "Correct";
            StartCoroutine(ResetCode());
        }
        else if (_inputCode.text.Length >= _codeLength)
        {
            _inputCode.text = "Failed";
            StartCoroutine(ResetCode());
        }
    }

    private IEnumerator ResetCode()
    {
        _isReseting = true;
        

        yield return new WaitForSeconds(_codeResetTimeInSeconds);

        _inputCode.text = string.Empty;
        _isReseting = false;
        taskOTurnOff();
    }

    public void taskOTurnOff()
    {
        gameObject.SetActive(false);
    }
}
