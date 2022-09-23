using PYP_MVC_QRcode.Abstraction.Services;
using PYP_MVC_QRcode.Models;
using QRCoder;
using System.Text;

namespace PYP_MVC_QRcode.Services
{
    public class QrCardCreate : IQrCardCreate
    {
        public Task<string> CreateQrCode(CardContact cardContact)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCARD");
            sb.AppendLine("VERSION:2.1");
            sb.AppendLine("N:;" + cardContact.Firtname + ";" + cardContact.Surname + ";;;;");
            sb.AppendLine("FN:" + cardContact.Firtname + " " + cardContact.Surname);
            sb.AppendLine("TEL;CELL:" + cardContact.Phone);
            sb.AppendLine("EMAIL;PREF;INTERNET:" + cardContact.Email);
            sb.AppendLine("ADR;HOME:;;" + cardContact.City + ";" + cardContact.Country);
            sb.AppendLine("END:VCARD");

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(sb.ToString(), QRCodeGenerator.ECCLevel.Q);

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] byteImage = ms.ToArray();
                return Task.FromResult("data:image/png;base64," + Convert.ToBase64String(byteImage));
            }
        }
    }
}
