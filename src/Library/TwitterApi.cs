using System.Security.Cryptography;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using RestSharp.Authenticators;
using RestSharp;

namespace Ucu.Poo.Twitter
{
	/// <summary>
	/// Credit to Danny Tuppeny: https://blog.dantup.com/2016/07/simplest-csharp-code-to-post-a-tweet-using-oauth/
	/// Simple class for sending tweets to Twitter using Single-user OAuth
	/// https://dev.twitter.com/oauth/overview/single-user
	/// </summary>
	public abstract class TwitterApi
	{
		private const string twitterApiBaseUrl = "https://api.twitter.com/2/";
		private static string apiKey, apiKeySecret, accessToken, accessTokenSecret;
		private static HMACSHA1 sigHasher;
		readonly DateTime epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		readonly int limit;
		private RestClient twitterClient;

		/// <summary>
		/// Inicializa una nueva instancia de TwitterApi.
		/// </summary>
		/// <param name="limit">El tamaño máximo de los mensajes a publicar.</param>
		public TwitterApi(int limit = 280)
		{
			this.limit = limit;
			this.Initialize();
		}

		private void Initialize()
		{
			//bearear=AAAAAAAAAAAAAAAAAAAAAGXShgEAAAAAAIPfj7etfHxISa%2FQ%2BBsCI56%2F5OY%3DHqoK7V961MJltVb6wDFPlyLhulV6KQpHzaTqWcIXldwRRX5snq
			apiKey = "kPBQI5Q43spF7QZempfmxe7N4";
			apiKeySecret = "KoZwNVQ3tc9nGtNSbQx0aSThaHiC237eyfEnrsW4VFEfGyLXef";
			accessToken = "1396065818-dybxkKn4TFn1XerJCTgmqrG2OapBlYDqnrSkFqp";
			accessTokenSecret = "YAKqiICEKDdeP2163sxUNCGt7mhA2ESQ7z2klLEtrner3";

			var authenticator = OAuth1Authenticator.ForAccessToken(
				apiKey, apiKeySecret, accessToken, accessTokenSecret
			);
			var twitterOptions = new RestClientOptions(twitterApiBaseUrl)
			{
				Authenticator = authenticator
			};
			this.twitterClient = new RestClient(twitterOptions);

			sigHasher = new HMACSHA1(new ASCIIEncoding().GetBytes($"{apiKeySecret}&{accessTokenSecret}"));
		}

		/// <summary>
		/// Envía un tweet con el texto provisto como argumento y retorna la respuesta de la API de Twitter.
		/// </summary>
		public string Tweet(string message)
		{
			var data = new
			{
				text = this.CutTweetToLimit(message)
			};

			return this.PostRequest("tweets", data);
		}

		protected string PostRequest<T>(string url, T data) where T : class
		{
			var request = new RestRequest(url, Method.Post).AddJsonBody(data);
			var response = twitterClient.Execute(request);
			return response.Content;
		}

		protected string GetRequest<T>(string url, T data) where T : class
		{
			var request = new RestRequest(url, Method.Get).AddJsonBody(data);
			var response = twitterClient.Execute(request);
			return response.Content;
		}

		protected string PrepareOAuth(string url, Dictionary<string, string> data)
		{
			// seconds passed since 1/1/1970
			var timestamp = (int)((DateTime.UtcNow - epochUtc).TotalSeconds);

			// Add all the OAuth headers we'll need to use when constructing the hash
			Dictionary<string, string> oAuthData = new Dictionary<string, string>();
			oAuthData.Add("oauth_consumer_key", apiKey);
			oAuthData.Add("oauth_signature_method", "HMAC-SHA1");
			oAuthData.Add("oauth_timestamp", timestamp.ToString());
			oAuthData.Add("oauth_nonce", Guid.NewGuid().ToString());
			oAuthData.Add("oauth_token", accessToken);
			oAuthData.Add("oauth_version", "1.0");

			if (data != null) // add text data too, because it is a part of the signature
			{
				foreach (var item in data)
				{
					oAuthData.Add(item.Key, item.Value);
				}
			}

			// Generate the OAuth signature and add it to our payload
			oAuthData.Add("oauth_signature", GenerateSignature(url, oAuthData));

			// Build the OAuth HTTP Header from the data
			return GenerateOAuthHeader(oAuthData);
		}

		/// <summary>
		/// Generate an OAuth signature from OAuth header values
		/// </summary>
		private string GenerateSignature(string url, Dictionary<string, string> data)
		{
			var sigString = string.Join(
				"&",
				data
					.Union(data)
					.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}")
					.OrderBy(s => s)
			);

			string fullSigData = $"POST&{Uri.EscapeDataString(url)}&{Uri.EscapeDataString(sigString.ToString())}";

			return Convert.ToBase64String(
				sigHasher.ComputeHash(
					new ASCIIEncoding().GetBytes(fullSigData)
				)
			);
		}

		/// <summary>
		/// Generate the raw OAuth HTML header from the values (including signature)
		/// </summary>
		private string GenerateOAuthHeader(Dictionary<string, string> data)
		{
			return string.Format(
				"OAuth {0}",
				string.Join(
					", ",
					data
						.Where(kvp => kvp.Key.StartsWith("oauth_"))
						.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}=\"{Uri.EscapeDataString(kvp.Value)}\""
						).OrderBy(s => s)
					)
				);
		}

		/// <summary>
		/// Cuts the tweet text to fit the limit.
		/// </summary>
		/// <returns>Cutted tweet text</returns>
		/// <param name="tweet">Uncutted tweet text</param>
		protected string CutTweetToLimit(string tweet)
		{
			while (tweet.Length >= limit)
			{
				tweet = tweet.Substring(0, tweet.LastIndexOf(" ", StringComparison.Ordinal));
			}
			return tweet;
		}
	}
}
