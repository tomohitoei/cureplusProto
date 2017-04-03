using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLRemoting
{
    public class Util
    {
        //式をを計算して結果を返す　評価はまた別
        public static string calc(string exp)
        {

            //calc 
            //比較計算とかの場合は、別途

            //文字列比較の場合
            if (exp.IndexOf("==") != -1)
            {

                string[] delimiter = { "==" };

                string[] t = exp.Split(delimiter, StringSplitOptions.None);
                string left = t[0].Trim();
                string right = t[1].Trim();

                if (left == right)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }

            }
            else if (exp.IndexOf("!=") != -1)
            {

                string[] delimiter = { "!=" };

                string[] t = exp.Split(delimiter, StringSplitOptions.None);
                string left = t[0].Trim();
                string right = t[1].Trim();

                if (left != right)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }

            }
            else if (exp.IndexOf(">=") != -1)
            {

                string[] delimiter = { ">=" };
                string[] t = exp.Split(delimiter, StringSplitOptions.None);
                string left = t[0].Trim();
                string right = t[1].Trim();

                if (float.Parse(left) >= float.Parse(right))
                {
                    return "true";
                }
                else
                {
                    return "false";
                }


            }
            else if (exp.IndexOf("<=") != -1)
            {
                string[] delimiter = { "<=" };
                string[] t = exp.Split(delimiter, StringSplitOptions.None);
                string left = t[0].Trim();
                string right = t[1].Trim();

                if (float.Parse(left) <= float.Parse(right))
                {
                    return "true";
                }
                else
                {
                    return "false";
                }

            }
            else if (exp.IndexOf(">") != -1)
            {

                string[] delimiter = { ">" };
                string[] t = exp.Split(delimiter, StringSplitOptions.None);
                string left = t[0].Trim();
                string right = t[1].Trim();

                if (float.Parse(left) > float.Parse(right))
                {
                    return "true";
                }
                else
                {
                    return "false";
                }


            }
            else if (exp.IndexOf("<") != -1)
            {
                string[] delimiter = { "<" };
                string[] t = exp.Split(delimiter, StringSplitOptions.None);
                string left = t[0].Trim();
                string right = t[1].Trim();

                if (float.Parse(left) < float.Parse(right))
                {
                    return "true";
                }
                else
                {
                    return "false";
                }

            }
            else if (exp.IndexOf("*") != -1)
            {

                string[] delimiter = { "*" };
                string[] t = exp.Split(delimiter, StringSplitOptions.None);
                string left = t[0].Trim();
                string right = t[1].Trim();

                float k = float.Parse(left) * float.Parse(right);
                return "" + k;

            }
            else if (exp.IndexOf("/") != -1)
            {

                string[] delimiter = { "/" };
                string[] t = exp.Split(delimiter, StringSplitOptions.None);
                string left = t[0].Trim();
                string right = t[1].Trim();

                float k = float.Parse(left) / float.Parse(right);
                return "" + k;

            }
            else if (exp.IndexOf("+") != -1)
            {

                //数値の先頭が-で始まって、かつもう一つ-があれば、計算。それ以外は代入
                if (exp[0] == '+')
                {
                    if (exp.Substring(1).IndexOf("+") == -1)
                    {
                        return exp;
                    }
                    else
                    {
                        exp = exp.Substring(1);
                    }
                }

                string[] delimiter = { "+" };
                string[] t = exp.Split(delimiter, StringSplitOptions.None);
                string left = t[0].Trim();
                string right = t[1].Trim();

                float k = float.Parse(left) + float.Parse(right);

                return "" + k;

            }
            else if (exp.IndexOf("-") != -1)
            {

                //数値の先頭が-で始まって、かつもう一つ-があれば、計算。それ以外は代入
                bool flag_minus = false;
                if (exp[0] == '-')
                {
                    if (exp.Substring(1).IndexOf("-") == -1)
                    {
                        return exp;
                    }
                    else
                    {
                        flag_minus = true;
                        exp = exp.Substring(1);
                    }
                }

                string[] delimiter = { "-" };
                string[] t = exp.Split(delimiter, StringSplitOptions.None);
                string left = t[0].Trim();
                string right = t[1].Trim();

                if (flag_minus == true)
                {
                    left = "-" + left;
                }

                float k = float.Parse(left) - float.Parse(right);

                return "" + k;

            }
            else
            {

                return exp;

            }

        }

    }

}

