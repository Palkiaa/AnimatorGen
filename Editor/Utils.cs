using UnityEngine;

namespace AnimatorGen
{
    public static class Utils
    {
        public static AnimatorControllerParameter Copy(this AnimatorControllerParameter animatorControllerParameter)
        {
            return new AnimatorControllerParameter()
            {
                name = animatorControllerParameter.name,
                type = animatorControllerParameter.type,
                defaultFloat = animatorControllerParameter.defaultFloat,
                defaultInt = animatorControllerParameter.defaultInt,
                defaultBool = animatorControllerParameter.defaultBool
            };
        }
    }
}