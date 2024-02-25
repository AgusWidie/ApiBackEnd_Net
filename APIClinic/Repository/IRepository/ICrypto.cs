using APIClinic.Models.DTOs.Request;

namespace APIClinic.Repository.IRepository
{
    public interface ICrypto
    {
        string MD5Decrypt(InputCrypto param);
        string MD5Encryption(InputCrypto param);
    }
}
