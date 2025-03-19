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
        // �� �ʱ�ȭ
        consolColor = new Color(255 / 255, 255 / 255, 255 / 255, 0);
        consol.color = consolColor;

        // �۾� ���̱� ����
        StartCoroutine(StartCutScene());
    }

    private IEnumerator StartCutScene()
    {
        // �ؽ�Ʈ �Ҵ�
        consol.text = "Project CC\n\n���� : ���¾�";

        // �������
        while (consolColor.a < 1)
        {
            consolColor.a += Time.deltaTime;
            consol.color = consolColor;
            yield return null;
        }

        // ���
        yield return new WaitForSeconds(2f);

        // ��ο�����
        while (consolColor.a > 0)
        {
            consolColor.a -= Time.deltaTime;
            consol.color = consolColor;
            yield return null;
        }      

        SceneManager.LoadScene(1);
    }
}
