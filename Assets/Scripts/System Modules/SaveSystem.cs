using System;
using System.IO;
using UnityEngine;

namespace ZJ.System
{
    public static class SaveSystem
    {
        #region PlayerPrefs


        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public static void SaveByPlayerPrefs(string key, object data)
        {
            var json = JsonUtility.ToJson(data);
            Debug.Log(json);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// ��ȡ
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string LoadFromPlayerPrefs(string key)
        {
            return PlayerPrefs.GetString(key, null);
        }

        #endregion


        #region Json

        public static void SaveByJson(string saveFileName, object data)
        {
            var json = JsonUtility.ToJson(data);

            var path = Path.Combine(Application.persistentDataPath, saveFileName);

            try
            {
                File.WriteAllText(path, json);

#if UNITY_EDITOR
                Debug.Log("������...·��:" + path);
#endif

            }
            catch (Exception e)
            {
#if UNITY_EDITOR

                Debug.LogError("����·����" + path + "  ���⣺" + e);

#endif
            }
        }


        public static T LoadFromJson<T>(string saveFileName)
        {
            var path = Path.Combine(Application.persistentDataPath, saveFileName);

            try
            {


                var json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<T>(json);

                return data;
            }
            catch (Exception e)
            {
#if UNITY_EDITOR

                Debug.LogError("����·����" + path + "  ���⣺" + e);

#endif

                return default;
            }


        }



        public static void DeleteSaveFile(string saveFileName)
        {
            var path = Path.Combine(Application.persistentDataPath, saveFileName);

            try
            {
                File.Delete(path);
            }catch(Exception e)
            {
#if UNITY_EDITOR

                Debug.LogError("����·����" + path + "  ���⣺" + e);

#endif
            }

        }


        public static bool SaveFileExists(string saveFileName)
        {
            var path = Path.Combine(Application.persistentDataPath, saveFileName);

            return File.Exists(path);
        }
        #endregion
    }
}
