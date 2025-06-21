using LitMotion;
using Runtime.Data.Configs.Core;
using Runtime.Extensions.LitMotion;
using Runtime.Extensions.System;
using Runtime.Extensions.UnityEngine;
using Runtime.Infrastructure.Services.Game.Core;
using Runtime.Infrastructure.Services.Providers;
using Runtime.MonoBehaviours.Game;
using Runtime.MonoBehaviours.Game.Core;
using System;
using UnityEngine;
using VContainer;

namespace Runtime.Infrastructure.Services.Game.Helper
{
	internal class BoardItemAnimation
	{
		private ILevelInfoService _levelInfoService;
		private BoardProvider _boardProvider;
		private IGameConfig _gameConfig;

		private CompositeMotionHandle _сompositeMotionHandle;

		[Inject]
		private void Construct(BoardProvider boardProvider, IGameConfig gameConfig, ILevelInfoService levelInfoService)
		{
			_levelInfoService = levelInfoService;
			_boardProvider = boardProvider;
			_gameConfig = gameConfig;

			_сompositeMotionHandle = new();
		}
		public void MoveTo(IBoardItem stone, Vector2 newPosition, Action action)
		{
			var motion = stone.RectTransform.CreateMotion(
			stone.RectTransform.anchoredPosition,
				newPosition,
				_gameConfig.MoveDuration).WithOnComplete(()=>
				{
					action?.Invoke();
					_сompositeMotionHandle.Clear();
					});

			_сompositeMotionHandle.AddAutoRemove(motion.BindToAnchoredPosition());
		}

		public void MoveToCell(IBoardItem item, Action action = null)
		{
			var offset = _boardProvider.GetBoardOffset();

			var position = new Vector2(
							item.X * _gameConfig.CellSize + offset.x,
							(_levelInfoService.LevelInfo.HeightOfBoard - 1 - item.Y) * _gameConfig.CellSize + offset.y);
			MoveTo(item, position, action);
		}

		public void SwapWith(IBoardItem cellOne, IBoardItem cellTwo, Action action = null)
		{
			if (_сompositeMotionHandle.Count != 0)
			{
				return;
			}

			MoveToCell(cellOne);
			MoveToCell(cellTwo, action);
		}

		public void PlayDestroyAnimation(IBoardItem cell)
		{
			var handle = cell.GameObject.transform
				.CreateMotion((cell as IBoardItem).RectTransform.localScale, Vector3.zero, _gameConfig.DestroyAnimationDuration)
				.AddEase(Ease.InBack)
			.WithOnComplete(() =>
			{
				cell.OnCellDestroyCompleted();
				cell.OnCellDestroyGameObject();
			});

			_сompositeMotionHandle.AddAutoRemove(handle.BindToLocalScale());
		}

		public void PlaySpawnAnimation(IBoardItem stone)
		{
			var delay = UnityEngine.Random.Range(0f, 0.3f);

			var handle = stone.RectTransform
				.CreateMotion(Vector3.zero, Vector3.one, _gameConfig.SpawnAnimationDuration)
				.AddEase(Ease.OutBack)
				.AddDelay(delay);

			_сompositeMotionHandle.AddAutoRemove(handle.BindToLocalScale());
		}

	}
}
