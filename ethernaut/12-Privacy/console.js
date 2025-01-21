var data = await web3.eth.getStorageAt(instance, 5);
var key = data.slice(0, 34);
await contract.unlock(key);