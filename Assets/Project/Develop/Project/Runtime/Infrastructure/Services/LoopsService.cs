using System;
using Runtime.Infrastructure.Services.Core;
using UnityEngine;

namespace Runtime.Infrastructure.Services
{
	internal sealed class LoopsService : MonoBehaviour, ILoopsService
	{
		private Action _lateUpdated;
		private Action _fixedUpdate;
		private Action _updated;

		event Action ILoopsService.FixedUpdated
		{
			add => _fixedUpdate += value;
			remove => _fixedUpdate -= value;
		}

		event Action ILoopsService.LateUpdated
		{
			add => _lateUpdated += value;
			remove => _lateUpdated -= value;
		}

		event Action ILoopsService.Updated
		{
			add => _updated += value;
			remove => _updated -= value;
		}

		private void FixedUpdate()
		{
			_fixedUpdate?.Invoke();
		}

		private void LateUpdate()
		{
			_lateUpdated?.Invoke();
		}

		private void Update()
		{
			_updated?.Invoke();
		}
	}
}