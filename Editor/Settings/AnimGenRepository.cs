using System.Collections.Generic;
using UnityEditor;
using UnitySettings = UnityEditor.SettingsManagement.Settings;

namespace AnimatorGen.Settings
{
    public static class AnimGenRepository
    {
        private static UnitySettings _instance;

        public static UnitySettings Instance => _instance ??= new UnitySettings(PackageInfo.FullPackageName);

        private const string _animatorSettings = "animator-settings";

        public static List<AnimatorSettings> GetAnimatorSettings()
        {
            return Get(_animatorSettings, new List<AnimatorSettings>());
        }

        public static void SetAnimatorSettings(List<AnimatorSettings> animatorSettings)
        {
            Set(_animatorSettings, animatorSettings);
        }

        #region GetSetStuff

        public static void Save()
        {
            Instance.Save();
        }

        private static T Get<T>(string key, T fallback = default)
        {
            return Instance.Get(key, SettingsScope.Project, fallback);
        }

        private static void Set<T>(string key, T value)
        {
            Instance.Set(key, value, SettingsScope.Project);
        }

        private static bool ContainsKey<T>(string key)
        {
            return Instance.ContainsKey<T>(key, SettingsScope.Project);
        }

        #endregion GetSetStuff
    }
}