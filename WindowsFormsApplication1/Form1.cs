using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualBasic;

namespace WindowsFormsApplication1
{
    
    /// <summary>
    /// Класс Form1.
    /// Содержит функции, процедуры, а также описанные переменные, реализующие обмен информации между порльзователями 
    /// по схеме клиент-сервер.
    /// </summary>
    public partial class Form1 : Form
    {
       
        #region Объявляем переменные
        /// <summary>
        /// Переменные использующееся для хранения личной информации
        /// </summary>
        public string id, firstname, secondname,thirdname, pubemail;

        /// <summary>
        /// Поток для отображения пользователей в listbox1.
        /// В режиме обычных пользователей.
        /// </summary>
        public Thread ThreadListbox;

        /// <summary>
        /// Поток для отображения пользователей в listbox2.
        /// В режиме администратора.
        /// </summary>
        public Thread ThreadListboxAdm;

        /// <summary>
        /// Поток для отбражения истории переписки в richtextbox1.
        /// </summary>
        public Thread ThreadTextbox;

        /// <summary>
        /// Поток для регистрации пользователя в режиме онлайн.
        /// </summary>
        public Thread ThreadOnline;

        /// <summary>
        /// Хранит в переменной имя, фамилию и отчество выбранного собеседника пользователем. 
        /// </summary>
        public string sobesednik = "";

        /// <summary>
        /// Хранит количество сообщений.
        /// </summary>
        public int pubcount = 0;

        /// <summary>
        /// Хранит количество пользователей
        /// </summary>
        public int pubcountList = 0;

        /// <summary>
        /// Флаг используется для остановки потока functextbox.
        /// </summary>
        public byte Flag;

        /// <summary>
        /// Переменная, которая ранит список имен пользователей listbox. 
        /// </summary>
        public string[] listboxold = new string [999];

        /// <summary>
        /// Флаг используется для остановки потока functextbox.
        /// </summary>
        public int wait;
        #endregion
        
        #region Инициализирует компоненты
        /// <summary>
        /// Инициализирует компоненты. 
        /// Создает новые объекты потоков.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            ThreadListbox = new Thread(funclistbox);
            ThreadListboxAdm = new Thread(funclistboxAdm);
            ThreadTextbox = new Thread(functextbox);
            ThreadOnline = new Thread(funconline);
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
        }
        #endregion

        #region Кнопка Авторизации
            /// <summary>
            /// По нажатию , осуществляет авторизацию пользователя.
            /// В зависимости от прав содержащихся в БД показывает ту,
            /// или иную панель.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"><see cref="EventArgs"/></param>
            private void button1_Click(object sender, EventArgs e)
            {
               
                string email;
                string password;
                string log;

                email = textBox1.Text;
                password = textBox2.Text;
              
                label1.Visible = false;
                label2.Visible = false;
                try
                {
                    AuthMethod(email, password, out id, out firstname, out secondname, out thirdname, out log);

                    if ((log == "true") || (log == "Admin"))
                    {
                        if (log == "true")
                        {
                            textBox1.Visible = false;
                            textBox2.Visible = false;
                            button1.Visible = false;
                            listBox1.Visible = true;
                            richTextBox2.Visible = true;
                            richTextBox3.Visible = true;
                            button2.Visible = true;
                            label10.Visible = false;
                            label11.Visible = false;
                        }
                        else
                        {
                            textBox1.Visible = false;
                            textBox2.Visible = false;
                            label10.Visible = false;
                            label11.Visible = false;
                            button1.Visible = false;
                            listBox2.Visible = true;
                            textBox3.Visible = true;
                            textBox4.Visible = true;
                            textBox5.Visible = true;
                            textBox6.Visible = true;
                            textBox7.Visible = true;
                            button3.Visible = true;
                            button4.Visible = true;
                            button5.Visible = true;
                            button6.Visible = true;
                            label4.Visible = true;
                            label5.Visible = true;
                            label6.Visible = true;
                            label7.Visible = true;
                            label8.Visible = true;
                        }
                        

                        if (label1.Visible == true)
                        {
                            label1.Visible = false;
                        }
                        if (log == "true")
                        {
                            ThreadListbox.Start();
                            ThreadTextbox.Start();
                            ThreadOnline.Start();
                        }
                        else
                        {
                            ThreadListboxAdm.Start();
                        }
                    }
                    else
                    {
                        label1.Visible = true;
                    }
                }
                catch
                {
                    label2.Visible = true;

                }

            }
            #endregion

