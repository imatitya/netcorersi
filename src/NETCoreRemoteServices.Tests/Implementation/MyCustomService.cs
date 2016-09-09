using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using Contract;
using Newtonsoft.Json.Linq;
using NETCoreRemoteServices.Tests.Contracts;

namespace NETCoreRemoteServices.Tests.Implementation
{
    internal class MyCustomService : IMyCustomService
    {
        public const string ExpectedFileName = "cmd.exe";
        public void DoSomething()
        {
        }

        public string GetMachineName()
        {
            return Environment.MachineName;
        }

        public string GetEnvironmentVariables()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
            {
                sb.AppendLine($"Key = {entry.Key}, Value={entry.Value}");
            }
            return sb.ToString();
        }

        public long Sum(long x, long y)
        {
            return x + y;
        }

        public int Sum(int x, int y)
        {
            return x + y;
        }

        public User GetAuthor()
        {
            return new User
            {
                FirstName = "Idan",
                LastName = "Matityahu",
                Age = 27,
                Mail = "matityahuidan@gmail.com",
                BirthDate = new DateTime(1988, 12, 6)
            };
        }

        public bool NonPrimitive(ProcessStartInfo info)
        {
            return info.FileName == ExpectedFileName;
        }

        public string EmbeddedJObject()
        {
            JObject jObject = new JObject
            {
                ["fName"] = "Idan",
                ["lName"] = "Matityahu"
            };
            return jObject.ToString();
        }

        public void ThrowAnException()
        {
            throw new NotImplementedException("bla bla bla");
        }
    }
}