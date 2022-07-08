// Install the C# / .NET helper library from twilio.com/docs/csharp/install

using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace WhatsAppGateway
{
	class Program
	{
		static void Main(string[] args)
		{
			// Find your Account Sid and Token at twilio.com/console
			// DANGER! This is insecure. See http://twil.io/secure
			const string accountSid = "AC3d7e3e3900b32e7ac645fd55709fb6e2";//"AC2cfb3132942c748aea47016fd7fb5e8c";
			const string authToken = "f6e5e0350cd4cbe39e8a6f29a5ff411d";//"b342895fad9fee487533be3900bab5af";

			TwilioClient.Init(accountSid, authToken);

			var message = MessageResource.Create(
				body: "Hello there!",
				from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
				to: new Twilio.Types.PhoneNumber("whatsapp:+4915110370024")
			);

			Console.WriteLine(message.Sid);
		}
	}
}

