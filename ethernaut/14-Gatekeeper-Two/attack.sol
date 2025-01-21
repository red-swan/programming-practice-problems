// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

interface IGatekeeperTwo {
    function enter(bytes8 _gateKey) external returns(bool);
}

contract GatecrasherTwo {
    
    IGatekeeperTwo victim;
    bytes8 key;

    constructor(address _victim) {
        victim = IGatekeeperTwo(_victim);
        uint64 halfKey = uint64(bytes8(keccak256(abi.encodePacked(this))));
        uint64 keyMaker = type(uint64).max;
        key = bytes8(halfKey ^ keyMaker);
        victim.enter(bytes8(key));
    }
    
    function destroy() public {
        selfdestruct(payable(msg.sender));
    }
}