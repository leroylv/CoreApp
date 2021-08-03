using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoreApp.Data.Enums;

namespace CoreApp.Data
{
    public class VATProvider
    {
        public virtual async Task<(VatCheckService.checkVatResponse, VatNumberStatus vatNumberStatus, string exception)> GetVatNumberStatusAsync(string fullVatNumber)
        {
            if (string.IsNullOrWhiteSpace(fullVatNumber))
                return (null, VatNumberStatus.Empty, "Empty input");
            fullVatNumber = fullVatNumber.Trim();

            var r = new Regex(@"^(\w{2})(.*)");
            var match = r.Match(fullVatNumber);
            if (!match.Success)
                return (null, VatNumberStatus.Invalid, "Invalid input");

            var twoLetterIsoCode = match.Groups[1].Value;
            var vatNumber = match.Groups[2].Value;

            return await DoVatCheckAsync(twoLetterIsoCode, vatNumber);
        }

        protected virtual async Task<(VatCheckService.checkVatResponse, VatNumberStatus vatNumberStatus, string exception)> DoVatCheckAsync(string twoLetterIsoCode, string vatNumber)
        {
            if (string.IsNullOrEmpty(twoLetterIsoCode))
                return (null, VatNumberStatus.Empty, "ISO code empty");

            if (string.IsNullOrEmpty(vatNumber))
                return (null, VatNumberStatus.Empty, "VAT number empty");

            if (vatNumber == null)
                vatNumber = string.Empty;
            vatNumber = vatNumber.Trim().Replace(" ", string.Empty);

            if (twoLetterIsoCode == null)
                twoLetterIsoCode = string.Empty;
            if (!string.IsNullOrEmpty(twoLetterIsoCode))
                twoLetterIsoCode = twoLetterIsoCode.ToUpper();

            try
            {
                var s = new VatCheckService.checkVatPortTypeClient();
                var result = await s.checkVatAsync(new VatCheckService.checkVatRequest
                {
                    vatNumber = vatNumber,
                    countryCode = twoLetterIsoCode
                });

                return (result, result.valid ? VatNumberStatus.Valid : VatNumberStatus.Invalid, null);
            }
            catch (Exception ex)
            {
                return (null, VatNumberStatus.Unknown, ex.Message);
            }
        }

    }
}
