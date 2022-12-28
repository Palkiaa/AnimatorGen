using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorGen.Editor
{
    

    public class AnimGenSettingsProvider : SettingsProvider
    {
        private class Styles
        {
            public static GUIContent types = new GUIContent("Component Types");
            public static GUIContent enabled = new GUIContent("Enabled");
            public static GUIContent reset = new GUIContent("Reset to defaults");
        }

        public AnimGenSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
        }

        public override void OnGUI(string searchContext)
        {
            //animGenSettings.

            //m_CustomSettings.Update();
        
            //if (GUILayout.Button(Styles.reset))
            //{
            //    obj.Types = CompSortingSettings.defaultOrder.Select(s => new SerializedType(s)).ToList();
            //    dirty = true;
            //}
            //
            //EditorGUILayout.Space();
            //
            ////text = EditorGUILayout.TextField(Styles.number, text);
            //allTypes = ComponentDatabase.GetAllTypes();
            //allTypes = allTypes.Where(s => !obj.Types.Any(t => t.Name == s.Name)).ToList();
            //
            //SerializedTypeDrawer.options = allTypes.Select(s => s.Name).ToList();
            //
            //EditorGUI.BeginChangeCheck();
            //
            //obj.Enabled = EditorGUILayout.Toggle(Styles.enabled, obj.Enabled);
            //
            //EditorGUILayout.Space();
            //
            //reordableList.DoLayoutList();
            //
            //if (EditorGUI.EndChangeCheck() || dirty)
            //{
            //    m_CustomSettings.ApplyModifiedProperties();
            //
            //    CompSortingRepository.SetEnabled(obj.Enabled);
            //    CompSortingRepository.SetTypes(obj.Types);
            //    CompSortingRepository.Save();
            //
            //    dirty = false;
            //}
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new AnimGenSettingsProvider("Project/AnimGen", SettingsScope.Project)
            {
                // Automatically extract all keywords from the Styles.
                keywords = GetSearchKeywordsFromGUIContentProperties<Styles>()
            };
            return provider;
        }
    }
}