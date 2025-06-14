using System.Collections.Generic;
using UnityEngine;

namespace SaveLoadSystem.Examples
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _disableOnStart;
        [SerializeField] private List<GameObject> _enableOnStart;


        private void OnEnable()
        {
            SaveSlotUI.OnGameStarted += HandleGameStart;
        }

        private void OnDisable()
        {
            SaveSlotUI.OnGameStarted -= HandleGameStart;
        }

        private void HandleGameStart()
        {
            foreach (GameObject go in _disableOnStart)
            {
                go.SetActive(false);
            }

            foreach (GameObject go in _enableOnStart)
            {
                go.SetActive(true);
            }
        }
    }
}