using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR

//[MenuItem("Task List/Open Task List")]
namespace TaskList
{
    using System.Collections.Generic;
    using UnityEngine;

    /**
     * <summary>
     * To use, simply place in an Editor folder. 
     * The new window will be available under Window > Custom> ToDo List.
     * </summary>
     */
    public class InEditorTodo : EditorWindow
    {
        [SerializeField] List<string> tdL = new();
        [SerializeField] List<bool> completed = new();

        private UnityEditorInternal.ReorderableList reorderableList;

        [MenuItem("Task List/Open")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<InEditorTodo>("To-Do List").Show();
        }

        private void OnEnable()
        {
            string dp = Application.dataPath;
            string[] parts = dp.Split('/');
            string s = parts[^1]; // Use the last part of the path for unique identification
            var data = EditorPrefs.GetString($"{s}_TODOList", JsonUtility.ToJson(this, false));
            JsonUtility.FromJsonOverwrite(data, this);

            while (tdL.Count > completed.Count)
            {
                completed.Add(false);
            }
            while (tdL.Count < completed.Count)
            {
                tdL.Add("NEW");
            }

            InitReorderableList();
        }

        private void InitReorderableList()
        {
            reorderableList = new UnityEditorInternal.ReorderableList(tdL, typeof(string), true, false, false, false)
            {
                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "TO DO List");
                },

                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    rect.y += 2;
                    rect.height = EditorGUIUtility.singleLineHeight;
                    float toggleWidth = 16;
                    float buttonWidth = 20;
                    float padding = 5;

                    EditorGUI.BeginChangeCheck();
                    bool newComp = EditorGUI.Toggle(new Rect(rect.x, rect.y, toggleWidth, rect.height), completed[index]);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(this, "Toggle To-Do Completion");
                        completed[index] = newComp;
                    }

                    EditorGUI.BeginChangeCheck();
                    string newVal = EditorGUI.TextField(new Rect(rect.x + toggleWidth + padding, rect.y, rect.width - toggleWidth - buttonWidth - 2 * padding, rect.height), tdL[index]);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(this, "Set To-Do Name");
                        tdL[index] = newVal;
                    }

                    if (GUI.Button(new Rect(rect.x + rect.width - buttonWidth, rect.y, buttonWidth, rect.height), "X"))
                    {
                        Undo.RecordObject(this, "Delete To-Do");
                        tdL.RemoveAt(index);
                        completed.RemoveAt(index);
                    }
                },

                onReorderCallbackWithDetails = (UnityEditorInternal.ReorderableList list, int oldIndex, int newIndex) =>
                {
                    bool temp = completed[oldIndex];
                    completed.RemoveAt(oldIndex);
                    completed.Insert(newIndex, temp);
                }
            };
        }

        protected void OnDisable()
        {
            string dp = Application.dataPath;
            string[] parts = dp.Split('/');
            string s = parts[parts.Length - 1]; // Use the last part of the path for unique identification
            var data = JsonUtility.ToJson(this, false);
            EditorPrefs.SetString($"{s}_TODOList", data);
        }

        private void OnGUI()
        {
            reorderableList.DoLayoutList();

            Rect buttonRect = GUILayoutUtility.GetRect(48, 24, GUILayout.ExpandWidth(false));
            buttonRect.x = (position.width - 48) / 2;
            buttonRect.y -= 17;
            if (GUI.Button(buttonRect, "+"))
            {
                Undo.RecordObject(this, "Add To-Do");
                tdL.Add("NEW");
                completed.Add(false);
            }
        }
    }
}
#endif