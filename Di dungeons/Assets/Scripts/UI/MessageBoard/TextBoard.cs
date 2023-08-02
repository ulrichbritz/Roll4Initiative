using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UB
{
    public class TextBoard : MonoBehaviour
    {
        public static TextBoard instance;

        public Queue<string> updateMessageQueue;

        [Header("UI components")]
        [SerializeField] GameObject updateMessagePrefab;
        [SerializeField] RectTransform updateMessageParent;
        

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            //updateMessageQueue = new Queue<string>();

            //updateMessageQueue.Enqueue("The Queue is Awake!");
        }

        /*
        void CheckNewMessages()
        {
            if(updateMessageQueue.Count > 0)
            {
                foreach (string line in updateMessageQueue)
                {
                    CreateUpdateMessage(line, Color.black);
                }
            }
        }
        */

        public void CreateUpdateMessage(string updateMessage, Color messageColor)
        {
            GameObject cm = Instantiate(updateMessagePrefab, updateMessagePrefab.transform.position, Quaternion.identity);

            cm.transform.SetParent(updateMessageParent);

            cm.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            TextMeshProUGUI textBox = cm.transform.GetComponentInChildren<TextMeshProUGUI>();
            textBox.text = updateMessage;
            textBox.color = messageColor;

        }
    }
}

