using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using Google.Maps;
using Google.Maps.Geocoding;
using UBotPlugin;

namespace Advanced_Google_Maps
{
    public class MapsFindByAddess : IUBotCommand
    {
        private string _returnValue = "";

        public MapsFindByAddess()
        {
            _parameters.Add(new UBotParameterDefinition("Address", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Region", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Report", UBotType.UBotTable));
        }

        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {
            var address = parameters["Address"];
            var region = parameters["Region"];
            var report = parameters["Report"];

            var request = new GeocodingRequest();
            request.Address = address;
            request.Region = region;
            GeocodeResponse response = new GeocodingService().GetResponse(request);
            string[,] table = new string[response.Results.Length+1,3];

            table[0,0] = "Formatted Address";
            table[0,1] = "Latitude";
            table[0,2] = "Longitude";
           
            if (response.Status == ServiceResponseStatus.Ok)
            {
                Result[] results = response.Results;
                for (int i = 1; i <= results.Length; i++)
                {
                    var s = results[i];
                    table[i,0] = s.FormattedAddress;
                    table[i,1] = s.Geometry.Location.Latitude.ToString(CultureInfo.InvariantCulture);
                    table[i,2] = s.Geometry.Location.Longitude.ToString(CultureInfo.InvariantCulture);
                }
                Thread.Sleep(1000);
            }
            ubotStudio.SetTable(report, table);
        }

        public string Category
        {
            get { return "Google Maps"; }
        }

        public string CommandName
        {
            get { return "maps find by address"; }
        }

        public object ReturnValue
        {
            get { return _returnValue; }
        }

        public bool IsContainer
        {
            get { return false; }
        }

        private readonly List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();

        public IEnumerable<UBotParameterDefinition> ParameterDefinitions
        {
            get { return _parameters; }
        }

        public UBotVersion UBotVersion
        {
            get { return UBotVersion.Standard; }
        }

        public UBotType ReturnValueType
        {
            get { return UBotType.String; }
        }
    }
}

