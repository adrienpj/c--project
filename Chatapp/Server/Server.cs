using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Communication;

namespace ChatServer
{
    class Server
    {
        private int port;


        public Server(int port)
        {
            this.port = port;
        }


        public void start()
        {
            TcpListener l = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), port);
            l.Start();
            TcpClient comm = l.AcceptTcpClient();
            Console.WriteLine("Connection established ");
            new Thread(new Receiver(comm).Menu).Start();
                

            
        }

        class Receiver
        {
            private TcpClient comm;

            public Receiver(TcpClient s)
            {
                comm = s;
            }

            public void Menu()
            {
                List<DataBase_Accounts> A_Db = new List<DataBase_Accounts>();
                DataBase_Accounts A1 = new DataBase_Accounts("ui", "ui2");
                A_Db.Add(A1);
                DataBase_Accounts A2 = new DataBase_Accounts("jean", "jean321");
                A_Db.Add(A2);
                A_Db = checkAccount(A_Db);
                List<DataBase_Topics> T_Db = new List<DataBase_Topics>();
                List<DataBase_Accounts> T1A_Db = new List<DataBase_Accounts>();
                T1A_Db.Add(A1);
                T1A_Db.Add(A2);
                DataBase_Topics T1 = new DataBase_Topics("Cats", "Do cats always lands on their feets", T1A_Db);
                T_Db.Add(T1);
                List<DataBase_Accounts> T2A_Db = new List<DataBase_Accounts>();
                T2A_Db.Add(A1);
                DataBase_Topics T2 = new DataBase_Topics("Cyberpunk2077", "New JVC vote on the new game go and see !", T2A_Db);
                T_Db.Add(T2);
                Boolean end = false;
                while (!end)
                {
                    Expr msg = (Expr)Net.rcvMsg(comm.GetStream());
                    switch (msg.ToString())
                    {
                        case "Send a message":
                            A_Db = SendMessage(A_Db);
                            break;

                        case "Sign out":
                            A_Db = checkAccount(A_Db);
                            break;

                        case "Check my profile":
                            string messages = "your messages : \n";
                            for (int i = 0; i< A_Db[0].getMessages.Count; i++)
                            {
                                messages = messages + " " + A_Db[0].getMessages[i].ToString() + "\n";
                            }
                            string profile = "Your UserName is : " + A_Db[0].getUsername + " your password is : " + A_Db[0].getPassword +" " + messages;
                            Net.sendMsg(comm.GetStream(), new Result(profile));
                            break;

                        case "See all topics":
                            AllTopics(T_Db);
                            break;

                        case "Join a topic":
                            JoinTopic(T_Db, A_Db);
                            break;

                        case "Creat a topic":
                            T_Db = CreatTopic(T_Db, A_Db);
                            break;

                        case "exit":
                            end = true;
                            break;
                    }

                    
                }
                
            }

            public List<DataBase_Accounts> checkAccount(List<DataBase_Accounts> DataBase)
            {
                Net.sendMsg(comm.GetStream(), new Result("Enter your UserName : "));
                Expr msg = (Expr)Net.rcvMsg(comm.GetStream());
                Net.sendMsg(comm.GetStream(), new Result("Enter you Password"));
                Expr msg2 = (Expr)Net.rcvMsg(comm.GetStream());
                int count = 0;
                for (int i = 0; i < DataBase.Count; i++)
                {
                    if (msg.ToString().Equals(DataBase[i].getUsername) && msg2.ToString().Equals(DataBase[i].getPassword))
                    {
                        Net.sendMsg(comm.GetStream(), new Result("Welcome !"));
                        DataBase_Accounts temp = DataBase[0];
                        DataBase[0] = DataBase[i];
                        DataBase[i] = temp;
                        count++;
                    }
                }
                if (count == 0)
                {
                    DataBase.Add(new DataBase_Accounts(msg.ToString(), msg.ToString()));
                    Net.sendMsg(comm.GetStream(), new Result("Welcome you just registered !"));
                    DataBase.Reverse();

                }
                return DataBase;
            }

            public List<DataBase_Accounts> SendMessage(List<DataBase_Accounts> DataBase)
            {
                AllAccount(DataBase);
                Expr msg = (Expr)Net.rcvMsg(comm.GetStream());
                int count = -1;
                for (int i = 0; i < DataBase.Count; i++)
                {
                    if (msg.ToString().Equals(DataBase[i].getUsername))
                    {
                        count = i;
                    }
                }
                if (count == -1)
                {
                    Net.sendMsg(comm.GetStream(), new Result("This contact doesn't exist",false));
                    SendMessage(DataBase);

                }
                else
                {
                    Net.sendMsg(comm.GetStream(), new Result("Enter your message"));
                    Expr msg2 = (Expr)Net.rcvMsg(comm.GetStream());
                    Messages text = new Messages(msg2.ToString(), DataBase[0]);
                    DataBase[count].AddContent(text);
                   
                }
                return DataBase;
            }

            public void AllAccount(List<DataBase_Accounts> DataBase)
            {
                string accounts = "Your contacts : ";
                for (int i = 1; i < DataBase.Count; i++)
                {
                    accounts = accounts + " " + DataBase[i].getUsername;
                }
                Net.sendMsg(comm.GetStream(), new Result(accounts));
            }

            public void AllTopics(List<DataBase_Topics> T_Db)
            {
                string topics = "The topics are : ";
                for (int i = 0; i < T_Db.Count; i++)
                {
                    List<DataBase_Accounts> accounts = T_Db[i].getChatters;
                    string chatters = ", The affiliate chatters : ";
                    for (int j = 0; j < T_Db[i].getChatters.Count; ++j)
                    {
                        chatters = " " + chatters +" "+ accounts[j].getUsername;
                    }
                    topics += T_Db[i].getSubject +",  "+ T_Db[i].getContent + chatters +"\n";
                }
                Net.sendMsg(comm.GetStream(), new Result(topics));
            }

            public void JoinTopic(List<DataBase_Topics> T_Db, List<DataBase_Accounts> DataBase)
            {
                AllTopics(T_Db);
                Expr msg = (Expr)Net.rcvMsg(comm.GetStream());
                int count = 0;
                for (int i = 0; i < T_Db.Count; i++)
                {
                    if (msg.ToString().Equals(T_Db[i].getSubject))
                    {
                        Net.sendMsg(comm.GetStream(), new Result("Welcome in the " + T_Db[i].getSubject + " topic!"));
                        count++;
                        UseTopic(T_Db[i], DataBase);
                    }
                }
                if (count == 0)
                {
                    Net.sendMsg(comm.GetStream(), new Result("Sorry this topic doesn't exist", false));
                }

            }

            public void UseTopic(DataBase_Topics T1, List<DataBase_Accounts> DataBase)
            {
                Expr msg = (Expr)Net.rcvMsg(comm.GetStream());
                switch (msg.ToString())
                {
                    case "Send a message to all the chatters":
                        DataBase = SendChatters(T1, DataBase);
                        break;
                    case "Add some content":
                        T1 = Addcontent(T1, DataBase);
                        break;
                }
            }

            public List<DataBase_Accounts> SendChatters(DataBase_Topics T1, List<DataBase_Accounts> DataBase)
            {
                Net.sendMsg(comm.GetStream(), new Result("Enter you message"));
                Expr msg = (Expr)Net.rcvMsg(comm.GetStream());
                for (int i = 0; i < T1.getChatters.Count; i++)
                {
                    for (int j = 1; j < DataBase.Count; j++)
                    {
                        if (T1.getChatters[i].getUsername.Equals(DataBase[j].getUsername))
                        {
                            Messages text = new Messages(msg.ToString(), DataBase[0]);

                            DataBase[j].AddContent(text);
                        }
                    }
                }
                return DataBase;
            }
            public List<DataBase_Topics> CreatTopic(List<DataBase_Topics> T_Db, List<DataBase_Accounts> DataBase)
            {
                Net.sendMsg(comm.GetStream(), new Result("Enter the subject of your topic "));
                Expr msg = (Expr)Net.rcvMsg(comm.GetStream());
                Net.sendMsg(comm.GetStream(), new Result("Enter the content of your topic "));
                Expr msg2 = (Expr)Net.rcvMsg(comm.GetStream());
                List<DataBase_Accounts> TA_Db = new List<DataBase_Accounts>();
                TA_Db.Add(DataBase[0]);
                DataBase_Topics T = new DataBase_Topics(msg.ToString(), msg2.ToString(), TA_Db);
                T_Db.Add(T);
                return T_Db;
            }

            public DataBase_Topics Addcontent(DataBase_Topics T_Db, List<DataBase_Accounts> DataBase)
            {
                Net.sendMsg(comm.GetStream(), new Result("Enter the content you want to add"));
                Expr msg = (Expr)Net.rcvMsg(comm.GetStream());
                T_Db.setContent(msg.ToString());
                T_Db.AddChatters(DataBase[0]);
                return T_Db;
            }



        }

    }
}
