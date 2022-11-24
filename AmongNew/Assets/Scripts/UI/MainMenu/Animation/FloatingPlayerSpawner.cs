using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlayerSpawner : MonoBehaviour {

    [SerializeField] private FloatingPlayer _floatingPlayerPrefab;
    [SerializeField] private Transform _canvasParent;
    [SerializeField] private Vector2 _waitTime = new Vector2(1.25f, 3.25f);
    [SerializeField] private int _maxFloatingPlayers = 5;
    [SerializeField] private List<FloatingBound> _bounds = new List<FloatingBound>();
    public List<Color> ColorsList = new List<Color>();

    private List<FloatingPlayer> _spawnedFloatingPlayerList = new List<FloatingPlayer>();
    private List<FloatingBound> _usedBounds = new List<FloatingBound>();
    private List<Color> _usedColors = new List<Color>();

    private Dictionary<FloatingBound.Direction, FloatingBound> _directionToBound = new Dictionary<FloatingBound.Direction, FloatingBound>();

    private void Start() {
        // Creates a pool of floating players. Can be customized.
        for (int i = 0; i < _maxFloatingPlayers; i++) {
            FloatingPlayer newFloatingPlayer = Instantiate(_floatingPlayerPrefab);
            newFloatingPlayer.transform.SetParent(_canvasParent);
            newFloatingPlayer.gameObject.SetActive(false);

            _spawnedFloatingPlayerList.Add(newFloatingPlayer);
        }

        foreach (FloatingBound bound in _bounds) {
            _directionToBound.Add(bound.CurrentDirection, bound);
        }

        StartCoroutine(SpawnPlayer());
    }

    /// <summary>
    /// Spawns a player randomly into space
    /// </summary>
    private IEnumerator SpawnPlayer() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(_waitTime.x, _waitTime.y));

            FloatingPlayer selectedFloatingPlayer = null;
            FloatingBound selectedBound = null;
            Color selectedColor = Color.red;
            
            // Selecting a random side, being careful not to select one two times in a row

            if (_usedBounds.Count <= 0) {
                _usedBounds = new List<FloatingBound>(_bounds);
            }

            selectedBound = _usedBounds[Random.Range(0, _usedBounds.Count)];
            _usedBounds.Remove(selectedBound);

            // Selecting a random color, being careful not to select two colors in a row

            if (_usedColors.Count <= 0) {
                _usedColors = new List<Color>(ColorsList);
            }

            selectedColor = _usedColors[Random.Range(0, _usedColors.Count)];
            _usedColors.Remove(selectedColor);

            // Checks if there is an available floating player (disabled game object)
            for (int i = 0; i < _spawnedFloatingPlayerList.Count; i++) {
                if (!_spawnedFloatingPlayerList[i].gameObject.activeInHierarchy) {
                    selectedFloatingPlayer = _spawnedFloatingPlayerList[i];
                    selectedFloatingPlayer.gameObject.SetActive(true);
                    break;
                }
            }

            if (selectedFloatingPlayer != null) {
                selectedFloatingPlayer.transform.position = selectedBound.GetRandomPosition();
                Vector3 velocity = new Vector3();
                FloatingBound selectedOppositeBound = null;

                // Selects the velocity and opposite bound for the current floating player

                switch (selectedBound.CurrentDirection) {
                    case FloatingBound.Direction.TOP:
                        velocity.y = Random.Range(-10, -50);
                        selectedOppositeBound = _directionToBound[FloatingBound.Direction.BOTTOM];
                        break;

                    case FloatingBound.Direction.BOTTOM:
                        velocity.y = Random.Range(10, 50);
                        selectedOppositeBound = _directionToBound[FloatingBound.Direction.TOP];
                        break;

                    case FloatingBound.Direction.LEFT:
                        velocity.x = Random.Range(10, 50);
                        selectedOppositeBound = _directionToBound[FloatingBound.Direction.RIGHT];
                        break;

                    case FloatingBound.Direction.RIGHT:
                        velocity.x = Random.Range(-10, -50);
                        selectedOppositeBound = _directionToBound[FloatingBound.Direction.LEFT];
                        break;
                }

                selectedFloatingPlayer.Initialize(velocity, selectedOppositeBound, selectedColor);
            }
        }
    }

}