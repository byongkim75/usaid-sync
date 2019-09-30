using System;
using System.Collections.Generic;
using System.Text;

namespace api.test
{
    public class Claim
    {
        public Claim(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public string Type { get; set; }

        public string Value { get; set; }
    }

    public class User
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public IList<Claim> Claims { get; set; }

        public ProviderType ProviderType { get; set; }
    }

    public enum ProviderType
    {
        ADFS = 1,
        LoginGov = 2
    }
}
