using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communication;

namespace Chatapp
{
    class Client
    {
        private string hostname;
        private int port;

        public Client(string h, int p)
        {
            hostname = h;
            port = p;
        }

        public void start()
        {
            TcpClient comm = new TcpClient(hostname, port);
            Console.WriteLine("Connection established");
            checkAccount(comm);
            Boolean end = false;
            while (!end)
            {
                Console.WriteLine("What do you want to do ?\nSend a message ? Check my profile ? Sign out ? See all topics ? Join a topic ? Creat a topic ?  exit ?");
                string choice = Console.ReadLine();
                Net.sendMsg(comm.GetStream(), new Expr(choice));
                switch (choice)
                {
                    case "Send a message":
                        SendMessage(comm);
                        break;

                    case "Sign out":
                        checkAccount(comm);
                        break;

                    case "Check my profile":
                        Console.WriteLine((Result)Net.rcvMsg(comm.GetStream()));
                        break;

                    case "See all topics":
                        Console.WriteLine((Result)Net.rcvMsg(comm.GetStream()));
                        break;

                    case "Join a topic":
                        JoinTopic(comm);
                        break;

                    case "Creat a topic":
                        CreatTopic(comm);
                        break;

                    case "exit":
                        end = true;
                        Console.WriteLine("Goodbye");
                        break; ;
                }


            }
            
        }

        public void SendMessage(TcpClient comm)
        {
            Console.WriteLine("choose one of your contact");
            Console.WriteLine((Result)Net.rcvMsg(comm.GetStream()));
            string Text = Console.ReadLine();
            Net.sendMsg(comm.GetStream(), new Expr(Text));
            Result res = (Result)Net.rcvMsg(comm.GetStream());
            Console.WriteLine(res);
            if (res.getRes == true)
            {
                string Text2 = Console.ReadLine();
                Net.sendMsg(comm.GetStream(), new Expr(Text2));
                Console.WriteLine("your message : " + Text2 + " was send succesufully to " + Text);
            }
            else
            {
                SendMessage(comm);
            }
            

        }

        public void checkAccount(TcpClient comm)
        {
            Console.WriteLine((Result)Net.rcvMsg(comm.GetStream()));
            string UserName = Console.ReadLine();
            Net.sendMsg(comm.GetStream(), new Expr(UserName));
            Console.WriteLine((Result)Net.rcvMsg(comm.GetStream()));
            string Password = Console.ReadLine();
            Net.sendMsg(comm.GetStream(), new Expr(Password));
            Console.WriteLine((Result)Net.rcvMsg(comm.GetStream()));
        }

        public void JoinTopic(TcpClient comm)
        {
            Console.WriteLine("Which topic do you want to join ?");
            Console.WriteLine((Result)Net.rcvMsg(comm.GetStream()));
            string choice = Console.ReadLine();
            Net.sendMsg(comm.GetStream(), new Expr(choice));
            Result res = (Result)Net.rcvMsg(comm.GetStream());
            Console.WriteLine(res);;
            if (res.getRes==true)
            {
                UseTopic(comm);
            }
            else
            {
                JoinTopic(comm);
            }
        }

        public void UseTopic(TcpClient comm)
        {
            Console.WriteLine("what do you want to do ?\nSend a message to all the chatters ?, Add some content ?");
            string choice = Console.ReadLine();
            Net.sendMsg(comm.GetStream(), new Expr(choice));
            switch (choice)
            {
                case "Send a message to all the chatters":
                    SendChatters(comm);
                    break;

                case "Add some content":
                    Addcontent(comm);
                    break;
            }
        }

        public void SendChatters(TcpClient comm)
        {
            Console.WriteLine((Result)Net.rcvMsg(comm.GetStream()));
            string text = Console.ReadLine();
            Net.sendMsg(comm.GetStream(), new Expr(text));
        }

        public void CreatTopic(TcpClient comm)
        {
            Console.WriteLine((Result)Net.rcvMsg(comm.GetStream()));
            string text = Console.ReadLine();
            Net.sendMsg(comm.GetStream(), new Expr(text));
            Console.WriteLine((Result)Net.rcvMsg(comm.GetStream()));
            string text2 = Console.ReadLine();
            Net.sendMsg(comm.GetStream(), new Expr(text2));

        }

        public void Addcontent(TcpClient comm)
        {
            Console.WriteLine((Result)Net.rcvMsg(comm.GetStream()));
            string text = Console.ReadLine();
            Net.sendMsg(comm.GetStream(), new Expr(text));
        }

    }
}
