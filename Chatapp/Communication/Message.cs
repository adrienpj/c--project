using System;

namespace Communication
{
  
    public interface Message
    {
        string ToString();
    }

    [Serializable]
    public class Expr : Message
    {
        private string Text;
        private int Number;
        private Boolean End;

        public Expr(string text)
        {
            this.Text = text;
            this.Number = 0;
            this.End = false;
        }
        public Expr(int number)
        {
            this.Number = number;
            this.Text = null;
            this.End = false;
        }
        public Expr(Boolean end)
        {
            this.Number = 0;
            this.Text = null;
            this.End = end;
        }
        public Expr(int number, string text)
        {
            this.Number = number;
            this.Text = text;
            this.End = false;
        }

        public string getText
        {
            get { return Text; }
        }
        public Boolean getEnd
        {
            get { return End; }
        }
        public int getNumber
        {
            get { return Number; }
        }
        public override string ToString()
        {
            return  Text;
        }
    }

    [Serializable]
    public class Result : Message
    {
        private String Text;
        private Boolean Res;

        public Result(String text)
        {
            this.Text = text;
            this.Res = true;
        }
        public Result(String text, Boolean res)
        {
            this.Text = text;
            this.Res = res;
        }

        public String getText
        {
            get { return Text; }
        }
        public Boolean getRes
        {
            get { return Res; }
        }

        public override string ToString()
        {
            return Text;
        }
    }

}
