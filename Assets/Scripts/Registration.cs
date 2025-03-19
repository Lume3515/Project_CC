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

    #region// µî·Ï
    // ¹è°æÈ­¸é
    [SerializeField] GameObject backGround;

    // »ç¿ëÀÚÀÇ ÀÔ·Â ¹Þ¾Æ¿À±â   
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

    #region// ¹öÆ°

    // ·Î±×ÀÎ ¹öÆ°
    public void LogInBT()
    {
        backGround.SetActive(true);
        input3_Obj.SetActive(false);

        input[1].contentType = TMP_InputField.ContentType.Password;
        placeholder2.text = "ºñ¹Ð¹øÈ£ ÀÔ·Â";
        ChangeExitBTName("·Î±×ÀÎ");

        CleanText();


    }

    // ´Ð³×ÀÓ º¯°æ ¹öÆ°
    //public void ChangeNickNameBT()
    //{
    //    backGround.SetActive(true);
    //    input3_Obj.SetActive(true);

    //    placeholder2.text = "ºñ¹Ð¹øÈ£ ÀÔ·Â";
    //    placeholder3.text = "º¯°æÇÒ ´Ð³×ÀÓ";

    //    input[1].contentType = TMP_InputField.ContentType.Password;
    //    input[2].contentType = TMP_InputField.ContentType.Standard;
    //    ChangeExitBTName("´Ð³×ÀÓ º¯°æ");

    //    CleanText();
    //}

    // È¸¿ø°¡ÀÔ ¹öÆ°
    public void SignUpBT()
    {
        backGround.SetActive(true);
        input3_Obj.SetActive(true);

        placeholder2.text = "ºñ¹Ð¹øÈ£ ÀÔ·Â";
        placeholder3.text = "ºñ¹Ð¹øÈ£ È®ÀÎ";

        input[1].contentType = TMP_InputField.ContentType.Password;
        input[2].contentType = TMP_InputField.ContentType.Password;
        ChangeExitBTName("È¸¿ø°¡ÀÔ");

        CleanText();
    }

    // °è¼ÓÇÏ±â ¹öÆ°
    public void ContinueBT()
    {


        switch (continueButton_TMP.text)
        {
            case "·Î±×ÀÎ":

                LogIn();
                break;

            case "´Ð³×ÀÓ º¯°æ":
                ChangeNickName();
                break;

            case "È¸¿ø°¡ÀÔ":
                SignUp();
                break;
        }
    }


    public void Exit()
    {
        backGround.SetActive(false);
    }

    #endregion

    #region// ±â´É
    private bool fuctionReturn;

    private void TextPattern(string text, int number)
    {
        if (!Regex.IsMatch(text, "^.{1,8}$")) // 8ÀÚ ÀÌÇÏ¸¸ °¡´É
        {
            ChangeText("8ÀÚ ÀÌÇÏ¸¸ »ç¿ë°¡´ÉÇÕ´Ï´Ù.");
            fuctionReturn = true;
            StartCoroutine(ShowText());
        }
        else if (!Regex.IsMatch(text, "^[^\\s]{1,8}$"))// ¶ç¾î¾²±â ±ÝÁö
        {
            ChangeText("¶ç¾î¾²±â´Â Çã¿ëµÇÁö ¾Ê½À´Ï´Ù.");
            fuctionReturn = true;
            StartCoroutine(ShowText());
        }
        else if (!Regex.IsMatch(text, "^[^°¡-ÆR¤¡-¤¾¤¿-¤Ó]*$") && number == 1) // ºñ¹ø¿¡¸¸ / ÇÑ±Û ±ÝÁö
        {
            ChangeText("ºñ¹ø¿¡ ÇÑ±ÛÀº »ç¿ëÇÏ½Ç ¼ö ¾ø½À´Ï´Ù.");
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

    // ÅØ½ºÆ® Á¤¸®
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
            Debug.Log("·Î±×ÀÎÀÌ ¼º°øÇß½À´Ï´Ù. : " + bro);

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
            ChangeText("¾ÆÀÌµð³ª ºñ¹Ð¹øÈ£°¡ Æ²·È½À´Ï´Ù.");
            StartCoroutine(ShowText());
            changeNickName = false;
        }
    }


    private void SignUp()
    {
        TextPattern(input[0].text, 0); // id
        TextPattern(input[1].text, 1); // pw

        if (fuctionReturn) return;

        if (input[1].text != input[2].text) // ºñ¹øÈ®ÀÎÀÌ °°ÀºÁö?
        {
            ChangeText("ºñ¹Ð¹øÈ£ È®ÀÎÀÌ Æ²·È½À´Ï´Ù.");
            StartCoroutine(ShowText());
            return;
        }

        var bro = Backend.BMember.CustomSignUp(input[0].text, input[2].text);
        nickName = input[0].text;

        if (bro.IsSuccess())
        {
            var bro_2 = Backend.BMember.UpdateNickname(input[0].text);

            // ´Ð³×ÀÓ Áßº¹½Ã »õ·Î¿î ´Ð³×ÀÓÀ¸·Î º¯°æ
            //while (!bro_2.IsSuccess())
            //{
            //    nickName = Random.Range(0, 50000).ToString();
            //    bro_2 = Backend.BMember.UpdateNickname(nickName);
            //}

            ChangeText($"È¯¿µÇÕ´Ï´Ù. {nickName}´Ô\nID : {input[0].text}");
            StartCoroutine(ShowText());
            //Debug.Log("È¸¿ø°¡ÀÔ¿¡ ¼º°øÇß½À´Ï´Ù. : " + bro);
        }
        else
        {

            ChangeText("ÀÌ¹Ì ÀÖ´Â ¾ÆÀÌµð ¶Ç´Â ´Ð³×ÀÓÀÔ´Ï´Ù.");
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
            TextPattern(input[2].text, 0); // ´Ð³×ÀÓ º¯°æ

            if (fuctionReturn) return;

            var bro = Backend.BMember.UpdateNickname(input[2].text);

            changeNickName = false;

            if (bro.IsSuccess())
            {
                nickName = input[2].text;

                ChangeText($"´Ð³×ÀÓ º¯°æ¿¡ ¼º°øÇß½À´Ï´Ù. {nickName}´Ô");
                StartCoroutine(ShowText());
                //Debug.Log("´Ð³×ÀÓ º¯°æ¿¡ ¼º°øÇß½À´Ï´Ù : " + bro);
            }
            else
            {
                ChangeText("ÀÌ¹Ì ÀÖ´Â ´Ð³×ÀÓ ¶Ç´Â ºñ¹Ð¹øÈ£, ¾ÆÀÌµð°¡ ´Ù¸¨´Ï´Ù.");
                StartCoroutine(ShowText());
            }

        }
    }

    // Á¤º¸Ã¢ ¹è°æ
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
