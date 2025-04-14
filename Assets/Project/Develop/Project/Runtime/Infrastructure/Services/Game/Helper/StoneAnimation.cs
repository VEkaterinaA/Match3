using LitMotion;
using Runtime.Data.Configs;
using Runtime.Data.Configs.Core;
using Runtime.Infrastructure.Services.Game.Core;
using Runtime.MonoBehaviours.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Runtime.Infrastructure.Services.Game.Helper
{
	internal class StoneAnimation
	{
		private IBoardService _boardService;
		private MatchChecker _matchChecker;
		private IGameConfig _gameConfig;

		private CompositeMotionHandle _сompositeMotionHandle;

		[Inject]
		private void Construct(IBoardService boardService, IGameConfig gameConfig, MatchChecker matchChecker)
		{
			_boardService = boardService;
			_matchChecker = matchChecker;
			_gameConfig = gameConfig;
		}
		public void MoveTo(Vector2 newPosition, Action action)
		{
			var motion = _rectTransform.CreateMotion(
			_rectTransform.anchoredPosition,
				newPosition,
				_moveDuration).WithOnComplete(action);

			_сompositeMotionHandle.AddAutoRemove(motion.BindToAnchoredPosition());
		}

		public void MoveToCell(int newX, int newY, float cellSize, Vector2 offset, Single height, Action action = null)
		{

			var position = new Vector2(
							newX * cellSize + offset.x,
							(height - 1 - newY) * cellSize + offset.y
);
			MoveTo(position, action);
		}

		public void SwapWith(Stone stoneOne, Stone stoneTwo)
		{
			if (_сompositeMotionHandle.Count != 0)
			{
				return;
			}

			_boardService.SwapGemsInBoard(stoneOne, stoneTwo);

			MoveToCell(stoneOne.X, stoneOne.Y, _gameConfig.CellSize, _boardService.GetBoardOffset(), _gameConfig.Height);
			MoveToCell(stoneTwo.X, stoneTwo.Y, _gameConfig.CellSize, _boardService.GetBoardOffset(), _gameConfig.Height, () => TrySwapOrRevert(stoneOne,stoneTwo));
		}

		private void TrySwapOrRevert(Stone stoneOne, Stone stoneTwo)
		{
			if (_matchChecker.IsValidGemPlacement(stoneOne) && _matchChecker.IsValidGemPlacement(stoneTwo))
			{
				_boardService.SwapGemsInBoard(stoneOne, stoneTwo);

				MoveToCell(stoneOne.X, stoneOne.Y, _gameConfig.CellSize, _boardService.GetBoardOffset(), _gameConfig.Height);
				MoveToCell(stoneTwo.X, stoneTwo.Y, _gameConfig.CellSize, _boardService.GetBoardOffset(), _gameConfig.Height, () => _сompositeMotionHandle.Clear());
			}
			else
			{
				_boardService.HandleMatchesAfterSwap();
			}
		}

		public void PlayDestroyAnimation()
		{
			var handle = transform
				.CreateMotion(transform.localScale, Vector3.zero, _destroyAnimationDuration)
				.AddEase(Ease.InBack)
			.WithOnComplete(() =>
			{
					DestroyGem();
					GemDestroyComplete?.Invoke(this);
				});

			_сompositeMotionHandle.AddAutoRemove(handle.BindToLocalScale());

		}

		public void PlaySpawnAnimation(Stone stone)
		{
			transform.localScale = Vector3.zero;
			var handle = transform
				.CreateMotion(Vector3.zero, Vector3.one, _spawnAnimationDuration)
				.AddEase(Ease.OutBack);

			_сompositeMotionHandle.AddAutoRemove(handle.BindToLocalScale());
		}
	}
}
