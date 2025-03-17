using Cysharp.Threading.Tasks;
using Runtime.Data.Progress;
using System;
using System.Collections.Generic;

namespace Runtime.Infrastructure.Services.SaveProgressServices.Core
{
	internal interface IPersistentProgressService
	{
		internal event Action<IPersistentProgress> ProgressCreated;

		internal event Action ProgressSavingStarted;

		internal event Action ActiveProgressChanged;

		internal IReadOnlyDictionary<String, IPersistentProgress> ProgressSlots { get; }

		internal IReadOnlyProgress ReadOnlyProgress { get; }

		internal IUserInfo UserInfo { get; }

		internal Boolean IsAllowSaveProgress { get; set; }

		internal IPersistentProgress ActiveProgress { get; set; }

		internal UniTask<IPersistentProgress> CreateProgressSlot(String name, IPersistentProgress persistentProgress = null);

		internal UniTask LoadingSavedProgress(String name, IPersistentProgress persistentProgress = null);

		internal UniTask LoadingActiveProgress();

		internal void DeleteProgressSlot(IPersistentProgress persistentProgress);

		internal void DeleteProgressSlot(String slotID);

		internal UniTask SaveActiveProgressAsync();

		internal void SaveActiveProgress();
	}
}
