using Runtime.Visual.UI.UIDocumentWrappers.Screens;
using System;
using UnityEngine.UIElements;

namespace Runtime.Visual.UI.VisualElements.TemplateWrappers.Joysticks
{
	internal sealed class HandJoystick : Joystick
	{
		public HandJoystick(TemplateContainer templateContainer, Boolean isVisible, Boolean isDynamic, Boolean isBlockClickCheckViaJoystick) : base(templateContainer, isVisible, isDynamic, isBlockClickCheckViaJoystick)
		{ }

		/*internal override void HandlePointerDownPerformance()
		{
			if (IsDynamic)
			{
				if (InputScreen.MovementJoystick.IsPointerOverJoystick)
				{
					return;
				}

				SetPanelPosition(InputService.PointerPosition);
			}

			if (TemplateContainer.worldBound.Contains(PanelPointerPosition))
			{
				PointerIsDown = true;

				EventCallPress();

				IsPointerOverJoystick = true;

				UpdateHandlePosition(PanelPointerPosition);

			}
		}

		internal override void HandlePointerMovePerformance()
		{
			if (PointerIsDown)
			{
				UpdateHandlePosition(PanelPointerPosition);
			}
		}

		protected override void Subscribe()
		{
			InputService.PointerDownPerformed += HandlePointerDownPerformance;
			InputService.PointerMovePerformed += HandlePointerMovePerformance;
			InputService.PointerDownCanceled += HandlePointerDownCancellation;
		}

		protected override void Unsubscribe()
		{
			InputService.PointerDownPerformed -= HandlePointerDownPerformance;
			InputService.PointerMovePerformed -= HandlePointerMovePerformance;
			InputService.PointerDownCanceled -= HandlePointerDownCancellation;
		}*/
	}
}
