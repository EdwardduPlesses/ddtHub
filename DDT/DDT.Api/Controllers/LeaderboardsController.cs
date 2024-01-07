using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DDT.Application.Common.Validation;
using DDT.Application.Interfaces;
using DDT.Application.Leaderboards;
using DDT.Domain.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.AspNetCore.Controllers.Controller", Version = "1.0")]

namespace DDT.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class LeaderboardsController : ControllerBase
    {
        private readonly ILeaderboardsService _appService;
        private readonly IUnitOfWork _unitOfWork;

        public LeaderboardsController(ILeaderboardsService appService, IUnitOfWork unitOfWork)
        {
            _appService = appService ?? throw new ArgumentNullException(nameof(appService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified List&lt;LeaderboardDataDto&gt;.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        /// <response code="401">Unauthorized request.</response>
        /// <response code="403">Forbidden request.</response>
        [HttpGet("data")]
        [ProducesResponseType(typeof(List<LeaderboardDataDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LeaderboardDataDto>>> GetLeaderboard(
            [FromQuery] LeaderboardGetDto leaderboardGetData,
            CancellationToken cancellationToken = default)
        {
            var result = default(List<LeaderboardDataDto>);
            result = await _appService.GetLeaderboard(leaderboardGetData, cancellationToken);
            return Ok(result);
        }
    }
}