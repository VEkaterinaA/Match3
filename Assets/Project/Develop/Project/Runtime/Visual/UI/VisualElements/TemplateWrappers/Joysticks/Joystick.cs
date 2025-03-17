using Runtime.Extensions.System;
using Runtime.Extensions.UnityEngine.UIElements;
using Runtime.Visual.UI.UIDocumentWrappers.Screens;
using Runtime.Visual.UI.VisualElements.TemplateWrappers.Core;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using Touch = UnityEngine.Touch;

namespace Runtime.Visual.UI.VisualElements.TemplateWrappers.Joysticks
{
	internal class Joystick : TemplateWrapper, IDisposable
	{
/*		private IInputConfig _inputConfig;

		private Vector2 _axis;

		protected IInputService InputService;
		protected InputScreen InputScreen;

		protected Boolean IsBlockClickCheckViaJoystick;
		protected Boolean PointerIsDown;
		protected Boolean IsDynamic;
		protected Boolean IsVisible;

		internal Touch? CurrentTouch;

		internal event Action Pressed;

		internal event Action Released;

		internal event Action Moved;

		internal event Action JumpJoystickAction;

		internal Boolean IsPointerOverJoystick;

		internal Vector2 Axis
		{
			get => _axis;
			private set
			{
				_axis = value;

				Moved?.Invoke();
			}
		}

		protected Vector2 PanelPointerPosition
		{
			get
			{
				var pointerPosition = InputService.PointerPosition;

				var panelPointerPosition = RuntimePanelUtils.ScreenToPanel(TemplateContainer.panel, pointerPosition);
				panelPointerPosition.y = (Screen.height - pointerPosition.y);

				return panelPointerPosition;
			}
		}

		private VisualElement JoystickOutlineVisualElement { get; }

		private VisualElement JoystickHandleVisualElement { get; }*/

		internal Joystick(TemplateContainer templateContainer, Boolean isVisible, Boolean isDynamic, Boolean isBlockClickCheckViaJoystick) : base(templateContainer)
		{
/*			IsBlockClickCheckViaJoystick = isBlockClickCheckViaJoystick;
			IsDynamic = isDynamic;
			IsVisible = isVisible;

			JoystickOutlineVisualElement = templateContainer.Q<VisualElement>(nameof(JoystickOutlineVisualElement));
			JoystickHandleVisualElement = templateContainer.Q<VisualElement>(nameof(JoystickHandleVisualElement));

			Style.position = Position.Absolute;*/
		}

/*		[Inject]
		internal void Construct(IInputService inputService, IInputConfig inputConfig, IScreensService screensService, IPlatformManager platformManager)
		{
			_platformManager = platformManager;
			InputService = inputService;
			_inputConfig = inputConfig;


			screensService.InvokeAfterInitialization(() =>
			{
				if (!_platformManager.IsMobilePlatform || !IsVisible)
				{
					TemplateContainer.style.visibility = Visibility.Hidden;
				}

				InputScreen = screensService.Get<InputScreen>();
				Subscribe();
			});
		}*/

		void IDisposable.Dispose()
		{
			//Unsubscribe();
		}

