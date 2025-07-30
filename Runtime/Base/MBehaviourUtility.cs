using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Michis.VfxUtils.Base
{
    public static class MBehaviourUtility
    {
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void ProhibitInEditMode(MBehaviour behaviour)
        {
            if (!Application.isPlaying)
            {
                UnityEditor.EditorApplication.delayCall += () => { Object.DestroyImmediate(behaviour); };
                throw new InvalidOperationException("This behaviour may only be created while the application is playing.");
            }
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void AssertIsPlaying()
        {
            Debug.Assert(Application.isPlaying);            
        }
    }
}