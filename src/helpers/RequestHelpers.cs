using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Client.NiceHash.Helpers {
	public class RequestHelpers {
		private Random generator = new Random();

		public long GenerateTimeHeaderValue() {
			return DateTimeOffset.Now.ToUnixTimeSeconds();
		}

		public string GenerateNonce() {
			return String.Join("",
				Enumerable.Range(1, 36).Select(x => {
					return Convert.ToChar(
						Convert.ToInt32(
							Math.Floor(25 * generator.NextDouble()) + 65
						)
					);
				})
			);
		}

		public string GenerateIdempotencyKey() {
			return Guid.NewGuid().ToString();
		}

		public string GenerateSignature(
			string apiKey,
			string apiSecret,
			long xTime,
			string nonce,
			string organisationId,
			string method,
			string path,
			string query,
			string body
		) {
			var toSign = new List<byte[]>();
			var sep = new byte[] { 0x00 };

			toSign.Add(Encoding.ASCII.GetBytes(apiKey));
			toSign.Add(sep);
			toSign.Add(Encoding.ASCII.GetBytes(xTime.ToString()));
			toSign.Add(sep);
			toSign.Add(Encoding.ASCII.GetBytes(nonce));
			toSign.Add(sep);
			toSign.Add(sep);
			toSign.Add(Encoding.ASCII.GetBytes(organisationId));
			toSign.Add(sep);
			toSign.Add(sep);
			toSign.Add(Encoding.ASCII.GetBytes(method));
			toSign.Add(sep);
			toSign.Add(Encoding.ASCII.GetBytes(path));
			toSign.Add(sep);
			toSign.Add(Encoding.ASCII.GetBytes(query));

			if (!string.IsNullOrWhiteSpace(body)) {
				toSign.Add(sep);
				toSign.Add(Encoding.UTF8.GetBytes(body));
			}

			var sig = "";

			using (var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(apiSecret))) {
				sig = hmac.ComputeHash(new MemoryStream(toSign.Aggregate(new byte[0], (ret, next) => {
					return ret.Concat(next).ToArray();
				}))).Aggregate("", (s, e) => s + String.Format("{0:x2}",e), s => s );
			}

			return $"{apiKey}:{sig}";
		}
	}
}