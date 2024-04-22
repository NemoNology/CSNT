using System;

namespace CSNT.Clientserverchat.Data.Models
{
    public struct Message
    {
        public string Text;
        public Client Sender;
        public DateTime SendTime;

        public Message(string text, Client sender, DateTime sendTime)
        {
            Text = text;
            Sender = sender;
            SendTime = sendTime;
        }
    }
}