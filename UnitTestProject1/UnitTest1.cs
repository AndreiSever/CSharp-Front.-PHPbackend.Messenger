using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Windows.Forms;
namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void funclistboxMethod1()
        {
           
           string[] online = new string[999];
           string[] firstname = new string[999];
           string[] secondname = new string[999];
           string[] thirdname = new string[999];
           string[] message = new string[999];
           int count;
           string id="2";
           byte Result = 0;
           WindowsFormsApplication1.Form1 sut = new WindowsFormsApplication1.Form1();
           sut.funclistboxMethod(id, out count, online, firstname, secondname, thirdname, message);
           
            if ((online!= null) && (firstname!= null)&& (secondname!= null)&& (thirdname!= null))
            {
             Result = 1;
            
            }
            
            Assert.AreEqual(1, Result);


           
        }
        [TestMethod]
        public void funclistboxMethod2()
        {

            string[] online = new string[999];
            string[] firstname = new string[999];
            string[] secondname = new string[999];
            string[] thirdname = new string[999];
            string[] message = new string[999];
            int count;
            string id = "2";
            byte Result = 1;
            WindowsFormsApplication1.Form1 sut = new WindowsFormsApplication1.Form1();
            sut.funclistboxMethod(id, out count, online, firstname, secondname, thirdname, message);
            for (int i = 0; i <= count - 1; i++)
            {
                if (firstname[i] + " "+secondname[i] + " "+thirdname[i] == "Андрей Подкопаев Игоревич")
                {
                    Result = 0;
                }
            }
            

            Assert.AreEqual(1, Result);



        }
        [TestMethod]
        public void funclistboxMethod3()
        {

            string[] online = new string[999];
            string[] firstname = new string[999];
            string[] secondname = new string[999];
            string[] thirdname = new string[999];
            string[] message = new string[999];
            int count;
            string id = "2";
            byte Result = 0;
            WindowsFormsApplication1.Form1 sut = new WindowsFormsApplication1.Form1();
            sut.funclistboxMethod(id, out count, online, firstname, secondname, thirdname, message);
            for (int i = 0; i <= count - 1; i++)
            {
                if (firstname[i] + " " + secondname[i] + " " + thirdname[i] == "Сергей Иванов Сергеевич")
                {
                    Result = 1;
                }
            }


            Assert.AreEqual(1, Result);



        }
        [TestMethod]
        public void AuthMethod1()
        {

            string email="122";
            string password="122";
            string log;
            string id, firstname, secondname, thirdname;
            byte Result = 0;
            WindowsFormsApplication1.Form1 sut = new WindowsFormsApplication1.Form1();
            sut.AuthMethod(email, password, out id, out firstname, out secondname, out thirdname, out log);

            if (log == "false")
            {
                Result = 1;
            }
            
            Assert.AreEqual(1, Result);



        }
        [TestMethod]
        public void AuthMethod2()
        {

            string email = "1";
            string password = "1";
            string log;
            string id, firstname, secondname, thirdname;
            byte Result = 0;
            WindowsFormsApplication1.Form1 sut = new WindowsFormsApplication1.Form1();
            sut.AuthMethod(email, password, out id, out firstname, out secondname, out thirdname, out log);

            if ((log == "true")&&(id=="2")&&(firstname=="Андрей")&&(secondname=="Подкопаев")&&(thirdname=="Игоревич"))
            {
                Result = 1;
            }

            Assert.AreEqual(1, Result);



        }

        [TestMethod]
        public void SelectedMethod1()
        {

            string firstname="Андрей";
            string secondname="Подкопаев";
            string thirdname="Игоревич",sobesednik="offline Сергей Иванов Сергеевич";
            string[] firstname1 = new string[999];
            string[] secondname1 = new string[999];
            string[] thirdname1 = new string[999];
            string[] message1 = new string[999];
            string[] date1 = new string[999];
            int count;
            string id = "2";
            byte Result = 0;
            WindowsFormsApplication1.Form1 sut = new WindowsFormsApplication1.Form1();
            sut.SelectedMethod(sobesednik, id, firstname, secondname, thirdname, out count, date1, firstname1, secondname1, thirdname1, message1);

            if (firstname1[0] + " " + secondname1[0] + " " + thirdname1[0] + " " + message1[0] + " " + date1[0] == "Андрей Подкопаев Игоревич Привет 2016-01-04 07:14:00")
                {
                    Result = 1;
                }
           


            Assert.AreEqual(1, Result);



        }

        [TestMethod]
        public void funclistboxAdmMethod1()
        {

            
            string[] firstname = new string[999];
            string[] secondname = new string[999];
            string[] thirdname = new string[999];
            
            int count;
            string id = "6";
            byte Result = 0;
            WindowsFormsApplication1.Form1 sut = new WindowsFormsApplication1.Form1();
            sut.funclistboxAdmMethod(id, out count, firstname, secondname, thirdname);

            if ((firstname != null) && (secondname != null) && (thirdname != null))
            {
                Result = 1;

            }

            Assert.AreEqual(1, Result);



        }
        [TestMethod]
        public void funclistboxAdmMethod2()
        {

            string[] firstname = new string[999];
            string[] secondname = new string[999];
            string[] thirdname = new string[999];

            int count;
            string id = "6";
            byte Result = 1;
            WindowsFormsApplication1.Form1 sut = new WindowsFormsApplication1.Form1();
            sut.funclistboxAdmMethod(id, out count, firstname, secondname, thirdname);
            for (int i = 0; i <= count - 1; i++)
            {
                if (firstname[i] + " " + secondname[i] + " " + thirdname[i] == "Наталья Туманик ")
                {
                    Result = 0;
                }
            }


            Assert.AreEqual(1, Result);



        }
        [TestMethod]
        public void funclistboxAdmMethod3()
        {


            string[] firstname = new string[999];
            string[] secondname = new string[999];
            string[] thirdname = new string[999];

            int count;
            string id = "6";
            byte Result = 0;
            WindowsFormsApplication1.Form1 sut = new WindowsFormsApplication1.Form1();
            sut.funclistboxAdmMethod(id, out count,firstname, secondname, thirdname);
            for (int i = 0; i <= count - 1; i++)
            {
                if (firstname[i] + " " + secondname[i] + " " + thirdname[i] == "Сергей Иванов Сергеевич")
                {
                    Result = 1;
                }
            }


            Assert.AreEqual(1, Result);



        }

        [TestMethod]
        public void SelectedAdmMethod1()
        {

            string sobesednik = "Сергей Иванов Сергеевич";
            string firstname1;
            string secondname1;
            string thirdname1;
            string pubemail;
            string password;
            string id = "6";
            byte Result = 0;
            WindowsFormsApplication1.Form1 sut = new WindowsFormsApplication1.Form1();
            sut.SelectedAdmMethod(sobesednik, id, out firstname1, out secondname1, out thirdname1, out pubemail, out password);

            if ((firstname1=="Сергей")&&(secondname1=="Иванов")&&( thirdname1== "Сергеевич")&& (pubemail=="7")&&( password == "7"))
            {
                Result = 1;
            }



            Assert.AreEqual(1, Result);



        }
    }
}