        #region Кнопка Авторизации. Метод AuthMethod.
            /// <summary>
            /// По нажатию , осуществляет авторизацию пользователя.
            /// В зависимости от прав содержащихся в БД показывает ту,
            /// или иную панель.
            /// Вызывается из Button1_Click.
            /// </summary>
            public void AuthMethod(string email, string password, out string id, out string firstname, out string secondname, out string thirdname, out string log) 
            {
                HttpWebRequest req;
                HttpWebResponse resp;
                StreamReader sr;
                string content;

                byte[] buffer = Encoding.UTF8.GetBytes(email);
                email = Convert.ToBase64String(buffer);
                //richTextBox1.Text += email;
                buffer = Encoding.UTF8.GetBytes(password);
                password = Convert.ToBase64String(buffer);
                //richTextBox1.Text += email;

                req = (HttpWebRequest)WebRequest.Create("http://fuckingskype.funny-pixels.ru/?action=login&email=" + email + "&password=" + password);
                resp = (HttpWebResponse)req.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                content = sr.ReadToEnd();
                sr.Close();
                req = null;

                string input;
                string pattern = @"<login>([a-zA-Z0-9]*)</login>";
                input = content;
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                Match match = rgx.Match(input);

                 log = match.Groups[1].Value.ToString();

                string pattern1 = @"<id>(.*)</id>";
                input = content;
                Regex rgx1 = new Regex(pattern1, RegexOptions.IgnoreCase);
                Match match1 = rgx1.Match(input);

                buffer = Convert.FromBase64String(match1.Groups[1].Value.ToString());
                id = Encoding.UTF8.GetString(buffer);


                string pattern2 = @"<firstname>(.*)</firstname>";
                input = content;
                Regex rgx2 = new Regex(pattern2, RegexOptions.IgnoreCase);
                Match match2 = rgx2.Match(input);

                buffer = Convert.FromBase64String(match2.Groups[1].Value.ToString());
                firstname = Encoding.UTF8.GetString(buffer);
                //richTextBox1.Text = log;

                string pattern3 = @"<secondname>(.*)</secondname>";
                input = content;
                Regex rgx3 = new Regex(pattern3, RegexOptions.IgnoreCase);
                Match match3 = rgx3.Match(input);

                buffer = Convert.FromBase64String(match3.Groups[1].Value.ToString());
                secondname = Encoding.UTF8.GetString(buffer);


                string pattern4 = @"<thirdname>(.*)</thirdname>";
                input = content;
                Regex rgx4 = new Regex(pattern4, RegexOptions.IgnoreCase);
                Match match4 = rgx4.Match(input);

                buffer = Convert.FromBase64String(match4.Groups[1].Value.ToString());
                thirdname = Encoding.UTF8.GetString(buffer);
                return;
            }
            #endregion

        #region Обновляет список пользователей,их статус и тд.
                /// <summary>
                /// Функция,которая отправляет запросы каждые 5с.
                /// Ответ от сервера парсится и заносится в listbox.
                /// Данныя функция используется при авторизации пользователя
                /// </summary>
                void funclistbox()
                {
                    string[] online = new string[999];
                    string[] firstname = new string[999];
                    string[] secondname = new string[999];
                    string[] thirdname = new string[999];
                    string[] message = new string[999];
                    
                    int count;
                    funclistboxMethod(id, out count, online, firstname, secondname, thirdname, message);

                    if (pubcountList != count)
                    {

                        if ((listboxold[pubcountList] != (online[pubcountList] + " " + firstname[pubcountList] + " " + secondname[pubcountList] + " " + thirdname[pubcountList] + " " + message[pubcountList])) && (listboxold != null))
                        {
                            listBox1.Invoke(new Action(() => { listBox1.Items.Clear(); }));

                            for (int i = 0; i <= pubcountList - 1; i++)
                            {

                                listboxold[i] = null;

                            }
                        }
                    }
                    for (int i = 0; i <= count - 1; i++)
                    {
                        if (listboxold[i] == null)
                        {
                            listBox1.Invoke(new Action(() => { listBox1.Items.Insert(i, online[i] + " " + firstname[i] + " " + secondname[i] + " " + thirdname[i] + " " + message[i]); }));
                            listboxold[i] = online[i] + " " + firstname[i] + " " + secondname[i] + " " + thirdname[i] + " " + message[i];
                        }
                        if ((listboxold[i] != (online[i] + " " + firstname[i] + " " + secondname[i] + " " + thirdname[i] + " " + message[i])) && (listboxold[i] != null))
                        {
                            listBox1.Invoke(new Action(() => { listBox1.Items[i] = online[i] + " " + firstname[i] + " " + secondname[i] + " " + thirdname[i] + " " + message[i]; }));
                            listboxold[i] = online[i] + " " + firstname[i] + " " + secondname[i] + " " + thirdname[i] + " " + message[i];

                        }
                    }
                    pubcountList = count;


                    Thread.Sleep(5000);
                    funclistbox();

                }
                #endregion 
            
