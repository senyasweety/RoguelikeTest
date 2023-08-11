using UnityEngine;

namespace DefaultNamespace.Tools
{
    public static class ConsoleTools
    {
        public static void LogSuccess(string prompt, int size = 12)
        {
            Debug.Log($"<size={GetMaxSize(size)}><color=YELLOW>{prompt}</color></size>");
        }

        public static void LogInfo(string prompt, int size = 12)
        {
            Debug.Log($"<size={GetMaxSize(size)}><color=ORANGE>{prompt}</color></size>");
        }

        public static void LogError(string prompt, int size = 12)
        {
            Debug.Log($"<b><size={GetMaxSize(size)}><color=RED>{prompt}</color></size></b>");
        }

        private static int GetMaxSize(int size) => Mathf.Min(size, 20);
    }
}