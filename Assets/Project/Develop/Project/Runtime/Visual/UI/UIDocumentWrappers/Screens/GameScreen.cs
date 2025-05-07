using Runtime.Extensions.System;
using Runtime.Infrastructure.Services.Game.Core;
using System;
using UnityEngine.UIElements;
using VContainer;
using Screen = Runtime.Visual.UI.UIDocumentWrappers.Screens.Core.Screen;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens
{
	internal sealed class GameScreen : Screen
	{
		private ILevelInfoService _levelInfoService;

		private Label TargetLabel { get;}
		private Label QuantityLabel { get; }
		private Label MoveLimitLabel { get; }
		private Label TimeLimitLabel { get; }
		private VisualElement MoveContainer { get; }
		private VisualElement TimeContainer { get; }

		internal GameScreen(UIDocument uiDocument) : base(uiDocument)
		{
			TargetLabel = RootVisualElement.Q<Label>(nameof(TargetLabel));
			QuantityLabel = RootVisualElement.Q<Label>(nameof(QuantityLabel));
			MoveLimitLabel = RootVisualElement.Q<Label>(nameof(MoveLimitLabel));
			TimeLimitLabel = RootVisualElement.Q<Label>(nameof(TimeLimitLabel));
			MoveContainer = RootVisualElement.Q<VisualElement>(nameof(MoveContainer));
			TimeContainer = RootVisualElement.Q<VisualElement>(nameof(TimeContainer));

		}


		[Inject]
		internal void Construct(ILevelInfoService levelInfoService)
		{
			_levelInfoService = levelInfoService;

			_levelInfoService.InvokeAfterInitialization(UpdateSettingsGameScreen);
		}

		private void UpdateSettingsGameScreen()
		{
			TargetLabel.text = _levelInfoService.LevelInfo.TargetType.ToString();

			if(_levelInfoService.LevelInfo.TargetType == Data.Constants.Enums.TargetType.ScorePoints)
			{
				QuantityLabel.style.display = DisplayStyle.None;
			}
			else
			{
				QuantityLabel.style.display = DisplayStyle.Flex;
				QuantityLabel.text = _levelInfoService.LevelInfo.Quantity.ToString();
			}

			if(_levelInfoService.LevelInfo.MoveLimit == 0)
			{
				MoveLimitLabel.style.display = DisplayStyle.None;
			}
			else
			{
				MoveLimitLabel.style.display = DisplayStyle.Flex;
				MoveLimitLabel.text = _levelInfoService.LevelInfo.MoveLimit.ToString();
			}

			if (_levelInfoService.LevelInfo.MoveLimit == 0)
			{
				MoveContainer.style.display = DisplayStyle.None;
			}
			else
			{
				MoveContainer.style.display = DisplayStyle.Flex;
				MoveLimitLabel.text = _levelInfoService.LevelInfo.MoveLimit.ToString();
			}

			if (_levelInfoService.LevelInfo.TimeLimit == 0)
			{
				TimeContainer.style.display = DisplayStyle.None;
			}
			else
			{
				TimeContainer.style.display = DisplayStyle.Flex;
				TimeLimitLabel.text = _levelInfoService.LevelInfo.TimeLimit.ToString();
			}

		}
	}
}