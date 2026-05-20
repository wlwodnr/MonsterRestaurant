using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class CropManager : MonoBehaviour
{

    public static CropManager Instance {  get; private set; }

    
    [SerializeField]
    private Tilemap cropTilemap;

    [SerializeField]
    private int cropSortingOrder = 1;

    private Dictionary<Vector3Int, string> plantedCropIds = new Dictionary<Vector3Int, string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log($"중복된 CropManager가 있어 파괴합니다: {gameObject.name}");
            Destroy(gameObject);
            return;
        }

        if(cropTilemap == null)
        {
            GameObject tilemapObj = GameObject.Find("CropTilemap");
            if(tilemapObj != null)
            {
                cropTilemap = tilemapObj.GetComponent<Tilemap>();
            }
            if(cropTilemap == null)
            {
                Debug.LogError("[CropManager] 씬에서 CropTilemap 오브젝트 또는 컴포넌트를 찾을 수 없습니다");
            }
        }

    }


    private void Start()
    {
        ConfigureCropLayer();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PlantCrop();
        }
    }

    private void PlantCrop()
    {
        if (TileManager.Instance == null || cropTilemap == null)
        {
            Debug.Log("필요한 컴포넌트나, 타일이 인스펙터에 지정되지 않았습니다.");
            return;
        }

        Vector3Int clickCellPos = TileManager.Instance.GetMouseCellPosition(Input.mousePosition);

        if (TileManager.Instance.CanPlantCrop(clickCellPos))
        {
            if (cropTilemap.HasTile(clickCellPos))
            {
                Debug.Log("이미 심겨져 있는 타일입니다.");
                return;
            }

            //cropTilemap.SetTile(clickCellPos, cropGrowthStages[0]);

            //plantedCropStages.Add(clickCellPos, 0);

            string startCropId = "Potato_Level_0";

            CropData cropData = GameDataManager.Instance.GetCropData(startCropId);

            if(cropData == null)
            {
                Debug.LogError($"[CropManager] {startCropId}에 해당하는 데이터를 GameDataManager에서 찾을 수 없습니다");
                return;
            }

            TileBase seedTile = Resources.Load<TileBase>(cropData.IconPath);

            if(seedTile == null)
            {
                Debug.LogError($"[CropManager] 리소스 로드 실패. 경로를 확인하세요: Resources/{cropData.IconPath}");
                return;
            }

            cropTilemap.SetTile(clickCellPos, seedTile);
            plantedCropIds.Add(clickCellPos, startCropId);


            Debug.Log($"[{clickCellPos}]좌표에 씨앗 타일을 심고 등록했습니다.");
            
        }
        else
        {
            Debug.Log("식물 심기 실패");
        }
    }

    private void ConfigureCropLayer()
    {
        if(cropTilemap != null)
        {
            TilemapRenderer renderer = cropTilemap.GetComponent<TilemapRenderer>();

            if(renderer != null)
            {
                renderer.sortingOrder = cropSortingOrder;
                Debug.Log($"식물 타일맵의 SortingOrder가 {cropSortingOrder}로 설정되어 있습니다.");
            }
        }
    }


    public void GrowCrop()
    {
        if (TileManager.Instance == null || cropTilemap == null)
        {
            Debug.Log("필요한 컴포넌트나, 타일이 인스펙터에 지정되지 않았습니다.");
            return;
        }
        if(plantedCropIds.Count == 0)
        {
            Debug.Log("장부에 심겨진 식물이 없어 성장을 진행하지 않습니다");
            return;
        }

        List<Vector3Int> cropPositions = new List<Vector3Int>(plantedCropIds.Keys);
        int grownCount = 0;

        foreach(Vector3Int pos in cropPositions)
        {
            string currentId = plantedCropIds[pos];
            CropData currentData = GameDataManager.Instance.GetCropData(currentId);

            if (currentData == null)
            {
                continue;
            }
            
            if(string.IsNullOrEmpty(currentData.NextLevelID))
            {
                Debug.Log("성장을 다 했거나, 다음 성장이 설정되지 않았습니다.");
                continue;
            }

            string nextId = currentData.NextLevelID;
            CropData nextData = GameDataManager.Instance.GetCropData(nextId);

            if(nextData == null)
            {
                Debug.Log($"[CropManager] 다음 성장 ID [{nextId} 데이터를 찾을 수 없습니다. 데이터 시트에 해당 ID값을 가진 데이터가 있는지 확인해주세요.]");
                continue;
            }

            TileBase nextTile = Resources.Load<TileBase>(nextData.IconPath);

            if(nextTile == null)
            {
                Debug.Log($"[CropManager] 리소스 로드 실패! 경로: Resources/{nextData.IconPath}");
                continue;
            }


            cropTilemap.SetTile(pos, nextTile);
            plantedCropIds[pos] = nextId;

            grownCount++;


        }
        Debug.Log($"{grownCount}개의 식물 타일이 성장하였습니다");
        
    }
}