        #region Обновляет список пользователей,их статус и тд.Вызывается funclistbox
                /// <summary>
                /// Функция,которая отправляет запросы каждые 5с.
                /// Ответ от сервера парсится и заносится в listbox.
                /// Данныя функция используется при авторизации пользователя.
                /// Данная функция вызывается funclistbox.
                /// </summary>
                public void funclistboxMethod(string id, out int count, string[] online, string[] firstname, string[] secondname, string[] thirdname, string[] message)
                {
                    HttpWebRequest req;
                    HttpWebResponse resp;
                    StreamReader sr;
                    string content;


                    req = (HttpWebRequest)WebRequest.Create("http://fuckingskype.funny-pixels.ru/?action=listbox&id=" + id);
                    resp = (HttpWebResponse)req.GetResponse();
                    sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                    content = sr.ReadToEnd();
                    sr.Close();
                    req = null;

                    count = 0;
                    string input = content;
                    string pattern = @"<login>([a-zA-Z0-9]*)</login>";
                    Regex regex = new Regex(pattern);
                    Match match = regex.Match(input);
                    while (match.Success)
                    {
                        online[count] = match.Groups[1].Value;
                        count = count + 1;
                        match = match.NextMatch();
                    }

                    count = 0;
                    input = content;
                    string pattern1 = @"<firstname>([а-яА-Я0-9]*)</firstname>";
                    Regex regex1 = new Regex(pattern1);
                    Match match1 = regex1.Match(input);
                    while (match1.Success)
                    {
                        firstname[count] = match1.Groups[1].Value;
                        count = count + 1;
                        match1 = match1.NextMatch();
                    }

                    count = 0;
                    input = content;
                    string pattern2 = @"<secondname>([а-яА-Я0-9]*)</secondname>";
                    Regex regex2 = new Regex(pattern2);
                    Match match2 = regex2.Match(input);
                    while (match2.Success)
                    {
                        secondname[count] = match2.Groups[1].Value;
                        count = count + 1;
                        match2 = match2.NextMatch();
                    }

                    count = 0;
                    input = content;
                    string pattern4 = @"<thirdname>([а-яА-Я0-9]*)</thirdname>";
                    Regex regex4 = new Regex(pattern4);
                    Match match4 = regex4.Match(input);
                    while (match4.Success)
                    {
                        thirdname[count] = match4.Groups[1].Value;
                        count = count + 1;
                        match4 = match4.NextMatch();
                    }

                    count = 0;
                    input = content;
                    string pattern3 = @"<message>(.*)</message>";
                    Regex regex3 = new Regex(pattern3);
                    Match match3 = regex3.Match(input);
                    while (match3.Success)
                    {
                        message[count] = match3.Groups[1].Value;
                        count = count + 1;
                        match3 = match3.NextMatch();
                    }
                    //double count1 = count, firstname1=firstname, secondname, thirdname, message, online;
                    //return;
                }
            #endregion
        
