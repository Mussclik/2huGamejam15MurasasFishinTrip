#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class StaticHighlighter : EditorWindow
{
    [MenuItem("Tools/Game State Viewer")]
    public static void ShowWindow()
    {
        GetWindow<StaticHighlighter>("Game State Viewer");
    }

    private void OnEnable()
    {
        EditorApplication.update += Repaint; // Ensures the window updates constantly
    }

    private void OnDisable()
    {
        EditorApplication.update -= Repaint; // Stops updates when closed
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Current GameState", EditorStyles.boldLabel);

        if (GameManager.instance == null)
        {
            EditorGUILayout.HelpBox("GameManager instance is NULL. Ensure it exists in the scene.", MessageType.Warning);
            return;
        }

        // Display and allow changing the GameManager.gamestate
        GameManager.Gamestate = (Gamestate)EditorGUILayout.EnumPopup("Game State", GameManager.Gamestate);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Current Player Stats", EditorStyles.boldLabel);

        if (PlayerMovement.instance == null)
        {
            EditorGUILayout.HelpBox("Player instance is NULL. Ensure it exists in the scene.", MessageType.Warning);
            return;
        }

        // Read-only display of player's velocity
        Rigidbody playerRigidbody = PlayerMovement.instance.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            EditorGUILayout.Vector3Field("Player Velocity", playerRigidbody.velocity);
        }
        else
        {
            EditorGUILayout.HelpBox("Rigidbody not found on PlayerMovement instance.", MessageType.Warning);
        }
        if (PlayerMovement.instance != null)
        {
            EditorGUILayout.Vector3Field("Player Desired Velocity", PlayerMovement.instance.DesiredDirection);
        }
        else
        {
            EditorGUILayout.HelpBox("PlayerMovement instance ist null.", MessageType.Warning);
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Current Fishing Area", EditorStyles.boldLabel);

        // Display the current FishingArea if it exists
        if (PlayerMovement.instance != null)
        {
            FishingArea currentFishingArea = PlayerMovement.instance.currentFishingArea;
            currentFishingArea = (FishingArea)EditorGUILayout.ObjectField("Fishing Area", currentFishingArea, typeof(FishingArea), true);

            // Update the PlayerMovement's currentFishingArea
            if (currentFishingArea != PlayerMovement.instance.currentFishingArea)
            {
                PlayerMovement.instance.currentFishingArea = currentFishingArea;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("PlayerMovement instance is missing or doesn't have a reference to currentFishingArea.", MessageType.Warning);
        }
    }
}
#endif
