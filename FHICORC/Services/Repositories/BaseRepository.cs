using System;
using FHICORC.Configuration;
using FHICORC.Core.WebServices;

namespace FHICORC.Services.Repositories
{
    public class BaseRepository
    {
        protected IRestClient _restClient = IoCContainer.Resolve<IRestClient>();

    }
}
