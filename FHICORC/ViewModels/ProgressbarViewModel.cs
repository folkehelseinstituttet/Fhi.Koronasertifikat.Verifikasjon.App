using FHICORC.Services;
using FHICORC.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace FHICORC.ViewModels
{
    internal class ProgressbarViewModel : BaseViewModel
    {
        public string ProgressbarText => "PROGRESSBAR_LOADING_TEXT".Translate();
    }
}
