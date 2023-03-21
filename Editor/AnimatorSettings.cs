using System;

using UnityEditor;

[Serializable]
public class AnimatorSettings
{
    public GUID AssetId
    {
        get
        {
            if (GUID.TryParse(assetId, out var guid))
            {
                return guid;
            }

            return new GUID();
        }
        set
        {
            assetId = value.ToString();
        }
    }

    public string assetId;
    public bool GenerateCode;
    public bool AutoGenerateCode;
    public string ClassFile;
    public string ClassName;
    public string ClassNamespace;

    public bool CreateParameters;
    public bool CreateLayers;

    public AnimatorSettings()
    {
    }

    public AnimatorSettings(AnimatorSettings animatorSettings)
    {
        AssetId = animatorSettings.AssetId;
        AutoGenerateCode = animatorSettings.AutoGenerateCode;
        GenerateCode = animatorSettings.GenerateCode;
        ClassFile = animatorSettings.ClassFile;
        ClassName = animatorSettings.ClassName;
        ClassNamespace = animatorSettings.ClassNamespace;

        CreateParameters = animatorSettings.CreateParameters;
        CreateLayers = animatorSettings.CreateLayers;
    }
}