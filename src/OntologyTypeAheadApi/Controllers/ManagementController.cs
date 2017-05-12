﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OntologyTypeAheadApi.Helpers;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using OntologyTypeAheadApi.Enums;
using OntologyTypeAheadApi.Service.Contract;
using OntologyTypeAheadApi.Service.Implementation;
using OntologyTypeAheadApi.Models.Request;
using OntologyTypeAheadApi.Models.Response;

namespace OntologyTypeAheadApi.Controllers
{
    public class ManagementController : ApiController
    {
        private readonly IDatastoreService _datastoreService;

        [HttpGet]
        [Route("management/populate")]
        public async Task<HttpResponseMessage> PopulateDataStore()
        {
            var request = new ConformedRequest()
            {
                Route = "management/populate",
            };
            var response = await _datastoreService.PopulateDatastore();

            return HttpResponseHelper.StandardiseResponse(request, response);
        }

        public ManagementController(IDatastoreService datastoreService)
        {
            _datastoreService = datastoreService;
        }
    }
}