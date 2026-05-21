using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; }
    //[SerializeField]
    Tilemap PlowableGroundTile;
    [SerializeField]private TileBase PlowedTile;
    [SerializeField] private TileBase WetTile;
    //[SerializeField] private CropManager cropManager;


    [SerializeField]private List<Vector3Int> collectedTilePositions = new List<Vector3Int>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log($"중복된 TileManager가 있어 파괴합니다: {gameObject.name}");
            Destroy(gameObject);
        }
        if (PlowableGroundTile == null)
        {
            GameObject tilemapObj = GameObject.Find("PlowableGroundTile");
            if (tilemapObj != null)
            {
                PlowableGroundTile = tilemapObj.GetComponent<Tilemap>();
            }
            if (PlowableGroundTile == null)
            {
                Debug.LogError("[TimeManager] 씬에서 PlowableGroundTile 오브젝트 또는 컴포넌트를 찾을 수 없습니다");
            }
        }
    }

    void Start()
    {
        Debug.Log("타일 변경 시작");
        CollectGameObjectTilePositions();
    }

    void Update()//
    {
        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    ChangeAllCollectedTiles();
        //}
        //if(Input.GetKeyDown(KeyCode.S))
        //{
        //    WateringInTile();
        //}
    }

    public void CollectGameObjectTilePositions()
    {
        if(PlowableGroundTile  == null)
        {
            Debug.Log("PlowableGroundTile이 지정되지 않았습니다.");
            return;
        }

        collectedTilePositions.Clear();

        foreach (Transform child in PlowableGroundTile.transform)
        {
            Vector3Int cellPosition = PlowableGroundTile.WorldToCell(child.position);
            collectedTilePositions.Add(cellPosition);
        }
        Debug.Log($"총 {collectedTilePositions.Count}개의 자식 오브젝트 타일을 찾았습니다");
    }


    private void ChangeAllCollectedTiles()
    {
        if (PlowedTile == null)
        {
            Debug.Log("타일맵 또는 변경할 PlowedTile 타일이 설정되지 않았습니다.");
            return;
        }
        if(collectedTilePositions.Count == 0 )
        {
            Debug.Log("변경할 오브젝트 타일이 리스트에 없습니다.");
            return;
        }
        List<GameObject> toDestroy = new List<GameObject>();

        foreach(Transform child in PlowableGroundTile.transform)
        {
            toDestroy.Add(child.gameObject);
        }
        foreach(GameObject obj in toDestroy)
        {
            Destroy(obj);
        }
        foreach(Vector3Int pos in collectedTilePositions)
        {
            PlowableGroundTile.SetTile(pos, PlowedTile);
        }
        Debug.Log($"{collectedTilePositions.Count}개의 구역을 개간했습니다");
    }
    private void WateringInTile()
    {
        if (WetTile == null || PlowableGroundTile == null)
        {
            Debug.Log("타일맵 또는  WetTile 타일이 설정되지 않았습니다.");
            return;
        }
        if(collectedTilePositions.Count == 0)
        {
            Debug.Log("탐색된 타일 좌표가 리스트에 없습니다.");
        }

        int wateredCount = 0;

        foreach(Vector3Int pos in collectedTilePositions)
        {
            TileBase currentTile = PlowableGroundTile.GetTile(pos);

            if(currentTile == PlowedTile)
            {
                PlowableGroundTile.SetTile(pos, WetTile);
                wateredCount++;
            }
            else
            {
                Debug.Log("현재 타일을 개간하지 않았습니다.");
                return;
            }
        }
        Debug.Log($"{wateredCount}개의 개간된 땅에 물을 뿌렸습니다");


    }

    public Vector3Int GetMouseCellPosition(Vector3 mousePosition)
    {
        if (PlowableGroundTile == null) return Vector3Int.zero;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPos.z = 0;

        return PlowableGroundTile.WorldToCell(worldPos);
    }

    public bool CanPlantCrop(Vector3Int cellPosition)
    {
        if (PlowableGroundTile == null)
        {
            return false;
        }
        if (!collectedTilePositions.Contains(cellPosition))
        {
            Debug.Log($"[{cellPosition}] 이 구역은 농사를 지을 수 없는 영역입니다.");
            return false;
        }

        TileBase currentTile = PlowableGroundTile.GetTile(cellPosition);

        if (currentTile == PlowedTile || currentTile == WetTile)
        {
            return true;
        }

        Debug.Log($"{currentTile} 땅은 농사를 지을 수 있는 타일이지만, 개간되지 않아 준비되지 않았습니다.");
        return false;
    }

    public void DryingGround()
    {
        int DryingCount = 0;
        if (WetTile == null || PlowableGroundTile == null)
        {
            Debug.Log("타일맵 또는  WetTile 타일이 설정되지 않았습니다.");
            return;
        }
        if (collectedTilePositions.Count == 0)
        {
            Debug.Log("탐색된 타일 좌표가 리스트에 없습니다.");
        }

        foreach (Vector3Int pos in collectedTilePositions)
        {
            TileBase currentTile = PlowableGroundTile.GetTile(pos);

            if (currentTile == WetTile)
            {
                PlowableGroundTile.SetTile(pos, PlowedTile);
                DryingCount++;
            }
            else
            {
                Debug.Log("땅이 젖어있지 않아 변화가 없습니다.");
                return;
            }
        }
        Debug.Log($"{DryingCount}개의 개간된 땅이 매말랐습니다.");
        if(DryingCount != 0)
        {
            CropManager.Instance.GrowCrop();
        }

    }

    public void RequestedPlowing()
    {
        ChangeAllCollectedTiles();
    }
    public void RequestedWatering()
    {
        WateringInTile();
    }


}
