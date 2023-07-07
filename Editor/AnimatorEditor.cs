using System.IO;
using System.Linq;
using AnimatorGen.Settings;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorGen
{
    [CustomEditor(typeof(AnimatorController))]
    public class AnimatorEditor : Editor
    {
        private AnimatorSettings _originalSettings;
        private AnimatorSettings _settings;

        private GUID AssetId
        {
            get
            {
                var path = AssetDatabase.GetAssetPath(target.GetInstanceID());
                GUID guid = AssetDatabase.GUIDFromAssetPath(path);
                return guid;
            }
        }

        private GUIStyle FileExplorerButton => new(GUI.skin.button)
        {
            fixedWidth = 30
        };

        public override VisualElement CreateInspectorGUI()
        {
            LoadSettings(AssetId);
            return base.CreateInspectorGUI();
        }

        private void LoadSettings(GUID assetId)
        {
            _settings = AnimGenSettings.GetSettings(assetId);
            if (_settings == null)
            {
                _settings = new AnimatorSettings
                {
                    AssetId = assetId
                };
            }

            _originalSettings = new AnimatorSettings(_settings);
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
            _settings.GenerateCode = EditorGUILayout.Toggle("Generate C# Class", _settings.GenerateCode);
            _settings.AutoGenerateCode = EditorGUILayout.Toggle("Auto generate class", _settings.AutoGenerateCode);

            GUI.enabled = _settings.GenerateCode;

            GUILayout.BeginHorizontal();
            _settings.ClassFile = EditorGUILayout.TextField("C# Class File", _settings.ClassFile);
            if (GUILayout.Button("...", FileExplorerButton))
            {
                var path = EditorUtility.SaveFilePanelInProject($"AnimatorController Class for '{animator.name}'", animator.name, "cs", "Location in which to save the generated animator controller class file.");
                if (!string.IsNullOrWhiteSpace(path))
                {
                    _settings.ClassFile = path;
                    Repaint();
                }
            }

            GUILayout.EndHorizontal();
            _settings.ClassName = EditorGUILayout.TextField("C# Class Name", _settings.ClassName);
            _settings.ClassNamespace = EditorGUILayout.TextField("C# Class Namespace", _settings.ClassNamespace);

            _settings.CreateParameters = EditorGUILayout.Toggle("Create parameters", _settings.CreateParameters);
            _settings.CreateLayers = EditorGUILayout.Toggle("Create layers", _settings.CreateLayers);

            if (EditorGUI.EndChangeCheck())
            {
                AnimGenSettings.SetSettings(_settings);
            }

            GUILayout.BeginHorizontal();

            GUI.enabled = true;

            if (GUILayout.Button("Revert"))
            {
                GUI.FocusControl(string.Empty);
                _settings = new AnimatorSettings(_originalSettings);
                Repaint();
            }

            if (GUILayout.Button("Save"))//Apply
            {
                AnimGenSettings.SetSettings(_settings);
                AnimGenSettings.SaveSettings();
            }

            GUI.enabled = _settings.GenerateCode;

            if (GUILayout.Button("Re-generate"))
            {
                var generator = new AnimatorControllerGenerator();
                var code = generator.GenerateCode(animator, _settings);

                var outputPath = Path.GetFullPath(_settings.ClassFile);

                File.WriteAllText(outputPath, code);
                AssetDatabase.Refresh();
            }

            GUI.enabled = true;

            GUILayout.EndHorizontal();

            //EditorGUILayout.Space();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            DoParameters(animator);

            base.OnInspectorGUI();
        }

        private void DoParameters(AnimatorController animator)
        {
            EditorGUILayout.LabelField("Default animator prameter values:");
            var parametersDirty = false;

            var parameters = animator.parameters.Select(s => s.Copy()).ToList();

            for (int i = 0; i < parameters.Count; i++)
            {
                var item = parameters[i];

                var type = item.type.ToString();

                var label = $"{type}\t - {item.name}";
                switch (item.type)
                {
                    case AnimatorControllerParameterType.Float:
                        var newFloat = EditorGUILayout.FloatField(label, item.defaultFloat);

                        if (item.defaultFloat != newFloat)
                        {
                            item.defaultFloat = newFloat;
                            parametersDirty = true;
                        }
                        break;

                    case AnimatorControllerParameterType.Int:
                        var newInt = EditorGUILayout.IntField(label, item.defaultInt);

                        if (item.defaultInt != newInt)
                        {
                            item.defaultInt = newInt;
                            parametersDirty = true;
                        }
                        break;

                    case AnimatorControllerParameterType.Bool:
                        var newBool = EditorGUILayout.Toggle(label, item.defaultBool);

                        if (item.defaultBool != newBool)
                        {
                            item.defaultBool = newBool;
                            parametersDirty = true;
                        }
                        break;

                    case AnimatorControllerParameterType.Trigger:
                        GUI.enabled = false;
                        EditorGUILayout.LabelField(label);
                        GUI.enabled = true;
                        break;
                }
            }

            if (parametersDirty)
            {
                var currentParameters = animator.parameters.ToList();

                //Because we can only add/remove parameters and not alter the original list:
                //The idea is when adding it here it's a new object/reference
                //and likewise when removing we're removing the original object references with the old value(s)
                foreach (var parameter in parameters)
                    animator.AddParameter(parameter);

                foreach (var parameter in currentParameters)
                    animator.RemoveParameter(parameter);
            }
        }
    }
}