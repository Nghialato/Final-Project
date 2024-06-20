using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace _Scripts.GameNetwork
{
    public class SendData : MonoBehaviour
    {
        [SerializeField] private string URL;
        [Serializable]
        public class DataSend
        {
            public string entryPort;
            public string dataSend;
        }

        public List<DataSend> listData;

        public void UploadData(string respond)
        {
            if (respond != "Done") return;
            var form = new WWWForm();
            foreach (var data in listData)
            {
                form.AddField(data.entryPort, data.dataSend);
            }
            var www = UnityWebRequest.Post(URL, form);
            www.SendWebRequest();
        }
    }
}