using System;
using System.Linq;
using MichisMeshMakers.Editor.Containers.Abstract;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor
{
    public class ParticleSystemAllocationFix : AssetPostprocessor
    {
        private static bool _lastSelectionContainedMeshContainers;

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            EditorSceneManager.sceneOpened += OnSceneOpened;
            EditorApplication.delayCall += ResetParticleSystemsMemoryBufferAndForceGarbageCollection;
        }
        
        private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            ResetParticleSystemsMemoryBufferAndForceGarbageCollection();
        }

        [MenuItem(Menu.Tools + "Flush Particle System Allocations")]
        public static void FlushParticleSystemAllocations()
        {
            foreach (ParticleSystem particleSystem in Object.FindObjectsByType<ParticleSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            
            ResetParticleSystemsMemoryBufferAndForceGarbageCollection();
            
            Debug.Log("Flushed all particle system allocations to prevent memory leak warnings.");
            
            SceneView.RepaintAll();
        }

        private static void ResetParticleSystemsMemoryBufferAndForceGarbageCollection()
        {
            ParticleSystem.ResetPreMappedBufferMemory();
            GC.Collect(int.MaxValue, GCCollectionMode.Forced, true);
        }
    }
}