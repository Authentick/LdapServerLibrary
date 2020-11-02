using System;
using System.Formats.Asn1;
using LdapServer.Models;

namespace LdapServer
{
    public class Class1
    {
        public void Foo(byte[] input)
        {
            MessageDecoder decoder = new MessageDecoder();
            LdapMessage message = decoder.TryDecode(input);

        }
    }
}
