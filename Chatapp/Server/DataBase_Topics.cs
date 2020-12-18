using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer
{
    class DataBase_Topics
    {
        private string Subject;
        private string Content;
        private List<DataBase_Accounts> Chatters;

        public DataBase_Topics(string subject, string content, List<DataBase_Accounts> chatters)
        {

            this.Subject = subject;
            this.Content = content;
            this.Chatters = chatters;
        }

        public string getSubject
        {
            get { return Subject; }
        }
        public string getContent
        {
            get { return Content; }
        }
        public void setContent(string text)
        {
            this.Content += "\n" + text;
        }
        public void AddChatters(DataBase_Accounts account)
        {
            Chatters.Add(account);
        }
        public List<DataBase_Accounts> getChatters
        {
            get { return Chatters; }
        }
    }
}
