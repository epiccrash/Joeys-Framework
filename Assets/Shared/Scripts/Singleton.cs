using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared
{
    /// <summary>
    /// A script which allows objects inheriting from it to act as single authoritative instances.
    /// </summary>
    /// <typeparam name="T">The script type of this Singleton. This should mach the script name.</typeparam>
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        /// <summary>
        /// Whether to avoid destroying this object on loading a new scene. Defaults to true.
        /// </summary>
        [Header("Don't Destroy On Load")]
        [Tooltip("Whether to avoid destroying this object on loading a new scene. Defaults to true.")]
        [SerializeField]
        private bool dontDestroyOnLoad = true;

        /// <summary>
        /// The private instance of the singleton.
        /// </summary>
        private static T _instance;
        /// <summary>
        /// The public accessor for this singleton instance.
        /// </summary>
        public static T Instance => _instance;

        /// <summary>
        /// Sets this script as the instance of the singleton, or destroys this object if the instance already exists.
        /// </summary>
        public virtual void Awake()
        {
            if (_instance && _instance != this) Destroy(gameObject);
            else
            {
                _instance = this as T;
                if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            }
        }
    }
}