		/*internal void SetVisibilityStyle(Visibility visibility)
		{
			if (!_platformManager.IsMobilePlatform)
			{
				TemplateContainer.style.visibility = Visibility.Hidden;
				return;
			}

			if (IsVisible & visibility == Visibility.Visible)
			{
				TemplateContainer.style.visibility = visibility;
			}
			else
			{
				TemplateContainer.style.visibility = Visibility.Hidden;
			}
		}

		internal void SetTouch(Touch touch)
		{
			if (IsDynamic)
			{
				SetPanelPosition(touch.position);
			}

			if (CurrentTouch.HasValue)
			{
				if (CurrentTouch.Value.fingerId == touch.fingerId && touch.phase is TouchPhase.Canceled or TouchPhase.Ended)
				{
					CurrentTouch = touch;
					return;
				}
			}


			if (TemplateContainer.worldBound.Contains(GetPanelPointerPosition(touch.position)))
			{
				CurrentTouch = touch;
			}
		}

		internal virtual void HandlePointerDownPerformance()
		{
			if (!CurrentTouch.HasValue)
				return;

			if (CurrentTouch.Value.phase != TouchPhase.Began)
				return;

			IsPointerOverJoystick = IsBlockClickCheckViaJoystick;

			Pressed?.Invoke();

			if (IsBlockClickCheckViaJoystick)
			{
				if (!CurrentTouch.HasValue)
				{
					return;
				}

				UpdateHandlePosition(GetPanelPointerPosition(CurrentTouch.Value.position));
			}
		}


		internal virtual void HandlePointerMovePerformance()
		{
			if (!CurrentTouch.HasValue)
				return;
			if (CurrentTouch.Value.phase != TouchPhase.Moved)
				return;

			UpdateHandlePosition(GetPanelPointerPosition(CurrentTouch.Value.position));
		}

		internal void PhaseCheckForCancellation()
		{
			if (!CurrentTouch.HasValue)
				return;
			if (CurrentTouch.Value.phase is not (TouchPhase.Ended or TouchPhase.Canceled))
				return;

			CurrentTouch = null;

			HandlePointerDownCancellation();
		}

		protected void HandlePointerDownCancellation()
		{
			var startTranslate = new Vector2(JoystickHandleVisualElement.style.translate.value.x.value, JoystickHandleVisualElement.style.translate.value.y.value);

			JoystickHandleVisualElement.style.CreateMotion(startTranslate, Vector2.zero, 0.1F).BindToTranslate();

			IsPointerOverJoystick = false;
			PointerIsDown = false;

			Axis = Vector2.zero;

			Released?.Invoke();
		}

		protected virtual void Subscribe()
		{

		}

		protected virtual void Unsubscribe()
		{

		}

		protected void EventCallPress()
		{
			Pressed?.Invoke();
		}

		protected void SetPanelPosition(Vector2 pointerPosition)
		{
			var panelPointerPosition = RuntimePanelUtils.ScreenToPanel(TemplateContainer.parent.panel, pointerPosition);
			panelPointerPosition.y = (Screen.height - pointerPosition.y);

			var targetPosition = (panelPointerPosition - TemplateContainer.layout.center);

			Style.translate = new Translate(targetPosition.x, targetPosition.y);
		}

		protected void UpdateHandlePosition(Vector2 panelPointerPosition)
		{
			IsPointerOverJoystick = false;

			var handleTargetLocalPosition = (panelPointerPosition - TemplateContainer.worldBound.center);

			var joystickOutlineRadius = (JoystickOutlineVisualElement.worldBound.width * 0.5f);

			var handleClampedLocalPosition = Vector2.ClampMagnitude(handleTargetLocalPosition, joystickOutlineRadius);

			JoystickHandleVisualElement.style.translate = new Translate(handleClampedLocalPosition.x, handleClampedLocalPosition.y);

			var distanceToOutline = (handleTargetLocalPosition.magnitude - joystickOutlineRadius);

			if (IsDynamic && (distanceToOutline > 0.0F))
			{
				var delta = Vector2.ClampMagnitude(handleClampedLocalPosition, distanceToOutline);

				var translateX = (Style.translate.value.x.value + delta.x);
				var translateY = (Style.translate.value.y.value + delta.y);

				Style.translate = new Translate(translateX, translateY);
			}

			handleClampedLocalPosition /= joystickOutlineRadius;

			if ((handleClampedLocalPosition.x > 0.0F) && (handleClampedLocalPosition.x < _inputConfig.MinJoystickInputX))
			{
				handleClampedLocalPosition.x = _inputConfig.MinJoystickInputX;
			}
			else if ((handleClampedLocalPosition.x < 0.0F) && (handleClampedLocalPosition.x > -_inputConfig.MinJoystickInputX))
			{
				handleClampedLocalPosition.x = -_inputConfig.MinJoystickInputX;
			}

			Axis = new Vector2(handleClampedLocalPosition.x, -handleClampedLocalPosition.y);

			if (Axis.y > 0.8F)
			{
				JumpJoystickAction?.Invoke();
			}
		}

		private Vector2 GetPanelPointerPosition(Vector2 pointerPosition)
		{
			var panelPointerPosition = RuntimePanelUtils.ScreenToPanel(TemplateContainer.panel, pointerPosition);
			panelPointerPosition.y = (Screen.height - pointerPosition.y);

			return panelPointerPosition;
		}*/
	}
}