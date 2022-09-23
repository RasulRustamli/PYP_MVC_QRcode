using PYP_MVC_QRcode.Models;

namespace PYP_MVC_QRcode.Abstraction.Services
{
    public interface IQrCardCreate
    {
        Task<string> CreateQrCode(CardContact cardContact);
    }
}
