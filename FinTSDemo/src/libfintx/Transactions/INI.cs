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
using System.Threading.Tasks;

namespace libfintx
{
    public static class INI
    {
        /// <summary>
        /// INI
        /// </summary>
        public static async Task<String> Init_INI(FinTsClient client)
        {
            var connectionDetails = client.ConnectionDetails;
            if (!client.Anonymous)
            {
                /// <summary>
                /// Sync
                /// </summary>
                try
                {
                    string segments;

                    /// <summary>
                    /// INI
                    /// </summary>
                    if (connectionDetails.HbciVersion == 220)
                    {
                        string segments_ =
                            "HKIDN:" + SEGNUM.SETVal(3) + ":2+280:" + connectionDetails.BlzPrimary + "+" + connectionDetails.UserId + "+" + client.SystemId + "+1'" +
                            "HKVVB:" + SEGNUM.SETVal(4) + ":2+0+0+0+" + FinTsConfig.ProductId + "+" + FinTsConfig.Version + "'";

                        segments = segments_;
                    }
                    else if (connectionDetails.HbciVersion == 300)
                    {
                        string segments_ =
                            "HKIDN:" + SEGNUM.SETVal(3) + ":2+280:" + connectionDetails.BlzPrimary + "+" + connectionDetails.UserId + "+" + client.SystemId + "+1'" +
                            "HKVVB:" + SEGNUM.SETVal(4) + ":3+0+0+0+" + FinTsConfig.ProductId + "+" + FinTsConfig.Version + "'";

                        if (client.HITANS != null && client.HITANS.Substring(0, 3).Equals("6+4"))
                        {
                            client.SEGNUM = SEGNUM.SETInt(5);
                            segments_ = HKTAN.Init_HKTAN(client, segments_);
                        }
                        else
                        {
                            client.SEGNUM = SEGNUM.SETInt(4);
                        }

                        segments = segments_;
                    }
                    else
                    {
                        //Since connectionDetails is a re-usable object, this shouldn't be cleared.
                        //connectionDetails.UserId = string.Empty;
                        //connectionDetails.Pin = null;

                        Log.Write("HBCI version not supported");

                        throw new Exception("HBCI version not supported");
                    }

                    var message = FinTSMessage.Create(client, "1", "0", segments, client.HIRMS);
                    var response = await FinTSMessage.Send(client, message);

                    Helper.Parse_Segment(client, response);

                    client.HITAN = Helper.Parse_String(Helper.Parse_String(response, "HITAN:", "'").Replace("?+", "??"), "++", "+").Replace("??", "?+");

                    return response;
                }
                catch (Exception ex)
                {
                    //Since connectionDetails is a re-usable object, this shouldn't be cleared.
                    //connectionDetails.UserId = string.Empty;
                    //connectionDetails.Pin = null;

                    Log.Write(ex.ToString());

                    throw new Exception("Software error", ex);
                }
            }
            else
            {
                /// <summary>
                /// Sync
                /// </summary>
                try
                {
                    Log.Write("Starting Synchronisation anonymous");

                    string segments;

                    if (connectionDetails.HbciVersion == 300)
                    {
                        string segments_ =
                            "HKIDN:" + SEGNUM.SETVal(2) + ":2+280:" + connectionDetails.BlzPrimary + "+" + "9999999999" + "+0+0'" +
                            "HKVVB:" + SEGNUM.SETVal(3) + ":3+0+0+1+" + FinTsConfig.ProductId + "+" + FinTsConfig.Version + "'";

                        segments = segments_;
                    }
                    else
                    {
                        //Since connectionDetails is a re-usable object, this shouldn't be cleared.
                        //connectionDetails.UserId = string.Empty;
                        //connectionDetails.Pin = null;

                        Log.Write("HBCI version not supported");

                        throw new Exception("HBCI version not supported");
                    }

                    client.SEGNUM = SEGNUM.SETInt(4);

                    string message = FinTsMessageAnonymous.Create(connectionDetails.HbciVersion, "1", "0", connectionDetails.Blz, connectionDetails.UserId, connectionDetails.Pin, "0", segments, null, client.SEGNUM);
                    string response = await FinTSMessage.Send(client, message);

                    var messages = Helper.Parse_Segment(client, response);
                    var result = new HBCIDialogResult(messages, response);
                    if (!result.IsSuccess)
                    {
                        Log.Write("Synchronisation anonymous failed. " + result);
                        return response;
                    }

                    // Sync OK
                    Log.Write("Synchronisation anonymous ok");

                    /// <summary>
                    /// INI
                    /// </summary>
                    if (connectionDetails.HbciVersion == 300)
                    {
                        string segments__ =
                            "HKIDN:" + SEGNUM.SETVal(3) + ":2+280:" + connectionDetails.BlzPrimary + "+" + connectionDetails.UserId + "+" + client.SystemId + "+1'" +
                            "HKVVB:" + SEGNUM.SETVal(4) + ":3+0+0+0+" + FinTsConfig.ProductId + "+" + FinTsConfig.Version + "'" +
                            "HKSYN:" + SEGNUM.SETVal(5) + ":3+0'";

                        segments = segments__;
                    }
                    else
                    {
                        Log.Write("HBCI version not supported");

                        throw new Exception("HBCI version not supported");
                    }

                    client.SEGNUM = SEGNUM.SETInt(5);

                    message = FinTSMessage.Create(client, "1", "0", segments, client.HIRMS);
                    response = await FinTSMessage.Send(client, message);

                    Helper.Parse_Segment(client, response);

                    client.HITAN = Helper.Parse_String(Helper.Parse_String(response, "HITAN:", "'").Replace("?+", "??"), "++", "+").Replace("??", "?+");

                    return response;
                }
                catch (Exception ex)
                {
                    //Since connectionDetails is a re-usable object, this shouldn't be cleared.
                    //connectionDetails.UserId = string.Empty;
                    //connectionDetails.Pin = null;

                    Log.Write(ex.ToString());

                    DEBUG.Write("Software error: " + ex.ToString());

                    throw new Exception("Software error: " + ex.ToString());
                }
            }
        }
    }
}
