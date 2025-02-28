namespace RealEstateApi.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(string email);
    }
}
