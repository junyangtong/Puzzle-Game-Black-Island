using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadManager : MonoBehaviour
{
    public GameObject LoadScreen;
    public Slider slider;
    public Text text;

    public void LoadPart0()
    {
        StartCoroutine(LoadAsyncScene("Part0"));
    }
    public void LoadPart1()
    {
        StartCoroutine(LoadAsyncScene("Part1"));
    }
    public void LoadMenu()
    {
        // 同步加载
        SceneManager.LoadScene("Menu");
    }
    public void LoadSelectLevel()
    {
        // 同步加载
        SceneManager.LoadScene("SelectLevel");
    }
    IEnumerator LoadAsyncScene(string sceneName)
    {
        LoadScreen.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            slider.value = asyncLoad.progress;

            text.text = asyncLoad.progress * 100 + "%";

            if(asyncLoad.progress >= 0.9f)
            {
                slider.value = 1;

                text.text = "按下任意按键继续";
                if(Input.anyKeyDown)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
