using Microsoft.AspNetCore.Mvc;

namespace OdinShopping.Services
{
    public interface ICartService
    {
        public Task<int> GetCartId();
    }
}
