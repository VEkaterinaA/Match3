using Runtime.Infrastructure.Services.Input.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Screens.Core;
using Runtime.Visual.UI.VisualElements.TemplateWrappers.Joysticks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens
{
	internal sealed class ConsoleScreen : ScreenBehaviour
	{
		private IInputService _inputService;

		[SerializeField]
		private Button _closeButton;

		[Inject]
		internal void Construct(IInputService inputService)
		{
			_inputService = inputService;
		}

		protected override void Subscribe()
		{
			base.Subscribe();

			_inputService.IsEnabled = false;
			_closeButton.onClick.AddListener(ScreensService.Hide<ConsoleScreen>);
		}

		protected override void Unsubscribe()
		{
			base.Unsubscribe();

			_inputService.IsEnabled = true;
			_closeButton.onClick.RemoveListener(ScreensService.Hide<ConsoleScreen>);
		}
	}
}