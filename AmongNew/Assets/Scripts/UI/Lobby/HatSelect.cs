using System.Collections.Generic;
using UnityEngine;

public class HatSelect : MonoBehaviour {

    public List<GameObject> _hatList = new List<GameObject>();

    private int _currentHatIndex = -1;

    private void OnEnable() {
        PlayerPrefs.SetInt("PlayerHat", -1);
    }

    public void OnHatChange(int index) {
        _currentHatIndex += index;
        _currentHatIndex = Mathf.Clamp(_currentHatIndex, -1, _hatList.Count - 1);
        PlayerPrefs.SetInt("PlayerHat", _currentHatIndex);

        for (int i = 0; i < _hatList.Count; i++) {
            _hatList[i].SetActive(i == _currentHatIndex);
        }
    }
    
}