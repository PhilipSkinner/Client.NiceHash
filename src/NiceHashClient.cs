using System;
using Client.NiceHash.Helpers;

namespace Client.NiceHash {
    public class NiceHashClient : INiceHashClient {
    	private readonly RequestHelpers helpers;

    	public NiceHashClient() {
    		this.helpers = new RequestHelpers();
    	}

    	public void Test() {
    		Console.WriteLine($"X-Time: {this.helpers.GenerateTimeHeaderValue()}");
    		Console.WriteLine($"X-Nonce: {this.helpers.GenerateNonce()}");
    		Console.WriteLine($"X-Request-Id: {this.helpers.GenerateIdempotencyKey()}");
    		var res = this.helpers.GenerateSignature("4ebd366d-76f4-4400-a3b6-e51515d054d6", "fd8a1652-728b-42fe-82b8-f623e56da8850750f5bf-ce66-4ca7-8b84-93651abc723b", 1543597115712, "9675d0f8-1325-484b-9594-c9d6d3268890", "da41b3bc-3d0b-4226-b7ea-aee73f94a518", "GET", "/main/api/v2/hashpower/orderBook", "algorithm=X16R&page=0&size=100", "");

			Console.WriteLine($"X-Auth: {res}");

    		if (res != "4ebd366d-76f4-4400-a3b6-e51515d054d6:21e6a16f6eb34ac476d59f969f548b47fffe3fea318d9c99e77fc710d2fed798") {
    			throw new Exception("Not good");
    		}


    	}
    }
}
