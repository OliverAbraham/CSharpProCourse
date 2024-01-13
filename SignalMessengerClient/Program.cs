using Google.Protobuf.WellKnownTypes;
using libsignal;
using libsignal.state;
using libsignal.state.impl;
using libsignal.util;
using libsignalservice;
using libsignalservice.configuration;
using libsignalservice.messages;
using libsignalservice.messages.multidevice;
using libsignalservice.push;
using libsignalservice.util;
using libsignalservicedotnet.crypto;
using System;

namespace SignalMessengerClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Sending a message to a user using the signal messenger !");

            uint signedPreKeyId = 1;
            IdentityKeyPair     identityKey        = KeyHelper.generateIdentityKeyPair();
            IList<PreKeyRecord> oneTimePreKeys     = KeyHelper.generatePreKeys(0, 100);
            PreKeyRecord        lastResortKey      = KeyHelper.generatePreKeys(0, 1).First(); //KeyHelper.generateLastResortPreKey(); ???
            SignedPreKeyRecord  signedPreKeyRecord = KeyHelper.generateSignedPreKey(identityKey, signedPreKeyId);



            //var        URL         = "https://my.signal.server.com";
            var         uuid        = new Guid();
            var         username    = "+491716839652";
            var         password    = "testpassword";
            var         deviceID    = 1;
            var         provider    = new StaticCredentialsProvider(uuid, username, password, deviceID);
            var         userAgent   = "Windows";
            
            var urls = new SignalServiceConfiguration(
                new SignalServiceUrl[]         {new SignalServiceUrl         ("https://textsecure-service.whispersystems.org")},
                new SignalCdnUrl[]             {new SignalCdnUrl             ("https://textsecure-service-staging.whispersystems.org")},
                new SignalCdnUrl[]             {new SignalCdnUrl             ("https://textsecure-service-dev.whispersystems.org")},
                new SignalContactDiscoveryUrl[]{new SignalContactDiscoveryUrl("https://textsecure-service-dev.whispersystems.org")});       // possibly wrong value!

            var httpClient = new HttpClient();
            var accountManager = new SignalServiceAccountManager(urls, uuid, username, password, deviceID, userAgent, httpClient);

            //var captchaToken = "12345";
            //await accountManager.RequestSmsVerificationCodeAsync(captchaToken);

            var receivedSmsVerificationCode = "12345";
            var signalingKey = KeyHelper.getRandomSequence(999999); //GenerateSignalingKey();
            var signalProtocolRegistrationId = KeyHelper.getRandomSequence(999999); // generateRandomInstallId()

            var pin = "1234";
            byte[] unidentifiedAccessKey = new byte[4] { 1, 2, 3, 4 };
            //await accountManager.VerifyAccountWithCodeAsync(receivedSmsVerificationCode, signalingKey.ToString(), signalProtocolRegistrationId, fetchesMessages:false, pin, unidentifiedAccessKey, unrestrictedUnidentifiedAccess:true);

            //var gcmRegistrationId = "12345";
            //await accountManager.SetGcmIdAsync(gcmRegistrationId);
            //await accountManager.SetPreKeysAsync(identityKey.getPublicKey(), signedPreKeyRecord, oneTimePreKeys);

            var protocolStore = new InMemorySignalProtocolStore(identityKey, signalProtocolRegistrationId);


            // Sending text messages

            var messageSender = new SignalServiceMessageSender(urls, provider, protocolStore, userAgent, httpClient, 
                                isMultiDevice: false, attachmentsV3:false, pipe:null, unidentifiedPipe:null, eventListener:null);

            var receipient = new SignalServiceAddress(uuid, "+491716839652", relay:null);
            var timestamp = DateTime.Now.Ticks;
            var message = new SignalServiceDataMessage(timestamp, "Hello, world!");
            var transcriptMessage = new SentTranscriptMessage(timestamp, message);
            var syncMessage = SignalServiceSyncMessage.ForSentTranscript(transcriptMessage);
           
            await messageSender.SendMessageAsync(syncMessage, unidenfifiedAccess:null);


            //var unidenfifiedAccess = new UnidentifiedAccessPair(
            //                            new UnidentifiedAccess(new byte[]{1, 2, 3, 4 }, new byte[]{1, 2, 3, 4 }), 
            //                            new UnidentifiedAccess(new byte[]{1, 2, 3, 4 }, new byte[]{1, 2, 3, 4 }));
            //await messageSender.SendMessageAsync(syncMessage, unidenfifiedAccess);

        }
    }
}
