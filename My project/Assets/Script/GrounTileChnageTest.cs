using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GrounTileChnageTest : MonoBehaviour
{
    [SerializeField]Tilemap targetTile;
    [SerializeField]private TileBase PlowedTile;
    [SerializeField] private TileBase WetTile;

    [SerializeField]private List<Vector3Int> collectedTilePositions = new List<Vector3Int>();


    void Start()
    {
        Debug.Log("타일 변경 시작");
        CollectGameObjectTilePositions();
    }

    void Update()//
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            ChangeAllCollectedTiles();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            WateringInTile();
        }
    }

    public void CollectGameObjectTilePositions()
    {
        if(targetTile  == null)
        {
            Debug.Log("targetTile이 지정되지 않았습니다.");
            return;
        }

        collectedTilePositions.Clear();

        foreach (Transform child in targetTile.transform)
        {
            Vector3Int cellPosition = targetTile.WorldToCell(child.position);
            collectedTilePositions.Add(cellPosition);
        }
        Debug.Log($"총 {collectedTilePositions.Count}개의 자식 오브젝트 타일을 찾았습니다");
    }


    public void ChangeAllCollectedTiles()
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

        foreach(Transform child in targetTile.transform)
        {
            toDestroy.Add(child.gameObject);
        }
        foreach(GameObject obj in toDestroy)
        {
            Destroy(obj);
        }
        foreach(Vector3Int pos in collectedTilePositions)
        {
            targetTile.SetTile(pos, PlowedTile);
        }
        Debug.Log($"{collectedTilePositions.Count}개의 구역을 개간했습니다");
    }
    public void WateringInTile()
    {
        if (WetTile == null || targetTile == null)
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
            TileBase currentTile = targetTile.GetTile(pos);

            if(currentTile == PlowedTile)
            {
                targetTile.SetTile(pos, WetTile);
                wateredCount++;
            }
            else
            {
                Debug.Log("현재 타일을 개간하지 않았습니다.");
                return;
            }
            Debug.Log($"{wateredCount}개의 개간된 땅에 물을 뿌렸습니다");
        }

    }


}
