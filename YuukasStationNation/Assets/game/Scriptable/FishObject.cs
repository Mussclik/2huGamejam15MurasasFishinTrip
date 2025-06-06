using System;
using UnityEngine;
using UnityEditor.TerrainTools;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Fish", menuName = "Fishing/Fish Object"), Serializable]
public class FishObject : ScriptableObject
{
    public int fishID;
    public int difficulty;

    public float baseSize;
    public float baseWeight;
    public float baseFishPrice;
    public string fishName;
    [TextArea] public string description;
    public string refrence;

    public Sprite fishImage; // Use Sprite for UI, but you can switch to Texture2D if needed

    public Biome biome;
    
    public FishModifiers modifiers = new FishModifiers();

    public float Size
    {
        get { return Mathf.Clamp(baseSize * modifiers.size, baseSize * 0.1f, baseSize * 500f);}
    }
    public float Weight
    {
        get { return Mathf.Clamp(baseWeight * modifiers.weight, baseWeight * 0.1f, baseWeight * 500f); }
    }
    public float FishPrice
    {
        get { return Mathf.Clamp(baseFishPrice * modifiers.price, baseFishPrice * 0.1f, baseFishPrice * 500f); }
    }
}

public class FishModifiers
{
    public float weight = 1;
    public float size = 1;
    public float price = 1;
    public float terrainModifier = 1;
    public float TerrainDevience = 0;
    public int slotsMultiplier = 1;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="min">minimum modifier</param>
    /// <param name="max">maximum modifier</param>
    public void GenerateModifiers(float min = 0.8f, float max = 1.2f)
    {
        weight = UnityEngine.Random.Range(min, max);
        weight *= (weight > 1) ? 0.5f : 2f;
        size = UnityEngine.Random.Range(min, max);
        weight *= (size > 1) ? 0.5f : 2f;
        price = UnityEngine.Random.Range(min, max);
        weight *= (price > 1) ? 0.5f : 2f;


        TerrainDevience = Mathf.Abs(TerrainDevience);
        terrainModifier = 1 + UnityEngine.Random.Range(-TerrainDevience, TerrainDevience);

        weight *= terrainModifier * slotsMultiplier;
        size *= terrainModifier * slotsMultiplier;
        price *= terrainModifier * slotsMultiplier;

    }

}

#if UNITY_EDITOR

[CustomEditor(typeof(FishObject))]
public class FishObject_Editor : Editor
{
    private FishObject fishObject;

    private void OnEnable()
    {
        fishObject = (FishObject)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (fishObject.fishImage == null) return;
        
        Texture2D sprite = AssetPreview.GetAssetPreview(fishObject.fishImage);

        GUILayout.Label(sprite, EditorStyles.boldLabel);
        //GUI.DrawTexture(GUILayoutUtility.GetLastRect(), sprite);
    }

}

#endif