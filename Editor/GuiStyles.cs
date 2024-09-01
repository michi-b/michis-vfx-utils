using UnityEngine;

namespace MichisVfxUtils.Editor
{
    public static class GuiStyles
    {
        public static readonly GUIStyle RightAlignedLabel = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleRight
        };
    }
}