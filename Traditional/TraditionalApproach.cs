using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Traditional.PageObjects;


namespace Traditional
{
    public class TraditionalApproach
    {
        private HackathonPage hackathonPage;

        [SetUp]
        public void Initialize()
        {
            Console.WriteLine("Test Initialized");
        }

        [Test]
        public void Verify_Login_Page_Contents()
        {

            hackathonPage = new HackathonPage(PageUrl.Version2);

            //Login Button
            Assert.IsTrue(hackathonPage.IsLoginButtonPresent(),"Login Button Present ?");

            //User Name
            Assert.IsTrue(hackathonPage.IsUserNameLabelPresent(),"UserName Label Present ?");
            Assert.IsTrue(hackathonPage.IsUserNameInputFieldPresent(),"User Name Input Field Present ?");

            //Bug:Placeholder Showing User Name In V2
            Assert.AreEqual(hackathonPage.GetUserNameFieldPlaceHolder(),"Enter your username","Placeholder equal ?");

            //Bug: User Name icon is missing in V@
            Assert.IsTrue(hackathonPage.IsUserNameIconPresent(),"User Name Icon Present ?");

            //Password
            //Bug:Password Label Is Changed To "Pwd" in V2
            Assert.IsTrue(hackathonPage.IsPasswordLabelPresent(), "Password Label Present ?");
            Assert.IsTrue(hackathonPage.IsPasswordInputFieldPresent(), "Password Input Field Present ?");

            //Bug:Placeholder Showing Password of a user
            Assert.AreEqual(hackathonPage.GetPasswordFieldPlaceHolder(), "Enter your password", "Placeholder equal ?");

            //Bug: Password icon is missing in V2
            Assert.IsTrue(hackathonPage.IsPasswordIconPresent(), "Password Icon Present ?");

            //Remember Me
            Assert.IsTrue(hackathonPage.IsRememberMeCheckboxPresent(), "Remember Me Checkbox Present ?");
            Assert.IsTrue(hackathonPage.IsRememberMeTextBoxPresent(), "Remember Me Textbox Present ?");

            //Social Media Icons
            Assert.IsTrue(hackathonPage.IsTwitterIconPresent(), "Twitter Icon Present ?");
            Assert.IsTrue(hackathonPage.IsFacebookIconPresent(), "Facebook Icon Present ?");

            //Bug:LinkedIn Icon Is missing in V2
            Assert.IsTrue(hackathonPage.IsLinkedInIconPresent(), "Linked In Icon Present ?");


            //Top Part
            Assert.IsTrue(hackathonPage.IsTopLogoPresent(), "Top Logo Present ?");

            //Bug: "Login Form" is changed to "Logout Form"
            Assert.IsTrue(hackathonPage.IsLoginFormTextPresent(), "Login Form Text Present ?");

        }

        [Test]
        public void Verify_Login_Functionality()
        {
            hackathonPage = new HackathonPage(PageUrl.Version2);
            //Verify Error Thrown When Username and password are no entered
            hackathonPage.ClickOnLoginButton();
            Assert.IsTrue(hackathonPage.IsErrorMessagePresent(),"Error Message Should Be Present");
            
            /*Validation Message Has Been Changed In V2*/
            Assert.AreEqual(hackathonPage.GetErrorMessage(), "Please enter both username and password", "Message Equal ?");

            //Verify If Only UserName IS Entered, Error Is Thrown
            hackathonPage.EnterUserName("test");
            hackathonPage.ClickOnLoginButton();
            Assert.IsTrue(hackathonPage.IsErrorMessagePresent(), "Error Message Should Be Present");

            /*Bug: Validation Message Is Not Shown In The UI. However, the Assertion Is Correct.*/
            Assert.AreEqual(hackathonPage.GetErrorMessage(), "Password must be present", "Message Equal ?");
            hackathonPage.ClearTextField("Username");

            //Verify If Only Password IS Entered, Error Is Thrown
            hackathonPage.EnterPassword("test");
            hackathonPage.ClickOnLoginButton();
            Assert.IsTrue(hackathonPage.IsErrorMessagePresent(), "Error Message Should Be Present");

            /*Validation Message Is Not Aligned.*/
            Assert.AreEqual(hackathonPage.GetErrorMessage(), "Username must be present", "Message Equal ?");
            hackathonPage.ClearTextField("Password");


            //Verify User Is Logged In with both credentials
            hackathonPage.EnterUserName("test");
            hackathonPage.EnterPassword("test");
            hackathonPage.ClickOnLoginButton();
            Assert.IsFalse(hackathonPage.IsErrorMessagePresent(), "Error Message Should Be Present");
            Assert.IsTrue(hackathonPage.IsUserLoggedIn(),"Is User Logged In ?");

        }

