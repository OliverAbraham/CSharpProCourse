﻿/*	
 * 	
 *  This file is part of libfintx.
 *  
 *  Copyright (c) 2016 - 2020 Torsten Klinger
 * 	E-Mail: torsten.klinger@googlemail.com
 * 	
 * 	libfintx is free software; you can redistribute it and/or
 *	modify it under the terms of the GNU Lesser General Public
 * 	License as published by the Free Software Foundation; either
 * 	version 2.1 of the License, or (at your option) any later version.
 *	
 * 	libfintx is distributed in the hope that it will be useful,
 * 	but WITHOUT ANY WARRANTY; without even the implied warranty of
 * 	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * 	Lesser General Public License for more details.
 *	
 * 	You should have received a copy of the GNU Lesser General Public
 * 	License along with libfintx; if not, write to the Free Software
 * 	Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 * 	
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace libfintx
{
    public static class HKDME
    {
        /// <summary>
        /// Collective collect
        /// </summary>
        public static async Task<String> Init_HKDME(FinTsClient client, DateTime SettlementDate, List<Pain00800202CcData> PainData, string NumberofTransactions, decimal TotalAmount)
        {
            Log.Write("Starting job HKDME: Collective collect money");

            client.SEGNUM = SEGNUM.SETInt(3);

            var TotalAmount_ = TotalAmount.ToString().Replace(",", ".");

            var connectionDetails = client.ConnectionDetails;
            string segments = "HKDME:" + client.SEGNUM + ":2+" + connectionDetails.Iban + ":" + connectionDetails.Bic + "+" + TotalAmount_ + ":EUR++" + "+urn?:iso?:std?:iso?:20022?:tech?:xsd?:pain.008.002.02+@@";

            var message = pain00800202.Create(connectionDetails.AccountHolder, connectionDetails.Iban, connectionDetails.Bic, SettlementDate, PainData, NumberofTransactions, TotalAmount);

            segments = segments.Replace("@@", "@" + (message.Length - 1) + "@") + message;

            if (Helper.IsTANRequired("HKDME"))
            {
                client.SEGNUM = SEGNUM.SETInt(4);
                segments = HKTAN.Init_HKTAN(client, segments);
            }

            var TAN = await FinTSMessage.Send(client, FinTSMessage.Create(client, client.HNHBS, client.HNHBK, segments, client.HIRMS));

            client.HITAN = Helper.Parse_String(Helper.Parse_String(TAN, "HITAN", "'").Replace("?+", "??"), "++", "+").Replace("??", "?+");

            Helper.Parse_Message(client, TAN);

            return TAN;
        }
    }
}
