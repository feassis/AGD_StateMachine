using System.Collections.Generic;
using UnityEngine;
using StatePattern.Main;
using StatePattern.Events;
using StatePattern.Enemy;

namespace StatePattern.Level
{
    public class LevelService
    {
        private List<LevelScriptableObject> levelScriptableObjects;
        private int currentLevel = 0;

        private GameObject loadedLevel;

        public LevelService(List<LevelScriptableObject> levelScriptableObjects)
        {
            this.levelScriptableObjects = levelScriptableObjects;
            SubscribeToEvents();
        }

        private void SubscribeToEvents() => GameService.Instance.EventService.OnLevelSelected.AddListener(LoadLevel);

        private void UnsubscribeToEvents() => GameService.Instance.EventService.OnLevelSelected.RemoveListener(LoadLevel);

        public void LoadLevel(int levelID)
        {
            if(loadedLevel != null)
            {
                GameObject.Destroy(loadedLevel);
                loadedLevel = null;
            }

            currentLevel = levelID;
            var levelData = levelScriptableObjects.Find(levelSO => levelSO.ID == levelID);
            loadedLevel = GameObject.Instantiate(levelData.LevelPrefab);
            UnsubscribeToEvents();
        }

        public void GoToNextLevel()
        {
            if(currentLevel >= levelScriptableObjects.Count)
            {
                return;
            }

            GameService.Instance.EventService.OnLevelSelected.InvokeEvent(currentLevel + 1);
            GameService.Instance.UIService.StartALevel();
        }

        public List<EnemyScriptableObject> GetEnemyDataForLevel(int levelId) => levelScriptableObjects.Find(level => level.ID == levelId).EnemyScriptableObjects;
    }
}