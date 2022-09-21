using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(RespawnSystem))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RespawnSystem respawn = (RespawnSystem)target;
        if (GUILayout.Button("Create spawn point"))
        {
            respawn.CreateSpawnPoint();
        }
    }
}
