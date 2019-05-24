using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ZTools {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        [Header ("Singleton Parameter")]
        /// <summary>
        /// Keep persistent between scene changes
        /// </summary>
        public bool IsDontDestroyOnLoad = true;
        /// <summary>
        /// Whether certain actions should be logged
        /// </summary>
        public bool DebugMode;
        /// <summary>
        /// Should the object be hidden -> A combination of not shown in the hierarchy, not saved to to scenes and not unloaded
        /// by The object will not be unloaded by Resources.UnloadUnusedAssets.
        /// </summary>
        public bool IsHideFlags = false;

        private static T instance;
        public static T Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<T> ();
                    if (instance == null) {
                        GameObject obj = new GameObject ("[ Singleton Obj ]");

                        instance = obj.AddComponent<T> ();
                    }
                }
                return instance;
            }
        }

        //如果已有實例，強制刪除新物件-讓此實例是唯一
        protected virtual void Awake () {
            // 隐藏实例化的new game object或不刪除此物件
            //if (IsHideFlags)
            //this.gameObject.hideFlags = HideFlags.HideAndDontSave;

            if (IsDontDestroyOnLoad)
                DontDestroyOnLoad (this.gameObject);
            if (instance == null) {
                instance = this as T;
            } else {
                Destroy (gameObject);
            }
            initializationSet ();
        }

        protected virtual void initializationSet () { }
        protected void ShowMessage (string s) {
            if (DebugMode)
                Debug.Log (s);
        }
    }
}