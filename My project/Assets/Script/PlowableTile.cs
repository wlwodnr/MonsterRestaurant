using UnityEngine;

public class PlowableTile : MonoBehaviour
{
    public enum TileState
    {
        Nomal,
        Plowed,
        Wet,
        None,
    }
    public TileState CurrentState { get; private set; }

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CurrentState = SetCurrentTileState();
    }


    private TileState SetCurrentTileState()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null)
        {
            Debug.Log("[PlowableFiled]: 현재 타일의 스프라이트 렌더러 혹은 스프라이트가 할당되지 않았습니다.");
            return TileState.None;
        }
        string spriteName = spriteRenderer.sprite.name;

        if (spriteName == "Ground_2")
        {
            return TileState.Nomal;
        }
        else if (spriteName == "Ground_1")
        {
            return TileState.Plowed;
        }
        else if (spriteName == "Ground_3")
        {
            return TileState.Wet;
        }
        else
        {
            Debug.Log($"[PlowableFiled]: {spriteName}은 설정된 스프라이트의 이름과 일치하지 않습니다. 코드 혹은 스프라이트 렌더러에 들어간 스프라이트의 이름을 확인해주세요.");
            return TileState.None;
        }
    }

    public void ChangeState(TileState newstate)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        string sheetPath = "GroundTile/Ground";
        string targetSpriteName = "";
        switch (newstate)
        {
            case TileState.Nomal:
                {
                    targetSpriteName += "Ground_2";
                    break;
                }
            case TileState.Plowed:
                {
                    targetSpriteName += "Ground_1";
                    break;
                }
            case TileState.Wet:
                {
                    targetSpriteName += "Ground_3";
                    break;
                }
            case TileState.None:
            default:
                {
                    Debug.Log($"[PlowableFiled]: 잘못된 상태입니다 {targetSpriteName}는 사전 설정되지 않은 값입니다. 변경을 취소합니다.");
                    return;
                }
        }

        Sprite[] allSprites = Resources.LoadAll<Sprite>(sheetPath);
        bool isLoaded = false;
        if(allSprites != null && allSprites.Length > 0)
        {
            foreach(Sprite sprite in allSprites)
            {
                if(sprite.name == targetSpriteName)
                {
                    spriteRenderer.sprite = sprite;
                    CurrentState = newstate; 
                    isLoaded = true;
                    break;
                }
            }
        }
        if (!isLoaded)
        {
            Debug.LogError($"[PlowableTile]: {sheetPath} 파일 안에서 '{targetSpriteName}' 자식 스프라이트를 찾을 수 없습니다. 인스펙터 슬라이싱 이름을 확인하세요.");
        }
    }
}