using UnityEngine;

namespace CGTK.Utils.Preload
{
    internal static class Preloader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeOnLoad()
        {
            foreach (GameObject _prefab in PreloadSettings.Prefabs)
            {
                if(_prefab == null) continue;
                
                GameObject _instance = Object.Instantiate(_prefab);
                Object.DontDestroyOnLoad(_instance);
            }
        }
    }
}