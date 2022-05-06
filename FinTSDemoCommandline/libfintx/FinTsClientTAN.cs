﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace libfintx
{
    public partial class FinTsClient
    {
        /// <summary>
        /// Confirm order with TAN
        /// </summary>
        /// <param name="TAN"></param>
        /// <returns>
        /// Bank return codes
        /// </returns>
        public async Task<HBCIDialogResult> TAN(string TAN)
        {
            string BankCode = await Transaction.TAN(this, TAN);
            var result = new HBCIDialogResult(Helper.Parse_BankCode(BankCode), BankCode);

            return result;
        }

        /// <summary>
        /// Confirm order with TAN
        /// </summary>
        /// <param name="TAN"></param>
        /// <param name="MediumName"></param>
        /// <returns>
        /// Bank return codes
        /// </returns>
        public async Task<HBCIDialogResult> TAN4(string TAN, string MediumName)
        {
            string BankCode = await Transaction.TAN4(this, TAN, MediumName);
            var result = new HBCIDialogResult(Helper.Parse_BankCode(BankCode), BankCode);

            return result;
        }

        /// <summary>
        /// Request tan medium name
        /// </summary>
        /// <param name="connectionDetails">ConnectionDetails object must atleast contain the fields: Url, HBCIVersion, UserId, Pin, Blz</param>
        /// <returns>
        /// TAN Medium Name
        /// </returns>
        public async Task<HBCIDialogResult<List<string>>> RequestTANMediumName()
        {
            HKTAN.SegmentId = "HKTAB";

            HBCIDialogResult result = await InitializeConnection();
            if (!result.IsSuccess)
                return result.TypedResult<List<string>>();

            // Should not be needed when processing HKTAB
            //result = ProcessSCA(connectionDetails, result, tanDialog);
            //if (!result.IsSuccess)
            //    return result.TypedResult<List<string>>();

            string BankCode = await Transaction.HKTAB(this);
            result = new HBCIDialogResult<List<string>>(Helper.Parse_BankCode(BankCode), BankCode);
            if (!result.IsSuccess)
                return result.TypedResult<List<string>>();

            // Should not be needed when processing HKTAB
            //result = ProcessSCA(connectionDetails, result, tanDialog);
            //if (!result.IsSuccess)
            //    return result.TypedResult<List<string>>();

            BankCode = result.RawData;
            string BankCode_ = "HITAB" + Helper.Parse_String(BankCode, "'HITAB", "'");
            return result.TypedResult(Helper.Parse_TANMedium(BankCode_));
        }
    }
}
