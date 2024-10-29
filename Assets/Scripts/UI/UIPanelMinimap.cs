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

        PaintTexture();

        targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    private void PaintTexture()
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int z = 0; z < texture.height; z++)
            {
                var id = WorldConstructor.GetIdtextureByPerlin(
                    x - textureWidth / 2 + (int)(CameraController.Instance.FocusPosition.x / Config.TileSize),
                    z - textureHeight / 2 + (int)(CameraController.Instance.FocusPosition.z / Config.TileSize),
                    GameProcess.Instance.GameWorld.Seed);
                var txt = WorldViewer.Instance.Textures[(int)id];
                texture.SetPixel(x, z, txt.BaseColor);
            }
        }

        texture.Apply();
    }
}