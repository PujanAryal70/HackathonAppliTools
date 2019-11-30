using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Applitools.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace Traditional
{
    public static class SiteDriver
    {
        private static IWebDriver _driver;
        private static IJavaScriptExecutor _javascriptexecutor;

        public static IWebDriver Driver { get {
                return _driver;
            } }

        public static void Start(string url)
        {           
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            _driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),options);
            _driver.Navigate().GoToUrl(url);
            _javascriptexecutor = (IJavaScriptExecutor)_driver;
        }


        public static bool IsElementPresent(string select, How selector, int timeOut = 0)
        {
            try
            {
                FindElement(select, selector, _driver, timeOut);
                return true;
            }
            catch (Exception ex)
            {
                // Don't handle NotSupportedException
                if (ex is NotSupportedException)
                    throw;
                return false;
            }
        }

        internal static IWebElement FindElement(string select, How selector)
        {
            return FindElement(select, selector, _driver);
        }

        public static IEnumerable<IWebElement> FindElements(string select, How selector)
        {
            return FindElements(select, selector, _driver);
        }

        internal static IWebElement FindElement(string select, How selector, ISearchContext context, int elementTimeOut = 2000)
        {
            switch (selector)
            {
                case How.ClassName:
                    return WaitandReturnElementExists(By.ClassName(select), context, elementTimeOut);
                case How.CssSelector:
                    return WaitandReturnElementExists(By.CssSelector(select), context, elementTimeOut);
                case How.Id:
                    return WaitandReturnElementExists(By.Id(select), context, elementTimeOut);
                case How.LinkText:
                    return WaitandReturnElementExists(By.LinkText(select), context, elementTimeOut);
                case How.Name:
                    return WaitandReturnElementExists(By.Name(select), context, elementTimeOut);
                case How.PartialLinkText:
                    return WaitandReturnElementExists(By.PartialLinkText(select), context, elementTimeOut);
                case How.TagName:
                    return WaitandReturnElementExists(By.TagName(select), context, elementTimeOut);
                case How.XPath:
                    return WaitandReturnElementExists(By.XPath(select), context, elementTimeOut);
            }
            throw new NotSupportedException(string.Format("Selector \"{0}\" is not supported.", selector));
        }

        public static IWebElement WaitandReturnElementExists(By locator, ISearchContext context, int elementTimeOut = 2000)
        {
            if (elementTimeOut == 0)
                return context.FindElement(locator);

            var wait = new WebDriverWait(new SystemClock(), _driver, TimeSpan.FromMilliseconds(2000), TimeSpan.FromMilliseconds(2000));
            IWebElement webElement = null;
            wait.Until(driver =>
            {
                try
                {
                    webElement = context.FindElement(locator);
                    return webElement != null;

                }
                catch (Exception ex)
                {
                    //Console.Out.WriteLine("unhandled exception" + ex.Message);
                    return false;
                }
            });
            return webElement;
        }

        public static void Close()
        {
            _driver.Quit();
        }

        public static void WaitForCondition(Func<bool> f, int milliSec = 0)
        {

            milliSec = (int)((milliSec == 0) ? 100 * 1000 : milliSec);
            var wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(milliSec));
            try
            {
                wait.Until(d =>
                {
                    try
                    {
                        return f();
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });
            }
            catch (UnhandledAlertException ex)
            {
                Console.Out.WriteLine("unhandled exception" + ex.Message);
            }

            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
        }

        internal static IEnumerable<IWebElement> FindElements(string select, How selector, ISearchContext context, int elementTimeOut = 2000)
        {
            switch (selector)
            {
                case How.ClassName:
                    return WaitandReturnElementsExists(By.ClassName(select), context, elementTimeOut);
                case How.CssSelector:
                    return WaitandReturnElementsExists(By.CssSelector(select), context, elementTimeOut);
                case How.Id:
                    return WaitandReturnElementsExists(By.Id(select), context, elementTimeOut);
                case How.LinkText:
                    return WaitandReturnElementsExists(By.LinkText(select), context, elementTimeOut);
                case How.Name:
                    return WaitandReturnElementsExists(By.Name(select), context, elementTimeOut);
                case How.PartialLinkText:
                    return WaitandReturnElementsExists(By.PartialLinkText(select), context, elementTimeOut);
                case How.TagName:
                    return WaitandReturnElementsExists(By.TagName(select), context, elementTimeOut);
                case How.XPath:
                    return WaitandReturnElementsExists(By.XPath(select), context, elementTimeOut);
            }
            throw new NotSupportedException(string.Format("Selector \"{0}\" is not supported.", selector));
        }

        public static IEnumerable<IWebElement> WaitandReturnElementsExists(By locator, ISearchContext context, int elementTimeOut = 2000)
        {
            if (elementTimeOut == 0)
                return context.FindElements(locator);

            var wait = new WebDriverWait(new SystemClock(), _driver, TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(1000));
            IEnumerable<IWebElement> webElement = null;
            wait.Until(driver =>
            {
                try
                {
                    webElement = context.FindElements(locator);
                    return webElement != null;

                }
                catch (Exception ex)
                {
                    //Console.Out.WriteLine("unhandled exception" + ex.Message);
                    return false;
                }
            });
            return webElement;
        }

        public static List<string> FindDisplayedElementsText(string select, How selector)
        {
            return FindElements(select, selector, _driver).Where(e => e.Displayed).Select(e => e.Text).ToList();
        }

        public static bool IsInAscendingOrder<T>(this List<T> values)
        {
            for (int i = 0; i < values.Count - 1; i++)
            {
                if (Comparer<T>.Default.Compare(values[i], values[i + 1]) > 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check whether values are in descending order or not
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool IsInDescendingOrder<T>(this List<T> values)
        {
            for (int i = 0; i < values.Count - 1; i++)
            {
                if (Comparer<T>.Default.Compare(values[i], values[i + 1]) < 0)
                    return false;
            }
            return true;
        }

        public static void ScrollToBottom()
        {
            _javascriptexecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
        }



    }
}
