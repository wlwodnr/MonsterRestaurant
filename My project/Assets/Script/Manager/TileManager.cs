using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; }


    [SerializeField]
    private List<PlowableTile> collectedTiles = new List<PlowableTile>();
    


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

    }

    void Start()
    {
        CollectAllTiles();
        
    }

    void Update()
    {

    }
    public void CollectAllTiles()
    {
        collectedTiles.Clear();
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Plowable");

        if (taggedObjects == null)
        {
            Debug.Log("개간 가능한 땅을 찾을 수 없습니다. 태그를 확인하거나, 개간 가능한 땅을 확인하세요");
            return;
        }
        else
        {
            foreach (GameObject obj in taggedObjects)
            {
                PlowableTile tile = obj.GetComponent<PlowableTile>();
                if(tile != null)
                {
                    collectedTiles.Add(tile);
                }
            }
            Debug.Log($"[TileManager]: 'Plowable' 태그를 통해 총 {collectedTiles.Count}개의 타일 객체를 찾았습니다.");
        }
    }

    public void DryingGround()
    {
        int dryingCount = 0;
        foreach(PlowableTile tile in collectedTiles)
        {
            if (tile.CurrentState == PlowableTile.TileState.Wet)
            {
                tile.ChangeState(PlowableTile.TileState.Plowed);
                dryingCount++;
            }
        }
        Debug.Log($"{dryingCount}개의 타일이 메말랐습니다.");
        if (dryingCount != 0 && CropManager.Instance != null)
        {
            CropManager.Instance.GrowCrop();
        }

    }

    public bool CanPlantCrop(GameObject targetObj)
    {
        if(targetObj == null)
        {
            return false;
        }
        PlowableTile tile = targetObj.GetComponent<PlowableTile>();
        if (tile == null)
        {
            return false;
        }
        return (tile.CurrentState == PlowableTile.TileState.Plowed || tile.CurrentState == PlowableTile.TileState.Wet);
    }


    public void RequestedPlowing(GameObject targetObj)
    {
        //ChangeAllCollectedTiles();
        if(targetObj == null)
        {
            Debug.Log("현재 개간할 수 있는 타일을 찾지 못했습니다.");
            return;
        }
        
        PlowableTile tile = targetObj.GetComponent<PlowableTile>();

        if (tile != null)
        {
            if(tile.CurrentState == PlowableTile.TileState.Nomal)
            {
                tile.ChangeState(PlowableTile.TileState.Plowed);
                Debug.Log("선택한 타일을 개간했습니다. - Ground_1로 스프라이트 변경 및 TileState를 Plowed로 변경");
            }
            else
            {
                Debug.Log("이미 타일이 개간되었을지도 모릅니다?");
            }
        }
    }


    public void RequestedWatering(GameObject targetObj)
    {
        if(targetObj == null )
        {
            return;
        }
        PlowableTile tile = targetObj.GetComponent<PlowableTile> ();

        if (tile.CurrentState == PlowableTile.TileState.Plowed)
        {
            tile.ChangeState(PlowableTile.TileState.Wet);
            Debug.Log("선택한 타일에 물을 줬습니다. - Ground_3로 스프라이트 변경 및 TileState를 Wet으로 변경");
        }
        else
        {
            Debug.Log("개간된 땅이 범위내에 없는 것 같습니다.");
        }
    }


}
