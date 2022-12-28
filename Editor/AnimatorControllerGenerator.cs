using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor.Animations;

using UnityEngine;

namespace AnimatorGen.Editor
{
    public class AnimatorControllerGenerator
    {
        private Exception ValidateSettings(AnimatorSettings settings)
        {
            return null;
        }

        private Exception ValidateTemplate(string templateContent)
        {
            return null;
        }

        public string GenerateCode(AnimatorController animator, AnimatorSettings settings)
        {
            string[] mainTemplate;

            var namespaceTag = "#NAMESPACE#";
            if (!string.IsNullOrWhiteSpace(settings.ClassNamespace))
            {
                mainTemplate = Templates.GetTemplateContentLines(Templates.MainWithNamespace);

                mainTemplate = ReplaceContent(mainTemplate, namespaceTag, settings.ClassNamespace);
            }
            else
            {
                mainTemplate = Templates.GetTemplateContentLines(Templates.Main);
            }

            var controllerClass = GenerateControllerClass(animator, settings);

            mainTemplate = ReplaceContent(mainTemplate, "#CONTROLLERMODEL", controllerClass);

            for (int i = 0; i < mainTemplate.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(mainTemplate[i]))
                {
                    mainTemplate[i] = string.Empty;
                }
            }

            var compiled = string.Join(Environment.NewLine, mainTemplate);

            return compiled;
        }

        private string[] GenerateControllerClass(AnimatorController animator, AnimatorSettings settings)
        {
            var controllerClass = Templates.GetTemplateContentLines(Templates.AnimationControllerModel);

            var classNameTag = "#CLASSNAME#";
            if (string.IsNullOrWhiteSpace(settings.ClassName))
            {
                throw new Exception($"Please specify a classname");
            }
            controllerClass = ReplaceContent(controllerClass, classNameTag, settings.ClassName);

            var parametersTag = "#PARAMETERS#";

            var typeTemplates = animator.parameters
                .Select(s => s.type)
                .Distinct()
                .ToDictionary(s => s, s =>
                {
                    var templateName = GetParamTemplateName(s);
                    return Templates.GetTemplateContentLines(templateName);
                });

            var parameters = new List<string>();// new StringBuilder();

            var rawNameTag = "#NAME_RAW#";
            var nameTag = "#NAME#";
            if (settings.CreateParameters)
            {
                //If theres a way to identify the variable after a rename then that would be nice
                //so that i can rather just update the string reference to it, meaning it wont break any code(?)
                for (int i = 0; i < animator.parameters.Length; i++)
                {
                    var p = animator.parameters[i];

                    var template = typeTemplates[p.type];
                    if (template != null && template.Any())
                    {
                        var param = ReplaceContent(template, rawNameTag, p.name);
                        param = ReplaceContent(param, nameTag, ParameteriseVariable(p.name));

                        parameters.AddRange(param);
                        if (i + 1 < animator.parameters.Length)
                        {
                            parameters.Add(string.Empty);
                        }
                    }
                }
            }

            //Wrap these in a class? Will help to keep things less cluttered/hard to find.
            if (settings.CreateLayers)
            {
                var layerTemplate = Templates.GetTemplateContent(Templates.Layer);
                if (!string.IsNullOrWhiteSpace(layerTemplate))
                {
                    foreach (var l in animator.layers)
                    {
                        var layer = layerTemplate.Replace(rawNameTag, l.name).Replace(nameTag, ParameteriseVariable(l.name));
                        parameters.Add(layer);
                    }
                }
            }

            if (settings.CreateParameters)
            {
                controllerClass = ReplaceContent(controllerClass, parametersTag, parameters.ToArray());
            }
            else
            {
                controllerClass = ReplaceContent(controllerClass, parametersTag, string.Empty);
            }

            var animatorTag = "#ANIMATOR#";
            controllerClass = ReplaceContent(controllerClass, animatorTag, "Animator");

            return controllerClass;
        }

        private string GetParamTemplateName(AnimatorControllerParameterType type)
        {
            return type switch
            {
                AnimatorControllerParameterType.Float => $"float",
                AnimatorControllerParameterType.Int => $"int",
                AnimatorControllerParameterType.Bool => $"bool",
                AnimatorControllerParameterType.Trigger => $"trigger",
                _ => null,
            };
        }

        private string ParameteriseVariable(string name)
        {
            return name.Replace(" ", "_");
        }

        public string[] ReplaceContent(string[] source, string tag, string contents)
        {
            var copy = new string[source.Length];
            source.CopyTo(copy, 0);

            for (int i = 0; i < copy.Length; i++)
            {
                if (copy[i].Contains(tag))
                {
                    copy[i] = copy[i].Replace(tag, contents);
                }
            }

            return copy;
        }

        public string[] ReplaceContent(string[] source, string tag, params string[] contents)
        {
            var sourceList = new List<string>(source);

            for (int Si = 0; Si < sourceList.Count; Si++)
            {
                var line = sourceList[Si];
                var tagIndex = line.IndexOf(tag);
                if (tagIndex == -1)
                    continue;

                var whitespace = new string(' ', tagIndex);

                for (int Ci = 0; Ci < contents.Length; Ci++)
                {
                    var contentLine = whitespace + contents[Ci];
                    //if (!string.IsNullOrWhiteSpace(contentLine))
                    //{
                    //    contentLine = whitespace + contentLine;
                    //}

                    if (Ci == 0)
                    {
                        sourceList[Si] = contentLine;
                    }
                    else
                    {
                        sourceList.Insert(Si + Ci, contentLine);
                    }
                }
            }

            return sourceList.ToArray();
        }
    }
}