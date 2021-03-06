/*
 * NetEbics -- .NET Core EBICS Client Library
 * (c) Copyright 2018 Bjoern Kuensting
 *
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.txt', which is part of this source code package.
 */

using System.Xml.Linq;
using libfintx.Config;

namespace libfintx.Xml
{
    internal class UserSignatureData : NamespaceAware, IXElementSerializer
    {
        internal IXElementSerializer OrderSignatureData { private get; set; }

        public XElement Serialize()
        {
            XNamespace ns = Namespaces.SignatureData;
            return new XElement(ns + XmlNames.UserSignatureData,
                OrderSignatureData.Serialize()
            );
        }
    }
}