/**  This script demonstrates implementation of the Service Locator Pattern.
*  If you're interested in learning about Service Locator Pattern, 
*  you can find a dedicated course on Outscal's website.
*  Link: https://outscal.com/courses
**/

using UnityEngine;
using StatePattern.Utilities;
using StatePattern.Enemy;
using StatePattern.Sound;
using StatePattern.Level;
using StatePattern.Player;
using System.Collections.Generic;
using StatePattern.UI;
using StatePattern.Events;

namespace StatePattern.Main
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        // Services:
        public SoundService SoundService { get; private set; }
        public EventService EventService { get; private set; }
        public LevelService LevelService { get; private set; }
        public PlayerService PlayerService { get; private set; }
        public EnemyService EnemyService { get; private set; }
        public PontuationService PontuationService { get; private set;}

        [SerializeField] private UIService uiService;
        public UIService UIService => uiService;

        // Scriptable Objects:
        [SerializeField] private SoundScriptableObject soundScriptableObject;
        [SerializeField] private PlayerScriptableObject playerScriptableObject;
        [SerializeField] private List<LevelScriptableObject> levelScriptableObjects;

        // Scene References:
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource bgMusicSource;

        protected override void Awake()
        {
            base.Awake();
            EventService = new EventService();
            SoundService = new SoundService(soundScriptableObject, sfxSource, bgMusicSource);
            LevelService = new LevelService(levelScriptableObjects);
            PlayerService = new PlayerService(playerScriptableObject);
            EnemyService = new EnemyService();
            PontuationService = new PontuationService();
        }

        private void Start() => UIService.ShowLevelSelectionUI(levelScriptableObjects.Count);
    }
}