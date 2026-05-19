using UnityEngine;
using UnityEngine.Tilemaps;

public class CropPlanter : MonoBehaviour
{
    [SerializeField]
    private TileManager groundMnager;

    [SerializeField]
    private Tilemap cropTilemap;

    [SerializeField]
    private TileBase cropTile;

    [SerializeField]
    private int cropSortingOrder = 1;

    private void Start()
    {
        ConfigureCropLayer();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(groundMnager == null || cropTilemap == null || cropTile == null)
            {
                Debug.Log("필요한 컴포넌트나, 타일이 인스펙터에 지정되지 않았습니다.");
                return;
            }

            Vector3Int clickCellPos = groundMnager.GetMouseCellPosition(Input.mousePosition);

            if(groundMnager.CanPlantCrop(clickCellPos))
            {
                if(cropTilemap.HasTile(clickCellPos))
                {
                    Debug.Log("이미 심겨져 있는 타일입니다.");
                    return;
                }

                cropTilemap.SetTile(clickCellPos, cropTile);

                Debug.Log($"[{clickCellPos}] 좌표에 식물을 심었습니다");
            }
            else
            {
                Debug.Log("식물 심기 실패");
            }
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
}
