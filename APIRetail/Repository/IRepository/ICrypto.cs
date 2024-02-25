using APIRetail.Models.DTO.Request.Crypto;

namespace APIRetail.Repository.IRepository
{
    public interface ICrypto
    {
        string MD5Encryption(InputCrypto param);
        string MD5Decrypt(InputCrypto param);
    }
}
