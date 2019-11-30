using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Traditional.PageObjects
{
    class HackathonPage
    {
        public IWebDriver Driver { get { return SiteDriver.Driver; } }

        #region Constructor

        public HackathonPage(string url)
        {
           SiteDriver.Start(url);
        }

        #endregion

        #region PageObjects

        //UserName
        public const string UserNameLabelXpath = "//label[contains(text(), 'Username')]";
        public const string UserNameInputFieldCssSelector = "input#username";
        public const string LoginButtonCssSelector = "button#log-in";
        public const string UserNameIconCssSelector = "input#username + div.os-icon-user-male-circle";

        //Password
        public const string PasswordLabelXpath = "//label[contains(text(), 'Pwd')]";
        public const string PasswordInputFieldCssSelector = "input#password";
        public const string PasswordIconCssSelector = "input#password + div.os-icon-fingerprint";

        //Remember Me
        public const string RememberMeCheckboxXpath = "//label[contains(text(), 'Remember Me')]/input[@class='form-check-input']";
        public const string RememberMeTextXpath = "//label[contains(text(), 'Remember Me')]";

        //Social Media Icons
        public const string TwitterIconXpath = "//img[@src='img/social-icons/twitter.png']";
        public const string FacebookIconXpath = "//img[@src='img/social-icons/facebook.png']";
        public const string LinkedInIconXpath = "//img[@src='img/social-icons/linkedin.png']";

        //Top Part
        public const string AppliToolsLogoCssSelector = "div.logo-w img";
        public const string LoginFormTextXpath = "//h4[contains(text(),'Login Form')]";

        public const string ValidationErrorCssSelector = "div.alert-warning";

        //Dashboard
        public const string LoggedInUserNameCssSelector = "div.logged-user-name";
        public const string AmountColumnCssSelector = "th#amount";
        public const string AmountsCssSelector = "td.bolder>span";
        public const string GridValueByRow = "//tr[{0}]/td/span";
        public const string CompareExpensesLinkCssSelector = "a#showExpensesChart";
        public const string CanvasChartCssSelector = "canvas#canvas";
        public const string FlashAdOneCssSelector = "div#flashSale";
        public const string FlashAdTwoCssSelector = "div#flashSale2";
        public const string ShowNextYearDataCssSelector = "button#addDataset";



        #endregion

        #region public methods

        public bool IsLoginButtonPresent()
        {
            return SiteDriver.IsElementPresent(LoginButtonCssSelector,How.CssSelector);
        }

        #region UserName Field

        public bool IsUserNameLabelPresent()
        {
            return SiteDriver.IsElementPresent(UserNameLabelXpath, How.XPath);
        }

        public bool IsUserNameInputFieldPresent()
        {
            return SiteDriver.IsElementPresent(UserNameInputFieldCssSelector, How.CssSelector);
        }

        public string GetUserNameFieldPlaceHolder()
        {
            return SiteDriver.FindElement(UserNameInputFieldCssSelector, How.CssSelector).GetAttribute("placeholder");
        }

        public bool IsUserNameIconPresent()
        {
            return SiteDriver.IsElementPresent(UserNameIconCssSelector, How.CssSelector);
        }

        #endregion

        #region Password Field
        public bool IsPasswordLabelPresent()
        {
            return SiteDriver.IsElementPresent(PasswordLabelXpath, How.XPath);
        }

        public bool IsPasswordInputFieldPresent()
        {
            return SiteDriver.IsElementPresent(PasswordInputFieldCssSelector, How.CssSelector);
        }

        public string GetPasswordFieldPlaceHolder()
        {
            return SiteDriver.FindElement(PasswordInputFieldCssSelector, How.CssSelector).GetAttribute("placeholder");
        }

        public bool IsPasswordIconPresent()
        {
            return SiteDriver.IsElementPresent(PasswordIconCssSelector, How.CssSelector);
        }


        #endregion

        #region Remember Me Field
        public bool IsRememberMeCheckboxPresent()
        {
            return SiteDriver.IsElementPresent(RememberMeCheckboxXpath, How.XPath);
        }

        public bool IsRememberMeTextBoxPresent()
        {
            return SiteDriver.IsElementPresent(RememberMeTextXpath, How.XPath);
        }


        #endregion

        #region Social Media Icons
        public bool IsTwitterIconPresent()
        {
            return SiteDriver.IsElementPresent(TwitterIconXpath, How.XPath);
        }

        public bool IsFacebookIconPresent()
        {
            return SiteDriver.IsElementPresent(FacebookIconXpath, How.XPath);
        }

        public bool IsLinkedInIconPresent()
        {
            return SiteDriver.IsElementPresent(LinkedInIconXpath, How.XPath);
        }


        #endregion

        #region Top Part
        public bool IsTopLogoPresent()
        {
            return SiteDriver.IsElementPresent(AppliToolsLogoCssSelector, How.CssSelector);
        }

        public bool IsLoginFormTextPresent()
        {
            return SiteDriver.IsElementPresent(LoginFormTextXpath, How.XPath);

        }



        #endregion

        public void EnterUserName(string username)
        {
            SiteDriver.FindElement(UserNameInputFieldCssSelector, How.CssSelector).SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            SiteDriver.FindElement(PasswordInputFieldCssSelector, How.CssSelector).SendKeys(password);
        }

        public void ClickOnLoginButton()
        {
            SiteDriver.FindElement(LoginButtonCssSelector, How.CssSelector).Click();
        }

        public bool IsErrorMessagePresent()
        {
            return SiteDriver.IsElementPresent(ValidationErrorCssSelector, How.CssSelector);
        }

        public string GetErrorMessage()
        {
            return SiteDriver.FindElement(ValidationErrorCssSelector, How.CssSelector).Text;
        }

        public bool IsUserLoggedIn()
        {
            SiteDriver.WaitForCondition(IsLoggedInUserNamePresent);
            return IsLoggedInUserNamePresent();
        }

        public bool IsLoggedInUserNamePresent()
        {
            return SiteDriver.IsElementPresent(LoggedInUserNameCssSelector, How.CssSelector);
        }

        public void ClearTextField(string fieldName)
        {
            switch (fieldName)
            {
                case "Password":
                    SiteDriver.FindElement(PasswordInputFieldCssSelector,How.CssSelector).Clear();
                    break;
                case "Username":
                    SiteDriver.FindElement(UserNameInputFieldCssSelector, How.CssSelector).Clear();
                    break;
                default:
                    Console.WriteLine("Invalid");
                    break;

            }


        }

        public void ClickOnAmountColumn()
        {
            SiteDriver.FindElement(AmountColumnCssSelector,How.CssSelector).Click();
        }

        public List<double> GetAllAmounts()
        {
            List<double> amounts = new List<double>();
            var amts = SiteDriver.FindDisplayedElementsText(AmountsCssSelector, How.CssSelector).ToList();
            foreach(var amt in amts)
            {
                var number = amt.Split(' ');
                Regex.Replace(number[1], ",", "");
                if (number[0].Equals("-"))
                {
                    amounts.Add(Convert.ToDouble(String.Concat(number[0],number[1])));
                }
                else
                {
                    amounts.Add(Convert.ToDouble(number[1]));
                }
                
            }

            return amounts;

        }

        public List<string> GetGridValueByRow(int row = 1)
        {
            return SiteDriver.FindDisplayedElementsText(String.Format(GridValueByRow, row), How.XPath);
        }




        #endregion

        public void ClickOnCompareExpensesLink()
        {
            SiteDriver.FindElement(CompareExpensesLinkCssSelector, How.CssSelector).Click();
        }

        public bool IsCanvasChartPresent()
        {
            return SiteDriver.IsElementPresent(CanvasChartCssSelector, How.CssSelector);
        }

        public bool IsFlashSaleAdsPresent()
        {
            return SiteDriver.IsElementPresent(FlashAdOneCssSelector, How.CssSelector) && SiteDriver.IsElementPresent(FlashAdTwoCssSelector, How.CssSelector);
        }

        public void ScrollToBottomOfAPage()
        {
            SiteDriver.ScrollToBottom();
        }

        public void ClickOnAddNextYearDataButton()
        {
            SiteDriver.FindElement(ShowNextYearDataCssSelector, How.CssSelector).Click();
        }
    }
    }

