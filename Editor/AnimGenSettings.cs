using System.Collections.Generic;

using UnityEditor;

[FilePath("AnimGenSettings/AnimGenSettings.json", FilePathAttribute.Location.ProjectFolder)]
internal class AnimGenSettings : ScriptableSingleton<AnimGenSettings>
{
    public List<AnimatorSettings> AnimatorSettings;

    public void SetSettings(AnimatorSettings animatorSettings)
    {
        var index = AnimatorSettings.FindIndex(s => s.AssetId == animatorSettings.AssetId);
        if (0 <= index)
        {
            AnimatorSettings[index] = animatorSettings;
            return;
        }

        AnimatorSettings.Add(new AnimatorSettings(animatorSettings));
        EditorUtility.SetDirty(this);
    }

    public AnimatorSettings GetSettings(GUID assetId)
    {
        var index = AnimatorSettings.FindIndex(s => s.AssetId == assetId);
        if (0 <= index)
        {
            return new AnimatorSettings(AnimatorSettings[index]);
        }

        return null;
    }

    public void SaveSettings()
    {
        Save(true);
    }
}