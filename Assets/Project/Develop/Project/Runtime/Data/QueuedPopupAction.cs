using Runtime.Visual.UI.UIDocumentWrappers.Popups.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtime.Data
{
	internal struct QueuedPopupAction
	{
		private readonly Action _showAction;

		internal Boolean CanOverlapOtherPopups => Popup.CanOverlapOtherPopups;

		internal Boolean IsShowed => Popup.IsShowed;

		internal Popup Popup { get; }

		internal QueuedPopupAction(Action showAction, Popup popup)
		{
			_showAction = showAction;
			Popup = popup;
		}

		internal void Invoke()
		{
			_showAction.Invoke();
		}
	}
}
