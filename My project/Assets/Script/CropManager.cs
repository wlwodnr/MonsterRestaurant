using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class CropManager : MonoBehaviour
{

    public static CropManager Instance {  get; private set; }

    //[SerializeField]
    //private TileManager TileManager;

    [SerializeField]
    private Tilemap cropTilemap;

    [SerializeField]
    private int cropSortingOrder = 1;

    [SerializeField]
    private TileBase[] cropGrowthStages;

    private Dictionary<Vector3Int, int> plantedCropStages = new Dictionary<Vector3Int, int>();

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
        if (TileManager.Instance == null || cropTilemap == null || cropGrowthStages == null || cropGrowthStages.Length == 0)
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

            cropTilemap.SetTile(clickCellPos, cropGrowthStages[0]);

            plantedCropStages.Add(clickCellPos, 0);

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
        if (TileManager.Instance == null || cropTilemap == null || cropGrowthStages == null)
        {
            Debug.Log("필요한 컴포넌트나, 타일이 인스펙터에 지정되지 않았습니다.");
            return;
        }
        if(plantedCropStages.Count == 0)
        {
            Debug.Log("장부에 심겨진 식물이 없어 성장을 진행하지 않습니다");
            return;
        }
        List<Vector3Int> cropPosition = new List<Vector3Int>(plantedCropStages.Keys);
        int grownCount = 0;

        foreach(Vector3Int pos in cropPosition)
        {
            int currentStage = plantedCropStages[pos];

            if(currentStage < cropGrowthStages.Length - 1)
            {
                int nextStage = currentStage + 1;
                plantedCropStages[pos] = nextStage;

                cropTilemap.SetTile(pos, cropGrowthStages[nextStage]);

                grownCount++;
            }

        }
        Debug.Log($"{grownCount}개의 식물 타일이 성장하였습니다");
        
    }
}
