using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer
{
    class Messages
    {
        string Text;
        DataBase_Accounts Account;
        public Messages(string text, DataBase_Accounts account)
        {
            this.Text = text;
            this.Account = account;

        }
        
        public string getText
        {
            get { return Text; }
        }
        public DataBase_Accounts getAccount
        {
            get { return Account; }
        }

        public override string ToString()
        {
            return Text + " from : "+ Account.getUsername;
        }

    }
}
