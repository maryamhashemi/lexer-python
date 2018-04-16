using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace lexerPython
{
class Program
    {
        static string Reverse(string text)
        {
            if (text == null) return null;
            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }

        static public void lexer()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Dictionary<string, string> ENtoFa = new Dictionary<string, string>();
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\maryam\Documents\Visual Studio 2015\Projects\lexerPython\CSDic.txt");

            foreach (string line in lines)
            {
                string[] words = line.Split(',');
                if (!ENtoFa.ContainsKey(words[1]))
                {
                    ENtoFa.Add(words[1], words[0]);
                }
            }
          
            char ch =' ';
            char[] token = new char[100];
            int state = 0;
            int length = 0;
            bool isRead = true;
            string[] keywords =   { "and", "assert", "break", "class",
                                    "continue", "def", "del", "elif",
                                    "else", "except", "exec", "finally",
                                    "for", "from", "global", "if",
                                    "import", "in", "is", "lambda",
                                    "not", "or","pass", "print",
                                    "raise", "return","try", "while",
                                    "Data", "Float","Int", "Numeric",
                                    "Oxphys", "array","close", "float",
                                    "int", "input","open", "range",
                                    "type", "write","zeros"};

            StreamReader file = new StreamReader(@"C:\Users\maryam\Documents\Visual Studio 2015\Projects\lexerPython\test.txt");

            while (!file.EndOfStream)
            {
                if (isRead)
                {
                    //read one character from file 
                    ch = (char)file.Read();
                }

                switch (state)
                {
                    case 0:
                        {
                            if (((ch <= 'z' && ch >= 'a') || (ch <= 'Z' && ch >= 'A') || ch == '_'))
                            { state = 1; token[length] = ch; length++; isRead = true; }

                            else if (ch <= '9' && ch >= '0')
                            { state = 3; token[length] = ch; length++; isRead = true; }

                            else if (ch == '\"')
                            { state = 7; token[length] = ch; length++; isRead = true; }

                            else if (ch == '\'')
                            { state = 17; token[length] = ch; length++; isRead = true; }

                            else if (ch == '#')
                            { state = 27; token[length] = ch; length++; isRead = true; }

                            else if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '%')
                            { state = 28; token[length] = ch; length++; isRead = true; }

                            else if (ch == '&' || ch == '|' || ch == '~' || ch == '^')
                            { state = 30; token[length] = ch; length++; isRead = true; }

                            else if (ch == '<')
                            { state = 32; token[length] = ch; length++; isRead = true; }

                            else if (ch == '>')
                            { state = 35; token[length] = ch; length++; isRead = true; }

                            else if (ch == '(' || ch == ')' || ch == '{' || ch == '}' ||
                                    ch == '[' || ch == ']')
                            { state = 38; token[length] = ch; length++; isRead = false; }

                            else if(ch == '=')
                            { state = 39; token[length] = ch; length++; isRead = false; }

                            else if((ch >= 0x600 && ch < 0x660) || 
                                    (ch >= 0x66A && ch <= 0x6f0) ||
                                    (ch >= 0x750 && ch <= 0x77f) || 
                                    (ch >= 0xfb50 && ch <= 0xfc3f) || 
                                    (ch >= 0xfe70 && ch <= 0xfefc))
                            { state = 40; token[length] = ch; length++; isRead = true; }

                           else if(ch == '٠' || ch == '١' || ch == '۲' || ch == '۳' ||
                                    ch == '۴' || ch == '۵' || ch == '۶' || ch == '۷' || 
                                    ch == '۸' || ch == '۹')
                            { state = 42; token[length] = ch; length++; isRead = true; }

                            else if(ch == '.')
                            { state = 44; token[length] = ch; length++; isRead = true; }
                            else
                                isRead = true;
                            break;
                        }
                    case 1:
                        {
                            if ((ch <= 'z' && ch >= 'a') || (ch <= 'Z' && ch >= 'A') || (ch <= '9' && ch >= '0') || ch == '_')
                            {
                                state = 1;
                                token[length] = ch;
                                length++;
                                isRead = true;
                            }
                            else { state = 2; isRead = false; }
                            break;
                        }
                    case 2:
                        {
                            bool isKey = false;

                            for (int i = 0; i < 43; i++)
                            {
                                if(keywords[i].Length >= length)
                                {
                                    isKey = true;
                                    for (int j = 0; j < length; j++)
                                    {
                                        if (token[j] != keywords[i][j])
                                        {
                                            isKey = false;
                                            break;
                                        }
                                    }
                                    if (isKey == true)
                                    {
                                        break;
                                    }
                                }                             
                            }
                            if (isKey == false)
                            {
                                for (int i = 0; i < length; i++)
                                    Console.Write(token[i]);
                                Console.Write(" is an identifier.\n");
                                length = 0;
                                Array.Clear(token, 0, token.Length);
                                state = 0;
                                isRead = false;
                            }
                            else
                            {
                                for (int i = 0; i < length; i++)
                                    Console.Write(token[i]);
                                Console.Write(" is a keyword.\n");
                                length = 0;
                                Array.Clear(token, 0, token.Length);
                                state = 0;
                                isRead = false;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (ch <= '9' && ch >= '0') { state = 3; token[length] = ch; length++; isRead = true; }
                            else if (ch == '.') { state = 5; token[length] = ch; length++; isRead = true; }
                            else { state = 4; isRead = false; }
                            break;
                        }
                    case 4:
                        {
                            for (int i = 0; i < length; i++)
                                Console.Write(token[i]);
                            Console.Write(" is an integer.\n");
                            length = 0;
                            Array.Clear(token, 0, token.Length);
                            state = 0;
                            isRead = false;
                            break;
                        }
                    case 5:
                        {
                            if (ch <= '9' && ch >= '0') { state = 5; token[length] = ch; length++; isRead = true; }
                            else { state = 6; isRead = false; }
                            break;
                        }
                    case 6:
                        {
                            for (int i = 0; i < length; i++)
                                Console.Write(token[i]);
                            Console.Write(" is a float.\n");
                            length = 0;
                            Array.Clear(token, 0, token.Length);
                            state = 0;
                            isRead = false;
                            break;
                        }
                    case 7:
                        {
                            if (ch == '\\') { state = 9; token[length] = ch; length++; isRead = true; }
                            else if (ch == '"') { state = 11; token[length] = ch; length++; isRead = true; }
                            else { state = 8; token[length] = ch; length++; isRead = true; }
                            break;
                        }
                    case 8:
                        {
                            if (ch == '\\') { state = 9; token[length] = ch; length++; isRead = true; }
                            else if (ch == '"') { state = 10; token[length] = ch; length++; isRead = false; }
                            else { state = 8; token[length] = ch; length++; isRead = true; }
                            break;
                        }
                    case 9:
                        {
                            if (ch == 'n' || ch == 't' || ch == '"' || ch == '\\') { state = 8; token[length] = ch; length++; isRead = true; }
                            break;
                        }
                    case 10:
                        {
                            for (int i = 0; i < length; i++)
                                Console.Write(token[i]);
                            Console.Write(" is a string.\n");
                            length = 0;
                            Array.Clear(token, 0, token.Length);
                            state = 0;
                            isRead = true;
                            break;
                        }
                    case 11:
                        {
                            if (ch == '"') { state = 12; token[length] = ch; length++; isRead = true; }
                            break;
                        }
                    case 12:
                        {
                            if (ch == '"') { state = 13; token[length] = ch; length++; isRead = true; }
                            break;
                        }
                    case 13:
                        {
                            if (ch == '"') { state = 14; token[length] = ch; length++; isRead = true; }
                            else { state = 13; token[length] = ch; length++; isRead = true; }
                            break;
                        }
                    case 14:
                        {
                            if (ch == '"') { state = 15; token[length] = ch; length++; isRead = true; }
                            break;
                        }
                    case 15:
                        {
                            if (ch == '"') { state = 16; token[length] = ch; length++; isRead = false; }
                            break;
                        }
                    case 16:
                        {
                            for (int i = 0; i < length; i++)
                                Console.Write(token[i]);
                            Console.Write(" is a multiline comment.\n");
                            length = 0;
                            Array.Clear(token, 0, token.Length);
                            state = 0;
                            isRead = true;
                            break;
                        }
                    case 17:
                        {
                            if (ch == '\\') { state = 19; token[length] = ch; length++; isRead = true; }
                            else if (ch == '\'') { state = 21; token[length] = ch; length++; isRead = true; }
                            else { state = 18; token[length] = ch; length++; isRead = true; }
                            break;
                        }
                    case 18:
                        {
                            if (ch == '\\') { state = 19; token[length] = ch; length++; isRead = true; }
                            else if (ch == '\'') { state = 20; token[length] = ch; length++; isRead = false; }
                            else { state = 18; token[length] = ch; length++; isRead = true; }
                            break;
                        }
                    case 19:
                        {
                            if (ch == 'n' || ch == 't' || ch == '\\' || ch == '\'') { state = 17; token[length] = ch; length++; isRead = true; }
                            break;
                        }

                    case 20:
                        {
                            for (int i = 0; i < length; i++)
                                Console.Write(token[i]);
                            Console.Write(" is a string.\n");
                            length = 0;
                            Array.Clear(token, 0, token.Length);
                            state = 0;
                            isRead = false;
                            break;
                        }
                    case 21:
                        {
                            if (ch == '\'') { state = 22; token[length] = ch; length++; isRead = true; }
                            break;
                        }
                    case 22:
                        {
                            if (ch == '\'') { state = 23; token[length] = ch; length++; isRead = true; }
                            break;
                        }
                    case 23:
                        {
                            if (ch == '\'') { state = 24; token[length] = ch; length++; isRead = true; }
                            else { state = 23; token[length] = ch; length++; isRead = true; }
                            break;
                        }

                    case 24:
                        {
                            if (ch == '\'') { state = 25; token[length] = ch; length++; isRead = true; }
                            break;
                        }
                    case 25:
                        {
                            if (ch == '\'') { state = 26; token[length] = ch; length++; isRead = false; }
                            break;
                        }
                    case 26:
                        {
                            for (int i = 0; i < length; i++)
                                Console.Write(token[i]);
                            Console.Write(" is a multiline comment.\n");
                            length = 0;
                            Array.Clear(token, 0, token.Length);
                            state = 0;
                            isRead = true;
                            break;
                        }
                    case 27:
                        {
                            if ((int)ch == 10)
                            {
                                for (int i = 0; i < length; i++)
                                    Console.Write(token[i]);
                                Console.Write(" is a single comment.\n");
                                length = 0;
                                Array.Clear(token, 0, token.Length);
                                state = 0;
                                isRead = true;
                            }
                            else { state = 27; token[length] = ch; length++; isRead = true; }
                            break;
                        }
                    case 28:
                        {
                            if (ch == '=') { state = 29; token[length] = ch; length++; isRead = false; }
                            else
                            {
                                for (int i = 0; i < length; i++)
                                    Console.Write(token[i]);
                                Console.Write(" is an arithmatic operator.\n");
                                length = 0;
                                Array.Clear(token, 0, token.Length);
                                state = 0;
                                isRead = false;
                            }
                            break;
                        }
                    case 29:
                        {
                            for (int i = 0; i < length; i++)
                                Console.Write(token[i]);
                            Console.Write(" is an arithmatic assignment.\n");
                            length = 0;
                            Array.Clear(token, 0, token.Length);
                            state = 0;
                            isRead = true;
                            break;
                        }
                    case 30:
                        {
                            if (ch == '=') { state = 31; token[length] = ch; length++; isRead = false; }
                            else
                            {
                                for (int i = 0; i < length; i++)
                                    Console.Write(token[i]);
                                Console.Write(" is a bitwise operator.\n");
                                length = 0;
                                Array.Clear(token, 0, token.Length);
                                state = 0;
                                isRead = false;
                            }
                            break;
                        }
                    case 31:
                        {
                            for (int i = 0; i < length; i++)
                                Console.Write(token[i]);
                            Console.Write(" is an bitwise assignment.\n");
                            length = 0;
                            Array.Clear(token, 0, token.Length);
                            state = 0;
                            isRead = true;
                            break;
                        }
                    case 32:
                        {
                            if (ch == '<') { state = 33; token[length] = ch; length++; isRead = false; }
                            else if (ch == '=') { state = 34; token[length] = ch; length++; isRead = false; }
                            else
                            {
                                for (int i = 0; i < length; i++)
                                    Console.Write(token[i]);
                                Console.Write(" is a relational operator.\n");
                                length = 0;
                                Array.Clear(token, 0, token.Length);
                                state = 0;
                                isRead = false;
                            }
                            break;
                        }
                    case 33:
                        {
                            for (int i = 0; i < length; i++)
                                Console.Write(token[i]);
                            Console.Write(" is a bitwise operator.\n");
                            length = 0;
                            Array.Clear(token, 0, token.Length);
                            state = 0;
                            isRead = true;
                            break;
                        }
                    case 34:
                        {
                            for (int i = 0; i < length; i++)
                                Console.Write(token[i]);
                            Console.Write(" is a relational operator.\n");
                            length = 0;
                            Array.Clear(token, 0, token.Length);
                            state = 0;
                            isRead = true;
                            break;
                        }
                    case 35:
                        {
                            if (ch == '>') { state = 33; token[length] = ch; length++; isRead = false; }
                            else if (ch == '=') { state = 34; token[length] = ch; length++; isRead = false; }
                            else
                            {
                                for (int i = 0; i < length; i++)
                                    Console.Write(token[i]);
                                Console.Write(" is a relational operator.\n");
                                length = 0;
                                Array.Clear(token, 0, token.Length);
                                state = 0;
                                isRead = false;
                            }
                            break;
                        }
                    case 36:
                        {
                            for (int i = 0; i < length; i++)
                                Console.Write(token[i]);
                            Console.Write(" is a bitwise operator.\n");
                            length = 0;
                            Array.Clear(token, 0, token.Length);
                            state = 0;
                            isRead = true;
                            break;
                        }
                    case 37:
                        {
                            for (int i = 0; i < length; i++)
                                Console.Write(token[i]);
                            Console.Write(" is a relational operator.\n");
                            length = 0;
                            Array.Clear(token, 0, token.Length);
                            state = 0;
                            isRead = true;
                            break;
                        }
                    case 38:
                        {
                            for (int i = 0; i < length; i++)
                                Console.Write(token[i]);
                            Console.Write(" is a delimiter.\n");
                            length = 0;
                            Array.Clear(token, 0, token.Length);
                            state = 0;
                            isRead = true;
                            break;
                        }
                    case 39:
                        {
                            for (int i = 0; i < length; i++)
                                Console.Write(token[i]);
                            Console.Write(" is an assignment operator.\n");
                            length = 0;
                            Array.Clear(token, 0, token.Length);
                            state = 0;
                            isRead = true;
                            break;
                        }
                    case 40:
                        {
                            if ((ch >= 0x600 && ch < 0x660) ||
                                    (ch >= 0x66A && ch <= 0x6f0) ||
                                    (ch >= 0x750 && ch <= 0x77f) ||
                                    (ch >= 0xfb50 && ch <= 0xfc3f) ||
                                    (ch >= 0xfe70 && ch <= 0xfefc))
                            { state = 40; token[length] = ch; length++; isRead = true; }
                            else
                            {
                                state = 41; isRead = false;
                            }
                            break;
                        }
                    case 41:
                        {
                            char[] temp = new char[length];
                            for (int i = 0; i < length; i++)
                                temp[i] = token[i];

                            string Faword = new string(temp);   
                                                         
                            if (ENtoFa.ContainsKey(Faword))
                            {
                                string value = ENtoFa[Faword];
                                bool isKey = false;

                                for (int i = 0; i < 43; i++)
                                {
                                    if (keywords[i] == value)
                                    {
                                        isKey = true;
                                    }
                                }
                                if (isKey == false)
                                {
                                    for (int i = 0; i < length; i++)
                                        Console.Write(token[i]);
                                    Console.Write(" is an identifier.\n");
                                    length = 0;
                                    Array.Clear(token, 0, token.Length);
                                    state = 0;
                                    isRead = false;
                                }
                                else
                                {
                                    for (int i = 0; i < length; i++)
                                        Console.Write(token[i]);
                                    Console.Write(" is a keyword.\n");
                                    length = 0;
                                    Array.Clear(token, 0, token.Length);
                                    state = 0;
                                    isRead = false;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < length; i++)
                                    Console.Write(token[i]);
                                Console.Write(" is an identifier.\n");
                                length = 0;
                                Array.Clear(token, 0, token.Length);
                                state = 0;
                                isRead = false;
                            }                          
                            break;
                        }
                    case 42:
                        {
                            if (ch == '٠' || ch == '١' || ch == '۲' || ch == '۳' ||
                                    ch == '۴' || ch == '۵' || ch == '۶' || ch == '۷' ||
                                    ch == '۸' || ch == '۹')
                            { state = 42; token[length] = ch; length++; isRead = true; }

                            else if (ch == '.') { state = 43; token[length] = ch; length++; isRead = true; }
                            else
                            {
                                for (int i = 0; i < length; i++)
                                    Console.Write(token[i]);
                                Console.Write(" is a persian INT number.\n");
                                length = 0;
                                Array.Clear(token, 0, token.Length);
                                state = 0;
                                isRead = false;                              
                            }
                            break;
                        }
                    case 43:
                        {
                            if (ch == '٠' || ch == '١' || ch == '۲' || ch == '۳' ||
                                    ch == '۴' || ch == '۵' || ch == '۶' || ch == '۷' ||
                                    ch == '۸' || ch == '۹')
                            { state = 43; token[length] = ch; length++; isRead = true; }
                            else
                            {
                                for (int i = 0; i < length; i++)
                                    Console.Write(token[i]);
                                Console.Write(" is a persian FLOAT number.\n");
                                length = 0;
                                Array.Clear(token, 0, token.Length);
                                state = 0;
                                isRead = false;
                            }
                            break;
                        }
                    case 44:
                        {
                            if (ch == '٠' || ch == '١' || ch == '۲' || ch == '۳' ||
                                    ch == '۴' || ch == '۵' || ch == '۶' || ch == '۷' ||
                                    ch == '۸' || ch == '۹')
                            { state = 43; token[length] = ch; length++; isRead = true; }

                            else if (ch <= '9' && ch >= '0')
                            { state = 5; token[length] = ch; length++; isRead = true; }
                            else
                            {
                                for (int i = 0; i < length; i++)
                                    Console.Write(token[i]);
                                Console.Write(" is a punctuation sign.\n");
                                length = 0;
                                Array.Clear(token, 0, token.Length);
                                state = 0;
                                isRead = false;
                            }
                            break;
                        }
                }
            }
        }
        static void Main(string[] args)
        {
            lexer();
            Console.WriteLine("press any key to continue");
            Console.ReadLine();
        }
    }
}
