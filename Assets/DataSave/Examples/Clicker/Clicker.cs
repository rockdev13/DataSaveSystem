using TMPro;
using UnityEngine;

namespace SaveLoadSystem.Examples
{
    public class Clicker : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _clickDisplay;

        private int _clickAmount;
        private int ClickAmount
        {
            get => _clickAmount;
            set
            {
                _clickAmount = value;
                _clickDisplay.text = value.ToString();
            }
        }

        private async void Awake()
        {
            ClickAmount = await DataManager.LoadAsync<int>("Clicks");
        }

        public async void Click()
        {
            ClickAmount++;
            await DataManager.SaveAsync("Clicks", ClickAmount);
        }
    }
}