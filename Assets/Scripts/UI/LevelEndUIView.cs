using StatePattern.Main;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StatePattern.UI
{
    public class LevelEndUIView : MonoBehaviour, IUIView
    {
        private LevelEndUIController controller;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private TextMeshProUGUI pointsText;

        private void Start() => SubscribeToButtonClicks();

        private void SubscribeToButtonClicks()
        {
            homeButton.onClick.AddListener(controller.OnHomeButtonClicked);
            quitButton.onClick.AddListener(controller.OnQuitButtonClicked);
            nextLevelButton.onClick.AddListener (controller.OnNextLevelButtonClicked);
        }

        public void SetController(IUIController controllerToSet) => controller = controllerToSet as LevelEndUIController;

        public void DisableView()
        {
            gameObject.SetActive(false);
            GameService.Instance.EventService.OnPointGained.RemoveListener(OnPointsGained);
        } 

        public void EnableView()
        {
            gameObject.SetActive(true);
            pointsText.text = $"Points: {GameService.Instance.PontuationService.Score}";
            GameService.Instance.EventService.OnPointGained.AddListener(OnPointsGained);
        }

        private void OnPointsGained(int score)
        {
            pointsText.text = $"Points: {score}";  
        }

        public void SetResultText(string textToSet) => resultText.SetText(textToSet);

    }
}