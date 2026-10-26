using UnityEngine;
using System.Collections;

public class ScreenshotHandler : MonoBehaviour
{
    public GameObject UI;

    private IEnumerator Screenshot()
    {
        yield return new WaitForEndOfFrame();

        // Desired output resolution
        int targetWidth = 1920;
        int targetHeight = 1080;

        // Take a full-screen screenshot first
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenTexture.Apply();

        // Resize to 1920x1080
        Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGB24, false);
        Color[] pixels = screenTexture.GetPixels();
        Color[] resizedPixels = resizedTexture.GetPixels();

        float ratioX = (float)Screen.width / targetWidth;
        float ratioY = (float)Screen.height / targetHeight;

        for (int y = 0; y < targetHeight; y++)
        {
            for (int x = 0; x < targetWidth; x++)
            {
                int px = Mathf.FloorToInt(x * ratioX);
                int py = Mathf.FloorToInt(y * ratioY);
                resizedPixels[y * targetWidth + x] = pixels[py * Screen.width + px];
            }
        }

        resizedTexture.SetPixels(resizedPixels);
        resizedTexture.Apply();

        // Save to gallery
        string name = "Screenshot_FurTastic_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        NativeGallery.SaveImageToGallery(resizedTexture, "Furtastic Pictures", name);

        Destroy(screenTexture);
        Destroy(resizedTexture);

        UI.SetActive(true);
    }

    public void TakeScreenshot()
    {
        UI.SetActive(false);
        StartCoroutine(Screenshot());
    }
}
