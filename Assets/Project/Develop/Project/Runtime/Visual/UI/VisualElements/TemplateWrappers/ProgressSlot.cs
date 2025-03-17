using Cysharp.Threading.Tasks;
using Runtime.Data.Progress;
using Runtime.Extensions.LitMotion;
using Runtime.Extensions.System;
using Runtime.Extensions.UnityEngine.UIElements;
using Runtime.Visual.UI.VisualElements.TemplateWrappers.Core;
using LitMotion;
using System;
using UnityEngine.UIElements;

namespace Runtime.Visual.UI.VisualElements.TemplateWrappers
{
	internal sealed class ProgressSlot : TemplateWrapper
	{
		private readonly CompositeMotionHandle _compositeMotionHandle = new CompositeMotionHandle();

		private const Single _motionDuration = 0.2F;
		private Boolean _isSelected;

		private const Ease _ease = Ease.OutCirc;

		internal event Action<ProgressSlot> Clicked;

		private VisualElement ScreenshotGlowVisualElement { get; }

		private VisualElement ScreenshotVisualElement { get; }

		private TextField NameTextField { get; }

		private Label DateTimeLabel { get; }

		private Button Button { get; }

		internal IPersistentProgress Progress { get; private set; }

		internal Boolean IsSelected
		{
			get => _isSelected;
			set
			{
				if (_isSelected == value)
				{
					return;
				}

				_isSelected = value;

				if (_isSelected)
				{
					//Button.EnableInClassList(ClassName.ButtonAnimation, false);

					NameTextField.SetEnabled(true);

					_compositeMotionHandle.Cancel();
					ScreenshotGlowVisualElement.style.CreateMotion(0.0F, 1.0F, _motionDuration).AddEase(_ease).BindToOpacity().AddTo(_compositeMotionHandle);
				}
				else
				{
					//Button.EnableInClassList(ClassName.ButtonAnimation, true);

					NameTextField.SetEnabled(false);

					_compositeMotionHandle.Cancel();
					ScreenshotGlowVisualElement.style.CreateMotion(1.0F, 0.0F, _motionDuration).AddEase(_ease).BindToOpacity().AddTo(_compositeMotionHandle);
				}
			}
		}

		internal ProgressSlot(TemplateContainer templateContainer) : base(templateContainer)
		{
			ScreenshotGlowVisualElement = templateContainer.Q<VisualElement>(nameof(ScreenshotGlowVisualElement));
			ScreenshotVisualElement = templateContainer.Q<VisualElement>(nameof(ScreenshotVisualElement));
			NameTextField = templateContainer.Q<TextField>(nameof(NameTextField));
			DateTimeLabel = templateContainer.Q<Label>(nameof(DateTimeLabel));
			Button = templateContainer.Q<Button>(nameof(Button));

			NameTextField.SetEnabled(false);

			Subscribe();
		}

		internal void Initialize(IPersistentProgress progress)
		{
			Progress = progress;
		}

		internal async void UpdateView()
		{
			DateTimeLabel.text = $"{Progress.SavedDateTime:d} {Progress.SavedDateTime:T}";
			ScreenshotVisualElement.style.backgroundImage = new StyleBackground(Progress.ScreenshotTexture2D);
			NameTextField.value = Progress.Name;

			TemplateContainer.style.height = 135.0F;
			TemplateContainer.style.width = 645.0F;

			TemplateContainer.style.display = DisplayStyle.None;

			await UniTask.NextFrame();

			TemplateContainer.style.display = DisplayStyle.Flex;
		}

		internal void Delete()
		{
			TemplateContainer.style.CreateMotion(1.0F, 0.0F, _motionDuration).InvokeAfterCompletion(() => TemplateContainer.parent.Remove(TemplateContainer)).BindToOpacity();

			Unsubscribe();
		}

		internal void Rename()
		{
			NameTextField.Focus();
		}

		private void HandleButtonClick()
		{
			Clicked?.Invoke(this);
		}

		private void Subscribe()
		{
			NameTextField.RegisterCallback<ChangeEvent<String>>(UpdateProgressName);

			Button.clicked += HandleButtonClick;
		}

		private void Unsubscribe()
		{
			NameTextField.UnregisterCallback<ChangeEvent<String>>(UpdateProgressName);

			Button.clicked -= HandleButtonClick;
		}

		private void UpdateProgressName(ChangeEvent<String> changeEvent)
		{
			Progress.Name = changeEvent.newValue;
		}
	}
}