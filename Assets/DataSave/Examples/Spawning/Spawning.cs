using System.Collections;
using System.Collections.Generic;
using SaveLoadSystem;
using UnityEngine;

namespace SaveLoadSystem.Examples
{
    public class Spawning : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private int _spawnInterval;
        [SerializeField] private GameObject _object;

        private List<Vector3> _objectPositions = new();
        private List<Transform> _spawnedObjects = new();

        private void Awake()
        {
            _objectPositions = DataManager.Load<List<Vector3>>("Object Positions");
            if (_objectPositions == null)
            {
                _objectPositions = new();
                return;
            }

            foreach (Vector3 position in _objectPositions)
            {
                GameObject go = CreateObject();
                go.transform.position = position;
                _spawnedObjects.Add(go.transform);
            }
        }

        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);

                GameObject go = CreateObject();
                _spawnedObjects.Add(go.transform);

                RefreshPositionList();
            }
        }

        private GameObject CreateObject()
        {
            GameObject go = Instantiate(_object, transform);
            go.transform.position = _spawnPoint.position;

            return go;
        }

        private void RefreshPositionList()
        {
            _objectPositions.Clear();

            foreach (Transform go in _spawnedObjects)
            {
                _objectPositions.Add(go.position);
            }

            DataManager.Save("Object Positions", _objectPositions);
        }
    }
}

