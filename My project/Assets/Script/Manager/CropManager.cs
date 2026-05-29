using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class CropManager : MonoBehaviour
{

    public static CropManager Instance {  get; private set; }

    
    //[SerializeField]
    private Tilemap cropTilemap;

    [SerializeField]
    private int cropSortingOrder = 1;

    //private Dictionary<Vector3Int, string> plantedCropIds = new Dictionary<Vector3Int, string>();

    private Dictionary<Vector3Int, CropObject> plantedCrops = new Dictionary<Vector3Int, CropObject> ();

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
        if(Input.GetMouseButtonUp(0))
        {
            TryPlantSeedAtMouse();
        }
    }

    private void TryPlantSeedAtMouse()
    {
        if (TileManager.Instance == null || cropTilemap == null)
        {
            Debug.Log("필요한 컴포넌트나, 타일이 인스펙터에 지정되지 않았습니다.");
            return;
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        Collider2D hitCollider = Physics2D.OverlapPoint(mousePos2D);

        if (hitCollider == null)
        {
            Debug.Log("클릭한 곳에 오브젝트가 없습니다.");
            return;
        }

        GameObject targetTileObj = hitCollider.gameObject;

        if (TileManager.Instance.CanPlantCrop(targetTileObj))
        {
            Vector3Int cellPos = cropTilemap.WorldToCell(targetTileObj.transform.position);

            if (plantedCrops.ContainsKey(cellPos))
            {
                Debug.Log("이미 심겨져 있는 타일입니다.");
                return;
            }

            string startCropId = "Potato_Level_0";
            PlantNewCropPrefab(cellPos, startCropId);
        }
        else
        {
            Debug.Log("이 땅은 개간되지 않았거나 식물을 심을 수 없는 땅입니다.");
        }        
    }

    private void PlantNewCropPrefab(Vector3Int cellPos, string cropId)
    {
        CropData cropData = GameDataManager.Instance.GetCropData(cropId);

        if(cropData == null)
        {
            Debug.Log($"[CropManager] {cropId}에 해당하는 데이터를 GameDataManager에서 찾을 수 없습니다.\nCrop의 Json파일에 해당 아이디를 갖고 있는 데이터가 있는지 확인해 주세요");
            return;
        }
        GameObject cropPrefab = Resources.Load<GameObject>(cropData.IconPath);

        if(cropPrefab == null)
        {
            Debug.Log($"[CropManager] 프리팹 리소스 로드 실패. 경로 확인 혹은 파일의 이름 확인해주세요 {cropData.IconPath}");
            return;
        }

        Vector3 spawnWorldPos = cropTilemap.GetCellCenterWorld(cellPos);
        GameObject spawnedObj = Instantiate(cropPrefab, spawnWorldPos, Quaternion.identity);

        CropObject cropObj = spawnedObj.GetComponent<CropObject>();

        if(cropObj == null)
        {
            cropObj = spawnedObj.AddComponent<CropObject>();
        }
        cropObj.Initialize(cellPos, cropId);

        SpriteRenderer sRenderer = spawnedObj.GetComponentInChildren<SpriteRenderer>();
        if(sRenderer != null)
        {
            sRenderer.sortingOrder = cropSortingOrder;
        }

        plantedCrops.Add(cellPos, cropObj);
        Debug.Log($"[{cellPos}] 좌표에 [{cropId}] 오브젝트를 생성하고 장부에 등록했습니다.");
    }

    public void HandleCropInteraction(CropObject clickedCrop)
    {
        CropData currentData = GameDataManager.Instance.GetCropData(clickedCrop.CropId);

        if(currentData == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(currentData.NextLevelID))
        {
            HarvestCrop(clickedCrop, currentData);
        }
        else
        {
            Debug.Log($"[CropManager] {currentData.Name}은 아직 자라는 중입니다.");
        }

    }

    private void HarvestCrop(CropObject cropObj, CropData data)
    {
        string itemID = cropObj.CropId.Split('_')[0];
        int rewardCount = 1;

        if(InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemID, rewardCount);
            Debug.Log($"인벤토리에 수확물 {data.Name}이 추가되었습니다.");
        }
        else
        {
            Debug.Log("인벤토리 메니저 없음. 추가요망");
        }

        if(plantedCrops.ContainsKey(cropObj.CellPosition))
        {
            plantedCrops.Remove(cropObj.CellPosition);
        }

        Destroy(cropObj.gameObject);
        Debug.Log($"[{cropObj.CellPosition}] 좌표의 식물을 성공적으로 수확하고 제거했습니다.");


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
        if(plantedCrops.Count == 0)
        {
            Debug.Log("장부에 심겨진 식물이 없어 성장을 진행하지 않습니다");
            return;
        }

        List<Vector3Int> cropPositions = new List<Vector3Int>(plantedCrops.Keys);
        int grownCount = 0;

        foreach(Vector3Int pos in cropPositions)
        {
            CropObject currentCropObj = plantedCrops[pos];
            string currentId = currentCropObj.CropId;

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


            Destroy(currentCropObj.gameObject); 
            plantedCrops.Remove(pos);
            PlantNewCropPrefab(pos, nextId);
            
            
            grownCount++;

        }
        Debug.Log($"{grownCount}개의 식물 타일이 성장하였습니다");
        
    }
}
