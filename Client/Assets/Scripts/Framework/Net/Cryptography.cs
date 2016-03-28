//----------------------------------------------
//            MobArts PiDan Project
// Copyright © 2010-2015 MobArts Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System;
using System.Security.Cryptography;
using System.Collections;
using System.Text;
using System.IO;

public class Cryptography 
{
    private static string DESKey {
        get { return @"P@+#wG+Z"; }
    }

    private static string DESIV {
        get { return @"L%n67}G\Mk@k%:~Y"; }
    }

    public static string DESEncrypt(string plainStr) {
        byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);
        string encrypt = Convert.ToBase64String(DESEncrypt(byteArray));
        return encrypt;
    }
    public static byte[] DESEncrypt(byte[] byteArray) {
        byte[] bKey = Encoding.UTF8.GetBytes(DESKey);
        byte[] bIV = Encoding.UTF8.GetBytes(DESIV);
        byte[] encryptArray = null;
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        try {
            using (MemoryStream mStream = new MemoryStream()) {
                using (CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write)) {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    encryptArray = mStream.ToArray();
                }
            }
        } catch { }
        des.Clear();
        return encryptArray;
    }

    public static string DESDecrypt(string encryptStr) {
        string decrypt = "";
        if (encryptStr != null && !encryptStr.Equals("")) {
            byte[] byteArray = Convert.FromBase64String(encryptStr);
            decrypt = Encoding.UTF8.GetString(DESDecrypt(byteArray));
        }
        return decrypt;
    }
    public static byte[] DESDecrypt(byte[] byteArray) {
        byte[] bKey = Encoding.UTF8.GetBytes(DESKey);
        byte[] bIV = Encoding.UTF8.GetBytes(DESIV);

        byte[] decryptArrat = null;
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        try {
            using (MemoryStream mStream = new MemoryStream()) {
                using (CryptoStream cStream = new CryptoStream(mStream, des.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write)) {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    decryptArrat = mStream.ToArray();
                    //decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                }
            }
        } catch { }
        des.Clear();
        return decryptArrat;
    }

    private static string AESKey {
        get { return @")O[NB]6,YF}+efcaj{+oESb9d8>Z'e9M"; }
    }
    private static string AESIV {
        get { return @"L+\~f4,Ir)b$=pkf"; }
    }
    public static string AESEncrypt(string plainStr) {
        byte[] bKey = Encoding.UTF8.GetBytes(AESKey);
        byte[] bIV = Encoding.UTF8.GetBytes(AESIV);
        byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);

        string encrypt = null;
        Rijndael aes = Rijndael.Create();
        try {
            using (MemoryStream mStream = new MemoryStream()) {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write)) {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(mStream.ToArray());
                }
            }
        } catch { }
        aes.Clear();

        return encrypt;
    }
    public static string AESEncrypt(string plainStr, bool returnNull) {
        string encrypt = AESEncrypt(plainStr);
        return returnNull ? encrypt : (encrypt == null ? String.Empty : encrypt);
    }
    public static string AESDecrypt(string encryptStr) {
        byte[] bKey = Encoding.UTF8.GetBytes(AESKey);
        byte[] bIV = Encoding.UTF8.GetBytes(AESIV);
        byte[] byteArray = Convert.FromBase64String(encryptStr);

        string decrypt = null;
        Rijndael aes = Rijndael.Create();
        try {
            using (MemoryStream mStream = new MemoryStream()) {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write)) {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                }
            }
        } catch { }
        aes.Clear();

        return decrypt;
    }
    public static string AESDecrypt(string encryptStr, bool returnNull) {
        string decrypt = AESDecrypt(encryptStr);
        return returnNull ? decrypt : (decrypt == null ? String.Empty : decrypt);
    }
	
}
