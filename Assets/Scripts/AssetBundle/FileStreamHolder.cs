using UnityEngine;
using System.Collections;
using System.Text;


    public class FileStreamHolder : ScriptableObject
    {
        //如果是public，则不用加
        [SerializeField]
        private string fileName;
        [SerializeField]
        private string content;

        public string Content
        {
            set { content = value; }
            get { return content; }
        }

        public string FileName
        {
            set { fileName = value; }
            get { return fileName; }
        }

    }