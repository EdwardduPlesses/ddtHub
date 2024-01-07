using System;
using System.Threading;
using System.Threading.Tasks;
using DDT.Application.Common.Interfaces;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace DDT.Application.Leaderboards
{
    public class GetLeardboardQuery : IRequest<LeaderboardGetDto>, IQuery
    {
        public GetLeardboardQuery()
        {
        }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetLeardboardQueryHandler : IRequestHandler<GetLeardboardQuery, LeaderboardGetDto>
    {
        [IntentManaged(Mode.Ignore)]
        public GetLeardboardQueryHandler()
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<LeaderboardGetDto> Handle(GetLeardboardQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("Your implementation here...");
        }
    }

    public class GetLeardboardQueryValidator : AbstractValidator<GetLeardboardQuery>
    {
        [IntentManaged(Mode.Merge)]
        public GetLeardboardQueryValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}