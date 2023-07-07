using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace AnimatorGen.Settings
{
    public class AnimGenSettingsProvider : SettingsProvider
    {
        private class Styles
        {
        }

        public AnimGenSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
        }

        public override void OnGUI(string searchContext)
        {
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new AnimGenSettingsProvider($"Project/{PackageInfo.DisplayName}", SettingsScope.Project)
            {
                // Automatically extract all keywords from the Styles.
                keywords = GetSearchKeywordsFromGUIContentProperties<Styles>()
            };
            return provider;
        }
    }
}