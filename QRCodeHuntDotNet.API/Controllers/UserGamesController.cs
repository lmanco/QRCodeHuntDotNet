using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCodeHuntDotNet.API.Controllers.Util;
using QRCodeHuntDotNet.API.DAL.Models;
using QRCodeHuntDotNet.API.DAL.Repositories;
using QRCodeHuntDotNet.API.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QRCodeHuntDotNet.API.Controllers
{
    [ApiVersion("1"), Route("user-games")]
    [ApiController]
    [Produces("application/json")]
    public class UserGamesController : ControllerBase
    {
        private const string NotFoundTitle = "Not Found";
        private const string NotFoundDetail = "The requested game was not found.";
        private const string AddUserGameCodeErrorTitle = "Invalid Code Number";
        private const string AddUserGameCodeTooLowErrorDetail = "Code number is too low.";
        private const string AddUserGameCodeTooHighErrorDetail = "Code number is too high.";
        private ActionResult<IResponseObject> GameNotFoundErrorResponse
        {
            get
            {
                IResponseObject response = _responseObjectFactory
                    .CreateErrorResponseObject(HttpStatusCode.NotFound, NotFoundTitle, NotFoundDetail);
                return NotFound(response);
            }
        }

        private readonly IUserGameRepository _userGameRepository;
        private readonly ICodeRepository _codeRepository;
        private readonly IMapper _mapper;
        private readonly IResponseObjectFactory _responseObjectFactory;
        private readonly IHttpContextHelper _httpContextHelper;

        public UserGamesController(IUserGameRepository userGameRepository, ICodeRepository codeRepository,
            IMapper mapper, IResponseObjectFactory responseObjectFactory, IHttpContextHelper httpContextHelper)
        {
            _userGameRepository = userGameRepository;
            _codeRepository = codeRepository;
            _mapper = mapper;
            _responseObjectFactory = responseObjectFactory;
            _httpContextHelper = httpContextHelper;
        }

        // GET: api/v1/user-games/gameName
        [HttpGet("{gameName}")]
        public async Task<ActionResult<IResponseObject>> GetCurrentUserGame(string gameName)
        {
            IEnumerable<Code> codes = _codeRepository.GetCodes(gameName);
            if (codes == null)
                return GameNotFoundErrorResponse;
            UserGame userGame = await FindOrCreateCurrentUserGame(gameName, codes);
            UserGameResponseDTO userGameDTO = _mapper.Map<UserGameResponseDTO>(userGame);
            return _responseObjectFactory.CreateResponseObject(userGameDTO);
        }

        // PATCH: api/v1/user-games/gameName/codeNum
        [HttpPatch("{gameName}/{codeNum}")]
        public async Task<ActionResult<IResponseObject>> AddUserGameCodeFound(string gameName, int codeNum)
        {
            IEnumerable<Code> codes = _codeRepository.GetCodes(gameName);
            if (codes == null)
                return GameNotFoundErrorResponse;
            if (codeNum < 1)
                return BadRequest(AddUserGameCodeErrorTitle, AddUserGameCodeTooLowErrorDetail);
            if (codeNum > codes.Count())
                return BadRequest(AddUserGameCodeErrorTitle, AddUserGameCodeTooHighErrorDetail);
            UserGame userGame = await FindOrCreateCurrentUserGame(gameName, codes);
            userGame.CodesFound = userGame.CodesFound.Select((codeFound, index) => codeFound || index == codeNum - 1).ToArray();
            return await TryPerformUserGameUpdate(userGame);
        }

        private async Task<UserGame> FindOrCreateCurrentUserGame(string gameName, IEnumerable<Code> codes)
        {
            long currentUserId = _httpContextHelper.GetCurrentUserId(HttpContext);
            UserGame userGame = await _userGameRepository.GetUserGame(currentUserId, gameName);
            if (userGame == null)
            {
                await _userGameRepository.CreateUserGame(new UserGame
                {
                    UserId = currentUserId,
                    GameName = gameName,
                    CodesFound = Enumerable.Range(0, codes.Count()).Select(_ => false).ToArray()
                }); ;
            }
            return await _userGameRepository.GetUserGame(currentUserId, gameName);
        }

        private async Task<ActionResult<IResponseObject>> TryPerformUserGameUpdate(UserGame userGame)
        {
            try
            {
                await _userGameRepository.UpdateUserGame(userGame);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_userGameRepository.ExistsInCurrentContext(userGame.UserId, userGame.GameName))
                    return GameNotFoundErrorResponse;
                else
                    throw;
            }
        }

        private ActionResult<IResponseObject> BadRequest(string errorTitle, string errorDetail)
        {
            ErrorResponseObject errorResponse = _responseObjectFactory
                .CreateErrorResponseObject(HttpStatusCode.BadRequest, errorTitle, errorDetail);
            return BadRequest(errorResponse);
        }
    }
}
