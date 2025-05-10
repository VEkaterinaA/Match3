using Runtime.Extensions.System;
using Runtime.Infrastructure.Services.Game.Core;
using UnityEngine.UIElements;
using VContainer;
using Screen = Runtime.Visual.UI.UIDocumentWrappers.Screens.Core.Screen;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens
{
	internal sealed class GameScreen : Screen
	{
		private ILevelInfoService _levelInfoService;
		private IBoardService _boardService;

		private Label TargetLabel { get; }
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
		internal void Construct(ILevelInfoService levelInfoService, IBoardService boardService)
		{
			_levelInfoService = levelInfoService;
			_boardService = boardService;

			_levelInfoService.InvokeAfterInitialization(InitSettingsGameScreen);
		}

		private void InitSettingsGameScreen()
		{
			TargetLabel.text = _levelInfoService.LevelInfo.TargetType.ToString();

			if (_levelInfoService.LevelInfo.TargetType == Data.Constants.Enums.TargetType.ScorePoints)
			{
				QuantityLabel.style.display = DisplayStyle.None;
			}
			else
			{
				QuantityLabel.style.display = DisplayStyle.Flex;
				UpdateQuantityLabel();

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