        #region По нажатию на пользователя выводит историю переписки
            /// <summary>
            /// По нажатию приостанавливает поток functextbox.
            /// Отправляет запрос и выводит в richtextbox историю переписки.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"><see cref="EventArgs"/></param>
            private void listBox1_SelectedValueChanged(object sender, EventArgs e)
            {
                int flagStop = 0;
                string a;
                a = listBox1.SelectedIndex.ToString();
                int b = int.Parse(a);
                if (b != -1)
                {
                    while (flagStop == 0)
                    {
                        if (wait == 0)
                        {

                            sobesednik = listBox1.Items[b].ToString();
                            richTextBox3.Text = "";
                            pubcount = 0;
                            
                            string[] firstname1 = new string[999];
                            string[] secondname1 = new string[999];
                            string[] thirdname1 = new string[999];
                            string[] message1 = new string[999];
                            string[] date1 = new string[999];

                            int count;
                            SelectedMethod(sobesednik, id, firstname, secondname, thirdname, out count, date1, firstname1, secondname1, thirdname1, message1);

                            if (pubcount != count)
                            {


                                for (int i = pubcount; i <= count - 1; i++)
                                {
                                    richTextBox3.Invoke(new Action(() => { richTextBox3.Text += firstname1[i] + " " + secondname1[i] + " " + thirdname1[i] + " " + date1[i] + "\n"; richTextBox3.Text += message1[i] + "\n\n"; }));
                                }
                                pubcount = count;
                            }

                            flagStop = 1;
                        }
                    }
                }
            }
            #endregion
        
        #region По нажатию на пользователя выводит историю переписки.Метод SelectedMethod.
            /// <summary>
            /// По нажатию приостанавливает поток functextbox.
            /// Отправляет запрос и выводит в richtextbox историю переписки.
            /// Вызывается функция из SelectedValueChanged.
            /// Так же используется в потоке обновления истории переписки.
            /// Вызывается из functextbox.
            /// </summary>
            public void SelectedMethod(string sobesednik, string id,string firstname, string secondname, string thirdname, out int count, string[] date1, string[] firstname1, string[] secondname1, string[] thirdname1, string[] message1)
            {
                HttpWebRequest req;
                HttpWebResponse resp;
                StreamReader sr;
                string content;
                string input;


                req = (HttpWebRequest)WebRequest.Create("http://fuckingskype.funny-pixels.ru/?action=message&id=" + id + "&firstname=" + firstname + "&secondname=" + secondname + "&thirdname=" + thirdname + "&sobesednik=" + sobesednik);
                resp = (HttpWebResponse)req.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                content = sr.ReadToEnd();
                sr.Close();
                req = null;

                count = 0;
                input = content;
                string pattern1 = @"<firstname>([а-яА-Я0-9]*)</firstname>";
                Regex regex1 = new Regex(pattern1);
                Match match1 = regex1.Match(input);
                while (match1.Success)
                {
                    firstname1[count] = match1.Groups[1].Value;
                    count = count + 1;
                    match1 = match1.NextMatch();
                }

                count = 0;
                input = content;
                string pattern2 = @"<secondname>([а-яА-Я0-9]*)</secondname>";
                Regex regex2 = new Regex(pattern2);
                Match match2 = regex2.Match(input);
                while (match2.Success)
                {
                    secondname1[count] = match2.Groups[1].Value;
                    count = count + 1;
                    match2 = match2.NextMatch();
                }

                count = 0;
                input = content;
                string pattern5 = @"<thirdname>([а-яА-Я0-9]*)</thirdname>";
                Regex regex5 = new Regex(pattern5);
                Match match5 = regex5.Match(input);
                while (match5.Success)
                {
                    thirdname1[count] = match5.Groups[1].Value;
                    count = count + 1;
                    match5 = match5.NextMatch();
                }

                count = 0;
                input = content;
                string pattern3 = @"<message>(.*)</message>";
                RegexOptions options = RegexOptions.Multiline;
                Regex regex3 = new Regex(pattern3, options);
                Match match3 = regex3.Match(input);
                while (match3.Success)
                {
                    byte[] buffer = Convert.FromBase64String(match3.Groups[1].Value.ToString());
                    message1[count] = Encoding.UTF8.GetString(buffer);
                    //message1[count] = match3.Groups[1].Value;
                    count = count + 1;
                    match3 = match3.NextMatch();
                }

                count = 0;
                input = content;
                string pattern4 = @"<date>(.*)</date>";
                Regex regex4 = new Regex(pattern4);
                Match match4 = regex4.Match(input);
                while (match4.Success)
                {

                    date1[count] = match4.Groups[1].Value;
                    count = count + 1;
                    match4 = match4.NextMatch();
                }
            }
        #endregion
        
