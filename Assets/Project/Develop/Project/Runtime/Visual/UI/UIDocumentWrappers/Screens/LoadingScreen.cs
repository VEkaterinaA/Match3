using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.Services.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Screens.Core;
using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;
using VContainer;
using Screen = Runtime.Visual.UI.UIDocumentWrappers.Screens.Core.Screen;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens
{
	internal sealed class LoadingScreen : Screen
	{
		private ILoopsService _loopsService;

		private AsyncOperationHandle _asyncOperationHandle;

		private Action _completionAction;

		private Boolean _isConstructed;

		private ProgressBar LoadingProgressBar { get; }

		internal LoadingScreen(UIDocument uiDocument) : base(uiDocument)
		{
			LoadingProgressBar = RootVisualElement.Q<ProgressBar>(nameof(LoadingProgressBar));

			IsShowed = true;
		}

		[Inject]
		internal void Construct(ILoopsService loopsService)
		{
			_loopsService = loopsService;

			_isConstructed = true;
		}

		internal async void VisualizeAsyncOperation(AsyncOperationHandle asyncOperation, Action completionAction)
		{
			_completionAction = completionAction;
			_asyncOperationHandle = asyncOperation;

			await UpdateProgressBar();

			_completionAction?.Invoke();
		}

		protected override void Initialize()
		{
			((IScreen) this).ShowInstantly();
		}

		protected override async void Subscribe()
		{
			await UniTask.WaitUntil(() => _isConstructed);

			base.Subscribe();
		}

		protected override void Unsubscribe()
		{
			base.Unsubscribe();
		}

		private async UniTask UpdateProgressBar()
		{
			while (!_asyncOperationHandle.IsDone)
			{
				UpdateProgressBar(_asyncOperationHandle.PercentComplete);
				await UniTask.Yield();
			}
		}
		private void UpdateProgressBar(Single progress)
		{
			LoadingProgressBar.style.width = new Length(progress * 100, LengthUnit.Percent);
		}
	}
}