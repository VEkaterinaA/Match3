using Runtime.Data;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.Extensions.System;
using Runtime.Infrastructure.Core;
using Runtime.Infrastructure.Factories.Core;
using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Popups.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Runtime.Infrastructure.Services.UIServices
{
	internal sealed class PopupsService : IPopupsService, IInitializationInformer, IDisposable
	{
		private readonly IUIDocumentsFactory<PopupType> _uiDocumentsFactory;

		private readonly Queue<QueuedPopupAction> _queuedPopupActions = new Queue<QueuedPopupAction>();
		private readonly Dictionary<Type, Popup> _popups = new Dictionary<Type, Popup>();
		private readonly Transform _popupsParentTransform;
		private Popup _lastShowedPopup;

		private Action _initialized;

		private Boolean _isInitialized;

		event Action IInitializationInformer.Initialized
		{
			add => _initialized += value;
			remove => _initialized -= value;
		}

		Boolean IInitializationInformer.IsInitialized => _isInitialized;

		private IPopupsService Service => this;

		[Inject]
		internal PopupsService(Transform popupsParentTransform, IUIDocumentsFactory<PopupType> uiDocumentsFactory)
		{
			_popupsParentTransform = popupsParentTransform;
			_uiDocumentsFactory = uiDocumentsFactory;

			uiDocumentsFactory.InvokeAfterInitialization(Initialize);
		}

		void IPopupsService.Show<TPopup>(Action completionAction)
		{
			var popup = Service.Get<TPopup>();

			ShowOrEnqueue(new QueuedPopupAction(HandleInvoke, popup));

			return;

			void HandleInvoke()
			{
				popup.Show(completionAction);
				_lastShowedPopup = popup;
			}
		}

		void IPopupsService.Show<TPopup>()
		{
			Service.Show<TPopup>(null);
		}

		void IPopupsService.Show(Type type, Action completionAction)
		{
			var popup = Service.Get(type);

			ShowOrEnqueue(new QueuedPopupAction(HandleInvoke, popup));

			return;

			void HandleInvoke()
			{
				popup.Show(completionAction);
				_lastShowedPopup = popup;
			}
		}

		void IPopupsService.Hide<TPopup>(Action completionAction)
		{
			Service.Get<TPopup>().Hide(completionAction);
		}

		void IPopupsService.Hide<TPopup>()
		{
			Service.Hide<TPopup>(null);
		}

		void IPopupsService.Hide(Type type, Action completionAction)
		{
			Service.Get(type).Hide(completionAction);
		}

		TPopup IPopupsService.Get<TPopup>()
		{
			return (_popups[typeof(TPopup)] as TPopup);
		}

		Popup IPopupsService.Get(Type type)
		{
			return _popups[type];
		}

		private void Initialize()
		{
			foreach (var popup in _uiDocumentsFactory.CreateAll(_popupsParentTransform))
			{
				_lastShowedPopup = (Popup) popup;

				_popups.Add(popup.GetType(), _lastShowedPopup);
			}

			Subscribe();

			_isInitialized = true;
			_initialized?.Invoke();
		}

		void IDisposable.Dispose()
		{
			Unsubscribe();
		}

		private void ShowOrEnqueue(QueuedPopupAction queuedPopupAction)
		{
			if (queuedPopupAction.CanOverlapOtherPopups)
			{
				queuedPopupAction.Invoke();
				return;
			}

			if (_lastShowedPopup.IsShowed)
			{
				_queuedPopupActions.Enqueue(queuedPopupAction);
			}
			else
			{
				queuedPopupAction.Invoke();
			}
		}

		private void HandlePopupHide()
		{
			if (_queuedPopupActions.TryDequeue(out var popupToShow))
			{
				ShowOrEnqueue(popupToShow);
			}
		}

		private void Subscribe()
		{
			foreach (var basePopup in _popups.Values)
			{
				basePopup.Hided += HandlePopupHide;
			}
		}

		private void Unsubscribe()
		{
			foreach (var basePopup in _popups.Values)
			{
				basePopup.Hided -= HandlePopupHide;
				//basePopup.Dispose();
			}
		}
	}
}
