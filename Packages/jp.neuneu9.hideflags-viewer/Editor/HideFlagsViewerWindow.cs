using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEditor;

namespace neuneu9.HideFlagsViewer
{
    /// <summary>
    /// HideFlags 状態を一覧表示
    /// </summary>
    public class HideFlagsViewerWindow : EditorWindow
    {
        private const string _windowName = "HideFlags Viewer";
        private GameObject[] _sceneObjects = null;
        private bool _isEditable = false;
        private Vector2 _scrollPosition = Vector2.zero;
        private int _sceneIndex = 0;
        private Scene _scene = default;
        private bool _excludeNone = false;

        [MenuItem("Window/" + _windowName)]
        private static void Open()
        {
            GetWindow<HideFlagsViewerWindow>(_windowName);
        }

        private void OnEnable()
        {
            _scene = SceneManager.GetSceneAt(_sceneIndex);
            ListSceneObjects(_scene);
        }

        private void OnGUI()
        {
            List<Scene> allScenes = new List<Scene>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                allScenes.Add(SceneManager.GetSceneAt(i));
            }

            EditorGUI.BeginChangeCheck();
            _sceneIndex = EditorGUILayout.Popup("Scene", _sceneIndex, allScenes.Select(x => x.name).ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                _scene = SceneManager.GetSceneAt(_sceneIndex);
                ListSceneObjects(_scene);
            }

            if (!_scene.IsValid())
            {
                _sceneIndex = 0;
                _scene = SceneManager.GetSceneAt(_sceneIndex);
                ListSceneObjects(_scene);
            }

            EditorGUILayout.Space();

            if (_sceneObjects != null && _sceneObjects.Any(x => x != null))
            {
                _isEditable = EditorGUILayout.Toggle("Edit HideFlags", _isEditable);
                _excludeNone = EditorGUILayout.Toggle("Exclude HideFlags.None", _excludeNone);

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("GameObject");
                EditorGUILayout.LabelField("HideFlags");
                EditorGUILayout.EndHorizontal();

                // テーブルデータ
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

                for (int i = 0; i < _sceneObjects.Length; i++)
                {
                    if (_sceneObjects[i] == null)
                    {
                        continue;
                    }

                    if (_excludeNone && _sceneObjects[i].hideFlags == HideFlags.None)
                    {
                        continue;
                    }

                    EditorGUILayout.BeginHorizontal();

                    // Object 列
                    EditorGUI.BeginDisabledGroup(true);

                    _sceneObjects[i] = (GameObject)EditorGUILayout.ObjectField(_sceneObjects[i], typeof(GameObject), false);

                    EditorGUI.EndDisabledGroup();

                    // HideFlags 列
                    EditorGUI.BeginDisabledGroup(!_isEditable);

                    EditorGUI.BeginChangeCheck();

                    var hideFlags = (HideFlags)EditorGUILayout.EnumFlagsField(_sceneObjects[i].hideFlags);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RegisterCompleteObjectUndo(_sceneObjects, "Change HideFlags");

                        // NOTE: -1 (Everything) を直接代入するとエラーを出すので無理やり回避
                        if (hideFlags == (HideFlags)(-1))
                        {
                            _sceneObjects[i].hideFlags = (HideFlags)127;
                        }
                        else
                        {
                            _sceneObjects[i].hideFlags = hideFlags;
                        }
                    }

                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
            }
            else
            {
                EditorGUILayout.HelpBox("No objects in this scene.", MessageType.Info);
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Reload"))
            {
                if (!_scene.IsValid())
                {
                    _sceneIndex = 0;
                    _scene = SceneManager.GetSceneAt(_sceneIndex);
                }

                ListSceneObjects(_scene);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void ListSceneObjects(Scene scene)
        {
            var sceneObjects = scene.GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren<Transform>(true)).Select(x => x.gameObject);
            _sceneObjects = sceneObjects.ToArray();
        }
    }
}
