using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer
{
    class DataBase_Accounts
    {
        private string Username, Password;
        private List<Messages> Messages;

        public DataBase_Accounts(string userName, string password)
        {
            this.Username = userName;
            this.Password = password;
            this.Messages = new List<Messages>();
        }

        public string getUsername
        {
            get { return Username; }
        }
        public string getPassword
        {
            get { return Password; }
        }
        public List<Messages> getMessages
        {
            get { return Messages; }
        }

       public void AddContent(Messages text)
        {
            Messages.Add(text);
        }
    }
}