        #region Поток, который обновлет историю переписки
            /// <summary>
            /// Функция, которая обновляет историю переписки в richtextbox.
            /// </summary>
            void functextbox()
            {
                if (sobesednik != "")
                {
                    wait = 1;
                
                    string[] firstname1 = new string[999];
                    string[] secondname1 = new string[999];
                    string[] thirdname1 = new string[999];
                    string[] message1 = new string[999];
                    string[] date1 = new string[999];
                 
                    int count;
                    SelectedMethod(sobesednik, id, firstname, secondname, thirdname, out count, date1, firstname1, secondname1, thirdname1, message1); 
                   

                    if (pubcount != count)
                    {


                        for (int i = pubcount; i <= count - 1; i++)
                        {
                            richTextBox3.Invoke(new Action(() => { richTextBox3.Text += firstname1[i] + " " + secondname1[i] + " " + thirdname1[i] + " " + date1[i] + "\n"; richTextBox3.Text += message1[i] + "\n\n"; }));
                        }
                        pubcount = count;
                    }
                }
                wait = 0;
                Thread.Sleep(5000);
                functextbox();
            }
            #endregion

        #region кнопка отправки сообщения
            /// <summary>
            /// Оправляет сообщение пользователя на сервер,где оно записывается в БД.
            /// И как следствие отображается в истории переписки двух пользователей.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"><see cref="EventArgs"/></param>
            private void button2_Click(object sender, EventArgs e)
            {
                if (richTextBox2.Text != "")
                {
                    HttpWebRequest req;
                    HttpWebResponse resp;
                    StreamReader sr;
                    string content;
                    string message;

                    message = richTextBox2.Text;
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    message = Convert.ToBase64String(buffer);
                    req = (HttpWebRequest)WebRequest.Create("http://fuckingskype.funny-pixels.ru/?action=send&id=" + id + "&sobesednik=" + sobesednik + "&message=" + message);
                    resp = (HttpWebResponse)req.GetResponse();

                    sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                    content = sr.ReadToEnd();
                    sr.Close();
                    req = null;
                    richTextBox2.Clear();
                }
            }
            #endregion

        #region Статус Онлайн пользователя
            /// <summary>
            /// Функция, которая отправляет запрос каждые 5с.
            /// При котором происходит запись в БД время пребывания пользователя в режиме онлайн.
            /// </summary>
            void funconline()
            {
                HttpWebRequest req;
                HttpWebResponse resp;
                StreamReader sr;
                string content;
                req = (HttpWebRequest)WebRequest.Create("http://fuckingskype.funny-pixels.ru/?action=online&id=" + id);
                resp = (HttpWebResponse)req.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                content = sr.ReadToEnd();
                sr.Close();
                req = null;
                Thread.Sleep(5000);
                funconline();
            }
            #endregion

        #region Обновляет список пользователей для администратора
            /// <summary>
            /// Функция,которая отправляет запросы каждые 5с.
            /// Ответ от сервера парсится и заносится в listbox.
            /// Данныя функция используется при авторизации администратора.
            /// </summary>
            void funclistboxAdm()
            {

               
                string[] firstname = new string[999];
                string[] secondname = new string[999];
                string[] thirdname = new string[999];
                
                int count;
                
                funclistboxAdmMethod(id, out count, firstname, secondname, thirdname);
                
                if (pubcountList != count)
                {

                    if ((listboxold[pubcountList] != (firstname[pubcountList] + " " + secondname[pubcountList] + thirdname[pubcountList])) && (listboxold != null))
                    {
                        listBox2.Invoke(new Action(() => { listBox2.Items.Clear(); }));
                        for (int i = 0; i <= pubcountList - 1; i++)
                        {

                            listboxold[i] = null;

                        }
                    }
                }

                for (int i = 0; i <= count - 1; i++)
                {
                    if (listboxold[i] == null)
                    {
                        listBox2.Invoke(new Action(() => { listBox2.Items.Insert(i, firstname[i] + " " + secondname[i] + " " + thirdname[i]); }));
                        listboxold[i] = firstname[i] + " " + secondname[i] + " " + thirdname[i];
                    }
                    if ((listboxold[i] != (firstname[i] + " " + secondname[i] + " " + thirdname[i])) && (listboxold[i] != null))
                    {
                        listBox2.Invoke(new Action(() => { listBox2.Items[i] = firstname[i] + " " + secondname[i] + " " + thirdname[i]; }));
                        listboxold[i] = firstname[i] + " " + secondname[i] + " " + thirdname[i];

                    }

                }

                pubcountList = count;
                Thread.Sleep(5000);
                funclistboxAdm();
            }
            #endregion

