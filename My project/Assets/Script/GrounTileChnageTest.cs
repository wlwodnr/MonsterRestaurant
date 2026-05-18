using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GrounTileChnageTest : MonoBehaviour
{
    [SerializeField] Tilemap targetTile;
    [SerializeField]private TileBase targetChange;


    [SerializeField]private List<GameObject> collectedGameObjectTiles = new List<GameObject>();

    void Start()
    {
        Debug.Log("타일 변경 시작");
        CollectGameObjectTiles();
        ChangeAllCollectedTiles();
    }

    void Update()//
    {
        
    }

    public void CollectGameObjectTiles()
    {
        if(targetTile  == null)
        {
            Debug.Log("targetTile이 지정되지 않았습니다.");
            return;
        }

        collectedGameObjectTiles.Clear();

        foreach(Transform child in targetTile.transform)
        {
            collectedGameObjectTiles.Add(child.gameObject);
        }
        Debug.Log($"총 {collectedGameObjectTiles.Count}개의 자식 오브젝트 타일을 찾았습니다");
    }


    public void ChangeAllCollectedTiles()
    {
        if (targetChange == null)
        {
            Debug.Log("타일맵 또는 변경할 targetChange 타일이 설정되지 않았습니다.");
            return;
        }
        if(collectedGameObjectTiles.Count == 0 )
        {
            Debug.Log("변경할 오브젝트 타일이 리스트에 없습니다.");
            return;
        }

        foreach(GameObject tileObj in collectedGameObjectTiles)
        {
            if(tileObj == null)
            {
                continue;
            }
            Vector3Int cellPosition = targetTile.WorldToCell(tileObj.transform.position);

            targetTile.SetTile(cellPosition, targetChange);

            Destroy(tileObj);
        }

        Debug.Log($"{collectedGameObjectTiles.Count}개의 오브젝트 타일을 변경");

        collectedGameObjectTiles.Clear();

    }
    

}
