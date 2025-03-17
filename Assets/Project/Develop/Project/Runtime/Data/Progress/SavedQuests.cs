using Runtime.Infrastructure.Core;
using System;

namespace Runtime.Data.Progress
{
	[Serializable]
	public sealed class SavedQuests : IPrototype<SavedQuests>
	{/*
		[SerializeField]
		private List<QuestID> _completedQuestsIDs;
		[SerializeField]
		private List<MiniQuestID> _completedMiniQuestIds;
		[SerializeField] [TextArea(10, 50)]
		private String _questPayload;

		[SerializeField]
		private QuestID _activeQuestID;

		internal List<QuestID> CompletedQuestsIDs => _completedQuestsIDs;
		
		internal List<MiniQuestID> CompletedMiniQuestIds => _completedMiniQuestIds;

		internal event Action QuestChangeStarted;

		internal event Action QuestChangeCompleted;

		internal String QuestPayload
		{
			get => _questPayload;
			set => _questPayload = value;
		}

		internal QuestID ActiveQuestID
		{
			get => _activeQuestID;
			set
			{
				QuestChangeStarted?.Invoke();

				_activeQuestID = value;

				QuestChangeCompleted?.Invoke();
			}
		}*/

		SavedQuests IPrototype<SavedQuests>.Clone()
		{
			var savedQuests = new SavedQuests();
			/*
						savedQuests._completedMiniQuestIds = new List<MiniQuestID>(_completedMiniQuestIds);
						savedQuests._completedQuestsIDs = new List<QuestID>(_completedQuestsIDs);
						savedQuests._activeQuestID = _activeQuestID;
						savedQuests._questPayload = _questPayload;*/

			return savedQuests;
		}
	}
}