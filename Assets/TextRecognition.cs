using UnityEngine;
using UnityEditor.Scripting.Python;
using UnityEditor;

using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System;

public class TextRecognition : MonoBehaviour
{
    public int resWidth = 2550;
    public int resHeight = 3300;

    private bool takeHiResShot = false;

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                            Application.dataPath,
                            width, height,
                            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    void runOCR(string filename) {
        string scriptPath = Path.Combine(Application.dataPath, "ocr.py");
        PythonRunner.SpawnClient(scriptPath, true, new string[1]{filename});
    }

    void Update()
    {
        takeHiResShot |= Input.GetKeyDown(KeyCode.Space);
        if (takeHiResShot)
        {
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            GetComponent<Camera>().targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            GetComponent<Camera>().Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            GetComponent<Camera>().targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            UnityEngine.Debug.Log(string.Format("Took screenshot to: {0}", filename));
            takeHiResShot = false;

            if (File.Exists(filename)) {
                UnityEngine.Debug.Log();
            }
            else {
                UnityEngine.Debug.Log("The file does not exist.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            runOCR("screenshots/screen_2550x3300_2022-04-26_12-02-39.png");
        }
    }
}