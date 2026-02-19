using System.Collections.Generic;
using System.Linq;
using FormForge.Domain.Exercises;
using FormForge.Networking.Athletes.DTO;

namespace FormForge.Core.Networking.AthleteTypes.Mapping
{
    public static class EExerciseTypeMapper
    {
        private static readonly Dictionary<EExerciseType, EExerciseTypeDto> DomainToDto =
            new Dictionary<EExerciseType, EExerciseTypeDto>
            {
                { EExerciseType.None, EExerciseTypeDto.none },
                { EExerciseType.Cycling, EExerciseTypeDto.cycling },
                { EExerciseType.Deadlift, EExerciseTypeDto.deadlift },
                { EExerciseType.Rowing, EExerciseTypeDto.rowing },
                { EExerciseType.Running, EExerciseTypeDto.running },
                { EExerciseType.Squat, EExerciseTypeDto.squat },
                { EExerciseType.Stretching, EExerciseTypeDto.stretching },
                { EExerciseType.BenchPress, EExerciseTypeDto.bench_press },
                { EExerciseType.CoreStability, EExerciseTypeDto.core_stability },
                { EExerciseType.OverheadPress, EExerciseTypeDto.overhead_press },
                { EExerciseType.YogaFlow, EExerciseTypeDto.yoga_flow },
            };

        private static readonly Dictionary<EExerciseTypeDto, EExerciseType> DtoToDomain =
            DomainToDto.ToDictionary(x => 
                x.Value, x => x.Key);

        public static EExerciseTypeDto ToDto(EExerciseType domain)
            => DomainToDto[domain];

        public static EExerciseType ToDomain(EExerciseTypeDto dto)
            => DtoToDomain[dto];
    }
}