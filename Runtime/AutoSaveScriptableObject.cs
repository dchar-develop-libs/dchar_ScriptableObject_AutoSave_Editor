#region 用于 ScriptableObject 自动保存 
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AutoSaveScriptableObject
{
    static AutoSaveScriptableObject()
    {
        // 注册保存事件
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        EditorApplication.quitting += OnEditorQuitting;
    }
    
    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            SaveAllScriptableObjects();
        }
    }
    
    private static void OnEditorQuitting()
    {
        SaveAllScriptableObjects();
    }
    
    private static void SaveAllScriptableObjects()
    {
        // 查找所有 ScriptableObject
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject");
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            
            if (obj != null && EditorUtility.IsDirty(obj))
            {
                EditorUtility.SetDirty(obj);
            }
        }
        
        AssetDatabase.SaveAssets();
        Debug.Log("自动保存完成！");  
    }

}
#endif
#endregion