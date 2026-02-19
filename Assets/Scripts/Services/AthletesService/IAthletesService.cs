using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormForge.Domain.Athletes;

namespace FormForge.Services.AthletesService
{
    public interface IAthletesService
    {
        UniTask<Athlete> CreateAthlete(EAthleteType athleteType, string athleteName);
        UniTask<IReadOnlyList<Athlete>> GetAthletes();
    }
}