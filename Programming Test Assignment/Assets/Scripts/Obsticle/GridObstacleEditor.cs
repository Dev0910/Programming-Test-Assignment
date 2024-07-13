using UnityEditor;
using UnityEngine;

public class GridObstacleEditor : EditorWindow
{
    private bool[,] grid = new bool[10, 10]; // 2D array to store the obstacle state of the grid
    private ObstacleData obstacleData; // Reference to the scriptable object holding obstacle data

    // Menu item to show the Grid Obstacle Editor window
    [MenuItem("Tools/Grid Obstacle Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridObstacleEditor>("Grid Obstacle Editor");
    }

    // Called when the editor window is enabled
    private void OnEnable()
    {
        LoadObstacleData(); // Load the existing obstacle data from the scriptable object
    }


    // Method to render the UI of the editor window
    private void OnGUI()
    {
        GUILayout.Label("Grid Obstacle Editor", EditorStyles.boldLabel); // Title label

        // Create a 10x10 grid of toggle buttons
        for (int x = 0; x < 10; x++)
        {
            GUILayout.BeginHorizontal(); // Begin a new horizontal group
            for (int y = 0; y < 10; y++)
            {
                grid[x, y] = GUILayout.Toggle(grid[x, y], ""); // Toggle button for each grid cell
            }
            GUILayout.EndHorizontal(); // End the horizontal group
        }

        // Button to save the obstacle data
        if (GUILayout.Button("Save Obstacles"))
        {
            SaveObstacles();
        }

        // Button to reload the obstacle data from the scriptable object
        if (GUILayout.Button("Load Obstacles"))
        {
            LoadObstacleData();
        }
    }

    // Method to load obstacle data from the scriptable object
    private void LoadObstacleData()
    {
        // Load the scriptable object from the specified path
        obstacleData = AssetDatabase.LoadAssetAtPath<ObstacleData>("Assets/ObstacleData.asset");

        // If the scriptable object doesn't exist, create a new one
        if (obstacleData == null)
        {
            obstacleData = ScriptableObject.CreateInstance<ObstacleData>();
            AssetDatabase.CreateAsset(obstacleData, "Assets/ObstacleData.asset");
            AssetDatabase.SaveAssets();
        }

        // Copy the data from the scriptable object to the local grid array
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                grid[x, y] = obstacleData.gridData[x, y];
            }
        }
    }
    // Method to save the obstacle data to the scriptable object
    private void SaveObstacles()
    {
        Debug.Log("Saving obstacle data...");

        // Copy the local grid data to the scriptable object
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                obstacleData.gridData[x, y] = grid[x, y];
            }
        }

        // Mark the scriptable object as dirty to ensure it gets saved
        EditorUtility.SetDirty(obstacleData);
        AssetDatabase.SaveAssets(); // Save all modified assets to disk
        AssetDatabase.Refresh(); // Refresh the asset database to reflect changes
    }
}
