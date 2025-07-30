using Michis.VfxUtils.CloneCache.Abstract;
using Michis.VfxUtils.CloneCache.Interfaces;
using Michis.VfxUtils.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace Michis.VfxUtils.Editor.CloneCache
{
    [CustomPropertyDrawer(typeof(CloneCashable), true)]
    public class GameObjectCachePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect instantiateButtonRect = position.TakeFromRight(20f);
            EditorGUI.PropertyField(position, property, label);
            using (new EditorGUI.DisabledGroupScope(!Application.isPlaying
                                                    || property.objectReferenceValue is not CloneCashable { }
                                                    || property.hasMultipleDifferentValues))
            {
                if (GUI.Button(instantiateButtonRect, "I"))
                {
                    var prefab = (CloneCashable)property.objectReferenceValue;
                    foreach (Object o in property.serializedObject.targetObjects)
                    {
                        if (o is not Behaviour behaviour)
                        {
                            continue;
                        }

                        var instance = (CloneCashable)((ICloneCashable)prefab).BorrowInstance();
                        instance.transform.SetParent(behaviour.transform, false);
                        instance.Return();
                    }
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}