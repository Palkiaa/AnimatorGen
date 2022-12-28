using System.IO;

using UnityEditor;
using UnityEditor.Animations;

using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorGen.Editor
{
    [CustomEditor(typeof(AnimatorController))]
    public class AnimatorEditor : UnityEditor.Editor
    {
        private AnimatorSettings Settings;

        private int InstanceId => target.GetInstanceID();

        private GUIStyle FileExplorerButton => new GUIStyle(GUI.skin.button)
        {
            fixedWidth = 30
        };

        public override VisualElement CreateInspectorGUI()
        {
            LoadSettings(InstanceId);
            return base.CreateInspectorGUI();
        }

        private void LoadSettings(int instanceId)
        {
            Settings = AnimGenSettings.instance.GetSettings(instanceId);
            if (Settings == null)
            {
                Settings = new AnimatorSettings
                {
                    InstanceId = instanceId
                };
            }
        }

        public override void OnInspectorGUI()
        {
            var animator = target as AnimatorController;

            EditorGUILayout.LabelField($"Animator name: {animator.name}");
            if (animator != null && GUILayout.Button("Open Animator"))
            {
                EditorApplication.ExecuteMenuItem("Window/Animation/Animator");
                AssetDatabase.OpenAsset(animator.GetInstanceID());
            }

            //EditorGUILayout.Space();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            EditorGUI.BeginChangeCheck();
            Settings.GenerateCode = EditorGUILayout.Toggle("Generate C# Class", Settings.GenerateCode);
            Settings.AutoGenerateCode = EditorGUILayout.Toggle("Auto generate class", Settings.AutoGenerateCode);

            GUI.enabled = Settings.GenerateCode;

            GUILayout.BeginHorizontal();
            Settings.ClassFile = EditorGUILayout.TextField("C# Class File", Settings.ClassFile);
            if (GUILayout.Button("...", FileExplorerButton))
            {
                var path = EditorUtility.SaveFilePanelInProject($"AnimatorController Class for '{animator.name}'", animator.name, "cs", "Location in which to save the generated animator controller class file.");
                if (!string.IsNullOrWhiteSpace(path))
                {
                    Settings.ClassFile = path;
                }
            }

            GUILayout.EndHorizontal();
            Settings.ClassName = EditorGUILayout.TextField("C# Class Name", Settings.ClassName);
            Settings.ClassNamespace = EditorGUILayout.TextField("C# Class Namespace", Settings.ClassNamespace);

            Settings.CreateParameters = EditorGUILayout.Toggle("Create parameters", Settings.CreateParameters);
            Settings.CreateLayers = EditorGUILayout.Toggle("Create layers", Settings.CreateLayers);

            GUI.enabled = true;

            if (EditorGUI.EndChangeCheck())
            {
                AnimGenSettings.instance.SetSettings(Settings);
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Revert"))
            {
                GUI.FocusControl(string.Empty);
                LoadSettings(InstanceId);
            }

            if (GUILayout.Button("Apply"))
            {
                AnimGenSettings.instance.SetSettings(Settings);
                AnimGenSettings.instance.SaveSettings();

                var generator = new AnimatorControllerGenerator();
                var code = generator.GenerateCode(animator, Settings);

                var outputPath = Path.GetFullPath(Settings.ClassFile);

                File.WriteAllText(outputPath, code);
                AssetDatabase.Refresh();
            }

            GUILayout.EndHorizontal();

            //EditorGUILayout.Space();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            EditorGUILayout.LabelField("Default animator prameter values:");
            for (int i = 0; i < animator.parameters.Length; i++)
            {
                var item = animator.parameters[i];

                string label = $"{item.type,-8} - {item.name}";
                switch (item.type)
                {
                    case AnimatorControllerParameterType.Float:
                        item.defaultFloat = EditorGUILayout.FloatField(label, item.defaultFloat);
                        break;

                    case AnimatorControllerParameterType.Int:
                        item.defaultInt = EditorGUILayout.IntField(label, item.defaultInt);
                        break;

                    case AnimatorControllerParameterType.Bool:
                        item.defaultBool = EditorGUILayout.Toggle(label, item.defaultBool);
                        break;

                    case AnimatorControllerParameterType.Trigger:
                        EditorGUILayout.LabelField(label);
                        break;
                }
            }

            base.OnInspectorGUI();
        }
    }
}