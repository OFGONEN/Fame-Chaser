/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace FFStudio
{
    public class LevelManager : MonoBehaviour
    {
#region Fields
        [ Header( "Event Listeners" ) ]
        public EventListenerDelegateResponse levelLoadedListener;
        public EventListenerDelegateResponse levelRevealedListener;
        public EventListenerDelegateResponse levelStartedListener;
		public EventListenerDelegateResponse listener_finish_line;

		[ Header( "Fired Events" ) ]
        public GameEvent levelFailedEvent;
        public GameEvent levelCompleted;

        [ Header( "Level Releated" ) ]
        public SharedFloatNotifier levelProgress;
        public DaddyPool[] daddy_pool_array;
		public SharedIntNotifier player_money_notifier;
		public SharedIntNotifier player_fame_notifier;

		// Private \\
		private Tween daddy_spawn_tween; 
#endregion

#region UnityAPI
        private void OnEnable()
        {
            levelLoadedListener.OnEnable();
            levelRevealedListener.OnEnable();
            levelStartedListener.OnEnable();
			listener_finish_line.OnEnable();

			daddy_spawn_tween = daddy_spawn_tween.KillProper();
		}

        private void OnDisable()
        {
            levelLoadedListener.OnDisable();
            levelRevealedListener.OnDisable();
            levelStartedListener.OnDisable();
			listener_finish_line.OnDisable();
        }

        private void Awake()
        {
            levelLoadedListener.response   = LevelLoadedResponse;
            levelRevealedListener.response = LevelRevealedResponse;
            levelStartedListener.response  = LevelStartedResponse;
			listener_finish_line.response  = LevelFinishedResponse;

			player_fame_notifier.SharedValue = 0;
			player_money_notifier.SharedValue = 0;
		}
#endregion

#region Implementation
        private void LevelLoadedResponse()
        {
			levelProgress.SharedValue = 0;
			player_fame_notifier.SharedValue = 0;
			player_money_notifier.SharedValue = 0;

			var levelData = CurrentLevelData.Instance.levelData;

            // Set Active Scene
			if( levelData.overrideAsActiveScene )
				SceneManager.SetActiveScene( SceneManager.GetSceneAt( 1 ) );
            else
				SceneManager.SetActiveScene( SceneManager.GetSceneAt( 0 ) );
		}

        private void LevelRevealedResponse()
        {

        }

        private void LevelStartedResponse()
        {
			var delay = CurrentLevelData.Instance.levelData.daddy_spawn_rate.GiveRandom();
			daddy_spawn_tween = DOVirtual.DelayedCall( delay, SpawnDaddy );
		}

        private void LevelFinishedResponse()
        {
			daddy_spawn_tween = daddy_spawn_tween.KillProper();
		}

        private void SpawnDaddy()
        {
			var money = GameSettings.Instance.daddy_money_amount.GiveRandom();
			var random = Random.Range( 0, daddy_pool_array.Length );

			var daddy = daddy_pool_array[ random ].GetEntity();
			daddy.Spawn( money );

			var delay = CurrentLevelData.Instance.levelData.daddy_spawn_rate.GiveRandom();
			daddy_spawn_tween = DOVirtual.DelayedCall( delay, SpawnDaddy );
		}
#endregion
    }
}