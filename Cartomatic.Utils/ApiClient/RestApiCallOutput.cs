using RestSharp;

namespace Cartomatic.Utils.ApiClient
{

    /// <summary>
    /// Api call output data container
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    public class RestApiCallOutput<TOut>
    {
        /// <summary>
        /// Output deserialized to a specified type
        /// </summary>
        public TOut Output { get; set; }

        /// <summary>
        /// Raw response object
        /// </summary>
        public IRestResponse Response { get; set; }
    }

}
