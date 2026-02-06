using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FormForge.AddressableConfiguration.Editor.Schema.GroupRules
{
	[CustomPropertyDrawer(typeof(AddressableSettingsGroupRule))]
	public class AddressableSettingsGroupRulePropertyDrawer : PropertyDrawer
	{
		private const int LINES = 3;
		
		private HashSet<SerializedProperty> m_targetProperties = new HashSet<SerializedProperty>();
		private SerializedProperty m_rightClickTargetProperty;
		private bool m_initialized;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			bool isExpanded = property.isExpanded;
			DrawFoldout(position, label, ref isExpanded, out Rect expandedContentRect);
			property.isExpanded = isExpanded;

			if (isExpanded)
			{
				DrawExpandedContents(expandedContentRect, property);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (!property.isExpanded)
			{
				return GetFoldoutHeight(withSpacing: false);
			}

			return GetFoldoutHeight(withSpacing: true) + GetStandardInspectorRowHeight(LINES);
		}
		
		private void DrawExpandedContents(Rect position, SerializedProperty property)
		{
			if (!m_initialized)
			{
				Initialize();
			}

			m_targetProperties.Add(property);

			DrawChildProperty(ref position, property, AddressableSettingsGroupRule.GROUP_NAME_PROPERTY_NAME);
			DrawFolderReference(ref position, property);
			DrawChildProperty(ref position, property, AddressableSettingsGroupRule.INCLUDE_SUBFOLDERS_PROPERTY_NAME);
		}
		
		private static void DrawChildProperty(ref Rect position, SerializedProperty property, string childPropertyName)
		{
			SerializedProperty childProperty = property.FindPropertyRelative(childPropertyName);
			Rect nextLineRect = GetNextStandardInspectorRow(ref position);
			EditorGUI.PropertyField(nextLineRect, childProperty);
		}

		private void DrawFolderReference(ref Rect position, SerializedProperty property)
		{
			SerializedProperty childProperty = property.FindPropertyRelative(AddressableSettingsGroupRule.FOLDER_REFERENCE_PROPERTY_NAME);
			Rect nextLineRect = GetNextStandardInspectorRow(ref position);

			if (childProperty == null)
			{
				return;
			}

			GUIContent folderReferenceLabel = new GUIContent(ObjectNames.NicifyVariableName(AddressableSettingsGroupRule.FOLDER_REFERENCE_PROPERTY_NAME));

			Object value = EditorGUI.ObjectField(nextLineRect,
				folderReferenceLabel,
				childProperty.objectReferenceValue,
				typeof(DefaultAsset),
				allowSceneObjects: false);

			if (value != childProperty.objectReferenceValue)
			{
				SetFolderReference(property, value);
			}
		}

		private void Initialize()
		{
			ClearSubscribedEvents();
			Selection.selectionChanged += OnSelectionChanged;
			EditorApplication.contextualPropertyMenu += OnPropertyContextMenu;
			m_initialized = true;
		}

		private void SetFolderReference(SerializedProperty property, Object value)
		{
			SerializedProperty folderReferenceProperty = property.FindPropertyRelative(AddressableSettingsGroupRule.FOLDER_REFERENCE_PROPERTY_NAME);

			folderReferenceProperty.objectReferenceValue = value;

			if (value == null)
			{
				property.serializedObject.ApplyModifiedProperties();
				return;
			}

			SetGroupNameToFolderNameIfEmpty(property);
		}

		private void SetGroupNameToFolderNameIfEmpty(SerializedProperty property)
		{
			SerializedProperty groupNameProperty = property.FindPropertyRelative(AddressableSettingsGroupRule.GROUP_NAME_PROPERTY_NAME);

			if (!string.IsNullOrEmpty(groupNameProperty.stringValue))
			{
				property.serializedObject.ApplyModifiedProperties();
				return;
			}

			SerializedProperty folderReferenceProperty = property.FindPropertyRelative(AddressableSettingsGroupRule.FOLDER_REFERENCE_PROPERTY_NAME);
			SetGroupNameToFolderName(property, groupNameProperty, folderReferenceProperty);
		}

		private void SetGroupNameToFolderName()
		{
			if (m_rightClickTargetProperty == null)
			{
				Debug.Log($"Set folder name. No target property!");
				return;
			}

			SetGroupNameToFolderName(m_rightClickTargetProperty);
		}

		private void SetGroupNameToFolderName(SerializedProperty property)
		{
			SerializedProperty groupNameProperty = property.FindPropertyRelative(AddressableSettingsGroupRule.GROUP_NAME_PROPERTY_NAME);
			SerializedProperty folderReferenceProperty = property.FindPropertyRelative(AddressableSettingsGroupRule.FOLDER_REFERENCE_PROPERTY_NAME);

			SetGroupNameToFolderName(property, groupNameProperty, folderReferenceProperty);
		}

		private void SetGroupNameToFolderName(SerializedProperty property, SerializedProperty groupNameProperty, SerializedProperty folderReferenceProperty)
		{
			string folderName = folderReferenceProperty.objectReferenceValue.name;
			groupNameProperty.stringValue = folderName;

			property.serializedObject.ApplyModifiedProperties();
		}

		private void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
		{
			SerializedProperty targetProperty;
			if (!TryMatchChildPropertyByPath(property.propertyPath, out targetProperty))
			{
				return;
			}

			AddSetGroupNameMenuItem(menu, property, targetProperty);
		}

		private void AddSetGroupNameMenuItem(GenericMenu menu, SerializedProperty property, SerializedProperty targetProperty)
		{
			SerializedProperty groupNameProperty = targetProperty.FindPropertyRelative(AddressableSettingsGroupRule.GROUP_NAME_PROPERTY_NAME);
			SerializedProperty folderReferenceProperty = targetProperty.FindPropertyRelative(AddressableSettingsGroupRule.FOLDER_REFERENCE_PROPERTY_NAME);

			bool matchGroupName = property.propertyPath == groupNameProperty.propertyPath;
			bool matchFolderReference = property.propertyPath == folderReferenceProperty.propertyPath;

			if (!matchGroupName && !matchFolderReference)
			{
				return;
			}

			m_rightClickTargetProperty = targetProperty;
			bool hasFolderTarget = folderReferenceProperty.objectReferenceValue != null;

			GUIContent menuItemLabel = new GUIContent("Set Group Name To Folder Name");

			if (hasFolderTarget)
			{
				menu.AddItem(menuItemLabel, false, SetGroupNameToFolderName);
			}
			else
			{
				menu.AddDisabledItem(menuItemLabel, false);
			}
		}

		private bool TryMatchChildPropertyByPath(string propertyPath, out SerializedProperty property)
		{
			foreach (SerializedProperty targetProperty in m_targetProperties)
			{
				if (propertyPath.StartsWith(targetProperty.propertyPath))
				{
					property = targetProperty;
					return true;
				}
			}

			property = null;
			return false;
		}

		private void OnSelectionChanged()
		{
			ClearSubscribedEvents();
			m_targetProperties.Clear();
			m_rightClickTargetProperty = null;
		}

		private void ClearSubscribedEvents()
		{
			Selection.selectionChanged -= OnSelectionChanged;
			EditorApplication.contextualPropertyMenu -= OnPropertyContextMenu;
		}
		
		private static void DrawFoldout(Rect position, GUIContent foldoutLabel, ref bool foldout, out Rect expandedContentRect, float indent = 0)
		{
			float foldoutHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			expandedContentRect = GetInsetRect(position, insetTop: foldoutHeight, insetLeft: indent);

			position.height = EditorGUIUtility.singleLineHeight;

			foldout = EditorGUI.Foldout(position, foldout, foldoutLabel, toggleOnLabelClick: true);
		}

		private static float GetFoldoutHeight(bool withSpacing = false)
		{
			if (withSpacing)
			{
				return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			}
			return EditorGUIUtility.singleLineHeight;
		}
		
		private static Rect GetNextStandardInspectorRow(ref Rect sourceArea)
		{
			(Rect nextLineRect, Rect remainingSpace) = SplitSingleLineLayoutRect(sourceArea);
			sourceArea = remainingSpace;
			return nextLineRect;
		}
		
		private static float GetStandardInspectorRowHeight(int lines)
		{
			if (lines <= 0)
			{
				return 0;
			}

			float height = EditorGUIUtility.singleLineHeight * lines;
			if (lines > 1)
			{
				height += EditorGUIUtility.standardVerticalSpacing * (lines - 1);
			}

			return height;
		}
		
		private static (Rect top, Rect bottom) SplitSingleLineLayoutRect(Rect source)
		{
			return SplitRectHorizontal(source, EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
		}
		
		private static (Rect top, Rect bottom) SplitRectHorizontal(Rect source, float topCellHeight, float spacing)
		{
			Rect bottom = GetInsetRect(source, insetTop: topCellHeight + spacing);
			source.yMax = source.yMin + topCellHeight;
			return (source, bottom);
		}
		
		private static Rect GetInsetRect(Rect source, float insetTop = 0, float insetBottom = 0, float insetLeft = 0, float insetRight = 0)
		{
			source.yMin += insetTop;
			source.yMax -= insetBottom;
			source.xMin += insetLeft;
			source.xMax -= insetRight;
			return source;
		}
	}
}
