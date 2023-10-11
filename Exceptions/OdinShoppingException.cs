using System.Runtime.Serialization;

namespace OdinShopping.Exceptions
{
    public class OdinShoppingException : Exception
    {
        public OdinShoppingException()
        {
        }

        public OdinShoppingException(string message) : base(message)
        {
        }

        public OdinShoppingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OdinShoppingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
