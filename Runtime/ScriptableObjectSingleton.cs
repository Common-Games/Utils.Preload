using System.Collections.Generic;
using System.Linq;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using JetBrains.Annotations;

namespace CGTK.Utils.Preload
{
	/// <summary> Singleton for <see cref="ScriptableObject"/>s</summary>
	/// <typeparam name="T"> Type of the Singleton. CRTP (the inheritor)</typeparam>
	public abstract class ScriptableObjectSingleton<T> : ScriptableObject 
		where T : ScriptableObjectSingleton<T>
	{
		#region Properties

		private static T _internalInstance;

		/// <summary> The static reference to the Instance </summary>
		[PublicAPI]
		public static T Instance
		{
			get
			{
				if (InstanceExists) return _internalInstance;

				T[] _found = Resources.FindObjectsOfTypeAll<T>();
				
				//TODO: Ensured?
				if (_found != null)
				{
					if (_found[0] != null)
					{
						return _internalInstance = _found[0];		
					}	
				}

				return null;
				//return _internalInstance = CreateInstance<T>();
			}
			protected set
			{
				_internalInstance = value;	
				
				#if UNITY_EDITOR
				if(_internalInstance == null) return;
				
				List<Object> _preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();

				if (_preloadedAssets.Contains(_internalInstance)) return;

				_preloadedAssets.Add(_internalInstance);

				PlayerSettings.SetPreloadedAssets(_preloadedAssets.ToArray());
				#endif
			}
		}

		/// <summary> Whether a Instance of the Singleton exists </summary>
		[PublicAPI]
		public static bool InstanceExists => (_internalInstance != null);

		#endregion

		#region Methods

		protected virtual void Reset() => Register();
		protected virtual void Awake() => Register();
		protected virtual void OnEnable() => Register();
		
		protected virtual void OnDisable() => Unregister();

		/// <summary> Associate Singleton with new instance. </summary>
		//[RuntimeInitializeOnLoadMethod]
		private void Register()
		{
			if(InstanceExists && (Instance != this)) //Prefer using already existing Singletons.
			{
				#if UNITY_EDITOR
				if (!EditorApplication.isPlaying)
				{
					DestroyImmediate(obj: this);
				}
				else
				{
					Destroy(obj: this);
				}
				#else
				Destroy(obj: this);
				#endif
				
				return;
			}
			
			Instance = this as T;
		}
		
		/// <summary> Clear Singleton association </summary>
		private void Unregister()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		#endregion
	}
}
