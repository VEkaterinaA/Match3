using Runtime.Visual.UI.UIDocumentWrappers.Popups.Core;
using System;
using UnityEngine.UIElements;

namespace Runtime.Visual.UI.UIDocumentWrappers.Popups
{
	internal class LevelEditorPopup : Popup
	{
		private VisualElement _moveLimitContainer;
		private VisualElement _targetSelectionContainer;
		private VisualElement _timeLimitContainer;

		private Toggle _moveLimitToggle;
		private SliderInt _moveLimitSlider;

		private DropdownField _targetDropdown;
		private IntegerField _targetQuantity;

		private Toggle _timeLimitToggle;
		private SliderInt _timeLimitSlider;

		private Button _playButton;

		public LevelEditorPopup(UIDocument uiDocument, Boolean canOverlapOtherPopups) : base(uiDocument, canOverlapOtherPopups)
		{
			_moveLimitContainer = RootVisualElement.Q<VisualElement>("MoveLimitContainer");
			_moveLimitToggle = _moveLimitContainer.Q<Toggle>("MoveLimitToggle");
			_moveLimitSlider = _moveLimitContainer.Q<SliderInt>("MoveLimitValue");

			_targetSelectionContainer = RootVisualElement.Q<VisualElement>("TargetSelectionContainer");
			_targetDropdown = _targetSelectionContainer.Q<DropdownField>("TargetDropdown");
			_targetQuantity = _targetSelectionContainer.Q<IntegerField>("TargetQuantity");

			_timeLimitContainer = RootVisualElement.Q<VisualElement>("TimeLimitContainer");
			_timeLimitToggle = _timeLimitContainer.Q<Toggle>("TimeLimitToggle");
			_timeLimitSlider = _timeLimitContainer.Q<SliderInt>("TimeLimitValue");

			_playButton = _timeLimitContainer.Q<Button>("PlayButton");

			_moveLimitToggle.RegisterValueChangedCallback(OnMoveLimitToggleChanged);
			_timeLimitToggle.RegisterValueChangedCallback(OnTimeLimitToggleChanged);
			_targetDropdown.RegisterValueChangedCallback(OnTargetDropdownChanged);
			_playButton.clicked += OnPlayBttonClick;

			UpdateMoveLimitVisibility(_moveLimitToggle.value);
			UpdateTimeLimitVisibility(_timeLimitToggle.value);
			UpdateTargetQuantityVisibility(_targetDropdown.index);
		}

		private void OnPlayBttonClick()
		{

		}

		private void OnTargetDropdownChanged(ChangeEvent<String> evt)
		{
			UpdateTargetQuantityVisibility(_targetDropdown.index);
		}

		private void UpdateTargetQuantityVisibility(Int32 selectedIndex)
		{
			if (selectedIndex == 0)
			{
				_targetQuantity.style.display = DisplayStyle.None;
				return;
			}

			_targetQuantity.style.display = DisplayStyle.Flex;
		}

		private void OnMoveLimitToggleChanged(ChangeEvent<bool> evt)
		{
			UpdateMoveLimitVisibility(evt.newValue);
		}

		private void OnTimeLimitToggleChanged(ChangeEvent<bool> evt)
		{
			UpdateTimeLimitVisibility(evt.newValue);
		}

		private void UpdateMoveLimitVisibility(bool isVisible)
		{
			for (int i = 1; i < _moveLimitContainer.childCount; i++)
				_moveLimitContainer[i].style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
		}


		private void UpdateTimeLimitVisibility(bool isVisible)
		{
			for (int i = 1; i < _timeLimitContainer.childCount; i++)
				_timeLimitContainer[i].style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
		}
	}
}
