using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Narazaka.Unity.MissingScripts
{
    public class CheckMissingScripts : EditorWindow
    {
        // Start is called before the first frame update
        [MenuItem("Tools/CheckMissingScripts")]
        static void GetW()
        {
            GetWindow<CheckMissingScripts>(nameof(CheckMissingScripts));
        }

        List<GameObject> MissingObjects;

        Vector2 ScrollPos;

        void OnGUI()
        {
            if (GUILayout.Button("check"))
            {
                MissingObjects = new List<GameObject>();
                Check();
            }

            EditorGUILayout.Separator();

            ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
            if (MissingObjects != null)
            {
                if (MissingObjects.Count == 0) EditorGUILayout.LabelField("none!");
                foreach (var obj in MissingObjects)
                {
                    if (GUILayout.Button(obj.name))
                    {
                        Selection.activeGameObject = obj;
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }

        void Check()
        {
            var objs = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var obj in objs)
            {
                var components = obj.GetComponents<Component>();
                if (components.Any(c => c == null)) MissingObjects.Add(obj);

                for (var i = 0; i < obj.transform.childCount; i++)
                {
                    Check(obj.transform.GetChild(i).gameObject);
                }
            }
        }

        void Check(GameObject obj)
        {
            var components = obj.GetComponents<Component>();
            if (components.Any(c => c == null)) MissingObjects.Add(obj);

            for (var i = 0; i < obj.transform.childCount; i++)
            {
                Check(obj.transform.GetChild(i).gameObject);
            }
        }
    }
}
