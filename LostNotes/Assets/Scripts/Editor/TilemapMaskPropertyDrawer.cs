using System;
using System.Reflection;
using LostNotes;
using UnityEditor;
using UnityEngine;

namespace Editor {
	[CustomPropertyDrawer(typeof(TilemapMask))]
	public class TilemapMaskPropertyDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			var target          = property.serializedObject.targetObject;
			var targetClassType = target.GetType();
			var field           = targetClassType.GetField(property.propertyPath, BindingFlags.NonPublic | BindingFlags.Instance);
			if (field == null)
				return;

			var data = field.GetValue(target) as TilemapMask;
			if (data == null)
				return;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(label);

			EditorGUILayout.BeginVertical();

			var newSize = EditorGUILayout.Vector2IntField("Size", data.Size);
			newSize.x = Math.Clamp(newSize.x, 1, 32);
			newSize.y = Math.Clamp(newSize.y, 1, 32);
			if (newSize != data.Size) {
				Undo.RecordObject(target, "Tilemap mask resized");
				data.Resize(newSize);
			}

			var style = new GUIStyle();
			style.fixedWidth  = 16;
			style.fixedHeight = 16;

			for (var y = 0; y < data.Size.y; y++) {
				EditorGUILayout.BeginHorizontal(style);
				for (var x = 0; x < data.Size.x; x++) {
					if (x == data.Size.x / 2 && y == data.Size.y / 2) {
						EditorGUILayout.LabelField("\u27a1", GUILayout.ExpandWidth(false), GUILayout.Width(16));
						continue;
					}

					var p        = new Vector2Int(x, y);
					var oldValue = data.IsSet(p);
					var newValue = EditorGUILayout.Toggle(oldValue, GUILayout.Width(16));
					if (oldValue != newValue) {
						Undo.RecordObject(target, "Tilemap mask modified");
						data.Set(p, newValue);
					}
				}

				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}
	}
}