        #region Обновляет список пользователей для администратора. Метод funclistboxAdmMethod.
            /// <summary>
            /// Функция,которая отправляет запросы каждые 5с.
            /// Ответ от сервера парсится и заносится в listbox.
            /// Данныя функция используется при авторизации администратора.
            /// Вызывается данный метод funclistboxAdmMethod из функции funclistboxAdm.
            /// </summary>
            public void funclistboxAdmMethod(string id, out int count, string[] firstname, string[] secondname, string[] thirdname)
            {
                HttpWebRequest req;
                HttpWebResponse resp;
                StreamReader sr;
                string content;

                req = (HttpWebRequest)WebRequest.Create("http://fuckingskype.funny-pixels.ru/?action=listboxAdm&id=" + id);
                resp = (HttpWebResponse)req.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                content = sr.ReadToEnd();
                sr.Close();
                req = null;
                string input = content;

                count = 0;
                input = content;
                string pattern1 = @"<firstname>([а-яА-Я0-9]*)</firstname>";
                Regex regex1 = new Regex(pattern1);
                Match match1 = regex1.Match(input);
                while (match1.Success)
                {
                    firstname[count] = match1.Groups[1].Value;
                    count = count + 1;
                    match1 = match1.NextMatch();
                }

                count = 0;
                input = content;
                string pattern2 = @"<secondname>([а-яА-Я0-9]*)</secondname>";
                Regex regex2 = new Regex(pattern2);
                Match match2 = regex2.Match(input);
                while (match2.Success)
                {
                    secondname[count] = match2.Groups[1].Value;
                    count = count + 1;
                    match2 = match2.NextMatch();
                }

                count = 0;
                input = content;
                string pattern3 = @"<thirdname>([а-яА-Я0-9]*)</thirdname>";
                Regex regex3 = new Regex(pattern3);
                Match match3 = regex3.Match(input);
                while (match3.Success)
                {
                    thirdname[count] = match3.Groups[1].Value;
                    count = count + 1;
                    match3 = match3.NextMatch();
                }
            }
            #endregion

        #region Получение информации о пользователе для редактирования
            /// <summary>
            /// По нажатию отправляет запрос с данными выбранного пользователя.
            /// И возвращает их в специально отведенные textbox.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"><see cref="EventArgs"/></param>
            private void listBox2_SelectedValueChanged(object sender, EventArgs e)
            {
                string a;
                a = listBox2.SelectedIndex.ToString();
                int b = int.Parse(a);
                if (b != -1)
                {
                    sobesednik = listBox2.Items[b].ToString();
                    string password, firstname1, secondname1, thirdname1;

                    SelectedAdmMethod(sobesednik, id,out firstname1, out secondname1, out thirdname1, out pubemail, out password);

                    textBox3.Text = firstname1;
                    textBox4.Text = secondname1;
                    textBox5.Text = thirdname1;
                    textBox6.Text = pubemail;
                    textBox7.Text = password;
                }

            }
            #endregion

        #region Получение информации о пользователе для редактирования. Метод SelectedAdmMethod.
            /// <summary>
            /// По нажатию отправляет запрос с данными выбранного пользователя.
            /// И возвращает их в специально отведенные textbox.
            /// Данный метод SelectedAdmMethod вызывается из listBox2_SelectedValueChanged.
            /// </summary>
            public void SelectedAdmMethod(string sobesednik, string id,out string firstname1, out string secondname1, out string thirdname1, out string pubemail, out string password)
            {
                HttpWebRequest req;
                HttpWebResponse resp;
                StreamReader sr;
                string content;
                string input;

                req = (HttpWebRequest)WebRequest.Create("http://fuckingskype.funny-pixels.ru/?action=find&id=" + id + "&sobesednik=" + sobesednik);
                resp = (HttpWebResponse)req.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                content = sr.ReadToEnd();
                sr.Close();
                req = null;

                string pattern = @"<firstname>([а-яА-Я0-9]*)</firstname>";
                input = content;
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                Match match = rgx.Match(input);

                firstname1 = match.Groups[1].Value.ToString();

                string pattern2 = @"<secondname>([а-яА-Я0-9]*)</secondname>";
                input = content;
                Regex rgx2 = new Regex(pattern2, RegexOptions.IgnoreCase);
                Match match2 = rgx2.Match(input);

                secondname1 = match2.Groups[1].Value.ToString();

                string pattern3 = @"<thirdname>([а-яА-Я0-9]*)</thirdname>";
                input = content;
                Regex rgx3 = new Regex(pattern3, RegexOptions.IgnoreCase);
                Match match3 = rgx3.Match(input);

                thirdname1 = match3.Groups[1].Value.ToString();

                string pattern4 = @"<email>(.*)</email>";
                input = content;
                Regex rgx4 = new Regex(pattern4, RegexOptions.IgnoreCase);
                Match match4 = rgx4.Match(input);
                byte[] buffer = Convert.FromBase64String(match4.Groups[1].Value.ToString());
                pubemail = Encoding.UTF8.GetString(buffer);
                //pubemail = match4.Groups[1].Value.ToString();

                

                string pattern5 = @"<password>(.*)</password>";
                input = content;
                Regex rgx5 = new Regex(pattern5, RegexOptions.IgnoreCase);
                Match match5 = rgx5.Match(input);
                buffer = Convert.FromBase64String(match5.Groups[1].Value.ToString());
                password = Encoding.UTF8.GetString(buffer);
                //textBox7.Text = match5.Groups[1].Value.ToString();
            }
            #endregion
            
