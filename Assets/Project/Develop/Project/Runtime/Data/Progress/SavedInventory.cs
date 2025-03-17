using AYellowpaper.SerializedCollections;
using Runtime.Data.Constants.Enums;
using Runtime.Infrastructure.Core;
using System;
using UnityEngine;

namespace Runtime.Data.Progress
{
	[Serializable]
	internal sealed class SavedInventory : IPrototype<SavedInventory>
	{
		[SerializeField]
		private SerializedDictionary<ItemID, Int32> _inventoryItemsQuantity;
		/// <summary>
		///  ItemID - item id
		///  Int32 - quantity of items added
		/// </summary>
		internal event Action<ItemID, Int32> ItemAdded;
		/// <summary>
		/// ItemID - item id
		/// Int32 - quantity of items removed
		/// Int32 - quantity of items remaining
		/// </summary>
		internal event Action<ItemID, Int32, Int32> ItemRemoved;

		internal void AddItem(ItemID itemID, Int32 quantity)
		{
			if (!_inventoryItemsQuantity.TryAdd(itemID, quantity))
			{
				_inventoryItemsQuantity[itemID] += quantity;
			}

			ItemAdded?.Invoke(itemID, quantity);
		}

		internal void RemoveItem(ItemID itemID, Int32 quantity)
		{
			_inventoryItemsQuantity[itemID] -= quantity;

			ItemRemoved?.Invoke(itemID, quantity, _inventoryItemsQuantity[itemID]);
		}

		internal Boolean Contains(ItemID itemID, Int32 quantity)
		{
			if (_inventoryItemsQuantity.TryGetValue(itemID, out var itemsQuantity))
			{
				return (itemsQuantity >= quantity);
			}

			return false;
		}

		internal Boolean StrictContains(ItemID itemID, Int32 quantity)
		{
			if (_inventoryItemsQuantity.TryGetValue(itemID, out var itemsQuantity))
			{
				return (itemsQuantity == quantity);
			}

			return false;
		}

		internal Boolean TryRemove(ItemID itemID, Int32 quantityToRemove)
		{
			if (_inventoryItemsQuantity.TryGetValue(itemID, out var itemsQuantity) && (itemsQuantity >= quantityToRemove))
			{
				RemoveItem(itemID, quantityToRemove);

				return true;
			}

			return false;
		}

		SavedInventory IPrototype<SavedInventory>.Clone()
		{
			var savedInventory = new SavedInventory();

			if (savedInventory._inventoryItemsQuantity is null)
			{
				savedInventory._inventoryItemsQuantity = new SerializedDictionary<ItemID, Int32>();
			}
			else
			{
				savedInventory._inventoryItemsQuantity = new SerializedDictionary<ItemID, Int32>(savedInventory._inventoryItemsQuantity);
			}

			return savedInventory;
		}
	}
}