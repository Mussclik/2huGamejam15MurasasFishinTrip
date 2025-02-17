using UnityEditor;
using UnityEngine;

public class StaticHighlighter : EditorWindow
{
    [MenuItem("Tools/Game State Viewer")]
    public static void ShowWindow()
    {
        GetWindow<StaticHighlighter>("Game State Viewer");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Current Game State", EditorStyles.boldLabel);

        if (GameManager.instance == null)
        {
            EditorGUILayout.HelpBox("GameManager instance is NULL. Ensure it exists in the scene.", MessageType.Warning);
            return;
        }

        // Display and allow changing the GameManager.gamestate
        GameManager.gamestate = (Gamestate)EditorGUILayout.EnumPopup("Game State", GameManager.gamestate);
    }
}
