using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.Runtime.Models.Athletes;

namespace FormForge.Services.AthletesService
{
    public interface IAthletesService
    {
        void CreateAthlete();
        Task<IReadOnlyList<Athlete>> GetAthletes();
    }
}