using System.IO;
using AnimatorGen.Settings;
using UnityEditor;
using UnityEditor.Animations;

namespace AnimatorGen.AssetProcessors
{
    public class AnimatorControllerAssetProcessor : AssetModificationProcessor
    {
        private const string _assetFilter = ".controller";

        private static AnimatorControllerGenerator _animatorControllerGenerator = new();

        protected static string[] OnWillSaveAssets(string[] paths)
        {
            foreach (var path in paths)
            {
                if (!path.EndsWith(_assetFilter))
                    continue;

                var guid = AssetDatabase.GUIDFromAssetPath(path);

                //AssetDatabase.GUIDFromAssetPath: "An all-zero GUID denotes an invalid asset path."
                if (guid == new GUID())
                    continue;

                var settings = AnimGenSettings.GetSettings(guid);
                if (settings == null || !settings.GenerateCode || !settings.AutoGenerateCode)
                    continue;

                if (string.IsNullOrWhiteSpace(settings.ClassFile) || string.IsNullOrWhiteSpace(settings.ClassName))
                    continue;

                var animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);

                var outputPath = Path.GetFullPath(settings.ClassFile);

                var code = _animatorControllerGenerator.GenerateCode(animatorController, settings);

                File.WriteAllText(outputPath, code);
                AssetDatabase.Refresh();
            }

            return paths;
        }

        protected static void OnWillCreateAsset(string assetName)
        {
        }

        protected static AssetDeleteResult OnWillDeleteAsset(string sourcePath, RemoveAssetOptions removeAssetOptions)
        {
            return AssetDeleteResult.DidNotDelete;
        }

        protected static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            return AssetMoveResult.DidNotMove;
        }
    }
}