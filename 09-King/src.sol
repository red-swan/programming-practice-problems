// SPDX-License-Identifier: MIT
pragma solidity ^0.6.0;

contract King {

  address payable king;
  uint public prize;
  address payable public owner;

  constructor() public payable {
    owner = msg.sender;  
    king = msg.sender;
    prize = msg.value;
  }

  receive() external payable {
    require(msg.value >= prize || msg.sender == owner);
    king.transfer(msg.value);
    king = msg.sender;
    prize = msg.value;
  }

  function _king() public view returns (address payable) {
    return king;
  }
}
// our instance
// 0x140816A8d5C6204455479b7dDbd4dB2c5d6587a6
// our init king
// 0x5cECE66f3EB19f7Df3192Ae37C27D96D8396433D
// prize size
// 1000000000000000000
// max int size 
// 115792089237316195423570985008687907853269984665640564039457584007913129639935 
// wei per ether 1000000000000000000
// ether
// 115792089237316195423570985008687907853269984665640564039457.584007913129639935
