using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

using CGTK.Utils.Extensions.Collections;

namespace CGTK.Utils.Preload
{
    using static PackageConstants;
    
    public class PreloadSettings : ScriptableObjectSingleton<PreloadSettings>
    {
        [SerializeField] internal GameObject[] prefabs = Array.Empty<GameObject>();

        public static IEnumerable<GameObject> Prefabs => Instance.prefabs;
        
        public void OnValidate()
        {
            #if UNITY_EDITOR
            List<Object> _preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            
            _preloadedAssets.AddRangeUnique(prefabs);

            PlayerSettings.SetPreloadedAssets(_preloadedAssets.ToArray());
            #endif
        }
    }

    #region Custom Editor

    #if UNITY_EDITOR
    public static class PreloadSettingsRegister
    {
        private static PreloadSettings  _settings;
        private static SerializedObject _settingsSerialized;
        private static SettingsProvider _settingsProvider;

        [SettingsProvider]
        public static SettingsProvider LoadSettingsProvider()
        {
            _settings = CreateSettings();
            
            _settingsSerialized ??= new SerializedObject(_settings);

            return _settingsProvider = CreateSettingsProvider();
        }
        
        private static PreloadSettings CreateSettings()
        {
            if (_settings != null) return _settings;

            _settings = AssetDatabase.LoadAssetAtPath<PreloadSettings>(SETTINGS_PATH);
            
            if (_settings != null) return _settings;

            if (!Directory.Exists(SETTINGS_PATH))
            {
                Directory.CreateDirectory(SETTINGS_PATH);
            }
            
            _settings = ScriptableObject.CreateInstance<PreloadSettings>();
            
            AssetDatabase.CreateAsset(_settings, SETTINGS_PATH);
            AssetDatabase.SaveAssets();

            return _settings;
        }

        private static SettingsProvider CreateSettingsProvider()
        {
            if (_settingsProvider != null) return _settingsProvider;
            
            return _settingsProvider = new SettingsProvider(path: "Project/CGTK/Preload", scopes: SettingsScope.Project)
            {
                guiHandler = searchContext =>
                {
                    EditorGUI.BeginChangeCheck();
                    
                    EditorGUILayout.PropertyField(_settingsSerialized.FindProperty(nameof(PreloadSettings.prefabs)), label: new GUIContent(text: "Prefabs"));
                    
                    if (EditorGUI.EndChangeCheck())
                    {
                        _settingsSerialized.ApplyModifiedProperties();
                        
                        _settings.OnValidate();
                    }                   
                },
                
                keywords = new HashSet<String>(collection: new[] { "preload", "bootstrap" })
            };
        }
    }
    #endif
    
    #endregion
}
