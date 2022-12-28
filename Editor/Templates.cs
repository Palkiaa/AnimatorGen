using System.IO;

namespace AnimatorGen.Editor
{
    public static class Templates
    {
        private static string PackageName => "com.spaghetti.animator-gen";
        private static string TemplatesPath => Path.Combine("Packages", PackageName, "Templates");

        public static string Main => "MainTemplate";
        public static string MainWithNamespace => "MainTemplateWithNamespace";

        public static string AnimationControllerModel => "AnimationControllerModel";

        public static string Bool => "bool";
        public static string Float => "float";
        public static string Int => "int";
        public static string Layer => "layer";
        public static string Trigger => "trigger";

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