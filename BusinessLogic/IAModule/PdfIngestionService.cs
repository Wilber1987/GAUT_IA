using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.ApiChat.AutomaticIA.TrainingModule;
using BusinessLogic.ApiChat.AutomaticIAOllama.Model;
using BusinessLogic.IA.Providers;
using BusinessLogic.IAModule.Utils;

namespace BusinessLogic.IAModule
{
    public class PdfIngestionService
    {
        private readonly IIAProvider _provider;

        public PdfIngestionService(IIAProvider provider)
        {
            _provider = provider;
        }

        
    }
}