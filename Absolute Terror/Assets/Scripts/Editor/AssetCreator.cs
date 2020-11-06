using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetCreator : MonoBehaviour
{
    [MenuItem("Assets/Create/ScriptableObjects/Job")]
    public static void CreateJob()
    {
        Job jobAsset = ScriptableObject.CreateInstance<Job>();
        AssetDatabase.CreateAsset(jobAsset, "Assets/Prefabs/Jobs/NewScriptableObjects.asset");
        AssetDatabase.SaveAssets();
        jobAsset.InitStats();
        Selection.activeObject = jobAsset;
    }
    [MenuItem("Assets/Create/ScriptableObjects/ModifierCondition/Job")]
    public static void CreateModifierConditionJob()
    {
        ModifierConditionJob modifierAsset = ScriptableObject.CreateInstance<ModifierConditionJob>();
        AssetDatabase.CreateAsset(modifierAsset, "Assets/Prefabs/Modifiers/NewScriptableObjects.asset");
        AssetDatabase.SaveAssets();
        Selection.activeObject = modifierAsset;
    }
    [MenuItem("Assets/Create/ScriptableObjects/ModifierCondition/Elemental")]
    public static void CreateModifierConditionElemental()
    {
        ModifierConditionElemental modifierAsset = ScriptableObject.CreateInstance<ModifierConditionElemental>();
        AssetDatabase.CreateAsset(modifierAsset, "Assets/Prefabs/Modifiers/NewScriptableObjects.asset");
        AssetDatabase.SaveAssets();
        Selection.activeObject = modifierAsset;
    }
    [MenuItem("Assets/Create/ScriptableObjects/ModifierCondition/Weapon")]
    public static void CreateModifierConditionWeapon()
    {
        ModifierConditionWeapon modifierAsset = ScriptableObject.CreateInstance<ModifierConditionWeapon>();
        AssetDatabase.CreateAsset(modifierAsset, "Assets/Prefabs/Modifiers/NewScriptableObjects.asset");
        AssetDatabase.SaveAssets();
        Selection.activeObject = modifierAsset;
    }
}
