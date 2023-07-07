using UnityEditor;

namespace AnimatorGen.Settings
{
    internal static class AnimGenSettings
    {
        public static void SetSettings(AnimatorSettings animatorSettings)
        {
            var settings = AnimGenRepository.GetAnimatorSettings();

            var index = settings.FindIndex(s => s.AssetId == animatorSettings.AssetId);
            if (0 <= index)
                settings[index] = new AnimatorSettings(animatorSettings);
            else
                settings.Add(new AnimatorSettings(animatorSettings));

            AnimGenRepository.SetAnimatorSettings(settings);
        }

        public static AnimatorSettings GetSettings(GUID assetId)
        {
            var settings = AnimGenRepository.GetAnimatorSettings();

            var index = settings.FindIndex(s => s.AssetId == assetId);
            if (0 <= index)
                return new AnimatorSettings(settings[index]);

            return null;
        }

        public static void SaveSettings()
        {
            AnimGenRepository.Save();
        }
    }
}