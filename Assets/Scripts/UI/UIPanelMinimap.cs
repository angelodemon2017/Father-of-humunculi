using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelMinimap : MonoBehaviour
{
    public static UIPanelMinimap Instance;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image targetImage;

    private Texture2D texture;
    private int textureWidth => (int)_rectTransform.sizeDelta.x;
    private int textureHeight => (int)_rectTransform.sizeDelta.y;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(Init());
    }

    public IEnumerator Init()
    {
        yield return new WaitForSeconds(0.1f);

        texture = new Texture2D(textureWidth, textureHeight);

        PaintOnlyGenerate();

        targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    private void PaintOnlyGenerate()
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int z = 0; z < texture.height; z++)
            {
                texture.SetPixel(x, z, Color.black);
            }
        }

        foreach (var t in GameProcess.Instance.GameWorld.worldTileDatas)
        {
            var x = t.Xpos + textureWidth / 2 - CameraController.Instance.FocusTile.x;
            var z = t.Zpos + textureHeight / 2 - CameraController.Instance.FocusTile.z;

            if (x < 0 || z < 0 || x >= texture.width || z >= texture.height)
            {
                continue;
            }

            var txt = WorldViewer.Instance.GetTE(t.Id);
            var tempColor = txt.BaseColor;
            var isNearTile = t.Xpos < (CameraController.Instance.FocusTile.x + Config.VisibilityChunkDistance * Config.ChunkTilesSize) && 
                t.Xpos > (CameraController.Instance.FocusTile.x - Config.VisibilityChunkDistance * Config.ChunkTilesSize) &&
                t.Zpos < (CameraController.Instance.FocusTile.z + Config.VisibilityChunkDistance * Config.ChunkTilesSize) &&
                t.Zpos > (CameraController.Instance.FocusTile.z - Config.VisibilityChunkDistance * Config.ChunkTilesSize);
            if (!isNearTile)
            {
                tempColor.b *= tempColor.b;
                tempColor.r *= tempColor.r;
                tempColor.g *= tempColor.g;
            }
            if (t.Xpos == CameraController.Instance.FocusTile.x && t.Zpos == CameraController.Instance.FocusTile.z)
            {
                tempColor = Color.white;
            }
            texture.SetPixel(x,
                z,
                tempColor);
        }

        texture.Apply();
    }
}