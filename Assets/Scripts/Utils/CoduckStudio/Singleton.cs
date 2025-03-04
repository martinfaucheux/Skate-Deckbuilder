using UnityEngine;
using System.Collections;

namespace CoduckStudio.Utils
{
    /// <summary>
    /// MONOBEHAVIOR PSEUDO SINGLETON ABSTRACT CLASS
    /// usage	: best is to be attached to a gameobject but if not that is ok,
    /// 		: this will create one on first access
    /// example	: '''public sealed class MyClass : Singleton<MyClass> {'''
    /// references	: http://tinyurl.com/d498g8c
    /// 		: http://tinyurl.com/cc73a9h
    /// 		: http://unifycommunity.com/wiki/index.php?title=Singleton
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool applicationIsQuitting = false;

        [RuntimeInitializeOnLoadMethod]
        static void RunOnStart()
        {
            Application.quitting += () => applicationIsQuitting = true;
        }

        private static T _instance = null;

        public static bool IsAwake { get { return (_instance != null); } }

        /// <summary>
        /// gets the instance of this Singleton
        /// use this for all instance calls:
        /// MyClass.Instance.MyMethod();
        /// or make your public methods static
        /// and have them use Instance
        /// </summary>
        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    return _instance;
                }

                if (_instance == null)
                {
                    _instance = (T)FindFirstObjectByType(typeof(T));
                    if (_instance == null)
                    {

                        string goName = typeof(T).ToString();

                        GameObject go = GameObject.Find(goName);
                        if (go == null)
                        {
                            go = new GameObject();
                            go.name = goName;
                        }

                        _instance = go.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// for garbage collection
        /// </summary>
        public virtual void OnApplicationQuit()
        {
            // release reference on exit
            _instance = null;
        }
    }
}