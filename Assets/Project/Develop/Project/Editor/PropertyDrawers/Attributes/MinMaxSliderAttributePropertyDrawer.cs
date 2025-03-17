using System;
using Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace HiddenLightEditor.PropertyDrawers.Attributes
{
	[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
	internal sealed class MinMaxSliderAttributePropertyDrawer : PropertyDrawer
	{
		private MinMaxSliderAttribute MinMaxSliderAttribute => (attribute as MinMaxSliderAttribute);

		public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			var minMaxAttribute = (MinMaxSliderAttribute)attribute;
			var propertyType = property.propertyType;

			var controlRect = EditorGUI.PrefixLabel(rect, label);

			var rects = SplitRect(controlRect, 3);

			if (propertyType == SerializedPropertyType.Vector2)
			{
				EditorGUI.BeginChangeCheck();

				var vector = property.vector2Value;
				var minValue = vector.x;
				var maxValue = vector.y;

				minValue = EditorGUI.FloatField(rects[0], Single.Parse(minValue.ToString("F2")));
				maxValue = EditorGUI.FloatField(rects[2], Single.Parse(maxValue.ToString("F2")));

				EditorGUI.MinMaxSlider(rects[1], ref minValue, ref maxValue, minMaxAttribute.MinLimit, minMaxAttribute.MaxLimit);

				if (minValue < minMaxAttribute.MinLimit)
				{
					minValue = minMaxAttribute.MinLimit;
				}

				if (maxValue > minMaxAttribute.MaxLimit)
				{
					maxValue = minMaxAttribute.MaxLimit;
				}

				vector = new Vector2(minValue > maxValue ? maxValue : minValue, maxValue);

				if (EditorGUI.EndChangeCheck())
				{
					property.vector2Value = vector;
				}
			}
			else if (propertyType == SerializedPropertyType.Vector2Int)
			{
				EditorGUI.BeginChangeCheck();

				var vector2Int = property.vector2IntValue;
				var minValue = (Single)vector2Int.x;
				var maxValue = (Single)vector2Int.y;

				minValue = EditorGUI.FloatField(rects[0], minValue);
				maxValue = EditorGUI.FloatField(rects[2], maxValue);

				EditorGUI.MinMaxSlider(rects[1], ref minValue, ref maxValue, minMaxAttribute.MinLimit, minMaxAttribute.MaxLimit);

				if (minValue < minMaxAttribute.MinLimit)
				{
					maxValue = minMaxAttribute.MinLimit;
				}

				if (minValue > minMaxAttribute.MaxLimit)
				{
					maxValue = minMaxAttribute.MaxLimit;
				}

				vector2Int = new Vector2Int(Mathf.FloorToInt((minValue > maxValue) ? maxValue : minValue), Mathf.FloorToInt(maxValue));

				if (EditorGUI.EndChangeCheck())
				{
					property.vector2IntValue = vector2Int;
				}
			}
		}

		private Rect[] SplitRect(Rect rectToSplit, Int32 rectsNumber)
		{
			var rects = new Rect[rectsNumber];

			for (var rectIndex = 0; rectIndex < rectsNumber; rectIndex++)
			{
				rects[rectIndex] = new Rect(rectToSplit.position.x + (rectIndex * rectToSplit.width / rectsNumber), rectToSplit.position.y, (rectToSplit.width / rectsNumber), rectToSplit.height);
			}

			var padding = (rects[0].width - 55.0f);
			var space = 5.0f;

			rects[0].width -= padding + space;
			rects[2].width -= padding + space;

			rects[1].x -= padding;
			rects[1].width += padding * 2;

			rects[2].x += padding + space;

			return rects;
		}
	}
}