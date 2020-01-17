using DMFX.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DMFX.Test.DMFXClient
{
    [TestFixture]
    public class TestDMFXClient
    {
        string _host = null;
        string _accountKey = null;

        [SetUp]        
        public void SetUp()
        {
            _host = ConfigurationManager.AppSettings["Host"];
            _accountKey = ConfigurationManager.AppSettings["AccountKey"];
        }

        [Test]
        public void InitSession_Success()
        {
            var client = CreateClient();
            EErrorCodes result = client.InitSession(_host, _accountKey);

            Assert.IsTrue(result == EErrorCodes.Success);

            result = client.CloseSession();
        }

        [Test]
        public void CloseSession_Success()
        {
            var client = CreateClient();
            EErrorCodes result = client.InitSession(_host, _accountKey);
            result = client.CloseSession();

            Assert.IsTrue(result == EErrorCodes.Success);
        }

        [TearDown]
        public void TearDown()
        {
        }

        #region Support methods
        private DMFX.DarkMindConnect.DMFXClient CreateClient()
        {
            DMFX.DarkMindConnect.DMFXClient client = new DMFX.DarkMindConnect.DMFXClient();

            return client;
        }
        #endregion
    }
}
