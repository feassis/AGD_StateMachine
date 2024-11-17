using StatePattern.Main;
using UnityEngine;

namespace StatePattern.Player
{
    public class PlayerService
    {
        private PlayerScriptableObject playerScriptableObject;
        private PlayerController playerController;

        public PlayerService(PlayerScriptableObject playerScriptableObject)
        {
            this.playerScriptableObject = playerScriptableObject;
            SubscribeToEvents();
        }

        private void SubscribeToEvents() => GameService.Instance.EventService.OnLevelSelected.AddListener(SpawnPlayer);

        private void UnsubscribeToEvents() => GameService.Instance.EventService.OnLevelSelected.RemoveListener(SpawnPlayer);

        public void SpawnPlayer(int levelId)
        {
            if(playerController != null)
            {
                playerController.ResetPlayer();
            }
            else
            {
                playerController = new PlayerController(playerScriptableObject);
            }
        }

        ~PlayerService()
        {
            UnsubscribeToEvents();
        }

        public PlayerController GetPlayer() => playerController;
    }
}