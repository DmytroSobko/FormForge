using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormForge.Networking.Athletes.Requests;
using FormForge.Runtime.Models.Athletes;

namespace FormForge.Services.AthletesService
{
    public interface IAthletesService
    {
        UniTask<Athlete> CreateAthlete(CreateAthleteRequest request);
        UniTask<IReadOnlyList<Athlete>> GetAthletes();
    }
}