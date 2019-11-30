using Applitools.Selenium;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Traditional.PageObjects;
using System.Drawing;
using Applitools;

namespace Traditional
{
    public class VisualAITests
    {
        private Eyes _eyes;
        private HackathonPage hackathonPage;
        [SetUp]
        public void Initialize()
        {
            Console.WriteLine("Test Initialized");
            _eyes = new Eyes
            {
                ApiKey = Environment.GetEnvironmentVariable("APPLITOOLS_API_KEY",
                    EnvironmentVariableTarget.User)
            };
            BatchInfo b = new BatchInfo("Hackathon");
            b.Id = "PARYAL";
            _eyes.Batch = b;

        }

        [Test]
        public void Verify_Login_Page_Contents()
        {
            hackathonPage = new HackathonPage(PageUrl.Version2);
            _eyes.Open(hackathonPage.Driver,"Hackathon","LoginPage");
            _eyes.CheckWindow("Login Page");          
        }

        [Test]
        public void Verify_Login_Functionality()
        {
            hackathonPage = new HackathonPage(PageUrl.Version2);
            hackathonPage.ClickOnLoginButton();
            _eyes.Open(hackathonPage.Driver, "Hackathon", "Validation");
            _eyes.CheckWindow("Validation without username and password");

            hackathonPage.EnterUserName("test");
            hackathonPage.ClickOnLoginButton();
            _eyes.CheckWindow("Validation without username only");
            hackathonPage.ClearTextField("Username");

            //Verify If Only Password IS Entered, Error Is Thrown
            hackathonPage.EnterPassword("test");
            hackathonPage.ClickOnLoginButton();
            _eyes.CheckWindow("Validation without password only");
            hackathonPage.ClearTextField("Password");

            //Verify User Is Logged In with both credentials
            hackathonPage.EnterUserName("test");
            hackathonPage.EnterPassword("test");
            hackathonPage.ClickOnLoginButton();
            _eyes.CheckWindow("Validation with both username and password");
        }

        [Test]
        public void Verify_Table_Sort_Test()
        {
            hackathonPage = new HackathonPage(PageUrl.Version2);
            hackathonPage.EnterUserName("test");
            hackathonPage.EnterPassword("test");
            hackathonPage.ClickOnLoginButton();
            SiteDriver.WaitForCondition(hackathonPage.IsLoggedInUserNamePresent);
            hackathonPage.ScrollToBottomOfAPage();
            hackathonPage.ClickOnAmountColumn();
            _eyes.Open(hackathonPage.Driver, "Hackathon", "Sorting Test");
            _eyes.CheckWindow("Verify Sorting");
        }

        [Test]
        public void Verify_Canvas_Chart()
        {
            hackathonPage = new HackathonPage(PageUrl.Version2);
            hackathonPage.EnterUserName("test");
            hackathonPage.EnterPassword("test");
            hackathonPage.ClickOnLoginButton();
            hackathonPage.ClickOnCompareExpensesLink();
            _eyes.Open(hackathonPage.Driver, "Hackathon", "Canvas Chart Test");
            _eyes.CheckWindow("Verify Canvas Chart");
            hackathonPage.ClickOnAddNextYearDataButton();
            _eyes.CheckWindow("Verify Canvas Chart After Adding Year 2019");
        }

        [Test]
        public void Verify_Ads_In_The_Page()
        {
            hackathonPage = new HackathonPage(PageUrl.Version2withAd);
            hackathonPage.EnterUserName("test");
            hackathonPage.EnterPassword("test");
            hackathonPage.ClickOnLoginButton();
            _eyes.Open(hackathonPage.Driver, "Hackathon", "Ads Test");
            _eyes.CheckWindow("Verify Ads In The Page");
        }

        [TearDown]
        public void TestCleanUp()
        {
            _eyes.CloseAsync();
            SiteDriver.Close();
        }
    }
}
