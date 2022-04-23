using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCodeHuntDotNet.API.DAL.Models;
using QRCodeHuntDotNet.API.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using QRCodeHuntDotNet.API.Controllers.Util;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace QRCodeHuntDotNet.API.Controllers
{
    [ApiVersion("1"), Route("codes")]
    [ApiController]
    [Produces("application/json")]
    public class CodesController : ControllerBase
    {
        private const string CreationErrorTitle = "Invalid New Codes";
        private const string NameExistsErrorDetail = "Codes for the given game name already exist.";
        private const string NotFoundTitle = "Not Found";
        private const string NotFoundDetail = "Codes for the requested game name were not found.";

        private ICodeRepository _codeRepository;
        private IResponseObjectFactory _responseObjectFactory;

        public CodesController(ICodeRepository codeRepository, IResponseObjectFactory responseObjectFactory)
        {
            _codeRepository = codeRepository;
            _responseObjectFactory = responseObjectFactory;
        }

        // GET: api/v1/codes/gameName
        [HttpGet("{gameName}")]
        public ActionResult<IResponseObject> GetCodes(string gameName)
        {
            IEnumerable<Code> codes = _codeRepository.GetCodes(gameName);
            if (codes == null)
            {
                IResponseObject response = _responseObjectFactory
                    .CreateErrorResponseObject(HttpStatusCode.NotFound, NotFoundTitle, NotFoundDetail);
                return NotFound(response);
            }
            return _responseObjectFactory.CreateResponseObject(codes);
        }

        // POST: api/v1/codes/gameName/numCodes
        [HttpPost("{gameName}/{numCodes}")]
        public ActionResult<IResponseObject> PostCodes(string gameName, int numCodes, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] CodesRequestDTO codesDTO = null)
        {
            if (_codeRepository.CodesDataFileExists(gameName))
            {
                ErrorResponseObject errorResponse = _responseObjectFactory
                    .CreateErrorResponseObject(HttpStatusCode.Conflict, CreationErrorTitle, NameExistsErrorDetail);
                return Conflict(errorResponse);
            }
            IEnumerable<Code> codes = Enumerable.Range(1, numCodes).Select(num => new Code
            {
                Key = Guid.NewGuid(),
                Num = num
            });
            string siteUrl = string.IsNullOrEmpty(codesDTO?.SiteUrlOverride) ?
                $"{HttpContext.Request.Scheme}{Uri.SchemeDelimiter}{HttpContext.Request.Host}" : codesDTO.SiteUrlOverride;
            _codeRepository.CreateList(gameName, codes, siteUrl);
            return CreatedAtAction("GetCodes", new { gameName = gameName }, codes);
        }
    }

    public class CodesRequestDTO
    {
        public string SiteUrlOverride { get; set; }
    }
}
