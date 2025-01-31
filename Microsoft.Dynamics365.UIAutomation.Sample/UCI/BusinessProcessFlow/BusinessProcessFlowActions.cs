// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class BusinessProcessFlowNextStageUCI
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly SecureString _mfaSecretKey = System.Configuration.ConfigurationManager.AppSettings["MfaSecretKey"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        /// <summary>
        ///  Business Process Flow - Open case record and call UCITestBusinessProcessFlowAction
        /// </summary>
        [TestMethod]
        public void UCITestBusinessProcessFlowSelectStage()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
               xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.ThinkTime(3000);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.SwitchView("My Active Cases");

                xrmApp.Grid.OpenRecord(0);

                UCITestBusinessProcessFlowAction("Identify", "Next Stage", xrmApp, client);

                UCITestBusinessProcessFlowAction("Research", "Close", xrmApp, client);
            }
        }

        //Open the selected Stage and choose the button based on their title
        [TestMethod]
        private void UCITestBusinessProcessFlowAction(string selectStage, string BpfButtonName, XrmApp xrmApp, WebClient client)
        {
            var xrmBrowser = client.Browser;
            var win = xrmBrowser.Driver.SwitchTo().Window(xrmBrowser.Driver.CurrentWindowHandle);
            //Select the Business Process Flow stage
            xrmApp.BusinessProcessFlow.SelectStage(selectStage);
            //Click the button
            win.FindElement(By.XPath("//button[@title='" + BpfButtonName + "']")).Click();
        }
    }
}
