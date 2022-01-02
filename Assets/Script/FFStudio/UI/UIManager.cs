/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes;

namespace FFStudio
{
    public class UIManager : MonoBehaviour
    {
#region Fields
        [ Header( "Event Listeners" ) ]
        public EventListenerDelegateResponse levelLoadedResponse;
        public EventListenerDelegateResponse levelCompleteResponse;
        public EventListenerDelegateResponse levelFailResponse;
        public EventListenerDelegateResponse tapInputListener;
		public EventListenerDelegateResponse ui_particle_listener;

		[ Header( "UI Elements" ) ]
        public UILoadingBar levelLoadingBar;
		public UIText levelLoadingText;
		public UILoadingBar levelProgressBar;
        public UIText levelCountText;
        public UIText informationText;
        public Image loadingScreenImage;
        public Image foreGroundImage;
        public RectTransform tutorialObjects;

        [ Header( "Fired Events" ) ]
        public GameEvent levelRevealedEvent;
        public GameEvent loadNewLevelEvent;
        public GameEvent resetLevelEvent;
        public ElephantLevelEvent elephantLevelEvent;

        [ Header( "Setup" ) ]
        public UIParticle_Pool ui_particle_pool;
        public RectTransform ui_dynamic_canvas;
#endregion

#region Unity API
        private void OnEnable()
        {
            levelLoadedResponse.OnEnable();
            levelFailResponse.OnEnable();
            levelCompleteResponse.OnEnable();
            tapInputListener.OnEnable();
			ui_particle_listener.OnEnable();
		}

        private void OnDisable()
        {
            levelLoadedResponse.OnDisable();
            levelFailResponse.OnDisable();
            levelCompleteResponse.OnDisable();
            tapInputListener.OnDisable();
			ui_particle_listener.OnDisable();
        }

        private void Awake()
        {
            levelLoadedResponse.response   = LevelLoadedResponse;
            levelFailResponse.response     = LevelFailResponse;
            levelCompleteResponse.response = LevelCompleteResponse;
            tapInputListener.response      = ExtensionMethods.EmptyMethod;

			ui_particle_listener.response = UIParticleResponse;

			ui_particle_pool.InitPool( ui_dynamic_canvas );

			informationText.textRenderer.text = "Tap to Start";
        }
#endregion

#region Implementation
        private void LevelLoadedResponse()
        {
			var sequence = DOTween.Sequence()
								.Append( levelLoadingBar.Disappear() )
								.Append( levelLoadingText.Disappear() )
								.Append( loadingScreenImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
								.AppendCallback( () => tapInputListener.response = StartLevel );

			levelCountText.textRenderer.text = "Level " + CurrentLevelData.Instance.currentLevel_Shown;

            levelLoadedResponse.response = NewLevelLoaded;
        }

        [ Button ]
        private void LevelCompleteResponse()
        {
            var sequence = DOTween.Sequence();

            Tween tween = null;

            informationText.textRenderer.text = "Completed \n\n Tap to Continue";

			sequence.Append( foreGroundImage.DOFade( 0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
                    // .Append( tween ) // TODO: UIElements tween.
					.Append( informationText.Appear() )
					.AppendCallback( () => tapInputListener.response = LoadNewLevel );

            elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelCompleted;
            elephantLevelEvent.Raise();
        }

        [ Button ]
        private void LevelFailResponse()
        {
            var sequence = DOTween.Sequence();

            Tween tween = null;

            informationText.textRenderer.text = "Level Failed \n\n Tap to Continue";

			sequence.Append( foreGroundImage.DOFade( 0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
                    // .Append( tween ) // TODO: UIElements tween.
					.Append( informationText.Appear() )
					.AppendCallback( () => tapInputListener.response = Resetlevel );

            elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelFailed;
            elephantLevelEvent.Raise();
        }

        private void UIParticleResponse()
        {
			var particle_event = ui_particle_listener.gameEvent as UIParticle_Event;

            var ui_particle = ui_particle_pool.GetEntity();
			ui_particle.imageRenderer.sprite = particle_event.sprite;

			ui_particle.Spawn( particle_event.position_start, particle_event.position_end );
		}

        private void NewLevelLoaded()
        {
			levelCountText.textRenderer.text = "Level " + CurrentLevelData.Instance.currentLevel_Shown;

			var sequence = DOTween.Sequence();

			Tween tween = null;

			sequence.Append( foreGroundImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					// .Append( tween ) // TODO: UIElements tween.
					.AppendCallback( levelRevealedEvent.Raise );

            elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
            elephantLevelEvent.Raise();
        }

		private void StartLevel()
		{
			foreGroundImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration );
			informationText.Disappear().OnComplete( levelRevealedEvent.Raise );
			tutorialObjects.gameObject.SetActive( false );

			tapInputListener.response = ExtensionMethods.EmptyMethod;

			elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
			elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
			elephantLevelEvent.Raise();
		}

		private void LoadNewLevel()
		{
			FFLogger.Log( "Load New Level" );
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			var sequence = DOTween.Sequence();

			sequence.Append( foreGroundImage.DOFade( 1f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
			        .Join( informationText.Disappear() )
			        .AppendCallback( loadNewLevelEvent.Raise );
		}

		private void Resetlevel()
		{
			FFLogger.Log( "Reset Level" );
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			var sequence = DOTween.Sequence();

			sequence.Append( foreGroundImage.DOFade( 1f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
			        .Join( informationText.Disappear() )
			        .AppendCallback( resetLevelEvent.Raise );

			elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
			elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
			elephantLevelEvent.Raise();
		}
#endregion
    }
}