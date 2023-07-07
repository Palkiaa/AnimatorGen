using System.IO;

namespace AnimatorGen
{
    public static class Templates
    {
        private static string TemplatesPath => Path.Combine("Packages", PackageInfo.FullPackageName, "Templates");

        public const string Main = "MainTemplate";
        public const string MainWithNamespace = "MainTemplateWithNamespace";

        public const string AnimationControllerModel = "AnimationControllerModel";

        public const string Bool = "bool";
        public const string Float = "float";
        public const string Int = "int";
        public const string Layer = "layer";
        public const string Trigger = "trigger";

        public static string GetTemplateContent(string fileName)
        {
            var templatePath = Path.GetFullPath(Path.Combine(TemplatesPath, $"{fileName}.txt"));
            if (File.Exists(templatePath))
            {
                return File.ReadAllText(templatePath);
            }

            return null;
        }

        public static string[] GetTemplateContentLines(string fileName)
        {
            var templatePath = Path.GetFullPath(Path.Combine(TemplatesPath, $"{fileName}.txt"));
            if (File.Exists(templatePath))
            {
                return File.ReadAllLines(templatePath);
            }

            return null;
        }
    }
}