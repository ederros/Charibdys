using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
    using UnityEditor;
#endif
public class FloorTile : Tile
{
    [SerializeField]
    List<Sprite> sprites;

    [SerializeField]
    public uint turnsRequired = 1;
    
    [SerializeField]
    float defenceOffset = 0;

    [SerializeField]
    int sightOffset = 0;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if(sprites.Count>0) sprite = sprites[Random.Range(0,sprites.Count)];
        return base.StartUp(position, tilemap, go);
    }

    public void OnEntityStep(EntityBehaviour entity){
        
        entity.sightRadius += sightOffset;
        entity.armor += defenceOffset;
    }

    public void OnEntityUnStep(EntityBehaviour entity){
        entity.sightRadius -= sightOffset;
        entity.armor -= defenceOffset;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Costum Tiles/Floor Tile")]
    public static void CreateAndSave(){
        string path = EditorUtility.SaveFilePanelInProject("Save tile", "new tile", "Asset","Save tile","Assets");
        if(path=="") return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<FloorTile>(),path);
    }
#endif
}
