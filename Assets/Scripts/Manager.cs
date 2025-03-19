using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Manager : MonoBehaviour
{
    public static class User_Info
    {
        public static string id;
        public static string pw;
    }

    public static class Manager_Info
    {
        public static string id;
        public static string pw;
    }

    private static Manager instance;
    public static Manager Instance => instance;

    private void Awake()
    {
        Manager_Info.id = "°ü¸®ÀÚ";
        Manager_Info.pw = "Lume3515";

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
