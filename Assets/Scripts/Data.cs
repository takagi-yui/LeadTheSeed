using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class Data : MonoBehaviour
{
    [Serializable]
    public class SaveData
    {
        public int stage;
        public int[] jewel;
        public int[] highScore;
    }
    public static SaveData data;

    void Awake()
    {
        Load();
    }

    void Update()
    {
		
	}

    public static void Save()
    {
        ICryptoTransform encryptor = rijndaelManaged().CreateEncryptor();
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
        cryptoStream.Write(bytes, 0, bytes.Length);
        cryptoStream.FlushFinalBlock();
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            PlayerPrefs.SetString("data", System.Convert.ToBase64String(memoryStream.ToArray()));
            PlayerPrefs.Save();
        }
        else
        {
            File.WriteAllText(Application.persistentDataPath + "/data", System.Convert.ToBase64String(memoryStream.ToArray()));
        }
    }

    public static void Load()
    {
        data = new SaveData();
        if (Application.platform == RuntimePlatform.WebGLPlayer ? PlayerPrefs.HasKey("data") : File.Exists(Application.persistentDataPath + "/data"))
        {
            ICryptoTransform decryptor = rijndaelManaged().CreateDecryptor();
            byte[] encrypted = System.Convert.FromBase64String(Application.platform == RuntimePlatform.WebGLPlayer ? PlayerPrefs.GetString("data") : File.ReadAllText(Application.persistentDataPath + "/data"));
            MemoryStream memoryStream = new MemoryStream(encrypted);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] decrypted = new byte[encrypted.Length];
            cryptoStream.Read(decrypted, 0, decrypted.Length);
            JsonUtility.FromJsonOverwrite(System.Text.Encoding.UTF8.GetString(decrypted), data);
        }
        else
        {
            Create();
        }
    }

    public static RijndaelManaged rijndaelManaged()
    {
        RijndaelManaged rijndael = new RijndaelManaged();
        rijndael.Padding = PaddingMode.Zeros;
        rijndael.Mode = CipherMode.CBC;
        rijndael.KeySize = 256;
        rijndael.BlockSize = 256;
        string pw = "DE36434D7E4A21CAF6C1AA0D3B1F8D06";
        string salt = "2ECAA708C857D77BC83AFCE5BD14D515";
        byte[] bSalt = System.Text.Encoding.UTF8.GetBytes(salt);
        Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(pw, bSalt);
        deriveBytes.IterationCount = 1000;
        rijndael.Key = deriveBytes.GetBytes(rijndael.KeySize / 8);
        rijndael.IV = deriveBytes.GetBytes(rijndael.BlockSize / 8);
        return rijndael;
    }

    public static void Create()
    {
        data.stage = 0;
        data.jewel = new int[10];
        data.highScore = new int[10];
        Save();
    }

    public static void Delete()
    {
        Create();
    }
}