        [Test]
        public void Verify_Table_Sort_Test()
        {
            hackathonPage = new HackathonPage(PageUrl.Version2);
            Dictionary<int, List<string>> tableData= new Dictionary<int,List<string>>()
            {
                {
                    1,
                    new List<string>()
                    {
                        "",
                        "Pending", "Yesterday", "7:45am",
                        "MailChimp Services", "- 320.00 USD"

                    }
                },
                {
                    2,
                    new List<string>()
                    {
                        "", "Complete", "Jan 7th", "9:51am",
                        "Ebay Marketplace", "- 244.00 USD"
                    }
                    
                },
                {
                    3,
                    new List<string>()
                    {
                        "", "Pending", "Jan 23rd", "2:7pm",
                        "Shopify product", "+ 17.99 USD"
                    }
                },
                {
                    4,
                    new List<string>()
                    {
                        "", "Pending", "Jan 9th", "7:45pm",
                        "Templates Inc", "+ 340.00 USD"
                    }
                   
                },
                {
                    5,
                    new List<string>()
                    {
                        "", "Declined", "Jan 19th", "3:22pm",
                        "Stripe Payment Processing", "+ 952.23 USD"
                    }
                },
                {
                    6,
                    new List<string>()
                    {
                        "", "Complete", "Today", "1:52am",
                        "Starbucks coffee", "+ 1,250.00 USD"
                    }
                }
            };
           
            
            hackathonPage.EnterUserName("test");
            hackathonPage.EnterPassword("test");
            hackathonPage.ClickOnLoginButton();
            SiteDriver.WaitForCondition(hackathonPage.IsLoggedInUserNamePresent);

            //Verify Sorting
            hackathonPage.ClickOnAmountColumn();
            var amounts = hackathonPage.GetAllAmounts();

            /*Bug : Data is not sorted in ascending order.*/
            Assert.IsTrue(amounts.IsInAscendingOrder(),"Are amounts sorted in ascending order ?");

            //Verify Rows Are Intact After Sort

            for (int i = 1; i <= tableData.Count; i++)
            {
                CollectionAssert.AreEqual(tableData[i], hackathonPage.GetGridValueByRow(i),
                    "Data Intact After Sort ?");
            }

        }

        [Test]
        public void Verify_Canvas_Chart()
        {
            hackathonPage = new HackathonPage(PageUrl.Version2);
            hackathonPage.EnterUserName("test");
            hackathonPage.EnterPassword("test");
            hackathonPage.ClickOnLoginButton();
            hackathonPage.ClickOnCompareExpensesLink();
            Assert.IsTrue(hackathonPage.IsCanvasChartPresent(),"Chart Present ?");
            /*
             * Chart cannot be validated using selenium traditional approach as we cannot access the data from the DOM element representing the chart.
             */
        }

        [Test]
        public void Verify_Ads_In_The_Page()
        {
            hackathonPage = new HackathonPage(PageUrl.Version2withAd);
            hackathonPage.EnterUserName("test");
            hackathonPage.EnterPassword("test");
            hackathonPage.ClickOnLoginButton();

            /*Bug: Test is passing even though image is missing in version 2 of the app.*/
            Assert.IsTrue(hackathonPage.IsFlashSaleAdsPresent(),"Flash Ads Present ?");

        }




        [TearDown]
        public void TestCleanUp()
        {
            SiteDriver.Close();
        }
    }
}
