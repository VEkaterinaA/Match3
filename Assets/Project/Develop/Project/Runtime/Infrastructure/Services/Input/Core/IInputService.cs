using System;
using UnityEngine;

namespace Runtime.Infrastructure.Services.Input.Core
{
	public interface IInputService
	{
		internal Vector2 PointerPosition { get; }

		internal Boolean IsEnabled { get; set; }

		internal event Action Select;
		internal event Action Swipe;
		internal event Action Drag;
	}
}