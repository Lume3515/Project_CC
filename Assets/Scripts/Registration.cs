using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class Registration : MonoBehaviour
{

    #region// ���
    // ���ȭ��
    [SerializeField] GameObject backGround;

    // ������� �Է� �޾ƿ���   
    [SerializeField] TMP_InputField[] input;


    private GameObject input3_Obj;

    private TextMeshProUGUI placeholder2;
    private TextMeshProUGUI placeholder3;

    [SerializeField] TextMeshProUGUI continueButton_TMP;

    private void Start()
    {
        //Debug.Log(input[1].transform.Find("Placeholder") == null);

        input3_Obj = input[2].gameObject;
        placeholder2 = input[1].GetComponentsInChildren<TextMeshProUGUI>(true).FirstOrDefault(t => t.gameObject.name == "Placeholder");
        placeholder3 = input[2].GetComponentsInChildren<TextMeshProUGUI>(true).FirstOrDefault(t => t.gameObject.name == "Placeholder");

        info_Color = new Color(99 / 255f, 221 / 255f, 211 / 255f, 0 / 255f);
        infoBG.color = info_Color;
        info_TMP.color = info_Color;
        //isDameSound = true;     
    }

    #endregion

    #region// ��ư

    // �α��� ��ư
    public void LogInBT()
    {
        backGround.SetActive(true);
        input3_Obj.SetActive(false);

        input[1].contentType = TMP_InputField.ContentType.Password;
        placeholder2.text = "��й�ȣ �Է�";
        ChangeExitBTName("�α���");

        CleanText();


    }

    // �г��� ���� ��ư
    //public void ChangeNickNameBT()
    //{
    //    backGround.SetActive(true);
    //    input3_Obj.SetActive(true);

    //    placeholder2.text = "��й�ȣ �Է�";
    //    placeholder3.text = "������ �г���";

    //    input[1].contentType = TMP_InputField.ContentType.Password;
    //    input[2].contentType = TMP_InputField.ContentType.Standard;
    //    ChangeExitBTName("�г��� ����");

    //    CleanText();
    //}

    // ȸ������ ��ư
    public void SignUpBT()
    {
        backGround.SetActive(true);
        input3_Obj.SetActive(true);

        placeholder2.text = "��й�ȣ �Է�";
        placeholder3.text = "��й�ȣ Ȯ��";

        input[1].contentType = TMP_InputField.ContentType.Password;
        input[2].contentType = TMP_InputField.ContentType.Password;
        ChangeExitBTName("ȸ������");

        CleanText();
    }

    // ����ϱ� ��ư
    public void ContinueBT()
    {


        switch (continueButton_TMP.text)
        {
            case "�α���":

                LogIn();
                break;

            case "�г��� ����":
                ChangeNickName();
                break;

            case "ȸ������":
                SignUp();
                break;
        }
    }


    public void Exit()
    {
        backGround.SetActive(false);
    }

    #endregion

    #region// ���
    private bool fuctionReturn;

    private void TextPattern(string text, int number)
    {
        if (!Regex.IsMatch(text, "^.{1,8}$")) // 8�� ���ϸ� ����
        {
            ChangeText("8�� ���ϸ� ��밡���մϴ�.");
            fuctionReturn = true;
            StartCoroutine(ShowText());
        }
        else if (!Regex.IsMatch(text, "^[^\\s]{1,8}$"))// ���� ����
        {
            ChangeText("����� ������ �ʽ��ϴ�.");
            fuctionReturn = true;
            StartCoroutine(ShowText());
        }
        else if (!Regex.IsMatch(text, "^[^��-�R��-����-��]*$") && number == 1) // ������� / �ѱ� ����
        {
            ChangeText("����� �ѱ��� ����Ͻ� �� �����ϴ�.");
            fuctionReturn = true;
            StartCoroutine(ShowText());
        }
        else
        {
            fuctionReturn = false;
        }
    }

    private void ChangeExitBTName(string btName)
    {
        continueButton_TMP.text = btName;
    }

    // �ؽ�Ʈ ����
    private void CleanText()
    {
        for (int i = 0; i < input.Length; i++)
        {
            input[i].text = string.Empty;
        }
    }

    private string nickName;

    private void LogIn()
    {
        TextPattern(input[0].text, 0); // id
        TextPattern(input[1].text, 1); // pw

        if (fuctionReturn)
        {
            changeNickName = false;
            return;
        }

        var bro = Backend.BMember.CustomLogin(input[0].text, input[1].text);

        if (bro.IsSuccess())
        {
            Debug.Log("�α����� �����߽��ϴ�. : " + bro);

            Manager.User_Info.id = input[0].text;
            Manager.User_Info.pw = input[1].text;
            //Manager.User_Info.nickName_Info = Backend.UserNickName;
            //Debug.Log(RegistrationInfo.nickName_info);

            if (changeNickName) return;

            if (input[0].text == Manager.Manager_Info.id && input[1].text == Manager.Manager_Info.pw)
                SceneManager.LoadScene(3);
            else
                SceneManager.LoadScene(2);
        }
        else if (!changeNickName)
        {
            ChangeText("���̵� ��й�ȣ�� Ʋ�Ƚ��ϴ�.");
            StartCoroutine(ShowText());
            changeNickName = false;
        }
    }


    private void SignUp()
    {
        TextPattern(input[0].text, 0); // id
        TextPattern(input[1].text, 1); // pw

        if (fuctionReturn) return;

        if (input[1].text != input[2].text) // ���Ȯ���� ������?
        {
            ChangeText("��й�ȣ Ȯ���� Ʋ�Ƚ��ϴ�.");
            StartCoroutine(ShowText());
            return;
        }

        var bro = Backend.BMember.CustomSignUp(input[0].text, input[2].text);
        nickName = input[0].text;

        if (bro.IsSuccess())
        {
            var bro_2 = Backend.BMember.UpdateNickname(input[0].text);

            // �г��� �ߺ��� ���ο� �г������� ����
            //while (!bro_2.IsSuccess())
            //{
            //    nickName = Random.Range(0, 50000).ToString();
            //    bro_2 = Backend.BMember.UpdateNickname(nickName);
            //}

            ChangeText($"ȯ���մϴ�. {nickName}��\nID : {input[0].text}");
            StartCoroutine(ShowText());
            //Debug.Log("ȸ�����Կ� �����߽��ϴ�. : " + bro);
        }
        else
        {

            ChangeText("�̹� �ִ� ���̵� �Ǵ� �г����Դϴ�.");
            StartCoroutine(ShowText());

        }
    }


    private bool changeNickName;
    private void ChangeNickName()
    {
        changeNickName = true;

        LogIn();

        if (changeNickName)
        {
            TextPattern(input[2].text, 0); // �г��� ����

            if (fuctionReturn) return;

            var bro = Backend.BMember.UpdateNickname(input[2].text);

            changeNickName = false;

            if (bro.IsSuccess())
            {
                nickName = input[2].text;

                ChangeText($"�г��� ���濡 �����߽��ϴ�. {nickName}��");
                StartCoroutine(ShowText());
                //Debug.Log("�г��� ���濡 �����߽��ϴ� : " + bro);
            }
            else
            {
                ChangeText("�̹� �ִ� �г��� �Ǵ� ��й�ȣ, ���̵� �ٸ��ϴ�.");
                StartCoroutine(ShowText());
            }

        }
    }

    // ����â ���
    [SerializeField] Image infoBG;
    [SerializeField] TextMeshProUGUI info_TMP;
   
    private Color info_Color;
    

    private bool inProgress;

    private void ChangeText(string text)
    {
        info_TMP.text = text;
    }

    private IEnumerator ShowText()
    {
        //if (isDameSound) StartCoroutine(DameSound());

        if (inProgress) yield break;

        inProgress = true;

        while (infoBG.color.a < 146 / 255f)
        {
            info_Color.a += Time.deltaTime;
            info_TMP.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, info_Color.a);
            infoBG.color = info_Color;

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        StartCoroutine(HideText());
    }


    private IEnumerator HideText()
    {
        while (infoBG.color.a > 0f / 255f)
        {
            info_Color.a -= Time.deltaTime;
            info_TMP.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, info_Color.a);
            infoBG.color = info_Color;

            yield return null;
        }

        inProgress = false;
    }
    #endregion;
}
