using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartScene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI consol;

    private Color consolColor;

    private void Start()
    {
        // 색 초기화
        consolColor = new Color(255 / 255, 255 / 255, 255 / 255, 0);
        consol.color = consolColor;

        // 글씨 보이기 시작
        StartCoroutine(StartCutScene());
    }

    private IEnumerator StartCutScene()
    {
        // 텍스트 할당
        consol.text = "Project CC\n\n개발 : 이태언";

        // 밝아지기
        while (consolColor.a < 1)
        {
            consolColor.a += Time.deltaTime;
            consol.color = consolColor;
            yield return null;
        }

        // 대기
        yield return new WaitForSeconds(2f);

        // 어두워지기
        while (consolColor.a > 0)
        {
            consolColor.a -= Time.deltaTime;
            consol.color = consolColor;
            yield return null;
        }      

        SceneManager.LoadScene(1);
    }
}