        #region кнопка Delete
            /// <summary>
            /// Удаляет выбранного пользователя.
            /// Доступно только администратору.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"><see cref="EventArgs"/></param>
            private void button3_Click(object sender, EventArgs e)
            {
                HttpWebRequest req;
                HttpWebResponse resp;
                StreamReader sr;
                string content;
                //string email;
                //email = textBox6.Text;
                byte[] buffer = Encoding.UTF8.GetBytes(pubemail);
                pubemail = Convert.ToBase64String(buffer);
                req = (HttpWebRequest)WebRequest.Create("http://fuckingskype.funny-pixels.ru/?action=delete&id=" + id + "&email=" + pubemail);
                resp = (HttpWebResponse)req.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                content = sr.ReadToEnd();
                sr.Close();
                req = null;
                label12.Visible = false;
                label3.Visible = false;
                label9.Visible = false;
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();

            }
            #endregion

        #region кнопка Insert
            /// <summary>
            /// Добавляет нового пользователя в БД.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"><see cref="EventArgs"/></param>
            private void button5_Click(object sender, EventArgs e)
            {
                HttpWebRequest req;
                HttpWebResponse resp;
                StreamReader sr;
                string content;
                string email;
                string password;
                string firstname;
                string secondname;
                string thirdname;

                firstname = textBox3.Text;
                secondname = textBox4.Text;
                thirdname = textBox5.Text;
                email = textBox6.Text;
               // byte[] buffer = Encoding.UTF8.GetBytes(email);
               // email = Convert.ToBase64String(buffer);
                password = textBox7.Text;
               // buffer = Encoding.UTF8.GetBytes(password);
               // password = Convert.ToBase64String(buffer);
                if (((firstname != "") && (secondname != "")) && (email != "") && (password != ""))
                {
                    req = (HttpWebRequest)WebRequest.Create("http://fuckingskype.funny-pixels.ru/?action=insert&id=" + id + "&email=" + email + "&password=" + password + "&firstname=" + firstname + "&secondname=" + secondname + "&thirdname=" + thirdname);
                    resp = (HttpWebResponse)req.GetResponse();
                    sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                    content = sr.ReadToEnd();
                    sr.Close();
                    req = null;

                    string input;
                    string pattern = @"<login>([a-zA-Z0-9]*)</login>";
                    input = content;
                    Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                    Match match = rgx.Match(input);
                    
                    label12.Visible = false;
                    label3.Visible = false;
                    label9.Visible = false;
                    if (match.Groups[1].Value.ToString() == "name")
                    {
                        label9.Visible = true;
                        label3.Visible = false;
                    }
                    else
                    {
                        if (match.Groups[1].Value.ToString() == "email")
                        {
                            label9.Visible = false;
                            label3.Visible = true;
                        }
                        else
                        {
                            textBox3.Clear();
                            textBox4.Clear();
                            textBox5.Clear();
                            textBox6.Clear();
                            textBox7.Clear();
                            label3.Visible = false;

                        }
                    }

                }
                else {
                    label12.Visible = true;
                    label9.Visible = false;
                    label3.Visible = false;
                }
            }
            #endregion

