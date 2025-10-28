using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class WebglLaunchParameters : ILaunchParameters
    {
        private Dictionary<string, string> _parameters;

        public WebglLaunchParameters() 
        {
            _parameters = new Dictionary<string, string>();

            var url = Application.absoluteURL;
            url = url.Replace(((char)0).ToString(), "");

            Log.Debug($"Application URL: {url}");

            // Ignore potential fragment
            var queryUrl = url.Split('#');
            queryUrl = queryUrl[0].Split("?");

            if(queryUrl.Length <= 1)
            {
                Log.Debug("Application url does not have any query parameters");
                return;
            }

            Log.Debug($"Application query parameters: {queryUrl[1]}");

            var pairs = queryUrl[1].Split("&");

            foreach(var pair in pairs )
            {
                var separated = pair.Split('=');
                _parameters[separated[0]] = separated.Length > 1 ? separated[1] : "";

                Log.Debug($"Adding key value pair: {separated[0]} : {_parameters[separated[0]]}");
            }
        }

        public bool TryGetValue(string key, out string value)
        {
            return _parameters.TryGetValue(key, out value);
        }
    }
}