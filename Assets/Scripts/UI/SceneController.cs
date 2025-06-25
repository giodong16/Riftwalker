using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading;

public class SceneController : MonoBehaviour
{
    public float minLoadingTime = 2f; //  thời gian tối thiểu
    public Animator animator;
    AsyncOperation asyncLoad;
    public void LoadSceneWithTransition(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        // Hiện canvas chuyển cảnh
        //UIManager.Instance.ShowUI(CanvasName.Canvas_Transition);
        Time.timeScale = 0;
        float startTime = Time.unscaledTime;

        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Đợi cho đến khi load xong
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Đảm bảo thời gian hiển thị tối thiểu
        while (Time.unscaledTime - startTime < minLoadingTime)
        {
            yield return null;
        }

        // Cho phép chuyển cảnh => gọi animation end sau đó gắn sự kiện vô cuối
        if (animator != null) {
            //animator.SetTrigger("endTransition");
            animator.Play("End");
        }
        GameController.Instance.ResetSceneLoadFlag();

    }

    public void EndTransition()
    {
        asyncLoad.allowSceneActivation = true;
        Time.timeScale = 1.0f;
    }
}
