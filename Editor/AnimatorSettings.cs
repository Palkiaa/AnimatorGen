using System;

[Serializable]
public class AnimatorSettings
{
    public int InstanceId;
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
        InstanceId = animatorSettings.InstanceId;
        AutoGenerateCode = animatorSettings.AutoGenerateCode;
        GenerateCode = animatorSettings.GenerateCode;
        ClassFile = animatorSettings.ClassFile;
        ClassName = animatorSettings.ClassName;
        ClassNamespace = animatorSettings.ClassNamespace;

        CreateParameters = animatorSettings.CreateParameters;
        CreateLayers = animatorSettings.CreateLayers;
    }
}