        #region кнопка Update
            /// <summary>
            /// Отправляет запрос с данными пользователя на сервер.
            /// Где происходит обновление информации о нем.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"><see cref="EventArgs"/></param>
            private void button4_Click(object sender, EventArgs e)
            {
                HttpWebRequest req;
                HttpWebResponse resp;
                StreamReader sr;
                string content;
                string email;
                string password;
                string firstname;
                string secondname;
                string thirdname;

                firstname = textBox3.Text;
                secondname = textBox4.Text;
                thirdname = textBox5.Text;
                email = textBox6.Text;
               // byte[] buffer = Encoding.UTF8.GetBytes(email);
              //  email = Convert.ToBase64String(buffer);
                password = textBox7.Text;
            //    buffer = Encoding.UTF8.GetBytes(password);
            //    password = Convert.ToBase64String(buffer);
            //    buffer = Encoding.UTF8.GetBytes(pubemail);
            //    pubemail = Convert.ToBase64String(buffer);
                if (((firstname != "") && (secondname != "")) && (email != "") && (password != ""))
                {
                    req = (HttpWebRequest)WebRequest.Create("http://fuckingskype.funny-pixels.ru/?action=update&id=" + id + "&email=" + email + "&pubemail=" + pubemail + "&password=" + password + "&firstname=" + firstname + "&secondname=" + secondname + "&thirdname=" + thirdname);
                    resp = (HttpWebResponse)req.GetResponse();
                    sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                    content = sr.ReadToEnd();
                    sr.Close();
                    req = null;

                    string input;
                    string pattern = @"<login>([a-zA-Z0-9]*)</login>";
                    input = content;
                    Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                    Match match = rgx.Match(input);

                    label12.Visible = false;
                    label3.Visible = false;
                    label9.Visible = false;
                    if (match.Groups[1].Value.ToString() == "name")
                    {
                        label9.Visible = true;
                        label3.Visible = false;
                    }
                    else
                    {
                        if (match.Groups[1].Value.ToString() == "email")
                        {
                            label9.Visible = false;
                            label3.Visible = true;
                        }
                        else
                        {
                            //textBox3.Clear();
                            //textBox4.Clear();
                            //textBox5.Clear();
                            //textBox6.Clear();
                            //textBox7.Clear();
                            label3.Visible = false;
                            label9.Visible = false;
                        }
                    }
                    //textBox3.Clear();
                    //textBox4.Clear();
                    //textBox5.Clear();
                    //textBox6.Clear();
                    //textBox7.Clear();
                }
                else {
                    label12.Visible = true;
                    label9.Visible = false;
                    label3.Visible = false;
                }
            }
            #endregion

        #region Очищение Textbox
            /// <summary>
            /// Очищает все textbox.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"><see cref="EventArgs"/></param>
            private void button6_Click(object sender, EventArgs e)
            {
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
            }
            #endregion

        #region Запрет на ввод пробелов textbox
            /// <summary>
            /// Запрещает ввод пробела для textbox3.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"><see cref="EventArgs"/></param>
            private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
            {
                char c = e.KeyChar;
                e.Handled = c == ' ';

            }
            /// <summary>
            /// Запрещает ввод пробела для textbox4.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"><see cref="EventArgs"/></param>
            private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
            {
                char c = e.KeyChar;
                e.Handled = c == ' ';
            }
            /// <summary>
            /// Запрещает ввод пробела для textbox5.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"><see cref="EventArgs"/></param>
            private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
            {
                char c = e.KeyChar;
                e.Handled = c == ' ';
            }
            /// <summary>
            /// Запрещает ввод пробела для textbox6.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"><see cref="EventArgs"/></param>
            private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
            {
                char c = e.KeyChar;
                e.Handled = c == ' ';
            }


            /// <summary>
            /// Запрещает ввод пробела для textbox7.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"><see cref="EventArgs"/></param>
            private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
            {
                char c = e.KeyChar;
                e.Handled = c == ' ';
            }
            #endregion

        #region Закрывает все потоки при завершении работы приложения
            /// <summary>
            /// Закрывает все потоки по завершению работы приложения.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"><see cref="EventArgs"/></param>
            private void OnApplicationExit(object sender, EventArgs e)
            {
                ThreadListboxAdm.IsBackground = true;
                ThreadListbox.IsBackground = true;
                ThreadTextbox.IsBackground = true;
                ThreadOnline.IsBackground = true;
            }
            #endregion
        
    }
   
    }
    
      
      


    

