using API.DTOs;
namespace API.Services
{
  
    public interface IBitcoinService
    {
        Task<List<BitcoinDataDTO>> FindBy(DateTime startDate);

    }
    
}
