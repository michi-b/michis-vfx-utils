using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Michis.VfxUtils.Editor.Extensions
{
    [PublicAPI]
    public static class RectExtensions
    {
        public static Rect Center(this Rect container, Vector2 size)
        {
            return new Rect(container.center - size * 0.5f, size);
        }

        public static Rect TakeFromLeftRelative(this ref Rect container, float relativeWidth)
        {
            return container.TakeFromLeft(container.width * relativeWidth);
        }

        public static Rect TakeFromRightRelative(this ref Rect container, float relativeWidth)
        {
            return container.TakeFromRight(container.width * relativeWidth);
        }

        public static Rect TakeFromLeft(this ref Rect container, float width)
        {
            var taken = new Rect(container.x, container.y, width, container.height);
            container.x += width;
            container.width -= width;
            return taken;
        }

        public static Rect TakeFromRight(this ref Rect container, float width)
        {
            var taken = new Rect(container.xMax - width, container.y, width, container.height);
            container.width -= width;
            return taken;
        }

        public static Rect TakeLineFromTop(this ref Rect container)
        {
            return container.TakeFromTop(EditorGUIUtility.singleLineHeight);
        }

        public static Rect TakeLineFromBottom(this ref Rect container)
        {
            return container.TakeFromBottom(EditorGUIUtility.singleLineHeight);
        }

        public static Rect TakeFromTop(this ref Rect container, float height)
        {
            var taken = new Rect(container.x, container.y, container.width, height);
            container.y += height;
            container.height -= height;
            return taken;
        }

        public static Rect TakeFromBottom(this ref Rect container, float height)
        {
            var taken = new Rect(container.x, container.yMax - height, container.width, height);
            container.height -= height;
            return taken;
        }
    }
}