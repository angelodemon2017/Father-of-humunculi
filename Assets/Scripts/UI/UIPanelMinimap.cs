using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelMinimap : MonoBehaviour
{
    public static UIPanelMinimap Instance;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image targetImage;
    [SerializeField] private float scale = 1;

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

        for (var _x = 0; _x < texture.width; _x++)
            for (var _z = 0; _z < texture.height; _z++)
            {
                var x = _x + CameraController.Instance.FocusTile.x / scale - texture.width / 2;
                var z = _z + CameraController.Instance.FocusTile.z / scale - texture.height / 2;

                Color tempColor = Color.black;
                if ((int)(_x * scale) == (int)((texture.width / 2) * scale) &&
                    (int)(_z * scale) == (int)((texture.width / 2) * scale))
                {
                    tempColor = Color.white;
                }
                else
                {
                    var tile = GameProcess.Instance.GameWorld.GetWorldTileForMap((int)(x * scale), (int)(z * scale));
                    if (tile != null)
                    {
                        var txt = WorldViewer.Instance.GetTE(tile.Id);
                        tempColor = txt.BaseColor;

                        if (_x < texture.width * 0.4f ||
                            _x > texture.width * 0.6f ||
                            _z < texture.height * 0.4f ||
                            _z > texture.height * 0.6f)
                        {
                            tempColor.b *= tempColor.b;
                            tempColor.r *= tempColor.r;
                            tempColor.g *= tempColor.g;
                        }
                    }
                }

                texture.SetPixel(_x,
                    _z,
                    tempColor);
            }

        texture.Apply();
    }
}