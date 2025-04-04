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

    #region// 등록
    // 배경화면
    [SerializeField] GameObject backGround;

    // 사용자의 입력 받아오기   
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

    #region// 버튼

    // 로그인 버튼
    public void LogInBT()
    {
        backGround.SetActive(true);
        input3_Obj.SetActive(false);

        input[1].contentType = TMP_InputField.ContentType.Password;
        placeholder2.text = "비밀번호 입력";
        ChangeExitBTName("로그인");

        CleanText();


    }

    // 닉네임 변경 버튼
    //public void ChangeNickNameBT()
    //{
    //    backGround.SetActive(true);
    //    input3_Obj.SetActive(true);

    //    placeholder2.text = "비밀번호 입력";
    //    placeholder3.text = "변경할 닉네임";

    //    input[1].contentType = TMP_InputField.ContentType.Password;
    //    input[2].contentType = TMP_InputField.ContentType.Standard;
    //    ChangeExitBTName("닉네임 변경");

    //    CleanText();
    //}

    // 회원가입 버튼
    public void SignUpBT()
    {
        backGround.SetActive(true);
        input3_Obj.SetActive(true);

        placeholder2.text = "비밀번호 입력";
        placeholder3.text = "비밀번호 확인";

        input[1].contentType = TMP_InputField.ContentType.Password;
        input[2].contentType = TMP_InputField.ContentType.Password;
        ChangeExitBTName("회원가입");

        CleanText();
    }

    // 계속하기 버튼
    public void ContinueBT()
    {


        switch (continueButton_TMP.text)
        {
            case "로그인":

                LogIn();
                break;

            case "닉네임 변경":
                ChangeNickName();
                break;

            case "회원가입":
                SignUp();
                break;
        }
    }


    public void Exit()
    {
        backGround.SetActive(false);
    }

    #endregion

    #region// 기능
    private bool fuctionReturn;

    private void TextPattern(string text, int number)
    {
        if (!Regex.IsMatch(text, "^.{1,8}$")) // 8자 이하만 가능
        {
            ChangeText("8자 이하만 사용가능합니다.");
            fuctionReturn = true;
            StartCoroutine(ShowText());
        }
        else if (!Regex.IsMatch(text, "^[^\\s]{1,8}$"))// 띄어쓰기 금지
        {
            ChangeText("띄어쓰기는 허용되지 않습니다.");
            fuctionReturn = true;
            StartCoroutine(ShowText());
        }
        else if (!Regex.IsMatch(text, "^[^가-힣ㄱ-ㅎㅏ-ㅣ]*$") && number == 1) // 비번에만 / 한글 금지
        {
            ChangeText("비번에 한글은 사용하실 수 없습니다.");
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

    // 텍스트 정리
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
            Debug.Log("로그인이 성공했습니다. : " + bro);

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
            ChangeText("아이디나 비밀번호가 틀렸습니다.");
            StartCoroutine(ShowText());
            changeNickName = false;
        }
    }


    private void SignUp()
    {
        TextPattern(input[0].text, 0); // id
        TextPattern(input[1].text, 1); // pw

        if (fuctionReturn) return;

        if (input[1].text != input[2].text) // 비번확인이 같은지?
        {
            ChangeText("비밀번호 확인이 틀렸습니다.");
            StartCoroutine(ShowText());
            return;
        }

        var bro = Backend.BMember.CustomSignUp(input[0].text, input[2].text);
        nickName = input[0].text;

        if (bro.IsSuccess())
        {
            var bro_2 = Backend.BMember.UpdateNickname(input[0].text);

            // 닉네임 중복시 새로운 닉네임으로 변경
            //while (!bro_2.IsSuccess())
            //{
            //    nickName = Random.Range(0, 50000).ToString();
            //    bro_2 = Backend.BMember.UpdateNickname(nickName);
            //}

            ChangeText($"환영합니다. {nickName}님\nID : {input[0].text}");
            StartCoroutine(ShowText());
            //Debug.Log("회원가입에 성공했습니다. : " + bro);
        }
        else
        {

            ChangeText("이미 있는 아이디 또는 닉네임입니다.");
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
            TextPattern(input[2].text, 0); // 닉네임 변경

            if (fuctionReturn) return;

            var bro = Backend.BMember.UpdateNickname(input[2].text);

            changeNickName = false;

            if (bro.IsSuccess())
            {
                nickName = input[2].text;

                ChangeText($"닉네임 변경에 성공했습니다. {nickName}님");
                StartCoroutine(ShowText());
                //Debug.Log("닉네임 변경에 성공했습니다 : " + bro);
            }
            else
            {
                ChangeText("이미 있는 닉네임 또는 비밀번호, 아이디가 다릅니다.");
                StartCoroutine(ShowText());
            }

        }
    }

    // 정보창 배경
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
