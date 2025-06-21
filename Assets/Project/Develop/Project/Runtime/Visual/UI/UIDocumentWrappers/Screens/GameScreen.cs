using FlyingBears.Runtime.Infrastructure.Factories.PrefabsAssets;
using FlyingBears.Runtime.Infrastructure.Factories.PrefabsAssets.Core;
using Runtime.Data.Configs;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.Extensions.System;
using Runtime.Infrastructure.Factories.Core;
using Runtime.Infrastructure.Services.Game.Core;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using Screen = Runtime.Visual.UI.UIDocumentWrappers.Screens.Core.Screen;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens
{
	internal sealed class GameScreen : Screen
	{
		private ILevelInfoService _levelInfoService;
		private IBoardService _boardService;
		private IUIConfig _uIConfig;

		private Label ScoreLabel { get; }
		private Label QuantityLabel { get; }
		private Label MoveLimitLabel { get; }
		private Label TimeLimitLabel { get; }
		private VisualElement TargetIcon { get; }
		private VisualElement MoveContainer { get; }
		private VisualElement TimeContainer { get; }
		private VisualElement ScoreContainer { get; }
		private VisualElement TargetContainer { get; }

		internal GameScreen(UIDocument uiDocument) : base(uiDocument)
		{
			ScoreLabel = RootVisualElement.Q<Label>(nameof(ScoreLabel));
			QuantityLabel = RootVisualElement.Q<Label>(nameof(QuantityLabel));
			MoveLimitLabel = RootVisualElement.Q<Label>(nameof(MoveLimitLabel));
			TimeLimitLabel = RootVisualElement.Q<Label>(nameof(TimeLimitLabel));
			TargetIcon = RootVisualElement.Q<VisualElement>(nameof(TargetIcon));
			MoveContainer = RootVisualElement.Q<VisualElement>(nameof(MoveContainer));
			TimeContainer = RootVisualElement.Q<VisualElement>(nameof(TimeContainer));
			ScoreContainer = RootVisualElement.Q<VisualElement>(nameof(ScoreContainer));
			TargetContainer = RootVisualElement.Q<VisualElement>(nameof(TargetContainer));

		}


		[Inject]
		internal void Construct(ILevelInfoService levelInfoService, IBoardService boardService, IUIConfig uIConfig)
		{
			_levelInfoService = levelInfoService;
			_boardService = boardService;
			_uIConfig = uIConfig;

			_levelInfoService.InvokeAfterInitialization(InitLevelSettingsGameScreen);
		}

		private void InitLevelSettingsGameScreen()
		{
			if (_levelInfoService.LevelInfo.TargetType == Data.Constants.Enums.TargetType.ScorePoints)
			{
				ScoreContainer.style.display = DisplayStyle.Flex;
				TargetContainer.style.display = DisplayStyle.None;
			}
			else
			{
				ScoreContainer.style.display = DisplayStyle.None;
				TargetContainer.style.display = DisplayStyle.Flex;

				UpdateTargetData();

				_levelInfoService.GoalQuantityChanged += UpdateQuantityLabel;
			}

			if (_levelInfoService.LevelInfo.MoveLimit == 0)
			{
				MoveContainer.style.display = DisplayStyle.None;
			}
			else
			{
				MoveContainer.style.display = DisplayStyle.Flex;
				UpdateMoveLimitLabel();

				_levelInfoService.MoveCompleted += UpdateMoveLimitLabel;
			}

			if (_levelInfoService.LevelInfo.TimeLimit == 0)
			{
				TimeContainer.style.display = DisplayStyle.None;
			}
			else
			{
				TimeContainer.style.display = DisplayStyle.Flex;
				UpdateTimeLimit();

				_levelInfoService.TimeChanged += UpdateTimeLimit;
			}

		}

		private void UpdateTimeLimit()
		{
			TimeLimitLabel.text = _levelInfoService.LevelInfo.TimeLimit.ToString();
		}

		private void UpdateTargetData()
		{
			TargetIcon.style.backgroundImage = new StyleBackground(_uIConfig.TargetTextures[_levelInfoService.LevelInfo.TargetType]);

			UpdateQuantityLabel();
		}
		private void UpdateQuantityLabel()
		{
			QuantityLabel.text = _levelInfoService.LevelInfo.GoalQuantity.ToString();
		}

		private void UpdateMoveLimitLabel()
		{
			MoveLimitLabel.text = _levelInfoService.LevelInfo.MoveLimit.ToString();
		}

		protected override void Unsubscribe()
		{
			base.Unsubscribe();

			_levelInfoService.TimeChanged -= UpdateTimeLimit;
			_levelInfoService.MoveCompleted -= UpdateMoveLimitLabel;
			_levelInfoService.GoalQuantityChanged -= UpdateQuantityLabel;
		}

	}
}