using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI gameOverText;
    private float halfCanvasWidth;
    void Start()
    {
        // このコンポーネントがCanvasについているのを前提とする
        this.halfCanvasWidth = GetComponent<UnityEngine.UI.CanvasScaler>().referenceResolution.x / 2;
        gameOverText.text = "";
    }
    void Update()
    {
        // ゲームオーバー判定
        var longest = GetLongRunSoldier();
        if (longest == null) return;
        var longestPosX = longest.GetComponent<RectTransform>().anchoredPosition.x;
        if (this.halfCanvasWidth < longestPosX)
        {
            StartCoroutine("GameOverProcess", "WIN : A");
        } else
        if (longestPosX < this.halfCanvasWidth * -1f)
        {
            StartCoroutine("GameOverProcess", "WIN : B");
        }
    }
    IEnumerator GameOverProcess(string text)
    {
        gameOverText.text = text;
        // 動き止める
        var players = FindObjectsOfType<PlayerInput>();
        foreach (var player in players) player.enabled = false;
        var soldiers = GameObject.FindGameObjectsWithTag("Soldier");
        foreach (var soldier in soldiers) soldier.GetComponent<Soldier>().enabled = false;
        // --
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private GameObject GetLongRunSoldier()
    {
        var soldiers = GameObject.FindGameObjectsWithTag("Soldier");
        var longest = 0f;
        GameObject candidate = null;
        foreach(var soldier in soldiers)
        {
            var distance = Mathf.Abs(soldier.GetComponent<RectTransform>().anchoredPosition.x);
            if (longest < distance)
            {
                longest = distance;
                candidate = soldier;
            }
        }
        return candidate;
    }
}
