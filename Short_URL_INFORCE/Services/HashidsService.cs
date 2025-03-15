using HashidsNet;

namespace Short_URL_INFORCE.Services
{
    public class HashidsService
    {
        private readonly Hashids _hashids;

        public HashidsService(Hashids hashids)
        {
            _hashids = hashids;
        }

        public string Encode(int id) 
        {
            return _hashids.Encode(id);
        }

        public int Decode(string shortUrl)
        {
            var decoded = _hashids.Decode(shortUrl);
            return decoded.Length > 0 ? decoded[0] : 0;
        }

    }
}
